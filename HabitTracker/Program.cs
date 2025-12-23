using System.Globalization;

namespace HabitTracker
{
    internal class Program
    {
        private Database db = new();
        private bool isRunning = true;
        private bool isReturnMainMenu = false;
        private int? currentHabitId = null;
        private string currentHabitName = "";

        static void Main()
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            db.CreateTables();

            while (isRunning)
            {
                if (currentHabitId == null || isReturnMainMenu)
                {
                    ShowMainMenu();
                    HandleMainMenuChoice();
                }

                if (!isRunning) break;

                ShowHabitMenu();
                HandleHabitMenuChoice();
            }

            Console.WriteLine("Goodbye!");
        }

        private void ShowMainMenu()
        {
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("0: Close App");
            Console.WriteLine("1: Change Existing Habit");
            Console.WriteLine("2: Create New Habit");
            Console.WriteLine("-------------------------------");
        }

        private void HandleMainMenuChoice()
        {
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "0":
                    isRunning = false;
                    break;

                case "1":
                    SelectExistingHabit();
                    break;

                case "2":
                    CreateNewHabit();
                    break;

                default:
                    Console.WriteLine("Oops! Incorrect choice action");
                    break;
            }

            isReturnMainMenu = false;
        }

        private void SelectExistingHabit()
        {
            var habits = db.GetAllHabits();

            if (habits.Count == 0)
            {
                Console.WriteLine("Oops! You don't have any habits yet.");
                return;
            }

            for (int i = 0; i < habits.Count; i++)
                Console.WriteLine($"{i + 1}. {habits[i].Name}");

            Console.WriteLine("Select habit number:");
            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > habits.Count)
            {
                Console.WriteLine("Oops! Incorrect habit number");
                return;
            }

            var selected = habits[choice - 1];
            currentHabitId = selected.Id;
            currentHabitName = selected.Name;

            Console.WriteLine($"Selected habit: {currentHabitName}");
        }

        private void CreateNewHabit()
        {
            Console.Write("Enter habit name: ");
            string? name = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Oops! Incorrect name of the habit");
                return;
            }

            currentHabitId = db.CreateHabit(name);
            currentHabitName = name;
            Console.WriteLine("The new habit has been created");
        }

        private void ShowHabitMenu()
        {
            Console.WriteLine($"HABIT MENU ({currentHabitName})");
            Console.WriteLine("0: Close App");
            Console.WriteLine("1: Return To Main Menu");
            Console.WriteLine("2: View All Records");
            Console.WriteLine("3: Insert Record");
            Console.WriteLine("4: Delete Record");
            Console.WriteLine("5: Update Record");
            Console.WriteLine("-------------------------------");
        }

        private void HandleHabitMenuChoice()
        {
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "0":
                    isRunning = false;
                    break;
                case "1":
                    isReturnMainMenu = true;
                    break;
                case "2":
                    db.ReadAllRecords(currentHabitId!.Value);
                    break;
                case "3":
                    InsertRecord();
                    break;
                case "4":
                    DeleteRecord();
                    break;
                case "5":
                    UpdateRecord();
                    break;
                default:
                    Console.WriteLine("Oops! Incorrect input");
                    break;
            }
        }

        private void InsertRecord()
        {
            Console.Write("Set today's date? (yes/no): ");
            string? setToday = Console.ReadLine()?.ToLower();

            string date;
            if (setToday == "yes")
            {
                date = DateTime.Now.ToString("dd.MM.yyyy");
            }
            else if (setToday == "no")
            {
                Console.Write("Enter the date (dd.MM.yyyy): ");
                date = Console.ReadLine()!;
                if (!DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _))
                {
                    Console.WriteLine("Oops! Incorrect date");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Incorrect input.");
                return;
            }

            Console.Write("Enter count: ");
            string? inputCount = Console.ReadLine();
            if (!int.TryParse(inputCount, out int count))
            {
                Console.WriteLine("Oops! Incorrect count");
                return;
            }

            db.InsertRecord(currentHabitId!.Value, date, count);
            Console.WriteLine("The new record has been added");
        }

        private void DeleteRecord()
        {
            var records = db.GetRecords(currentHabitId!.Value);
            if (records.Count == 0)
            {
                Console.WriteLine("No records to delete.");
                return;
            }

            Console.WriteLine("Existing records:");
            foreach (var r in records)
                Console.WriteLine($"ID: {r.Id}, Date: {r.Date}, Count: {r.Count}");

            Console.Write("Enter ID of record to delete: ");
            string? inputId = Console.ReadLine();
            if (!int.TryParse(inputId, out int id))
            {
                Console.WriteLine("Oops! Incorrect ID");
                return;
            }

            if (!db.RecordExists(id))
            {
                Console.WriteLine("Oops! Record with this ID does not exist.");
                return;
            }

            db.DeleteRecord(id);
            Console.WriteLine("Record deleted");
        }

        private void UpdateRecord()
        {
            var records = db.GetRecords(currentHabitId!.Value);
            if (records.Count == 0)
            {
                Console.WriteLine("No records to update.");
                return;
            }

            Console.WriteLine("Existing records:");
            foreach (var r in records)
                Console.WriteLine($"ID: {r.Id}, Date: {r.Date}, Count: {r.Count}");

            Console.Write("Enter ID of record to update: ");
            string? inputId = Console.ReadLine();
            if (!int.TryParse(inputId, out int id))
            {
                Console.WriteLine("Oops! Incorrect ID");
                return;
            }

            if (!db.RecordExists(id))
            {
                Console.WriteLine("Oops! Record with this ID does not exist.");
                return;
            }

            Console.Write("Enter new count: ");
            string? inputCount = Console.ReadLine();
            if (!int.TryParse(inputCount, out int newCount))
            {
                Console.WriteLine("Oops! Incorrect count");
                return;
            }

            db.UpdateRecord(id, newCount);
            Console.WriteLine("Record updated");
        }

    }
}
