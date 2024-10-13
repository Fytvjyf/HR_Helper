using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;

namespace HR_Helper
{
    public class Processing
    {
        public string FullPath_DB { get; private set; }

        public Processing(string db_path)
        {
            FullPath_DB = db_path;
            if (string.IsNullOrEmpty(FullPath_DB)
             || !File.Exists(FullPath_DB))
            {
                throw new Exception("Указан несуществующий путь к базе данных.");
            }
        }

        public void AddNewVacancies(List<Vacancy> vacancies)
        {
            if (vacancies == null) return;

            if (!CreateVacanciesTableIfNotExists())
                throw new Exception("Не удалось создать таблицу 'Vacancies'.");

            foreach (Vacancy vacancy in vacancies)
            {
                if (vacancy == null) continue;

                string command_text = @$"
                INSERT INTO Vacancies 
                    (DepartmentId, Name, Salary, NeedTest, TestId)
                VALUES
                    ({vacancy.DepartmentId},
                    {vacancy.Name},
                    {vacancy.Salary},
                    {vacancy.NeedTest},
                    {vacancy.TestId})
                ;";
                if (!ExecuteInputCommand(command_text))
                    throw new Exception(
                        $"Не удалось добавить кортеж '{vacancy}' в таблицу 'Vacancies'.");
            }
        }

        public void ChangeExistingVacancies(List<Vacancy> vacancies)
        {
            if (vacancies == null) return;

            foreach (Vacancy vacancy in vacancies)
            {
                if (vacancy == null) continue;

                string command_text = @$"
                UPDATE Vacancies
                SET DepartmentId = {vacancy.DepartmentId},
                    Name = {vacancy.Name},
                    Salary = {vacancy.Salary},
                    NeedTest = {vacancy.NeedTest},
                    TestId = {vacancy.TestId}
                WHERE Id = {vacancy.Id}
                ;";
                if (!ExecuteInputCommand(command_text))
                    throw new Exception(
                        $"Не удалось обновить '{vacancy}' в таблице 'Vacancies'.");
            }
        }

        public void AddNewApplicants(List<Applicant> applicants)
        {
            if (applicants == null) return;

            if (!CreateApplicantsTableIfNotExists())
                throw new Exception("Не удалось создать таблицу 'Applicants'.");

            foreach (Applicant applicant in applicants)
            {
                if (applicant == null) continue;

                string command_text = @$"
                INSERT INTO Applicants 
                    (FullName, 
                    Residence, 
                    PhoneNumber, 
                    Email, 
                    Desctiprion, 
                    DesiredSalary, 
                    VacancyId,
                    HiringStatus)
                VALUES
                    ({applicant.FullName},
                    {applicant.Residence},
                    {applicant.PhoneNumber},
                    {applicant.Email},
                    {applicant.Desctiprion},
                    {applicant.DesiredSalary},
                    {applicant.VacancyId},
                    {applicant.HiringStatus})
                ;";
                if (!ExecuteInputCommand(command_text))
                    throw new Exception(
                        $"Не удалось добавить кортеж '{applicant}' в таблицу 'Vacancies'.");
            }
        }

        public void ChangeExistingApplicants(List<Applicant> applicants)
        {
            if (applicants == null) return;

            foreach (Applicant applicant in applicants)
            {
                if (applicant == null) continue;

                string command_text = @$"
                UPDATE Applicants
                SET FullName = {applicant.FullName}, 
                    Residence = {applicant.Residence}, 
                    PhoneNumber = {applicant.PhoneNumber}, 
                    Email = {applicant.Email}, 
                    Desctiprion = {applicant.Desctiprion}, 
                    DesiredSalary = {applicant.DesiredSalary}, 
                    VacancyId = {applicant.VacancyId},
                    HiringStatus = {applicant.HiringStatus}
                WHERE Id = {applicant.Id}
                ;";
                if (!ExecuteInputCommand(command_text))
                    throw new Exception(
                        $"Не удалось добавить обновить '{applicant}' в таблице 'Vacancies'.");
            }
        }

