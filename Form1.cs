using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Lab678.Services;
using Lab678.Models;

namespace Lab678
{
    public partial class Form1 : Form
    {
        private DataService _dataService;
        private TabControl tabControl;
        private DataGridView dgvClients;
        private DataGridView dgvPledgeItems;
        private DataGridView dgvLoans;
        private DataGridView dgvPayments;
        private Label lblTitle;
        private Panel panelHeader;

        public Form1()
        {
            InitializeComponent();
            _dataService = new DataService();
            SetupCustomUI();
            LoadAllData();
        }

        private void SetupCustomUI()
        {
            // Настройки формы
            this.Text = "ИС Ломбард - Управление базой данных";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Панель заголовка
            panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 80;
            panelHeader.BackColor = Color.FromArgb(52, 73, 94);

            // Заголовок
            lblTitle = new Label();
            lblTitle.Text = "Информационная система\n\"ЛОМБАРД\"";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = false;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Dock = DockStyle.Fill;
            panelHeader.Controls.Add(lblTitle);

            // TabControl для разных таблиц
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10);

            this.Controls.Add(tabControl);
            this.Controls.Add(panelHeader);

            // Вкладка "Клиенты"
            TabPage tabClients = new TabPage("Клиенты");
            dgvClients = CreateDataGridView();
            tabClients.Controls.Add(dgvClients);
            tabControl.TabPages.Add(tabClients);

            // Вкладка "Залоговое имущество"
            TabPage tabPledgeItems = new TabPage("Залоговое имущество");
            dgvPledgeItems = CreateDataGridView();
            tabPledgeItems.Controls.Add(dgvPledgeItems);
            tabControl.TabPages.Add(tabPledgeItems);

            // Вкладка "Займы"
            TabPage tabLoans = new TabPage("Займы");
            dgvLoans = CreateDataGridView();
            tabLoans.Controls.Add(dgvLoans);
            tabControl.TabPages.Add(tabLoans);

            // Вкладка "Платежи"
            TabPage tabPayments = new TabPage("Платежи");
            dgvPayments = CreateDataGridView();
            tabPayments.Controls.Add(dgvPayments);
            tabControl.TabPages.Add(tabPayments);
        }

        private DataGridView CreateDataGridView()
        {
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 35;
            dgv.RowTemplate.Height = 30;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }

        private void LoadAllData()
        {
            LoadClients();
            LoadPledgeItems();
            LoadLoans();
            LoadPayments();
        }

        private void LoadClients()
        {
            var clients = _dataService.GetAllClients();
            var displayData = clients.Select(c => new
            {
                ID = c.Id,
                Фамилия = c.LastName,
                Имя = c.FirstName,
                Отчество = c.MiddleName,
                Паспорт = c.PassportNumber,
                Телефон = c.Phone,
                Адрес = c.Address,
                ДатаРегистрации = c.RegistrationDate.ToString("dd.MM.yyyy")
            }).ToList();

            dgvClients.DataSource = displayData;
        }

        private void LoadPledgeItems()
        {
            var items = _dataService.GetAllPledgeItems();
            var displayData = items.Select(p => new
            {
                ID = p.Id,
                Наименование = p.Name,
                Категория = p.Category,
                Описание = p.Description,
                ОценочнаяСтоимость = p.EstimatedValue.ToString("N0") + " ₽",
                Состояние = p.Condition
            }).ToList();

            dgvPledgeItems.DataSource = displayData;
        }

        private void LoadLoans()
        {
            var loans = _dataService.GetAllLoans();
            var displayData = loans.Select(l => new
            {
                ID = l.Id,
                Клиент = _dataService.GetClientById(l.ClientId)?.FullName ?? "Неизвестен",
                ЗалоговыйПредмет = _dataService.GetPledgeItemById(l.PledgeItemId)?.Name ?? "Неизвестен",
                СуммаЗайма = l.LoanAmount.ToString("N0") + " ₽",
                Процент = l.InterestRate.ToString("N1") + "%",
                ДатаВыдачи = l.IssueDate.ToString("dd.MM.yyyy"),
                СрокПогашения = l.DueDate.ToString("dd.MM.yyyy"),
                Статус = l.Status,
                УплаченнаяСумма = l.PaidAmount.ToString("N0") + " ₽",
                Остаток = (l.LoanAmount - l.PaidAmount).ToString("N0") + " ₽"
            }).ToList();

            dgvLoans.DataSource = displayData;
            
            // Цветовое выделение по статусу
            for (int i = 0; i < dgvLoans.Rows.Count; i++)
            {
                string status = loans[i].Status;
                if (status == "Активный")
                {
                    dgvLoans.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (status == "Просрочен")
                {
                    dgvLoans.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else if (status == "Погашен")
                {
                    dgvLoans.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }

        private void LoadPayments()
        {
            var payments = _dataService.GetAllPayments();
            var loans = _dataService.GetAllLoans();
            
            var displayData = payments.Select(p => new
            {
                ID = p.Id,
                НомерЗайма = p.LoanId,
                Клиент = _dataService.GetClientById(
                    loans.FirstOrDefault(l => l.Id == p.LoanId)?.ClientId ?? 0)?.FullName ?? "Неизвестен",
                ДатаПлатежа = p.PaymentDate.ToString("dd.MM.yyyy"),
                Сумма = p.Amount.ToString("N0") + " ₽",
                ТипПлатежа = p.PaymentType
            }).ToList();

            dgvPayments.DataSource = displayData;
        }
    }
}
