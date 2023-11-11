﻿using Microsoft.Data.SqlClient;

using System.Data;


namespace aaa_aspdotnet.src.Common.Helpers
{
    public static class SQLHelper
    {
        public static string GetConnectionString()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetSection("ConnectionStrings")["value"];
        }
        private static readonly string _connectionString = GetConnectionString();

        public static List<Dictionary<string, object>> ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, object>? parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = storedProcedureName;
                    command.CommandType = CommandType.StoredProcedure;

                    AddParameters(command, parameters);

                    return ExecuteReader(command);
                }
            }
        }
      
        public static int ExecuteNonQuery(string sqlQuery, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;

                    AddParameters(command, parameters);

                    return command.ExecuteNonQuery();
                }
            }
        }
        public static object ExecuteScalar(string sqlQuery, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;

                    AddParameters(command, parameters);

                    return command.ExecuteScalar();
                }
            }
        }
        public static T ExecuteScalar<T>(string sqlQuery, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;

                    AddParameters(command, parameters);

                    var result = command.ExecuteScalar();
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
        }

        public static List<Dictionary<string, object>> ExecuteQuery(string sqlQuery, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sqlQuery;

                    AddParameters(command, parameters);

                    return ExecuteReader(command);
                }
            }
        }

        private static void AddParameters(SqlCommand command, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
            }
        }

        private static List<Dictionary<string, object>> ExecuteReader(SqlCommand command)
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var result = new Dictionary<string, object>();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object columnValue = reader.GetValue(i);

                        result[columnName] = columnValue;
                    }

                    results.Add(result);
                }

                return results;
            }
        }




    }
}
