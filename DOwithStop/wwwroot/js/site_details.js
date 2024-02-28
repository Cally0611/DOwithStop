// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.



//Call the function Load to get data from API
$(document).ready(
    function () {
        setInterval(function () {
           //var randomnumber = Math.floor(Math.random() * 100);
            Load();
        }, 1000);
    });

//$(document).ready(
//    Load());

function Load() {
    var dt = new Date().toLocaleTimeString();
    $('#showtime').text(dt);
    //console.log("time");
    $.ajax({
        type: "GET",
        url: "/api/Details/TargetActual",
        dataType: "json",
        success: function (result) {
            //console.log(result);
            GetMachineDetails(result);
        },
        error: function (err) {
        }
    });
}

//const card = document.getElementById('cardmch1');
//card.addEventListener("click", flipCard);


function GetMachineDetails(machinedetails) {
    //console.log(machinedetails);
    machinedetails.forEach((number, index) => {
    

        const divmchID = document.getElementById('mch' + number.machineId.toString());
        
        const child1_cardheader = divmchID.querySelector('.card-header');
        child1_cardheader.textContent = number.machineName;

        const child2_cardbody = divmchID.querySelector('.card-body');
        const child2a_cardbodyact = child2_cardbody.querySelector('.actualtext');
        const child2b_cardbodyact = child2_cardbody.querySelector('.mchnorundetails');
        const child2a_cardbodytgt = child2_cardbody.querySelector('.targettext');
        child2a_cardbodytgt.innerHTML = number.targetPerShift;

        child2a_cardbodyact.innerHTML = number.actualNum;
        switch (parseInt(number.parentCodePerShift)) {
            case 1:
                child2a_cardbodyact.classList.add(checkActualColour(parseInt(number.actualNum), parseInt(number.targetPerShift), number.machineName));
                break;
            case 0:
            case 3:
            case 4:
               // child2a_cardbodyact.classList.add('actualtext-nojob');
                //child2a_cardbodytgt.classList.add('.actualtext-nojob');
                child2a_cardbodytgt.style.color = "grey";
                child2a_cardbodyact.style.color = "grey";
                break;
        }

        child2b_cardbodyact.innerHTML = checkReasonCode(number.parentCodePerShift, number.childCodePerShift) ;

        const footer = divmchID.querySelector('.card-footer');
        const p_chdfooter = footer.querySelector('.float-start');
        p_chdfooter.textContent = number.jobName1;
    });
}

function checkActualColour(actualNum, targetnum, mch) {
 
    const now = new Date();
    const currhour = now.getHours();
    const currminutes = now.getMinutes();
    //console.log(currhour + "-" + currminutes);
    var tgtperhour = targetnum / 12;
    tgtperhour = Math.round(tgtperhour);
    //console.log(mch + " TargetPerShift-" + targetnum + " TgtPerhour-" + tgtperhour);

    //if time is first 45 minutes past start of shift - dont check anything
    var startchecking = false;


            if (currhour == 7 && (currminutes >= 0 && currminutes < 35)
                || currhour == 19 && (currminutes >= 0 && currminutes < 35)) {
                //console.log("before 7.35");
            }
            else {
                switch (true) {
                    case (currhour >= 7 && currhour < 19):
                        var forecastperhour = (currhour - 6) * tgtperhour;
                        if (actualNum >= forecastperhour) {
                            return 'actualtext-ontarget';
                        }
                        else {
                            return 'actualtext-offtarget';
                        }
                        break;
                    case (currhour >= 19 && currhour <= 23):
                        var forecastperhour = (currhour - 18) * tgtperhour;
                        if (actualNum >= forecastperhour) {
                            return 'actualtext-ontarget';
                        }
                        else {
                            return 'actualtext-offtarget';
                        }
                        break;
                    case (currhour >= 0 && currhour < 7):
                        var forecastperhour = (currhour + 6) * tgtperhour;
                        if (actualNum >= forecastperhour) {
                            return 'actualtext-ontarget';
                        }
                        else {
                            return 'actualtext-offtarget';
                        }
                        break;
                }
            }
          
    
   

 

    }

  
function checkReasonCode(parentrrc, childrrc) {
    var preason = parseInt(parentrrc);
    var creason = parseInt(childrrc);
    var reasonondo = '';
    switch (preason) {
        case 0:
            reasonondo = 'Make Ready';
            break;
        case 1:
            reasonondo = 'Active';
            break;
        case 2:
            reasonondo = 'Breakdown';
            break;
        case 3:
            reasonondo = 'Maintenance';
            break;
        case 4:
            reasonondo = 'No Work';
            break;
        case 5:
            reasonondo = 'Plate Change';
            break;
        case 6:
            reasonondo = 'Cycle Service';
            break;
    }

    switch (creason) {
        case 0:
            reasonondo.concat(reasonondo,null);
            break;
        case 1:
            reasonondo.concat(reasonondo, '-Start Job');
            break;
        case 2:
            reasonondo.concat(reasonondo, '-Job end');
            break;
        case 3:
            reasonondo.concat(reasonondo, '-Plate Change');
            break;
        case 4:
            reasonondo.concat(reasonondo, '-Cycle Service');
            break;
    }

    return reasonondo;
}





