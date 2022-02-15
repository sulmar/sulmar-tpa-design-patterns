using System;

namespace DecoratorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Employee employee = new OvertimeSalaryDecorator(TimeSpan.FromHours(4), 
            //                        new BonusSalaryDecorator(
            //                             new BonusSalaryDecorator(
            //                                    new Junior())));

            //Employee employee = new BonusSalaryDecorator(
            //                        new BonusSalaryDecorator(
            //                             new OvertimeSalaryDecorator(TimeSpan.FromHours(4),
            //                                    new Junior())));

            Employee employee = EmployeeFactory.Create("PL");

            // employee = new OvertimeSalaryDecorator(TimeSpan.FromHours(2), employee);

            decimal salary = employee.GetSalary();
        }
    }


    public class EmployeeFactory 
    {
        public static Employee Create(string countryCode)
        {
            switch (countryCode)
            {
                case "PL": return CreatePLEmployee();
                case "US": return CreateUSEmployee();

                default: throw new NotSupportedException();
            }
        }

        private static Employee CreateUSEmployee()
        {
            return new OvertimeSalaryDecorator(TimeSpan.FromHours(4),
                            new BonusSalaryDecorator(
                                 new BonusSalaryDecorator(
                                        new Junior())));
        }

        private static Employee CreatePLEmployee()
        {
            return new BonusSalaryDecorator(
                                          new BonusSalaryDecorator(
                                               new OvertimeSalaryDecorator(TimeSpan.FromHours(4),
                                                      new Junior())));
        }
    }

    // Abstract Component
    public abstract class Employee
    {
        public abstract decimal GetSalary();
    }

    // Concrete Component
    public class Junior : Employee
    {
        public override decimal GetSalary()
        {
            return 1000;
        }
    }

    // Concrete Component
    public class Senior : Employee
    {
        public override decimal GetSalary()
        {
            return 2000;
        }
    }

    // Abstract Decorator
    public abstract class SalaryDecorator : Employee
    {
        protected Employee employee;

        protected SalaryDecorator(Employee employee)
        {
            this.employee = employee;
        }

        public override decimal GetSalary()
        {
            if (employee != null)
            {
                return employee.GetSalary();
            }
            else
                return decimal.Zero;
        }
    }

    // Concrete Decorator
    public class BonusSalaryDecorator : SalaryDecorator
    {
        private readonly decimal amount = 100;

        public BonusSalaryDecorator(Employee employee) : base(employee)
        {
        }

        public override decimal GetSalary()
        {
            return base.GetSalary() + amount;
        }
    }

    public class OvertimeSalaryDecorator : SalaryDecorator
    {
        public TimeSpan Duration { get; }
        public decimal AmountPerHour { get; } = 150;

        public OvertimeSalaryDecorator(TimeSpan duration, Employee employee) : base(employee)
        {
            Duration = duration;
        }        

        public override decimal GetSalary()
        {
            return base.GetSalary() + Duration.Hours * AmountPerHour;
        }
    }

    /*
    public class JuniorBonus : Junior
    {
        public override decimal GetSalary()
        {
            return base.GetSalary() + 100;
        }
    }

    public class JuniorDoubleBonus : JuniorBonus
    {
        public override decimal GetSalary()
        {
            return base.GetSalary() + 100;
        }
    }

    public class SeniorBonus : Senior
    {
        public override decimal GetSalary()
        {
            return base.GetSalary() + 500;
        }
    }

    */


}
