// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


document.addEventListener('contextmenu', event => event.preventDefault())
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
        //timeout: 3000,
        success: function (result) {
            //console.log(result);
            GetMachineDetails(result);
        },
        error: function (err,timeout) {
            console.log('TargetActual' + err);
            console.log('TargetActual' + timeout);
        }
    });
    $.ajax({
        type: "GET",
        url: "/api/Details/Oee",
        dataType: "json",
        success: function (result) {
            //console.log(result);
            GetOeeDetails(result);
        },
        error: function (err, timeout) {
            console.log('Oee' + err);
            console.log('Oee' + timeout);
        }
    });
    $.ajax({
        type: "GET",
        url: "/api/Details/Finishing",
        dataType: "json",
        success: function (result) {
            //console.log(result);
            GetFinishingDetails(result);
        },
        error: function (err, timeout) {
            console.log('Finishing' + err);
            console.log('Finishing' + timeout);
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
        const child2a_cardbodytgt = child2_cardbody.querySelector('.targettext');

        const child2b_cardbodymchrun = child2_cardbody.querySelector('.mchnorundetails');

        child2a_cardbodytgt.innerHTML = number.targetPerShift;

        child2a_cardbodyact.innerHTML = number.actualNum;
        switch (parseInt(number.parentCodePerShift)) {
            case 1:
                child2a_cardbodyact.removeAttribute("style");
                const classlistact = child2a_cardbodyact.classList;
                if (classlistact.length > 1) {
                   
                    classlistact.remove(classlistact[classlistact.length - 1]);
                }
                child2a_cardbodyact.classList.add(checkActualColour(parseInt(number.actualNum), parseInt(number.targetPerShift), number.machineName));
                break;
            case 0:
            case 3:
            case 4:
                child2a_cardbodyact.removeAttribute("style");
                child2a_cardbodytgt.removeAttribute("style");
                child2a_cardbodytgt.style.color = "grey";
                child2a_cardbodyact.style.color = "grey";
                
                
                break;
        }

        const jobstatus = checkReasonCode(number.parentCodePerShift, number.childCodePerShift);
        child2b_cardbodymchrun.innerHTML = jobstatus;

        if (jobstatus == 'Active') {
            child2b_cardbodymchrun.removeAttribute("style");
            child2b_cardbodymchrun.style.color = 'Black';
        }
        else {

            child2b_cardbodymchrun.innerHTML = jobstatus;
            child2b_cardbodymchrun.style.color = 'White';
        }






        const footer = divmchID.querySelector('.card-footer');
        const p_chdfooter = footer.querySelector('.float-start');
        
        p_chdfooter.textContent = number.jobName1;
    });
}

function GetOeeDetails(oeeresults) {
    oeeresults.forEach((number, index) => {

        const divmchID = document.getElementById('cardmch' + number.oeeMachineId.toString());

        const nodejobstatus = divmchID.querySelector('.mchnorundetails');

        const jobstatus = nodejobstatus.innerHTML;
       
     
        const oeefooter = document.getElementById('mchoee' + number.oeeMachineId.toString());
        if (jobstatus == 'Active') {
            oeefooter.innerHTML = 'OEE : ';
            if (number.oeeInPercentage >= number.oeeTarget) {
                oeefooter.innerHTML += '<span class="actualtext-ontarget">' + number.oeeInPercentage.toFixed(0) + '</span>';
            }
            else {
                oeefooter.innerHTML += '<span class="actualtext-offtarget">' + number.oeeInPercentage.toFixed(0) + '</span>';
            }

            oeefooter.innerHTML += ' / ';
            oeefooter.innerHTML += '<span class="oeetarget">' + number.oeeTarget.toFixed(0) + '</span>';
        }
        else {
            oeefooter.innerHTML = '';
        }
        
       
        //console.log(getactualtextcolor.classList);
       

        
        

//            number.oeeInPercentage
    });
}

function GetFinishingDetails(finresults) {
    //console.log(finresults[0].f1_Target);

    const f1Act = finresults[0].f1_Actual;
    const f1Tgt = finresults[0].f1_Target;
   
   

    const f2Act = finresults[0].f2_Actual;
    const f2Tgt = finresults[0].f2_Target;
    

    //F1 Actual /Target

    const f1_act = document.getElementById('f1_actual');
    f1_act.innerHTML = finresults[0].f1_Actual.toString();
    f1_act.removeAttribute("style");

    const classlistf1 = f1_act.classList;
    if (classlistf1.length > 1) {

        classlistf1.remove(classlistf1[classlistf1.length - 1]);
    }

    f1_act.classList.add(checkActualColour(parseInt(f1Act), parseInt(f1Tgt), 1));


    const f1_tgt = document.getElementById('f1_target');
 
    f1_tgt.innerHTML = finresults[0].f1_Target.toString();



      //F2 Actual /Target
    const f2_act = document.getElementById('f2_actual');
    f2_act.innerHTML = finresults[0].f2_Actual.toString();
    f2_act.removeAttribute("style");
    const classlistf2 = f2_act.classList;
    if (classlistf2.length > 1) {

        classlistf2.remove(classlistf2[classlistf2.length - 1]);
    }

    f2_act.classList.add(checkActualColour(parseInt(f2Act), parseInt(f2Tgt), 2));

    const f2_tgt = document.getElementById('f2_target');
    f2_tgt.innerHTML = finresults[0].f2_Target.toString();
  

    
}

