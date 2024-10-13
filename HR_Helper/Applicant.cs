using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HR_Helper
{
    public class Applicant
    {
        public int Id { get; private set; } = -1;
        public string? FullName { get; private set; } = "DefaultPerson";
        public string? Residence { get; private set; }
        public string? PhoneNumber { get; private set; } = null;
        public string? Email { get; private set; } = null;
        public string? Desctiprion { get; private set; } = null;
        public double DesiredSalary { get; private set; } = 0;
        public int VacancyId { get; private set; } = -1;
        public string? HiringStatus { get; private set; } = "Get resume";

        public Applicant() { }
        public Applicant(string? full_name, string? residence, string? phone, 
            string? email, string? description, double desired_salary, 
            int vacancy_id, string? hiring_status)
        {
            FullName = full_name;
            Residence = residence;
            PhoneNumber = phone;
            Email = email;
            Desctiprion = description;
            DesiredSalary = desired_salary;
            VacancyId = vacancy_id;
            HiringStatus = hiring_status;
        }

        public override string ToString()
            => $"Соискатель {FullName}. ID: {Id}";
    }
}
