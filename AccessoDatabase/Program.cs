using Npgsql;
using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using System.Data.Common;

namespace AccessoDatabase
{
    class Program
    {
        private static IConfiguration config;

        static void Main(string[] args)
        {
            // Leggo i files di configurazione
            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.development.json", true, true)
                .Build();

            // Per utilizzare anche il file Secrets.json come fatto nella Web App vedi qua:
            // https://stackoverflow.com/questions/42268265/how-to-get-manage-user-secrets-in-a-net-core-console-application
            // https://www.twilio.com/blog/2018/05/user-secrets-in-a-net-core-console-app.html

            Console.WriteLine("**** SqlServer connection");
            SqlServerSelect();

            Console.WriteLine("\n\n\n");

            Console.WriteLine("**** Postgresql connection");
            PostgresqlSelect();

            // In .NET Framework posso rimuovere il codice duplicato con una semplice chiamata
            // a DbProviderFactories.GetFactory("System.Data.SqLite")
            // https://docs.microsoft.com/en-us/dotnet/api/system.data.common.dbproviderfactories?view=netcore-2.2
            GenericDbAccess();
        }

        private static void SqlServerSelect()
        {
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
        }

        private static void PostgresqlSelect()
        {
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
        }

        private static void GenericDbAccess()
        {
            Console.WriteLine("**** Generic database connection:");
            DbProviderFactories.RegisterFactory("SqlServer", SqlClientFactory.Instance);

            var dbProvider = DbProviderFactories.GetFactory("SqlServer");
            using (var connection = dbProvider.CreateConnection())
            {
                connection.ConnectionString = config.GetConnectionString("SqlServer");
                connection.Open();

                var cmd = dbProvider.CreateCommand();
                cmd.CommandText = "select * from persone";
                cmd.Connection = connection;

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine("Generic: " + reader.GetInt32(0) + " " + reader.GetString(1) + " " + reader.GetString(2));
                }
                reader.Close();
            }
        }
    }
}
