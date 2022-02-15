using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyPattern
{

    public class ContractSalary
    {
        public decimal GrossMonthlySalary { get; set; }
    }

    public class B2BSalary
    {
        public decimal GrossMonthlySalary { get; set; }
        public TaxationMethod TaxationMethod { get; set; }

    }

    public class CalculationResult
    {
        public CalculationResultDetail[] Details { get; private set; }

        public CalculationResult()
        {
            Details = CreateDetails();
        }

        private static CalculationResultDetail[] CreateDetails()
        {
            return Enumerable.Range(1, 12)
                .Select(month => new CalculationResultDetail { Month = (Month) month })
                .ToArray();
        }

        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public class CalculationResultDetail
    {
        public Month Month { get; set; }
        public decimal MiesieczneWynagrodzenie { get; set; }
        public decimal UbezpieczenieSpoleczne { get; set; }
        public decimal KosztyOperacyjne { get; set; }
        public decimal UbezpieczenieZdrowotne { get; set; }
        public decimal UlgaDlaKlasySredniej { get; set; }

        

    }

    public enum TaxationMethod
    {
        Scale,
        Linear,
        Flat
    }

   
    public enum PPKType
    {   
        Lack,
        Standard,
        Individual
    }

    public class B2BSalaryCalculator
    {
        public CalculationResult SalaryResult(B2BSalary salary, bool polishOrder)
        {
            CalculationResult result = new CalculationResult();

            foreach (var detail in result.Details)
            {
                detail.MiesieczneWynagrodzenie = salary.GrossMonthlySalary;

                if (polishOrder)
                {

                    if (salary.TaxationMethod == TaxationMethod.Scale)
                    {

                    }
                    else if (salary.TaxationMethod == TaxationMethod.Linear)
                    {

                    }
                }
                else
                {
                    if (salary.TaxationMethod == TaxationMethod.Scale)
                    {

                    }
                    else if (salary.TaxationMethod == TaxationMethod.Linear)
                    {

                    }
                }
            }

            return result;
        }
    }

    // Abstract strategy
    public interface IPPKStrategy
    {
        decimal Calculate(decimal salary);
    }


    public interface ITaxationStrategy
    {
        decimal CalculateTax(decimal salary);
    }

    public class LinearTaxationStrategy : ITaxationStrategy
    {
        private readonly decimal rate = 0.19m;

        public decimal CalculateTax(decimal salary)
        {
            return salary * rate;
        }
    }

    public class ScaleTaxationStrategy : ITaxationStrategy
    {
        private readonly IDictionary<decimal, decimal> thresholds = new Dictionary<decimal, decimal>
        {
            { 1000, 0.19m },
            { 2000, 0.29m },
            { 3000, 0.39m },
        };
       
        public decimal CalculateTax(decimal salary)
        {
            if (thresholds.TryGetValue(salary, out decimal rate))
            {
                return salary * rate;
            }
            else
            {
                return 0;
            }
        }
    }

    public class FlatTaxationStrategy : ITaxationStrategy
    {
        public decimal CalculateTax(decimal salary)
        {
            return 100;
        }
    }

    public class LackPPKStrategy : IPPKStrategy
    {
        public decimal Calculate(decimal salary)
        {
            return 0;
        }
    }

    public class StandardPPKStrategy : IPPKStrategy
    {
        public decimal Calculate(decimal salary)
        {
            return salary;
        }
    }

    public class IndividualPPKStrategy : IPPKStrategy
    {
        public IndividualPPKStrategy(decimal employeeRate, decimal employerRate)
        {
            EmployeeRate = employeeRate;
            EmployerRate = employerRate;
        }

        public decimal EmployeeRate { get; }
        public decimal EmployerRate { get; }

        public decimal Calculate(decimal salary)
        {
            return salary * EmployeeRate * EmployerRate;
        }
    }

    public class ContractSalaryCalculator
    {
        private readonly IPPKStrategy pPKStrategy;
        private readonly ITaxationStrategy taxationStrategy;

        public ContractSalaryCalculator(IPPKStrategy pPKStrategy, ITaxationStrategy taxationStrategy)
        {
            this.pPKStrategy = pPKStrategy;
            this.taxationStrategy = taxationStrategy;
        }

        public decimal SalaryResult(ContractSalary salary)
        {
            decimal result = salary.GrossMonthlySalary;

            // 1. .. ....

            // 2. .. ....

            // 3. .. 
            result = pPKStrategy.Calculate(salary.GrossMonthlySalary);

            // 4. ... ...

            // 5. .. ...

            result = taxationStrategy.CalculateTax(salary.GrossMonthlySalary);


            return result;
        }
    }

    public class PPKStrategyFactory
    {
        public static IPPKStrategy Create(PPKType pPKType)
        {
            switch(pPKType)
            {
                case PPKType.Lack: return new LackPPKStrategy();
                case PPKType.Standard: return new StandardPPKStrategy();
                case PPKType.Individual: return new IndividualPPKStrategy(0.1m, 0.2m);

                default: throw new NotSupportedException();
            }
        }
    }


}
