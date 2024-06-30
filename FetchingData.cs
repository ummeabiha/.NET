using System;
using System.Data;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Fetching data from Region's table.");

        string sqlConnStr = "Server=DESKTOP-3MORQJH;Database=Teachers;Integrated Security=True;Encrypt=False;";

        string sqlQuery = "SELECT TOP 5 * FROM REGIONS";
        DataTable data = GetSqlData(sqlConnStr, sqlQuery);

        // Print fetched data to console
        foreach (DataRow row in data.Rows)
        {
            foreach (var item in row.ItemArray)
            {
                Console.Write(item + "\t");
            }
            Console.WriteLine();
        }
    }

    public static DataTable GetSqlData(string sqlConnStr, string sqlQuery)
    {
        DataTable DtObj = new DataTable();

        using (SqlConnection conObj = new SqlConnection(sqlConnStr))
        {
            using (SqlDataAdapter DaObj = new SqlDataAdapter(sqlQuery, conObj))
            {
                conObj.Open();
                DaObj.Fill(DtObj);
                conObj.Close();
            }
        }

        return DtObj;
    }
}





