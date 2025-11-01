using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using lab11.Models;

namespace lab11.Services
{
    public class DataService
    {
        private readonly string _dataFilePath;
        private PawnShopDatabase _database;

        public DataService(string dataFilePath = "pawnshop_data.json")
        {
            _dataFilePath = dataFilePath;
            LoadData();
        }

        // Загрузка данных из файла с обработкой исключений
        public void LoadData()
        {
            try
            {
                if (File.Exists(_dataFilePath))
                {
                    try
                    {
                        string json = File.ReadAllText(_dataFilePath);
                        _database = JsonSerializer.Deserialize<PawnShopDatabase>(json) ?? new PawnShopDatabase();
                    }
                    catch (JsonException ex)
                    {
                        MessageBox.Show($"Ошибка при чтении файла данных: {ex.Message}\nСоздан новый набор данных.", 
                            "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _database = new PawnShopDatabase();
                        InitializeTestData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Неизвестная ошибка при загрузке данных: {ex.Message}", 
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _database = new PawnShopDatabase();
                        InitializeTestData();
                    }
                }
                else
                {
                    _database = new PawnShopDatabase();
                    InitializeTestData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка при инициализации базы данных: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _database = new PawnShopDatabase();
            }
        }

        // Сохранение данных в файл с обработкой исключений
        public void SaveData()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_database, options);
                File.WriteAllText(_dataFilePath, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show($"Нет доступа для записи в файл: {ex.Message}", 
                    "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Ошибка ввода-вывода при сохранении: {ex.Message}", 
                    "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка при сохранении данных: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Получение всех клиентов
        public List<Client> GetAllClients()
        {
            try
            {
                return _database.Clients ?? new List<Client>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении списка клиентов: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Client>();
            }
        }

        // Получение всех залоговых предметов
        public List<PledgeItem> GetAllPledgeItems()
        {
            try
            {
                return _database.PledgeItems ?? new List<PledgeItem>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении списка залоговых предметов: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<PledgeItem>();
            }
        }

        // Получение всех займов
        public List<Loan> GetAllLoans()
        {
            try
            {
                return _database.Loans ?? new List<Loan>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении списка займов: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Loan>();
            }
        }

        // Получение всех платежей
        public List<Payment> GetAllPayments()
        {
            try
            {
                return _database.Payments ?? new List<Payment>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении списка платежей: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<Payment>();
            }
        }

        // Получение клиента по ID
        public Client? GetClientById(int id)
        {
            try
            {
                return _database.Clients.Find(c => c.Id == id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске клиента: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Получение залогового предмета по ID
        public PledgeItem? GetPledgeItemById(int id)
        {
            try
            {
                return _database.PledgeItems.Find(p => p.Id == id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске залогового предмета: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        // Добавление клиента с валидацией
        public void AddClient(Client client)
        {
            try
            {
                client.Validate();
                _database.Clients.Add(client);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка валидации клиента: {ex.Message}", 
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Добавление залогового предмета с валидацией
        public void AddPledgeItem(PledgeItem item)
        {
            try
            {
                item.Validate();
                _database.PledgeItems.Add(item);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка валидации залогового имущества: {ex.Message}", 
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении залогового имущества: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Добавление займа с валидацией
        public void AddLoan(Loan loan)
        {
            try
            {
                loan.Validate();
                _database.Loans.Add(loan);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка валидации займа: {ex.Message}", 
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении займа: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Добавление платежа с валидацией
        public void AddPayment(Payment payment)
        {
            try
            {
                payment.Validate();
                _database.Payments.Add(payment);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка валидации платежа: {ex.Message}", 
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении платежа: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Удаление клиента
        public void RemoveClient(int id)
        {
            try
            {
                var client = _database.Clients.Find(c => c.Id == id);
                if (client != null)
                {
                    _database.Clients.Remove(client);
                }
                else
                {
                    throw new ArgumentException($"Клиент с ID {id} не найден");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Удаление залогового предмета
        public void RemovePledgeItem(int id)
        {
            try
            {
                var item = _database.PledgeItems.Find(p => p.Id == id);
                if (item != null)
                {
                    _database.PledgeItems.Remove(item);
                }
                else
                {
                    throw new ArgumentException($"Залоговое имущество с ID {id} не найдено");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении залогового имущества: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Удаление займа
        public void RemoveLoan(int id)
        {
            try
            {
                var loan = _database.Loans.Find(l => l.Id == id);
                if (loan != null)
                {
                    _database.Loans.Remove(loan);
                }
                else
                {
                    throw new ArgumentException($"Займ с ID {id} не найден");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении займа: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Удаление платежа
        public void RemovePayment(int id)
        {
            try
            {
                var payment = _database.Payments.Find(p => p.Id == id);
                if (payment != null)
                {
                    _database.Payments.Remove(payment);
                }
                else
                {
                    throw new ArgumentException($"Платеж с ID {id} не найден");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении платежа: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        // Инициализация тестовых данных
        private void InitializeTestData()
        {
            try
            {
                _database.Clients = new List<Client>
                {
                    new Client
                    {
                        Id = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович",
                        PassportNumber = "4512 123456", Phone = "+7 (912) 345-67-89",
                        Address = "г. Москва, ул. Ленина, д. 10", RegistrationDate = new DateTime(2023, 1, 15)
                    }
                };

                _database.PledgeItems = new List<PledgeItem>
                {
                    new PledgeItem
                    {
                        Id = 1, Name = "Золотое кольцо 585 пробы", Category = "Ювелирные изделия",
                        Description = "Обручальное кольцо, вес 3.5г", EstimatedValue = 15000, Condition = "Отличное"
                    }
                };

                _database.Loans = new List<Loan>();
                _database.Payments = new List<Payment>();

                SaveData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации тестовых данных: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