function flipCard(clicked_id) {
   
        document.getElementById(clicked_id).classList.toggle("flipCard");
        var originalID = clicked_id;
        var mchID = parseInt(originalID.replace("cardmch", ""));
        $.ajax({
            type: "GET",
            url: "/api/Details/StopReasons",
            dataType: "json",
            success: function (result) {
                //console.log(result);
                PlotChart(result[mchID], mchID);
            },
            error: function (err) {
            }
        });

    // this code will run after 1 minute
    setTimeout(function () {
        document.getElementById(clicked_id).classList.toggle("flipCard");
    }, 60000);
}


function gradientcolor(ctx) {
    var background_1 = ctx.createLinearGradient(0, 0, 0, 90);
    background_1.addColorStop(0, '#E8E8E8');
    background_1.addColorStop(1, '#3366FF');

    return background_1;
}



function PlotChart(stopresults, mchID) {
    // JS - Destroy exiting Chart Instance to reuse <canvas> element
    let chartStatus = Chart.getChart('chartmch' + mchID.toString()); // <canvas> id
    if (chartStatus != undefined) {
        chartStatus.destroy();
    }
    //-- End of chart destroy
    var dataarr = [];
    var labelarr = [];
    Object.values(stopresults).forEach(val => {
        dataarr.push(val.stopDownTime);
        if (val.stop_SReason != '') {
            labelarr.push(val.stop_MReason + "-" + val.stop_SReason);
        }
        else {
            labelarr.push(val.stop_MReason);
        }

    });
    //console.log(stopresults);




    const ctx = document.getElementById('chartmch' + mchID.toString());
    const config = {
        type: 'bar',
        data: {

            labels: labelarr,
            datasets: [{
                label: 'Top 5 Stop Reasons',
                data: dataarr,
                backgroundColor: '#838996',
                /*color: 'white',*/
                barPercentage: 0.5,
                barThickness: 25,
                maxBarThickness: 30,
                minBarLength: 2,

            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    title: {
                        display: true,
                        text: 'Minutes',
                        align: 'center',
                        font: {
                            family: 'Segoe UI',
                            size: 13,
                            style: 'normal',
                            lineHeight: 1.2
                        }
                    },
                    min: 0,
                    max: 200,
                    fontColor: "red",
                    ticks: {
                        stepSize: 50,
                        color: "#f5f5f5",

                    },
                    border: {
                        display: true,
                        color: '#C0C0C0'
                    },
                    grid: {
                        color: function (context) {
                            if (context.tick.value == 0) {
                                return '#C0C0C0'
                            }
                        },

                    }

                },
                x: {
                    ticks: {
                        display: true,
                        align: 'center',
                        padding: -3,
                        // Include a dollar sign in the ticks
                        callback: function (value, index, values) {
                           // console.log(dataarr[value]);
                            return dataarr[value];
                        },
                       
                        color: "white"

                    }

                }
            },
            plugins: {
                title: {
                    display: true,
                    position: 'top',
                    align: 'center',
                    text: 'Top 5 Stop Reasons',
                    color: 'white',
                    font: {
                        family: 'Segoe UI',
                        size: 13,
                        weight: 'lighter'
                    }
                },
                legend: {
                    display: false
                },
                layout: {
                    padding: 20
                },
                datalabels: {
                    //color: 'white',
                    //anchor: 'start',
                    //font: {
                    //    family: 'Segoe UI',
                    //    size: 10,
                    //    weight: 'normal'
                    //},
                    //align: 'end',
                    //offset : 0,
                    //rotation : 270,
                    //formatter: function (value, context) {
                    //    return context.chart.data.labels[context.dataIndex];
                    //},
                    labels: {
                        //indexval: {
                        //   /* anchor: 'end',*/
                        //    align: 'bottom',

                        //    color: 'yellow',
                        //    offset: 0,
                        //    font: {
                        //        family: 'Segoe UI',
                        //        size: 10,
                        //        weight: 'normal'
                        //    },
                        //},
                        indextext: {
                            color: 'white',
                            anchor: 'start',
                            font: {
                                family: 'Segoe UI',
                                size: 10,
                                weight: 'normal'
                            },
                            align: 'end',
                            offset: 0,
                            rotation: 270,
                            formatter: function (value, context) {
                                return context.chart.data.labels[context.dataIndex];
                            }
                        },
                    }
                }
            }

        },
        //set it globally
        plugins: [ChartDataLabels]

    };
    //const myLineChart = new Chart(ctx, config);
    //myLineChart.destroy();



    const drawchart = new Chart(ctx, config);



}