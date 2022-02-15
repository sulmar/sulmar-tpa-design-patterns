using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StrategyPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Strategy Pattern!");

            HappyHoursPercentageOrderCalculatorTest();

            HappyHoursOrderCalculatorTest();

            GenderOrderCalculatorTest();

            //   B2BSalaryCalculatorTest();

            // ContractSalaryCalculatorTest();
        }

        private static void ContractSalaryCalculatorTest()
        {
            ContractSalary contractSalary = new ContractSalary { GrossMonthlySalary = 2000 };

            IPPKStrategy strategy = PPKStrategyFactory.Create(PPKType.Standard);
            ITaxationStrategy taxationStrategy = new LinearTaxationStrategy();

            ContractSalaryCalculator salaryCalculator = new ContractSalaryCalculator(strategy, taxationStrategy);

            var result = salaryCalculator.SalaryResult(contractSalary);

            Console.WriteLine(result);

        }

        private static void B2BSalaryCalculatorTest()
        {
            B2BSalaryCalculator salaryCalculator = new B2BSalaryCalculator();

            B2BSalary salary = new B2BSalary { GrossMonthlySalary = 2000, TaxationMethod = TaxationMethod.Linear };

            var result = salaryCalculator.SalaryResult(salary, true);

            Console.WriteLine(result);
        }

        private static void HappyHoursPercentageOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);

            ICanDiscountStrategy canDiscountStrategy = new HappyHoursCanDiscountStrategy(TimeSpan.Parse("08:30"), TimeSpan.Parse("15:00"));
            ICalculateDiscountStrategy calculateDiscountStrategy = new PercentageCalculateDiscountStrategy(0.1m);

            order.CanDiscountStrategy = canDiscountStrategy;
            order.DiscountStrategy = calculateDiscountStrategy;

            OrderCalculator orderCalculator = new OrderCalculator();
            decimal discount = orderCalculator.CalculateDiscount(order);
            
            string canDiscountParameters = JsonConvert.SerializeObject(order.CanDiscountStrategy, Formatting.Indented);
            string discountParameters = JsonConvert.SerializeObject(order.DiscountStrategy, Formatting.Indented);

            Console.WriteLine(canDiscountParameters);
            Console.WriteLine(discountParameters);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }


        private static void HappyHoursOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);

            HappyHoursPercentageOrderCalculator calculator = new HappyHoursPercentageOrderCalculator(TimeSpan.Parse("08:30"), TimeSpan.Parse("15:00"), 0.1m);
            decimal discount = calculator.CalculateDiscount(order);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }

        private static void GenderOrderCalculatorTest()
        {
            Customer customer = new Customer("Anna", "Kowalska");

            Order order = CreateOrder(customer);

            PercentageGenderOrderCalculator calculator = new PercentageGenderOrderCalculator(Gender.Female, 0.2m);
            decimal discount = calculator.CalculateDiscount(order);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }

        private static Order CreateOrder(Customer customer)
        {
            Product product1 = new Product(1, "Książka C#", unitPrice: 100m);
            Product product2 = new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m);
            Product product3 = new Product(3, "Zakładka do książki", unitPrice: 10m);

            Order order = new Order(DateTime.Parse("2020-06-12 14:59"), customer);
            order.AddDetail(product1);
            order.AddDetail(product2);
            order.AddDetail(product3, 5);

            return order;
        }
    }


    public interface IDiscountStrategy
    {
        bool CanDiscount(Order order);
        decimal CalculateDiscount(Order order);
    }

    public abstract class AbstractDiscountStrategy
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to;
        }
    }

    // Fixed, Percentage, Gratis

    public class HappyHoursPercentageDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(Order order)
        {
            throw new NotImplementedException();
        }

        public bool CanDiscount(Order order)
        {
            throw new NotImplementedException();
        }
    }

    public class HappyHoursFixedDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(Order order)
        {
            throw new NotImplementedException();
        }

        public bool CanDiscount(Order order)
        {
            throw new NotImplementedException();
        }
    }

    public class GenderPercentageDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscount(Order order)
        {
            throw new NotImplementedException();
        }

        public bool CanDiscount(Order order)
        {
            throw new NotImplementedException();
        }
    }

   


    public interface ICanDiscountStrategy
    {
        bool CanDiscount(Order order);
    }

    public interface ICalculateDiscountStrategy
    {
        decimal CalculateDiscount(Order order);
    }

    public class HappyHoursCanDiscountStrategy : ICanDiscountStrategy
    {
        public TimeSpan From { get; }
        public TimeSpan To { get; }

        public HappyHoursCanDiscountStrategy(TimeSpan from, TimeSpan to)
        {
            this.From = from;
            this.To = to;
        }

        public bool CanDiscount(Order order)
        {
            return order.OrderDate.TimeOfDay >= From && order.OrderDate.TimeOfDay <= To;
        }
    }

    public class GenderCanDiscountStrategy : ICanDiscountStrategy
    {
        public Gender Gender { get; }

        public GenderCanDiscountStrategy(Gender gender)
        {
            this.Gender = gender;
        }

        public bool CanDiscount(Order order)
        {
            return order.Customer.Gender == Gender;
        }
    }

    public class FixedCalculateDiscountStrategy : ICalculateDiscountStrategy
    {
        private readonly decimal discount;

        public FixedCalculateDiscountStrategy(decimal discount)
        {
            this.discount = discount;
        }

        public decimal CalculateDiscount(Order order)
        {
            return discount;
        }
    }

    public class PercentageCalculateDiscountStrategy : ICalculateDiscountStrategy
    {
        public decimal Percentage { get; }

        public PercentageCalculateDiscountStrategy(decimal percentage)
        {
            this.Percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            return order.Amount * Percentage;
        }
    }

    public class OrderCalculator
    {
        //private readonly ICanDiscountStrategy canDiscountStrategy;
        //private readonly ICalculateDiscountStrategy calculateDiscountStrategy;

        public OrderCalculator()
        {
            //this.canDiscountStrategy = canDiscountStrategy;
            //this.calculateDiscountStrategy = calculateDiscountStrategy;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order.CanDiscountStrategy.CanDiscount(order)) // Predykat - warunek upustu
            {
                return order.DiscountStrategy.CalculateDiscount(order);                                     // Obliczanie wartości upustu
            }
            else
                return decimal.Zero;
        }
    }


    // Happy Hours - 10% upustu w godzinach od 9 do 15
    public class HappyHoursPercentageOrderCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;

        private readonly decimal percentage;

        public HappyHoursPercentageOrderCalculator(TimeSpan from, TimeSpan to, decimal percentage)
        {
            this.from = from;
            this.to = to;
            this.percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to) // Predykat - warunek upustu
            {
                return order.Amount * percentage;                                     // Obliczanie wartości upustu
            }
            else
                return decimal.Zero;
        }
    }

    public class HappyHoursFixedOrderCalculator
    {
        private readonly TimeSpan from;
        private readonly TimeSpan to;

        private readonly decimal discount;

        public HappyHoursFixedOrderCalculator(TimeSpan from, TimeSpan to, decimal discount)
        {
            this.from = from;
            this.to = to;
            this.discount = discount;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order.OrderDate.TimeOfDay >= from && order.OrderDate.TimeOfDay <= to)
            {
                return discount;
            }
            else
                return decimal.Zero;
        }

    }


    // Gender - 20% upustu dla kobiet
    public class PercentageGenderOrderCalculator
    {
        private readonly Gender gender;
        private readonly decimal percentage;

        public PercentageGenderOrderCalculator(Gender gender, decimal percentage)
        {
            this.gender = gender;
            this.percentage = percentage;
        }

        public decimal CalculateDiscount(Order order)
        {
            if (order.Customer.Gender == gender)
            {
                return order.Amount * percentage;
            }
            else
                return decimal.Zero;
        }
    }

    



    #region Models

    public class Order
    {
        public DateTime OrderDate { get; set; }
        public Customer Customer { get; set; }
        public decimal Amount => Details.Sum(p => p.LineTotal);

        public ICollection<OrderDetail> Details = new Collection<OrderDetail>();

        public void AddDetail(Product product, int quantity = 1)
        {
            OrderDetail detail = new OrderDetail(product, quantity);

            this.Details.Add(detail);
        }

        public Order(DateTime orderDate, Customer customer)
        {
            OrderDate = orderDate;
            Customer = customer;
        }

        public ICanDiscountStrategy CanDiscountStrategy { get; set; }
        public ICalculateDiscountStrategy DiscountStrategy { get; set; }



    }

    public class Product
    {
        public Product(int id, string name, decimal unitPrice)
        {
            Id = id;
            Name = name;
            UnitPrice = unitPrice;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class OrderDetail
    {
        public OrderDetail(Product product, int quantity = 1)
        {
            Product = product;
            Quantity = quantity;

            UnitPrice = product.UnitPrice;
        }

        public int Id { get; set; }
        public Product Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }

    public class Customer
    {
        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;

            if (firstName.EndsWith("a"))
            {
                Gender = Gender.Female;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

    }

    public enum Gender
    {
        Male,
        Female
    }

    #endregion
}


