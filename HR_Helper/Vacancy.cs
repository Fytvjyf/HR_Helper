namespace HR_Helper
{
    public class Vacancy
    {
        public int Id { get; private set; }
        public int DepartmentId { get; private set; } = -1;
        public string? Name { get; private set; } = null;
        public double Salary { get; private set; } = 0;
        // Строкой, т.к. в sqlite нет булевых значений.
        public string? NeedTest { get; private set; } = "false";
        public int TestId { get; private set; } = -1;

        public Vacancy() { }
        public Vacancy(int depatrment_id, string? name, double salary, 
            bool need_test, int test_id)
        {
            DepartmentId = depatrment_id;
            Name = name;
            Salary = salary;
            NeedTest = need_test ? "true" : "false";
            TestId = test_id;
        }
        public Vacancy(int depatrment_id, string? name, double salary, 
            string? need_test, int test_id)
        {
            DepartmentId = depatrment_id;
            Name = name;
            Salary = salary;
            NeedTest = need_test;
            TestId = test_id;
        }

        public override string ToString()
            => $"Вакансия {Name}. ID: {Id}";
    }
}
