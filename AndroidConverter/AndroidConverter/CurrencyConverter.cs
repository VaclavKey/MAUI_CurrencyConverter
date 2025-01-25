using Newtonsoft.Json;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Runtime.Caching;

namespace AndroidConverter
{
    internal class CurrencyConverter
    {
        public static List<string> Currencies = new List<string>
        {
            "RUB",
            "USD",
            "EUR",
            "GBP",
            "JPY",
            "BYN",
            "PLN",
            "CNY",
            "TRY",
            "KZT"
        };


        private static readonly string apiKey = "eef797fed440454c4cb26d7e"; // Ключ API
        private static readonly string apiUrl = "https://v6.exchangerate-api.com/v6/{0}/latest/{1}"; // API для получения курсов валют


        private static string connectionString = "Data Source=" + GetDatabaseFilePath(); // Строка подключения к БД
        private static readonly HttpClient _httpClient = new HttpClient();


        public static string dbQuery = "INSERT INTO ExchangeRates (FromCurrency, ToCurrency, Rate) VALUES "; // Заготовка для запроса на добавление всех валют в БД
        public static string dbQueryText;



        public static async Task<decimal?> GetRate(string fromCurrency, string toCurrency)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();


                string query = $"SELECT Rate FROM ExchangeRates WHERE FromCurrency = '{fromCurrency}' and ToCurrency = '{toCurrency}'";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return Convert.ToDecimal(reader[0]);
                        }
                    }
                }
            }
            return 0;
        }

        private static async Task GetExchangeRates()
        {
            foreach (string currrency in Currencies)
            {
                string url = string.Format(apiUrl, apiKey, currrency); // Подстановка в ссылку API-ключа и необходимой валюты


                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();


                string content = await response.Content.ReadAsStringAsync();
                var rates = JsonConvert.DeserializeObject<ApiResponse>(content);

                if (rates != null)
                {
                    foreach (var rate in rates.Rates)
                    {
                        if (Currencies.Contains(rate.Key))
                        {
                            string insert = $"('{currrency}', '{rate.Key}', {rate.Value.ToString(CultureInfo.InvariantCulture)}),";
                            dbQuery += insert;
                        }
                        dbQueryText = dbQuery.Substring(0, dbQuery.Length - 1) + ';'; // Удаляю запятую в конце и заменяю её на ';'
                    }
                }
            }
        }

        public static async Task UpdateRates()
        {
            if (((DateTime.Now - GetTimer()).TotalHours > 24) || CacheEmpty())
            {
                await GetExchangeRates(); // Создание INSERT-запроса
                ClearRates(); // Очистка БД от устаревших курсов валют

                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();


                    using (SqliteCommand command = new SqliteCommand(dbQueryText, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                UpdateTimer(DateTime.Now);
            }
        }

        private static void ClearRates()
        {
            if (TableExists("ExchangeRates"))
            {
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();


                    string query = "DELETE FROM ExchangeRates;";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string query =
                        "CREATE TABLE ExchangeRates" +
                        "(" +
                        "ID INTEGER PRIMARY KEY," +
                        "FromCurrency TEXT NOT NULL," +
                        "ToCurrency TEXT NOT NULL," +
                        "Rate REAL NOT NULL" +
                        ");";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

        

        private static void UpdateTimer(DateTime value)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query1 = "DELETE FROM Cache;";
                string query2 = $"INSERT INTO Cache VALUES ('{value}');";

                using (SqliteCommand command = new SqliteCommand(query1, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (SqliteCommand command = new SqliteCommand(query2, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void InitializeCache()
        {
            if (!TableExists("Cache"))
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    string query = "CREATE TABLE Cache ( Timer DATETIME );";

                    using (SqliteCommand command = new SqliteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    
        private static DateTime GetTimer()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();


                string query = "SELECT * FROM Cache";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return Convert.ToDateTime(reader[0]);
                        }
                    }
                }
            }
            return DateTime.Now;
        }

        private static bool CacheEmpty()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();


                string query = "SELECT COUNT(*) FROM Cache;";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    var result = command.ExecuteScalar();

                    int rowCount = Convert.ToInt32(result);

                    return (rowCount > 0) ? false : true;
                }
            }
        }



        private static void CopyDatabaseFromResources(string localDbPath)
        {
            try
            {
                using (var stream = File.OpenRead(Path.Combine(FileSystem.AppDataDirectory, "Resources", "raw", "ExchangeRates.db")))
                using (var fileStream = new FileStream(localDbPath, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при копировании базы данных: {ex.Message}");
            }
        }

        private static string GetDatabaseFilePath()
        {
            string fileName = "ExchangeRates.db";
            string localDbPath = Path.Combine(FileSystem.AppDataDirectory, fileName);


            if (!File.Exists(localDbPath))
            {
                CopyDatabaseFromResources(localDbPath);
            }

            return localDbPath;
        }

        private static bool TableExists(string tableName)
        {
            try
            {
                string query = $"SELECT name FROM sqlite_master WHERE type='table' AND name = '{tableName}';";

                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();


                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        var result = command.ExecuteScalar();
                        Console.WriteLine("Table already exists");
                        return result != null;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Table doesn't exist");
                return false;
            }
        }
    }
}