using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class Database
    {
        private readonly string connectionString = "Data Source=habits.sqlite";

        public List<string> TakeAllNameTable()
        {
            SqliteConnection connection = new(connectionString);
            connection.Open();
            var nameAllTable = connection.CreateCommand();
            nameAllTable.CommandText =
            @"
                SELECT name
                FROM sqlite_master
                WHERE type = 'table'  
            ";
            var namesTables = new List<string>();
            using var reader = nameAllTable.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetString(0) != "sqlite_sequence")
                {
                    namesTables.Add(reader.GetString(0));
                }
            }
            return namesTables;
        }

        public List<List<string>> TakeAllRecords(string nameTable)
        {
            SqliteConnection connection = new(connectionString);
            connection.Open();
            var allRecords = connection.CreateCommand();
            allRecords.CommandText = $"SELECT ID, Date, Count FROM {nameTable}";
            var recordsHabit = new List<List<string>>();
            using var reader = allRecords.ExecuteReader();
            while (reader.Read())
            {
                List<string> recordHabit = new() {reader.GetInt32(0).ToString(), reader.GetString(1), reader.GetInt32(2).ToString()};
                recordsHabit.Add(recordHabit);
            }
            return recordsHabit;
        }

        public void CreateTable(string nameTable)
        {
            SqliteConnection connection = new(connectionString);
            connection.Open();
            var habitTable = connection.CreateCommand();
            habitTable.CommandText =
            @$"
                 CREATE TABLE IF NOT EXISTS {nameTable} (
                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT NOT NULL,
                    Count INT NOT NULL
                 )
            ";
            habitTable.ExecuteNonQuery();
        }

        public void ReadTable(string nameTable)
        {
            SqliteConnection connection = new(connectionString);
            using (connection)
            {
                connection.Open();
                var selectHabitTable = connection.CreateCommand();
                selectHabitTable.CommandText = $"SELECT * FROM {nameTable}";
                var reader = selectHabitTable.ExecuteReader();
                using (reader)
                {
                    if (reader.Read())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"ID: {reader.GetInt32(0)}, " + $"Date: {reader.GetString(1)}, " + $"Count: {reader.GetInt32(2)}");
                        }
                    } else
                    {
                        Console.WriteLine("Oops! You don't have any records yet.");
                        Console.WriteLine("-------------------------------");
                    }
                }
            }
        }

        public void InsertRecord(string nameTable, string dateHabit, int countHabit)
        {
            SqliteConnection connection = new(connectionString);
            connection.Open();
            var insertHabitTable = connection.CreateCommand();
            insertHabitTable.CommandText = $"INSERT INTO {nameTable} (Date, Count) VALUES (@date, @count);";
            insertHabitTable.Parameters.AddWithValue("@date", dateHabit);
            insertHabitTable.Parameters.AddWithValue("@count", countHabit);
            insertHabitTable.ExecuteNonQuery();
        }

        public void DeleteRecord(string nameTable, int idTable)
        {
            SqliteConnection connection = new(connectionString);
            connection.Open();
            var deleteHabitRecord = connection.CreateCommand();
            deleteHabitRecord.CommandText = $"DELETE FROM {nameTable} WHERE ID = {idTable}";
            deleteHabitRecord.ExecuteNonQuery();
        }

        public void UpdateRecord(string nameTable, int idTable, int newCount)
        {
            SqliteConnection connection = new(connectionString);
            connection.Open();
            var updateHabitRecord = connection.CreateCommand();
            updateHabitRecord.CommandText = $"UPDATE {nameTable} SET Count = {newCount} WHERE ID = {idTable}";
            updateHabitRecord.ExecuteNonQuery();
        }
    }
}