function checkActualColour(actualNum, targetnum, mch) {

    const now = new Date();
    const currhour = now.getHours();
    const currminutes = now.getMinutes();
    //console.log(currhour + "-" + currminutes);
    var tgtperhour = targetnum / 12;

    tgtperhour = tgtperhour.toFixed(2);

    //if time is first 45 minutes past start of shift - dont check anything
    var startchecking = false;


    if (currhour == 7 && (currminutes >= 0 && currminutes < 35)
        || currhour == 19 && (currminutes >= 0 && currminutes < 35)) {
        return 'actualtext-shiftstart';
    }
    else {
        switch (true) {
            case (currhour >= 7 && currhour < 19):
                var forecastperhour = (currhour - 7) * tgtperhour;
                //console.log(forecastperhour);
                forecastperhour = Math.round(forecastperhour);
             
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
            reasonondo.concat(reasonondo, null);
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
    //console.log('Run this');
    var flipcontainer = document.getElementById(clicked_id);
    
    var mchruncode = flipcontainer.querySelector('.mchnorundetails').textContent;


    var containsflipclass = flipcontainer.classList.contains("flipCard");


    var fliptimeout;
    if (mchruncode == 'Active' && containsflipclass == false) {
        document.getElementById(clicked_id).classList.toggle("flipCard");
        flipChartTimer(clicked_id);
       

        fliptimeout = setTimeout(function () {
            const flg_class = flipcontainer.classList.contains("flipCard");
            if (flg_class == true) {
                document.getElementById(clicked_id).classList.remove("flipCard");
            }
        }, 20000);
        alert(fliptimeout);
   
    }
    else if (mchruncode == 'Active' && containsflipclass == true) {
        alert(fliptimeout);
        clearTimeout(fliptimeout);
        document.getElementById(clicked_id).classList.remove("flipCard");
        
    }

}

function flipChartTimer(clicked_id) {
    var originalID = clicked_id;
    var mchID = parseInt(originalID.replace("cardmch", ""));
    $.ajax({
        type: "GET",
        url: "/api/Details/StopReasons",
        dataType: "json",
        success: function (result) {
            PlotChart1(result[mchID], mchID);
        },
        error: function (err) {
            console.log(err);
        }
    });

  
}

function gradientcolor(ctx) {
    var background_1 = ctx.createLinearGradient(0, 0, 0, 90);
    background_1.addColorStop(0, '#E8E8E8');
    background_1.addColorStop(1, '#3366FF');

    return background_1;
}


function PlotChart1(stopresults, mchID) {
    var dataarr = [];
    var labelarr = [];

    //console.log('This ' + stopresults + 'ID' + mchID);
 
    if (typeof stopresults !== 'undefined' && stopresults !== null) {
        //console.log("Got data");
        Object.values(stopresults).forEach(val => {
            dataarr.push(val.stopDownTime);
            if (val.stopReasonName != null && val.subSRName != null) {
                labelarr.push(val.subSRName);
               
            }
            else {
                labelarr.push(val.stopReasonName);
            }

        });
    }
    else {
        //console.log("No data");
        dataarr = [];
        labelarr = [];
        //console.log(dataarr.length);
    }


    // JS - Destroy exiting Chart Instance to reuse <canvas> element
    let chartStatus = Chart.getChart('chartmch' + mchID.toString()); // <canvas> id

    if (chartStatus != undefined) {
        chartStatus.destroy();
    }
    //-- End of chart destroy

    const ctx = document.getElementById('chartmch' + mchID.toString());

    const noData = {
        id: 'noData',
        afterDatasetsDraw: ((chart, args, plugins) => {
            const {
                ctx,
                data,
                chartArea: {
                    top, bottom, left, right, width, height
                } } = chart;
            ctx.save();

          /*  console.log("Total data" + data);*/
            if (dataarr.length == 0) {
                ctx.fillStyle = 'black';
                ctx.fillRect(left, top, width, height);
                ctx.font = 'bold 50px Arial';
                ctx.fillStyle = 'white';
                ctx.textAlign = 'center';
                ctx.fillText('Awaiting Data', left + width / 2, top + height / 2);
            }

        })
    };

    const config = {
        type: 'bar',
        data: {

            labels: labelarr,
            datasets: [{
                label: 'Top 5 Stop Reasons',
                data: dataarr,
                backgroundColor: '#838996',
                /*color: 'white',*/
                /*  barPercentage: 10,*/
                barThickness: 90,
                maxBarThickness: 200,
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
                            family: 'Arial',
                            size: 30,
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
                        font: {
                            family: 'Arial',
                            size: 40,

                        },
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
                        font: {
                            family: 'Arial',
                            size: 40,

                        },
                        color: "white"

                    }

                }
            },
            plugins: {
                title: {
                    //Change Title here
                    display: false,
                    position: 'top',
                    align: 'center',
                    text: 'Top  5 Stop Reasons',
                    color: 'white',
                    font: {
                        family: 'Arial',
                        size: 50,
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
                                family: 'Arial',
                                size: 40,
                                weight: 'normal'
                            },
                            align: 'end',
                            offset: 0,
                            rotation: 270,
                            formatter: function (value, context) {
                                const reaslbl = context.chart.data.labels[context.dataIndex];
                                const resaftsplit = context.chart.data.labels[context.dataIndex].split(' ');
                                if (resaftsplit.length > 2) {
                                    const joinfirsttwo = [resaftsplit[0], resaftsplit[1]].join(' ');
                                    let remainingtxt = '';
                                    for (var i = 2; i >= 2 && i < resaftsplit.length; i++) {

                                        remainingtxt += resaftsplit[i];
                                        remainingtxt += ' ';
                                    }

                                    return joinfirsttwo + "\n" + remainingtxt;
                                }
                                else {
                                    return reaslbl;
                                }
                               
                            }
                        },
                    }
                },

            }

        },
        //set it globally
        plugins: [ChartDataLabels, noData]

    };
    const drawchart = new Chart(ctx, config);

}

