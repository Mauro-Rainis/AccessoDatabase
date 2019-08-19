using System;
using System.Data.SqlClient;

namespace AccessoDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("**** SqlServer connection"); 

            // Connessione a SQLServer
            var connStringSqlServer = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\mauro\\source\\repos\\_da_samsung\\AccessoDatabase\\AccessoDatabase\\BearziPersone.mdf;Integrated Security=True";
            using (var conn = new SqlConnection(connStringSqlServer))
            {
                conn.Open();
                var sql = "select * from persone";
                using (SqlCommand sqlCmd = new SqlCommand(sql, conn))
                {
                    SqlDataReader dr = sqlCmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Console.WriteLine("Id:{0} Cognome:{1} Nome:{2}", dr[0], dr[1], dr[2]);
                        }
                    }
                }
            }

        }
    }
}
