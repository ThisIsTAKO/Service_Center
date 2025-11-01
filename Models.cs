using System;
using System.Collections.Generic;

namespace lab11.Models
{
    // Модель клиента с валидацией
    public class Client
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }

        public string FullName => $"{LastName} {FirstName} {MiddleName}";

        // Валидация данных клиента
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                throw new ArgumentException("Фамилия не может быть пустой");
            
            if (string.IsNullOrWhiteSpace(FirstName))
                throw new ArgumentException("Имя не может быть пустым");
            
            if (string.IsNullOrWhiteSpace(PassportNumber))
                throw new ArgumentException("Номер паспорта не может быть пустым");
            
            if (PassportNumber.Length < 10)
                throw new ArgumentException("Номер паспорта должен содержать минимум 10 символов");
            
            if (string.IsNullOrWhiteSpace(Phone))
                throw new ArgumentException("Телефон не может быть пустым");
        }
    }

    // Модель залогового имущества с валидацией
    public class PledgeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal EstimatedValue { get; set; }
        public string Condition { get; set; } = string.Empty;

        // Валидация данных залогового имущества
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Наименование не может быть пустым");
            
            if (string.IsNullOrWhiteSpace(Category))
                throw new ArgumentException("Категория не может быть пустой");
            
            if (EstimatedValue <= 0)
                throw new ArgumentException("Оценочная стоимость должна быть положительной");
            
            if (string.IsNullOrWhiteSpace(Condition))
                throw new ArgumentException("Состояние не может быть пустым");
        }
    }

    // Модель займа с валидацией
    public class Loan
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PledgeItemId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Активный";
        public decimal PaidAmount { get; set; }

        // Валидация данных займа
        public void Validate()
        {
            if (ClientId <= 0)
                throw new ArgumentException("Необходимо выбрать клиента");
            
            if (PledgeItemId <= 0)
                throw new ArgumentException("Необходимо выбрать залоговое имущество");
            
            if (LoanAmount <= 0)
                throw new ArgumentException("Сумма займа должна быть положительной");
            
            if (InterestRate < 0 || InterestRate > 100)
                throw new ArgumentException("Процентная ставка должна быть от 0 до 100");
            
            if (DueDate <= IssueDate)
                throw new ArgumentException("Срок погашения должен быть позже даты выдачи");
            
            if (PaidAmount < 0)
                throw new ArgumentException("Уплаченная сумма не может быть отрицательной");
            
            if (PaidAmount > LoanAmount)
                throw new ArgumentException("Уплаченная сумма не может превышать сумму займа");
        }
    }

    // Модель платежа с валидацией
    public class Payment
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; } = "Основной долг";

        // Валидация данных платежа
        public void Validate()
        {
            if (LoanId <= 0)
                throw new ArgumentException("Необходимо выбрать займ");
            
            if (Amount <= 0)
                throw new ArgumentException("Сумма платежа должна быть положительной");
            
            if (string.IsNullOrWhiteSpace(PaymentType))
                throw new ArgumentException("Тип платежа не может быть пустым");
        }
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
