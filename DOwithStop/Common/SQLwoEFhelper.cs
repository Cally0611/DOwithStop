using DOwithStop.Data;
using DOwithStop.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace DOwithStop.Common
{
    public class SQLwoEFhelper : ISqlOperation
    {
        private readonly CustomDBContext _customdbContext;

        // private readonly IConfiguration _configuration;


        public SQLwoEFhelper(CustomDBContext customdbContext)
        {
            _customdbContext = customdbContext;

        }

      
        public static object? ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }

        public List<T> ExecuteSPsql<T>(string spname) where T : new()
        {
            SqlConnection dbConnection = (SqlConnection)_customdbContext.Database.GetDbConnection();
            SqlCommand command = new SqlCommand();

            List<T> readdata = new List<T>();
            try
            {
                using (dbConnection)
                {
                    dbConnection.Open();
                    using (command)
                    {
                        command.Connection = dbConnection;
                 
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = spname;

                        //set this sqldatareader
                       SqlDataReader sqldreader = command.ExecuteReader();

                        while (sqldreader.Read())
                        {
                            T t = new T();

                            for (int inc = 0; inc < sqldreader.FieldCount; inc++)
                            {
                                Type type = t.GetType();
                                string name = sqldreader.GetName(inc);

                                if(name.All(c => char.IsLetterOrDigit(c) || c == '_') == true)
                                {
                                    name = name.Replace("_", "");
                                }
                                

                                PropertyInfo prop = type.GetProperty(name);
                                if (prop != null)
                                {
                                    if (name == prop.Name)
                                    {
                                        object? value = sqldreader.GetValue(inc);
                                        if (value != DBNull.Value)
                                        {
                                            prop.SetValue(t, ChangeType(value, prop.PropertyType), null);
                                        }
                                        else
                                        {
                                            value = null;
                                            //prop.SetValue(t, value, null);
                                        }


                                    }
                                }
                            }

                            readdata.Add(t);
                        }



                        sqldreader.Close();
                       
                    }
                    dbConnection.Close();
                    return readdata;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
               
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                    if (dbConnection != null)
                    {
                        if (dbConnection.State != ConnectionState.Closed)
                        {
                            dbConnection.Close();
                        }
                        dbConnection.Dispose();
                    }

                    //Clean Up Command Object
                    if (command != null)
                    {
                        command.Dispose();
                    }
               
            }
        }
    }
}
