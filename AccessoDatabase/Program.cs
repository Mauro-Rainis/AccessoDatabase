using Npgsql;
using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace AccessoDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            // Leggo i files di configurazione
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", true, true)
                .Build();

            Console.WriteLine("**** SqlServer connection");

            // Connessione a SQLServer
            // Spostiamo la configurazione della connessione al database nel file di configurazione
            var connStringSqlServer = config.GetConnectionString("SqlServer");
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

            Console.WriteLine("\n\n\n");

            // Connessione a Postgresql (codice ripetuto)
            Console.WriteLine("**** Postgresql connection");

            // Spostiamo la configurazione della connessione al database nel file di configurazione
            var connStringPostgresql = config.GetConnectionString("PostgreSql");

            using (var connPsql = new NpgsqlConnection(connStringPostgresql))
            {
                connPsql.Open();

                using (var cmd = new NpgsqlCommand("SELECT * FROM persone", connPsql))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("Id:{0} Cognome:{1} Nome:{2}", reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                        }
                            
                    }
                }
                
            }

            Console.WriteLine("\n\n\n");
            Console.WriteLine("Problemi con questo codice:");
            Console.WriteLine("1. Codice ripetuto");
            Console.WriteLine("2. Configurazione connessione nel codice sorgente");
            Console.WriteLine("2a. Devo ricompilare per modificare connessione");
            Console.WriteLine("2b. Ho dati sensibili su git condivisi con gli altri utenti di git");
        }
    }
}
