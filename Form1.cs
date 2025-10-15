using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Lab678.Services;
using Lab678.Models;
using Lab678.Forms;

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
        private Panel panelButtons;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnChangeTableColor;

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
            this.BackColor = Color.FromArgb(52, 73, 94);

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

            // Панель кнопок
            panelButtons = new Panel();
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Height = 60;
            panelButtons.BackColor = Color.FromArgb(236, 240, 241);

            btnAdd = new Button();
            btnAdd.Text = "Добавить";
            btnAdd.Size = new Size(120, 40);
            btnAdd.Location = new Point(10, 10);
            btnAdd.BackColor = Color.FromArgb(46, 204, 113);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnAdd.Click += BtnAdd_Click;
            panelButtons.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Text = "Изменить";
            btnEdit.Size = new Size(120, 40);
            btnEdit.Location = new Point(140, 10);
            btnEdit.BackColor = Color.FromArgb(52, 152, 219);
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnEdit.Click += BtnEdit_Click;
            panelButtons.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Text = "Удалить";
            btnDelete.Size = new Size(120, 40);
            btnDelete.Location = new Point(270, 10);
            btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnDelete.Click += BtnDelete_Click;
            panelButtons.Controls.Add(btnDelete);

            btnSave = new Button();
            btnSave.Text = "Сохранить данные";
            btnSave.Size = new Size(150, 40);
            btnSave.Location = new Point(400, 10);
            btnSave.BackColor = Color.FromArgb(155, 89, 182);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSave.Click += BtnSave_Click;
            panelButtons.Controls.Add(btnSave);

            btnChangeTableColor = new Button();
            btnChangeTableColor.Text = "Изменить цвет";
            btnChangeTableColor.Size = new Size(150, 40);
            btnChangeTableColor.Location = new Point(560, 10);
            btnChangeTableColor.BackColor = Color.FromArgb(52, 152, 219);
            btnChangeTableColor.ForeColor = Color.White;
            btnChangeTableColor.FlatStyle = FlatStyle.Flat;
            btnChangeTableColor.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnChangeTableColor.Click += BtnChangeTableColor_Click;
            panelButtons.Controls.Add(btnChangeTableColor);

            // TabControl для разных таблиц
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 10);
            tabControl.BackColor = Color.FromArgb(52, 73, 94);

            this.Controls.Add(tabControl);
            this.Controls.Add(panelButtons);
            this.Controls.Add(panelHeader);

            // Вкладка "Клиенты"
            TabPage tabClients = new TabPage("Клиенты");
            tabClients.BackColor = Color.FromArgb(52, 73, 94);
            dgvClients = CreateDataGridView();
            tabClients.Controls.Add(dgvClients);
            tabControl.TabPages.Add(tabClients);

            // Вкладка "Залоговое имущество"
            TabPage tabPledgeItems = new TabPage("Залоговое имущество");
            tabPledgeItems.BackColor = Color.FromArgb(52, 73, 94);
            dgvPledgeItems = CreateDataGridView();
            tabPledgeItems.Controls.Add(dgvPledgeItems);
            tabControl.TabPages.Add(tabPledgeItems);

            // Вкладка "Займы"
            TabPage tabLoans = new TabPage("Займы");
            tabLoans.BackColor = Color.FromArgb(52, 73, 94);
            dgvLoans = CreateDataGridView();
            tabLoans.Controls.Add(dgvLoans);
            tabControl.TabPages.Add(tabLoans);

            // Вкладка "Платежи"
            TabPage tabPayments = new TabPage("Платежи");
            tabPayments.BackColor = Color.FromArgb(52, 73, 94);
            dgvPayments = CreateDataGridView();
            tabPayments.Controls.Add(dgvPayments);
            tabControl.TabPages.Add(tabPayments);
        }

        private DataGridView CreateDataGridView()
        {
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = false;
            dgv.AllowUserToAddRows = true;
            dgv.AllowUserToDeleteRows = true;
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

        private DataGridView GetCurrentDataGridView()
        {
            if (tabControl.SelectedIndex == 0) return dgvClients;
            if (tabControl.SelectedIndex == 1) return dgvPledgeItems;
            if (tabControl.SelectedIndex == 2) return dgvLoans;
            if (tabControl.SelectedIndex == 3) return dgvPayments;
            return null;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int tabIndex = tabControl.SelectedIndex;
                
                switch (tabIndex)
                {
                    case 0: // Клиенты
                        var clientForm = new ClientForm();
                        if (clientForm.ShowDialog() == DialogResult.OK)
                        {
                            var newClient = clientForm.Client;
                            newClient.Id = GetNextClientId();
                            _dataService.AddClient(newClient);
                            _dataService.SaveData();
                            LoadClients();
                            MessageBox.Show("Клиент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                        
                    case 1: // Залоговое имущество
                        var pledgeItemForm = new PledgeItemForm();
                        if (pledgeItemForm.ShowDialog() == DialogResult.OK)
                        {
                            var newPledgeItem = pledgeItemForm.PledgeItem;
                            newPledgeItem.Id = GetNextPledgeItemId();
                            _dataService.AddPledgeItem(newPledgeItem);
                            _dataService.SaveData();
                            LoadPledgeItems();
                            MessageBox.Show("Залоговое имущество успешно добавлено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                        
                    case 2: // Займы
                        var loanForm = new LoanForm(_dataService);
                        if (loanForm.ShowDialog() == DialogResult.OK)
                        {
                            var newLoan = loanForm.Loan;
                            newLoan.Id = GetNextLoanId();
                            _dataService.AddLoan(newLoan);
                            _dataService.SaveData();
                            LoadLoans();
                            MessageBox.Show("Займ успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                        
                    case 3: // Платежи
                        var paymentForm = new PaymentForm(_dataService);
                        if (paymentForm.ShowDialog() == DialogResult.OK)
                        {
                            var newPayment = paymentForm.Payment;
                            newPayment.Id = GetNextPaymentId();
                            _dataService.AddPayment(newPayment);
                            _dataService.SaveData();
                            LoadPayments();
                            MessageBox.Show("Платеж успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var dgv = GetCurrentDataGridView();
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для редактирования", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                int tabIndex = tabControl.SelectedIndex;
                var selectedRow = dgv.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                
                switch (tabIndex)
                {
                    case 0: // Клиенты
                        var client = _dataService.GetClientById(id);
                        if (client != null)
                        {
                            var clientForm = new ClientForm(client);
                            if (clientForm.ShowDialog() == DialogResult.OK)
                            {
                                _dataService.SaveData();
                                LoadClients();
                                MessageBox.Show("Данные клиента успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                        
                    case 1: // Залоговое имущество
                        var pledgeItem = _dataService.GetPledgeItemById(id);
                        if (pledgeItem != null)
                        {
                            var pledgeItemForm = new PledgeItemForm(pledgeItem);
                            if (pledgeItemForm.ShowDialog() == DialogResult.OK)
                            {
                                _dataService.SaveData();
                                LoadPledgeItems();
                                MessageBox.Show("Данные залогового имущества успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                        
                    case 2: // Займы
                        var loan = _dataService.GetAllLoans().FirstOrDefault(l => l.Id == id);
                        if (loan != null)
                        {
                            var loanForm = new LoanForm(_dataService, loan);
                            if (loanForm.ShowDialog() == DialogResult.OK)
                            {
                                _dataService.SaveData();
                                LoadLoans();
                                MessageBox.Show("Данные займа успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                        
                    case 3: // Платежи
                        var payment = _dataService.GetAllPayments().FirstOrDefault(p => p.Id == id);
                        if (payment != null)
                        {
                            var paymentForm = new PaymentForm(_dataService, payment);
                            if (paymentForm.ShowDialog() == DialogResult.OK)
                            {
                                _dataService.SaveData();
                                LoadPayments();
                                MessageBox.Show("Данные платежа успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var dgv = GetCurrentDataGridView();
            if (dgv == null || dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;
            
            try
            {
                int tabIndex = tabControl.SelectedIndex;
                var selectedRow = dgv.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                
                switch (tabIndex)
                {
                    case 0: // Клиенты
                        _dataService.RemoveClient(id);
                        _dataService.SaveData();
                        LoadClients();
                        MessageBox.Show("Клиент успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        
                    case 1: // Залоговое имущество
                        _dataService.RemovePledgeItem(id);
                        _dataService.SaveData();
                        LoadPledgeItems();
                        MessageBox.Show("Залоговое имущество успешно удалено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        
                    case 2: // Займы
                        _dataService.RemoveLoan(id);
                        _dataService.SaveData();
                        LoadLoans();
                        MessageBox.Show("Займ успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        
                    case 3: // Платежи
                        _dataService.RemovePayment(id);
                        _dataService.SaveData();
                        LoadPayments();
                        MessageBox.Show("Платеж успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveGridDataToService();
                _dataService.SaveData();
                MessageBox.Show("Данные успешно сохранены в файл!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveGridDataToService()
        {
            // Простое сохранение - данные уже в памяти через DataService
            // Для полной реализации нужно синхронизировать DataGridView с объектами
        }

        private void BtnChangeTableColor_Click(object sender, EventArgs e)
        {
            var dgv = GetCurrentDataGridView();
            if (dgv == null)
            {
                MessageBox.Show("Выберите вкладку с таблицей", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.Color = dgv.DefaultCellStyle.BackColor;
            
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Изменить цвет фона ячеек
                dgv.DefaultCellStyle.BackColor = colorDialog.Color;
                
                // Изменить цвет чередующихся строк
                dgv.AlternatingRowsDefaultCellStyle.BackColor = AdjustColor(colorDialog.Color, -15);
                
                // Изменить цвет фона области вокруг таблицы
                dgv.BackgroundColor = colorDialog.Color;
            }
        }
        
        private Color AdjustColor(Color color, int amount)
        {
            int r = Math.Max(0, Math.Min(255, color.R + amount));
            int g = Math.Max(0, Math.Min(255, color.G + amount));
            int b = Math.Max(0, Math.Min(255, color.B + amount));
            return Color.FromArgb(r, g, b);
        }
        
        // Методы для генерации новых ID
        private int GetNextClientId()
        {
            var clients = _dataService.GetAllClients();
            return clients.Any() ? clients.Max(c => c.Id) + 1 : 1;
        }
        
        private int GetNextPledgeItemId()
        {
            var items = _dataService.GetAllPledgeItems();
            return items.Any() ? items.Max(p => p.Id) + 1 : 1;
        }
        
        private int GetNextLoanId()
        {
            var loans = _dataService.GetAllLoans();
            return loans.Any() ? loans.Max(l => l.Id) + 1 : 1;
        }
        
        private int GetNextPaymentId()
        {
            var payments = _dataService.GetAllPayments();
            return payments.Any() ? payments.Max(p => p.Id) + 1 : 1;
        }
    }
}
