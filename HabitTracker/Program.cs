using System.Text.RegularExpressions;

namespace HabitTracker
{
    internal class Program
    {
        public bool IsCorrectDate(string date)
        {
            return DateTime.TryParseExact(date, "dd.mm.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime parseDate);
        }

        static void Main()
        {
            var db = new Database();
            var p = new Program();
            bool isRunning = true;
            bool isReturnMainMenu = false;
            string nameCurrentTable = "";
            while (isRunning == true)
            {
                if (nameCurrentTable == "" || isReturnMainMenu == true)
                {
                    Console.WriteLine("MAIN MENU");
                    Console.WriteLine("Type 0 to Close App");
                    Console.WriteLine("Type 1 to Change Existing Habit");
                    Console.WriteLine("Type 2 to Create New Habit");
                    Console.WriteLine("-------------------------------");

                    string? choiceTable = Console.ReadLine();

                    switch (choiceTable)
                    {
                        case "0":
                            isRunning = false;
                            break;
                        case "1":
                            var namesTables = db.TakeAllNameTable();
                            if (namesTables.Count == 0)
                            {
                                Console.WriteLine("Oops! You don't have any habits yet.");
                                Console.WriteLine("-------------------------------");
                                continue;
                            } else
                            {
                                for (int i = 0; i < namesTables.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {namesTables[i]}");
                                }
                                Console.WriteLine("-------------------------------");
                                Console.WriteLine("Select the habit number:");
                                string? numberTable = Console.ReadLine();
                                if (numberTable == null)
                                {
                                    Console.WriteLine("Oops! Incorrect habit number");
                                    Console.WriteLine("-------------------------------");
                                    continue;
                                }
                                if (int.Parse(numberTable) - 1 <= namesTables.Count - 1)
                                {
                                    nameCurrentTable = namesTables[int.Parse(numberTable) - 1];
                                } else
                                {
                                    Console.WriteLine("Oops! Incorrect habit number");
                                    Console.WriteLine("-------------------------------");
                                    continue;
                                }
                            }
                            break;
                        case "2":
                            Console.WriteLine("Enter the name of the new habit");
                            string? inputCurrentTable = Console.ReadLine();
                            if (inputCurrentTable == null)
                            {
                                Console.WriteLine("Oops! Incorrect name of the habit");
                                Console.WriteLine("-------------------------------");
                                continue;
                            }
                            nameCurrentTable = inputCurrentTable.ToLower();
                            db.CreateTable(nameCurrentTable);
                            Console.WriteLine("The new habit has been created");
                            Console.WriteLine("-------------------------------");
                            break;
                        default:
                            Console.WriteLine("Oops! Incorrect choice action");
                            Console.WriteLine("-------------------------------");
                            continue;
                    }
                    isReturnMainMenu = false;
                }

                if (isRunning == false)
                {
                    break;
                }

                Console.WriteLine("-------------------------------");
                Console.WriteLine("HABIT MENU");
                Console.WriteLine("Type 0 to Close App");
                Console.WriteLine("Type 1 to Return To Main Menu");
                Console.WriteLine("Type 2 to View All Records");
                Console.WriteLine("Type 3 to Insert Record");
                Console.WriteLine("Type 4 to Delete Record");
                Console.WriteLine("Type 5 to Update Record");
                Console.WriteLine("-------------------------------");

                string? choiceAction = Console.ReadLine();

                switch (choiceAction)
                {
                    case "0":
                        isRunning = false;
                        break;
                    case "1":
                        isReturnMainMenu = true;
                        break;
                    case "2":
                        db.ReadTable(nameCurrentTable);
                        break;
                    case "3":
                        Console.WriteLine("Do you want to set today's date? ");
                        string? isSetTodayDate = Console.ReadLine();
                        if (isSetTodayDate == null)
                        {
                            Console.WriteLine("Incorrect input. Please, enter 'yes' or 'no'.");
                            Console.WriteLine("-------------------------------");
                            continue;
                        }
                        string? dateHabit = "";
                        switch (isSetTodayDate.ToLower())
                        {
                            case "yes":
                                dateHabit = (DateOnly.FromDateTime(DateTime.Now)).ToString();
                                break;
                            case "no":
                                Console.WriteLine("Enter the date (in format dd.mm.yyyy): ");
                                dateHabit = Console.ReadLine();
                                if (dateHabit == null)
                                {
                                    Console.WriteLine("Oops! Incorrect date");
                                    Console.WriteLine("-------------------------------");
                                    continue;
                                }
                                if (!p.IsCorrectDate(dateHabit))
                                {
                                    Console.WriteLine("Oops! Incorrect date");
                                    Console.WriteLine("-------------------------------");
                                    continue;
                                }
                                break;
                            default:
                                Console.WriteLine("Incorrect input. Please, enter 'yes' or 'no'.");
                                Console.WriteLine("-------------------------------");
                                break;
                        }
                        Console.WriteLine("Enter the count:");
                        string? stringCountHabit = Console.ReadLine();
                        if (stringCountHabit == null || Int32.TryParse(stringCountHabit, out int countHabit))
                        {
                            Console.WriteLine("Oops! Incorrect count");
                            Console.WriteLine("-------------------------------");
                            continue;
                        }
                        if (dateHabit == "" || dateHabit == null)
                        {
                            Console.WriteLine("Oops! Incorrect date");
                            Console.WriteLine("-------------------------------");
                            continue;
                        }
                        db.InsertRecord(nameCurrentTable, dateHabit, countHabit);
                        break;
                    case "4":
                        var recordsHabit = db.TakeAllRecords(nameCurrentTable);
                        if (recordsHabit.Count != 0)
                        {
                            foreach (List<string> recordHabit in recordsHabit)
                            {
                                Console.WriteLine($"ID: {recordHabit[0]}, Date: {recordHabit[1]}, Count: {recordHabit[2]}");
                            }
                            Console.WriteLine("Enter the ID of the record you want to delete:");
                            string? idRecord = Console.ReadLine();
                            if (idRecord == null || !int.TryParse(idRecord, out int idCurrentTable) || int.Parse(idRecord) <= recordsHabit.Count - 1)
                            {
                                Console.WriteLine("Oops! Incorrect ID of the record");
                                Console.WriteLine("-------------------------------");
                                continue;
                            }
                            db.DeleteRecord(nameCurrentTable, idCurrentTable);
                        } else
                        {
                            Console.WriteLine("Oops! You don't have any records yet.");
                            Console.WriteLine("-------------------------------");
                            continue;
                        }
                            break;
                    case "5":
                        recordsHabit = db.TakeAllRecords(nameCurrentTable);
                        if (recordsHabit.Count != 0)
                        {
                            foreach (List<string> recordHabit in recordsHabit)
                            {
                                Console.WriteLine($"ID: {recordHabit[0]}, Date: {recordHabit[1]}, Count: {recordHabit[2]}");
                            }
                            Console.WriteLine("Enter the ID of the record you want to update:");
                            string? idRecord = Console.ReadLine();
                            if (idRecord == null || !int.TryParse(idRecord, out int idCurrentTable) || int.Parse(idRecord) <= recordsHabit.Count - 1)
                            {
                                Console.WriteLine("Oops! Incorrect ID of the record");
                                Console.WriteLine("-------------------------------");
                                continue;
                            }
                            Console.WriteLine("Enter a new count:");
                            string? newCountHabit = Console.ReadLine();
                            if (newCountHabit == null || !int.TryParse(newCountHabit, out int newCountCurrentHabit))
                            {
                                Console.WriteLine("Oops! Incorrect a count of the record");
                                Console.WriteLine("-------------------------------");
                                continue;
                            }
                            db.UpdateRecord(nameCurrentTable, idCurrentTable, newCountCurrentHabit);
                        } else
                        {
                            Console.WriteLine("Oops! You don't have any records yet.");
                            Console.WriteLine("-------------------------------");
                            continue;
                        }
                            break;
                    default:
                        Console.WriteLine("Oops! Incorrecr input");
                        Console.WriteLine("-------------------------------");
                        continue;
                }
            }
        }
    }
}
