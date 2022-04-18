using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace SmartCodes.Models
{
    internal class Sql
    {
        
        
        
        static string database = ChooseDataBase();
        public static string connectionPermisson = @$"Server={FindServer()};Database={database};Trusted_Connection=True;";
        public static List<string> tableNames = GetTableNames();
        

        //Select
        public static void Select(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionPermisson))
            {
                connection.Open();
                using (SqlDataAdapter sDA = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    sDA.Fill(dataTable);
                    string columnNames = "";
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                        columnNames += dataTable.Columns[i].ColumnName + " | ";

                    Console.WriteLine(columnNames);
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        string result = "";
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                            result += dataRow[i] + " | ";

                        Console.WriteLine(result);
                    }
                }
            }
        }


        //Execute
        public static void Execute(string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionPermisson))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        if (command.ExecuteNonQuery() > 0)
                            Console.WriteLine("Done");

                        else
                            Console.WriteLine("Nothing Changed");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }


        //Find Server With Xml
        public static string FindServer() //localhost
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@$"{Directory.GetDirectoryRoot(Directory.GetCurrentDirectory())}Users\{Environment.UserName}\AppData\Roaming\Microsoft\SQL Server Management Studio\18.0");
            FileInfo fileInfo = new FileInfo(dirInfo + @"\UserSettings.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(fileInfo.ToString());

            var node = xmlDoc.DocumentElement.SelectNodes(@"/SqlStudio/SSMS/ConnectionOptions/ServerTypes/Element/Value/ServerTypeItem/Servers/Element/Item/ServerConnectionItem/Instance");

            foreach (XmlNode node2 in node)
                return node2.InnerXml;

            throw new Exception("Cant Find Server Enter Manulaly");
        }


        //GetDataBases
        public static List<string> GetDataBases()
        {
            using (SqlConnection connection = new SqlConnection(@$"Server={FindServer()};Trusted_Connection=True;"))
            {
                List<string> list = new List<string>();
                connection.Open();
                using (SqlDataAdapter sDA = new SqlDataAdapter("SELECT name FROM sys.databases", connection))
                {
                    DataTable dataTable = new DataTable();
                    sDA.Fill(dataTable);

                    Console.WriteLine("DB Names:");
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        string result = "";
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                            result += dataRow[i] + " ";

                        list.Add(result);
                    }
                    return list;
                }
            }
            throw new Exception();
        }



        //Chouse From DataBase
        public static string ChooseDataBase() //edit
        {
            do
            {
                foreach (var databasename in GetDataBases())
                    Console.WriteLine(databasename);

                string temp = GenericInput.StringInput("Choose DataBase");

                /*if (GetDataBases().Contains(temp)) return temp;*/ //islemir database de +1 araliq yazir

                foreach (var databasename in GetDataBases()) //edit with generic
                    if (databasename.Trim().Equals(temp)) return temp;

                Console.WriteLine("Not Found");
            } while (true);
        }


        //GetTableNames
        public static List<string> GetTableNames()
        {
            List<string> tablenames = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionPermisson))
            {
                connection.Open();
                using (SqlDataAdapter sDA = new SqlDataAdapter($"SELECT * FROM {database.Trim()}.sys.tables", connection))
                {
                    DataTable dataTable = new DataTable();
                    sDA.Fill(dataTable);


                    for (int i = 0; i < dataTable.Rows.Count; i++)
                        tablenames.Add(dataTable.Rows[i][0].ToString());
                }
                return tablenames;
            }
            throw new Exception();
        }

        public static void Operations()
        {
            byte choise;
            do
            {
                choise = GenericInput.NumberInput<byte>("0-break\n1-ShowAll");
                switch (choise)
                {
                    case 0: break;
                    case 1:
                        ShowAll(); break;
                    case 2:

                        break;
                    default:
                        Console.WriteLine("Wrong Input");
                        break;
                }
            } while (choise != 0);
            ShowAll();
        }

        public static void ShowAll()
        {
            foreach (var tablename in tableNames)
            {
                Console.WriteLine("------------------------------\n" + tablename);
                Select($"SELECT * FROM {tablename}");
            }
        }


        //Create
        public static void Create()
        {
            using (SqlConnection connection = new SqlConnection(connectionPermisson))
            {
                connection.Open();
                sbyte choise;

                do
                {
                    byte counter = 0;
                    foreach (var table in tableNames)
                    {
                        Console.WriteLine(counter + ")" + table);
                        counter++;
                    }
                    choise = GenericInput.NumberInput<sbyte>("-1)exit\nChoose table", -1, tableNames.Count-1);
                    if (choise == -1) break;

                    string command = $@"INSERT INTO {tableNames[choise]} VALUES ( ";

                    using (SqlDataAdapter sDA = new SqlDataAdapter($"SELECT * FROM {tableNames[choise]}", connection))
                    {
                        DataTable dataTable = new DataTable();
                        sDA.Fill(dataTable);

                        foreach (DataColumn column in dataTable.Columns)
                        {
                            if (column.ColumnName!="Id")
                            {
                                Type type = column.DataType;
                                if (type.Name == "DateTime") command += $"'{DateTime.Now}'";
                                else if (type.Name == "String") command += $"'{GenericInput.StringInput(column.ColumnName)}'";
                                else if (type.Name == "Int32") command += $"{GenericInput.NumberInput<Int32>(column.ColumnName)}";
                                else if (type.Name == "Double") command += $"{GenericInput.NumberInput<Double>(column.ColumnName)}";
                                else if(type.Name == "Decimal") command += $"{GenericInput.NumberInput<Decimal>(column.ColumnName)}";
                                if (column.ColumnName != dataTable.Columns[dataTable.Columns.Count-1 ].ColumnName) command += ",";
                            }
                        }
                        command += ")";
                        Execute(command);
                    }
                } while (true);

            }
        }
    }

}