        public List<Vacancy>? GetVacancies()
        {
            SqliteConnection connection = OpenConnection();
            if (connection == null) return null;

            List<Vacancy>? result = [];

            string commnd_text = $@"
            SELECT DepartmentId, 
                   Name, 
                   Salary, 
                   NeedTest, 
                   TestId
            FROM Vacancies
            ;";
            SqliteCommand command = new(commnd_text, connection);
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int.TryParse(reader.GetValue(0).ToString(), out int department_id);
                    string? name = reader.GetValue(1).ToString();
                    double.TryParse(reader.GetValue(2).ToString(), out double salary);
                    string? need_test = reader.GetValue(3).ToString();
                    int.TryParse(reader.GetValue(4).ToString(), out int test_id);

                    result.Add(
                        new(department_id,
                            name,
                            salary,
                            need_test,
                            test_id));
                }
            }
            catch (Exception e)
            {
                result = null;
                throw new Exception(e.Message);
            }

            connection.Close();

            return result;
        }

        public List<Applicant>? GetApplicants()
        {
            SqliteConnection connection = OpenConnection();
            if (connection == null) return null;

            List<Applicant>? result = [];

            string commnd_text = $@"
            SELECT FullName, 
                   Residence, 
                   PhoneNumber, 
                   Email, 
                   Desctiprion, 
                   DesiredSalary, 
                   VacancyId,
                   HiringStatus
            FROM Applicants
            ;";
            SqliteCommand command = new(commnd_text, connection);
            try
            {
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string? full_name = reader.GetValue(0).ToString();
                    string? residence = reader.GetValue(1).ToString();
                    string? phone = reader.GetValue(2).ToString();
                    string? email = reader.GetValue(3).ToString();
                    string? description = reader.GetValue(4).ToString();
                    double.TryParse(reader.GetValue(5).ToString(), out double desired_salary);
                    int.TryParse(reader.GetValue(6).ToString(), out int vacancy_id);
                    string? hiring_status = reader.GetValue(7).ToString();

                    result.Add(
                        new(full_name,
                        residence,
                        phone,
                        email,
                        description,
                        desired_salary,
                        vacancy_id,
                        hiring_status));
                }
            }
            catch (Exception e)
            {
                result = null;
                throw new Exception(e.Message);
            }

            connection.Close();
            return result;
        }

        // TODO 
        // Сделать get'ы с фильтрами

        private bool CreateVacanciesTableIfNotExists()
        {
            string command_text = @"
            CREATE TABLE IF NOT EXISTS Vacancies(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                DepartmentId INTEGER
                Name TEXT,
                Salary REAL,
                NeedTest TEXT,
                TestId INTEGER
                );
            ;";
            return ExecuteInputCommand(command_text);
        }

        private bool CreateApplicantsTableIfNotExists()
        {
            string command_text = @"
            CREATE TABLE IF NOT EXISTS Applicants(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT,
                Residence TEXT,
                PhoneNumber TEXT,
                Email TEXT,
                Desctiprion TEXT,
                DesiredSalary REAL,
                VacancyId INTEGER,
                HiringStatus TEXT
                );
            ;";
            return ExecuteInputCommand(command_text);
        }

        private bool ExecuteInputCommand(string command_text)
        {
            SqliteConnection connection = OpenConnection();
            if (connection == null) return false;

            bool successed_execute = true;
            SqliteCommand command = new(command_text, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch
            {
                successed_execute = false;
            }
            connection.Close();
            return successed_execute;
        }

        private SqliteConnection OpenConnection()
        {
            SqliteConnection connection = null;
            try
            {
                connection = new(FullPath_DB);
                connection.Open();
            }
            catch
            {
                throw new Exception("Не удалось подключиться к базе данных по пути:" +
                    $"/n{FullPath_DB}");
            }
            return connection;
        }
    }
}
