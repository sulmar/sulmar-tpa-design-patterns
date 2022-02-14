using Newtonsoft.Json;
using System.Linq;

namespace StrategyPattern
{

    public class ContractSalary
    {
        public decimal GrossMonthlySalary { get; set; }
        public PPKType PPKType { get; set; }

        public decimal EmployeeRate { get; set; }
        public decimal EmployerRate { get; set; }
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
        Linear
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

    public class ContractSalaryCalculator
    {
        public decimal SalaryResult(ContractSalary salary)
        {
            decimal result = salary.GrossMonthlySalary;

            if (salary.PPKType == PPKType.Lack)
            {
                
            }
            else
            if (salary.PPKType == PPKType.Standard)
            {

            }
            else
            if (salary.PPKType == PPKType.Individual)
            {
                result = result * salary.EmployeeRate * salary.EmployerRate;
            }

            return result;
        }
    }


}
