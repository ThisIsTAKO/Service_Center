using System;

namespace Lab678.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        
        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }

    public class RepairOrder
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string DeviceType { get; set; } = string.Empty;
        public string DeviceModel { get; set; } = string.Empty;
        public string ProblemDescription { get; set; } = string.Empty;
        public decimal EstimatedCost { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string Status { get; set; } = "Принят";
        public decimal PaidAmount { get; set; }
    }

    public class SparePart
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public int StockQuantity { get; set; }
        public string Supplier { get; set; } = string.Empty;
    }

    public class RepairWork
    {
        public int Id { get; set; }
        public int RepairOrderId { get; set; }
        public string MasterName { get; set; } = string.Empty;
        public string WorkDescription { get; set; } = string.Empty;
        public decimal LaborCost { get; set; }
        public TimeSpan WorkTime { get; set; }
        public DateTime WorkDate { get; set; }
    }

    public class Payment
    {
        public int Id { get; set; }
        public int RepairOrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; } = string.Empty;
    }

    public class ServiceCenterDatabase
    {
        public List<Client> Clients { get; set; } = new List<Client>();
        public List<RepairOrder> RepairOrders { get; set; } = new List<RepairOrder>();
        public List<SparePart> SpareParts { get; set; } = new List<SparePart>();
        public List<RepairWork> RepairWorks { get; set; } = new List<RepairWork>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}