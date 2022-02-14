using System;

namespace FactoryMethodPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Factory Method Pattern!");

            VisitCalculatorTest();

            VisitCalculateAmountTest();


            // PaymentTest();
        }

        private static void VisitCalculateAmountTest()
        {
            VisitFactory visitFactory = new VisitFactory();

            while (true)
            {
                Console.Write("Podaj rodzaj wizyty: (N)FZ (P)rywatna (F)irma: (T)eleporada: ");
                string visitType = Console.ReadLine();

                Console.Write("Podaj czas wizyty w minutach: ");
                if (double.TryParse(Console.ReadLine(), out double minutes))
                {
                    TimeSpan duration = TimeSpan.FromMinutes(minutes);

                    Visit visit = visitFactory.Create(visitType, duration, 100m);

                    decimal totalAmount = visit.CalculateCost();

                    Console.ForegroundColor = ConsoleColorFactory.Create(totalAmount);

                    Console.WriteLine($"Total amount {totalAmount:C2}");

                    Console.ResetColor();
                }
            }

        }

        private static void VisitCalculatorTest()
        {
            IVisitCalculatorFactory visitCalculatorFactory = new VisitCalculatorFactoryA();

            while (true)
            {
                Console.Write("Podaj rodzaj wizyty: (N)FZ (P)rywatna (F)irma: (T)eleporada: ");
                string visitType = Console.ReadLine();

                IVisitCalculator visitCalculator = visitCalculatorFactory.Create(visitType);

                Console.Write("Podaj czas wizyty w minutach: ");
                if (double.TryParse(Console.ReadLine(), out double minutes))
                {
                    TimeSpan duration = TimeSpan.FromMinutes(minutes);
                    Visit2 visit = Visit2.Create(duration, 100m);

                    decimal totalAmount = visitCalculator.CalculateCost(visit);

                    Console.ForegroundColor = ConsoleColorFactory.Create(totalAmount);

                    Console.WriteLine($"Total amount {totalAmount:C2}");

                    Console.ResetColor();
                }
            }

        }

        private static void PaymentTest()
        {
            while (true)
            {
                Console.Write("Podaj kwotę: ");

                decimal.TryParse(Console.ReadLine(), out decimal totalAmount);

                Console.Write("Wybierz rodzaj płatności: (G)otówka (K)karta płatnicza (P)rzelew: ");

                var paymentType = Enum.Parse<PaymentType>(Console.ReadLine());

                Payment payment = new Payment(paymentType, totalAmount);

                if (payment.PaymentType == PaymentType.Cash)
                {
                    CashPaymentView cashPaymentView = new CashPaymentView();
                    cashPaymentView.Show(payment);
                }
                else
                if (payment.PaymentType == PaymentType.CreditCard)
                {
                    CreditCardPaymentView creditCardView = new CreditCardPaymentView();
                    creditCardView.Show(payment);
                }
                else
                if (payment.PaymentType == PaymentType.BankTransfer)
                {
                    BankTransferPaymentView bankTransferPaymentView = new BankTransferPaymentView();
                    bankTransferPaymentView.Show(payment);
                }

                string icon = GetIcon(payment);
                Console.WriteLine(icon);
            }



        }

        private static string GetIcon(Payment payment)
        {
            switch (payment.PaymentType)
            {
                case PaymentType.Cash: return "[100]";
                case PaymentType.CreditCard: return "[abc]";
                case PaymentType.BankTransfer: return "[-->]";

                default: return string.Empty;
            }
        }
    }

    

    // Factory {Product}Factory
    public class ConsoleColorFactory
    {
        // {Product}
        public static ConsoleColor Create(decimal amount)
        {
            switch (amount)
            {
                case 0:
                    return ConsoleColor.Blue;
                case >= 200:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.White;
            }
        }
    }

    public class Visit2
    {
        public DateTime VisitDate { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal PricePerHour { get; set; }

        protected Visit2(TimeSpan duration, decimal pricePerHour)
        {
            VisitDate = DateTime.Now;
            Duration = duration;
            PricePerHour = pricePerHour;
        }

        public static Visit2 Create(TimeSpan duration, decimal pricePerHour)
        {
            return new Visit2(duration, pricePerHour);
        }

    }

    public interface IVisitCalculator
    {
        decimal CalculateCost(Visit2 visit);
    }

    public abstract class VisitCalculator : IVisitCalculator
    {
        public virtual decimal CalculateCost(Visit2 visit)
        {
            return (decimal)visit.Duration.TotalHours * visit.PricePerHour;
        }
    }

    public class NFZVisitCalculator : VisitCalculator, IVisitCalculator
    {
        public override decimal CalculateCost(Visit2 visit)
        {
            return 0;
        }
    }

    public class PrivateVisitCalculator : VisitCalculator, IVisitCalculator
    {

    }

    public class CompanyVisitCalculator : VisitCalculator, IVisitCalculator
    {
        private const decimal companyDiscountPercentage = 0.9m;
        public override decimal CalculateCost(Visit2 visit)
        {
            return base.CalculateCost(visit) * companyDiscountPercentage;
        }
    }



    #region Visit

    // Abstract Factory
    public interface IVisitCalculatorFactory
    {
        VisitCalculator Create(string kind);
    }

    // Concrete Factory
    public class VisitCalculatorFactoryA : IVisitCalculatorFactory
    {
        public VisitCalculator Create(string kind)
        {
            switch (kind)
            {
                case "N": return new NFZVisitCalculator();
                case "P": return new PrivateVisitCalculator();
                case "F": return new CompanyVisitCalculator();                

                default:
                    throw new NotSupportedException();
            }
        }
    }

    // Concrete Factory
    public class VisitCalculatorFactoryB : IVisitCalculatorFactory
    {
        public VisitCalculator Create(string kind)
        {
            switch (kind)
            {
                case "U": return new NFZVisitCalculator();
                case "P": return new PrivateVisitCalculator();
                case "C": return new CompanyVisitCalculator();

                default:
                    throw new NotSupportedException();
            }
        }
    }


    public class VisitFactory
    {
        // Product
        public Visit Create(string kind, TimeSpan duration, decimal pricePerHour)
        {
            switch(kind)
            {
                case "N": return new NFZVisit(duration);
                case "P": return new PrivateVisit(duration, pricePerHour);
                case "F": return new CompanyVisit(duration, pricePerHour);
                case "T": return new TeleVisit(duration, pricePerHour);

                default:
                    throw new NotSupportedException();
            }
        }
    }


    public class NFZVisit : Visit
    {
        public NFZVisit(TimeSpan duration) : base(duration, 0)
        {
        }
    }

    public class PrivateVisit : Visit
    {
        public PrivateVisit(TimeSpan duration, decimal pricePerHour) : base(duration, pricePerHour)
        {
        }
    }

    public class CompanyVisit : Visit
    {
        private const decimal companyDiscountPercentage = 0.9m;

        public CompanyVisit(TimeSpan duration, decimal pricePerHour) : base(duration, pricePerHour)
        {
        }

        public override decimal CalculateCost()
        {
            return base.CalculateCost() * companyDiscountPercentage;
        }
    }

    public class TeleVisit : Visit
    {
        public TeleVisit(TimeSpan duration, decimal pricePerHour) : base(duration, pricePerHour)
        {
        }

        public override decimal CalculateCost()
        {
            return 50;
        }
    }

    public abstract class Visit
    {
        public DateTime VisitDate { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal PricePerHour { get; set; }


        public Visit(TimeSpan duration, decimal pricePerHour)
        {
            VisitDate = DateTime.Now;
            Duration = duration;
            PricePerHour = pricePerHour;
        }
        

        // Polimorfizm (wielopostaciowość)
        public virtual decimal CalculateCost()
        {
            return (decimal) Duration.TotalHours * PricePerHour;
        }
    }


    #endregion

    #region Payment

    public class Payment
    {
        public DateTime PaymentDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal TotalAmount { get; set; }

        public Payment(DateTime paymentDate, PaymentType paymentType, decimal totalAmount)
        {
            PaymentDate = paymentDate;
            PaymentType = paymentType;
            TotalAmount = totalAmount;
        }

        public Payment(PaymentType paymentType, decimal totalAmount)
            : this(DateTime.Now, paymentType, totalAmount)
        { }

    }

    public enum PaymentType
    {
        Cash,
        CreditCard,
        BankTransfer
    }

    public abstract class PaymentView
    {
        public abstract void Show(Payment payment);
    }

    public class CashPaymentView : PaymentView
    {
        public override void Show(Payment payment)
        {
            Console.WriteLine($"Do zapłaty {payment.TotalAmount}");
            Console.Write("Otrzymano: ");
            decimal.TryParse(Console.ReadLine(), out decimal receivedAmount);

            decimal restAmount = payment.TotalAmount - receivedAmount;

            if (restAmount > 0)
            {
                Console.WriteLine($"Reszta {restAmount}");
            }
        }
    }

    public class CreditCardPaymentView : PaymentView
    {
        public override void Show(Payment payment)
        {
            Console.WriteLine($"Do zapłaty {payment.TotalAmount}");

            Console.WriteLine($"Nawiązywanie połączenia z bankiem...");

            Console.WriteLine("Transakcja zautoryzowana");
        }
    }


    public class BankTransferPaymentView : PaymentView
    {
        public override void Show(Payment payment)
        {
            Console.WriteLine($"Dane do przelewu {payment.TotalAmount}");
        }
    }

    #endregion
}
