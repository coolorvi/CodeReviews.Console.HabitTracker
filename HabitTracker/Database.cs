using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class Database
    {
        private readonly string connectionString = "Data Source=habits.sqlite";

        public void CreateTables()
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var tables = connection.CreateCommand();
            tables.CommandText = @"
                CREATE TABLE IF NOT EXISTS Habits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE
                );
                CREATE TABLE IF NOT EXISTS Records (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabitId INTEGER NOT NULL,
                    Date TEXT NOT NULL,
                    Count INTEGER NOT NULL,
                    FOREIGN KEY (HabitId) REFERENCES Habits(Id)
                );
            ";
            tables.ExecuteNonQuery();
        }

        public List<(int Id, string Name)> GetAllHabits()
        {
            var habits = new List<(int, string)>();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var table = connection.CreateCommand();
            table.CommandText = "SELECT Id, Name FROM Habits";
            using var reader = table.ExecuteReader();

            while (reader.Read())
                habits.Add((reader.GetInt32(0), reader.GetString(1)));

            return habits;
        }

        public int CreateHabit(string name)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var table = connection.CreateCommand();
            table.CommandText = "INSERT INTO Habits (Name) VALUES (@name); SELECT last_insert_rowid();";
            table.Parameters.AddWithValue("@name", name);

            return Convert.ToInt32(table.ExecuteScalar());
        }

        public void ReadAllRecords(int habitId)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var table = connection.CreateCommand();
            table.CommandText = "SELECT Id, Date, Count FROM Records WHERE HabitId=@habitId";
            table.Parameters.AddWithValue("@habitId", habitId);

            using var reader = table.ExecuteReader();

            bool empty = true;
            while (reader.Read())
            {
                empty = false;
                Console.WriteLine($"ID: {reader.GetInt32(0)}, Date: {reader.GetString(1)}, Count: {reader.GetInt32(2)}");
            }

            if (empty)
                Console.WriteLine("No records found.");
        }

        public void InsertRecord(int habitId, string date, int count)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var table = connection.CreateCommand();
            table.CommandText = "INSERT INTO Records (HabitId, Date, Count) VALUES (@habitId, @date, @count)";
            table.Parameters.AddWithValue("@habitId", habitId);
            table.Parameters.AddWithValue("@date", date);
            table.Parameters.AddWithValue("@count", count);

            table.ExecuteNonQuery();
        }

        public void DeleteRecord(int id)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var table = connection.CreateCommand();
            table.CommandText = "DELETE FROM Records WHERE Id=@id";
            table.Parameters.AddWithValue("@id", id);

            table.ExecuteNonQuery();
        }

        public void UpdateRecord(int id, int count)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var table = connection.CreateCommand();
            table.CommandText = "UPDATE Records SET Count=@count WHERE Id=@id";
            table.Parameters.AddWithValue("@count", count);
            table.Parameters.AddWithValue("@id", id);

            table.ExecuteNonQuery();
        }
        public List<(int Id, string Date, int Count)> GetRecords(int habitId)
        {
            var records = new List<(int, string, int)>();
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var table = connection.CreateCommand();
            table.CommandText = "SELECT Id, Date, Count FROM Records WHERE HabitId=@habitId";
            table.Parameters.AddWithValue("@habitId", habitId);

            using var reader = table.ExecuteReader();
            while (reader.Read())
                records.Add((reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));

            return records;
        }

        public bool RecordExists(int recordId)
        {
            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            var table = connection.CreateCommand();
            table.CommandText = "SELECT COUNT(1) FROM Records WHERE Id=@id";
            table.Parameters.AddWithValue("@id", recordId);

            return Convert.ToInt32(table.ExecuteScalar()) > 0;
        }

    }
}
