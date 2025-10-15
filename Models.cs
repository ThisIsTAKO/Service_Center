using System;
using System.Collections.Generic;

namespace Lab678.Models
{
    // Модель клиента
    public class Client
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PassportNumber { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}";
    }

    // Модель залогового имущества
    public class PledgeItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal EstimatedValue { get; set; }
        public string Condition { get; set; }
    }

    // Модель займа
    public class Loan
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PledgeItemId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } // Активный, Погашен, Просрочен
        public decimal PaidAmount { get; set; }
    }

    // Модель платежа
    public class Payment
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; } // Основной долг, Проценты
    }

    // Контейнер для всех данных
    public class PawnShopDatabase
    {
        public List<Client> Clients { get; set; }
        public List<PledgeItem> PledgeItems { get; set; }
        public List<Loan> Loans { get; set; }
        public List<Payment> Payments { get; set; }

        public PawnShopDatabase()
        {
            Clients = new List<Client>();
            PledgeItems = new List<PledgeItem>();
            Loans = new List<Loan>();
            Payments = new List<Payment>();
        }
    }
}
