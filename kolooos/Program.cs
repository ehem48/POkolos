using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ExampleCSV
{
    internal class Program
    {
        static void Main()
        {
            string filePath = "C:/Users/Łukasz/Desktop/studia/C#/kolooos/file.csv";
            List<Person> people = ReadDataFromCsv(filePath);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Menu główne:");
                Console.WriteLine("1. Wyświetl dane");
                Console.WriteLine("2. Dodaj osobę");
                Console.WriteLine("3. Modyfikuj osobę");
                Console.WriteLine("4. Usuń osobę");
                Console.WriteLine("5. Wyjście z programu");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        DisplayPeople(people);
                        break;
                    case "2":
                        AddPerson(people);
                        WriteDataToCsv(filePath, people);
                        break;
                    case "3":
                        ModifyPerson(people);
                        WriteDataToCsv(filePath, people);
                        break;
                    case "4":
                        RemovePerson(people);
                        WriteDataToCsv(filePath, people);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                        break;
                }
            }
        }

        internal class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public string Pesel { get; set; }
            public string Email { get; set; }
        }

        static List<Person> ReadDataFromCsv(string filePath)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    return csv.GetRecords<Person>().ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd odczytu danych z pliku CSV: {ex.Message}");
                return new List<Person>();
            }
        }

        static void WriteDataToCsv(string filePath, List<Person> people)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(people);
                }
                Console.WriteLine("Dane zapisane do pliku CSV.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd zapisu danych do pliku CSV: {ex.Message}");
            }
        }

        static void DisplayPeople(List<Person> people)
        {
            Console.WriteLine("\nDane odczytane z pliku CSV:");
            foreach (var person in people)
            {
                Console.WriteLine($"Imię: {person.FirstName}, Nazwisko: {person.LastName}, Wiek: {person.Age}, PESEL: {person.Pesel}, Email: {person.Email}");
            }
        }

        static void AddPerson(List<Person> people)
        {
            Person newPerson = new Person();
            try
            {
                Console.Write("Podaj imię: ");
                newPerson.FirstName = Console.ReadLine();

                Console.Write("Podaj nazwisko: ");
                newPerson.LastName = Console.ReadLine();

                Console.Write("Podaj wiek: ");
                newPerson.Age = int.Parse(Console.ReadLine());

                Console.Write("Podaj PESEL: ");
                newPerson.Pesel = ValidatePesel(Console.ReadLine());

                Console.Write("Podaj email: ");
                newPerson.Email = ValidateEmail(Console.ReadLine());

                people.Add(newPerson);
                Console.WriteLine("Osoba dodana do bazy.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Błąd formatu danych. Dodawanie osoby przerwane.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}. Dodawanie osoby przerwane.");
            }
        }

        static void ModifyPerson(List<Person> people)
        {
            Console.Write("Podaj PESEL osoby do modyfikacji: ");
            string pesel = Console.ReadLine();

            Person personToModify = people.FirstOrDefault(p => p.Pesel == pesel);
            if (personToModify != null)
            {
                try
                {
                    Console.Write("Podaj nowe imię: ");
                    personToModify.FirstName = Console.ReadLine();

                    Console.Write("Podaj nowe nazwisko: ");
                    personToModify.LastName = Console.ReadLine();

                    Console.Write("Podaj nowy wiek: ");
                    personToModify.Age = int.Parse(Console.ReadLine());

                    Console.Write("Podaj nowy PESEL: ");
                    personToModify.Pesel = ValidatePesel(Console.ReadLine());

                    Console.Write("Podaj nowy email: ");
                    personToModify.Email = ValidateEmail(Console.ReadLine());

                    Console.WriteLine("Osoba zaktualizowana.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Błąd formatu danych. Modyfikacja przerwana.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd: {ex.Message}. Modyfikacja przerwana.");
                }
            }
            else
            {
                Console.WriteLine("Osoba o podanym PESEL nie istnieje.");
            }
        }

        static void RemovePerson(List<Person> people)
        {
            Console.Write("Podaj PESEL osoby do usunięcia: ");
            string pesel = Console.ReadLine();

            Person personToRemove = people.FirstOrDefault(p => p.Pesel == pesel);
            if (personToRemove != null)
            {
                people.Remove(personToRemove);
                Console.WriteLine("Osoba usunięta z bazy.");
            }
            else
            {
                Console.WriteLine("Osoba o podanym PESEL nie istnieje.");
            }
        }

        static string ValidatePesel(string pesel)
        {
            if (pesel.Length == 11)
            {
                return pesel;
            }
            else
            {
                throw new ArgumentException("PESEL powinien zawierać 11 znaków typu int.");
            }
        }

        static string ValidateEmail(string email)
        {
            if (email.Contains("@"))
            {
                return email;
            }
            else
            {
                throw new ArgumentException("Nieprawidłowy format adresu email.");
            }
        }
    }
}
