using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Data;
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
        private DataGridView dgvRepairOrders;
        private DataGridView dgvSpareParts;
        private DataGridView dgvRepairWorks;
        private DataGridView dgvPayments;
        private Label lblTitle;
        private Panel panelHeader;
        private Panel panelButtons;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnSave;
        private Button btnChangeTableColor;
        private Panel panelFilters;
        private TextBox txtSearch;
        private ComboBox cmbColumnFilter;
        private ComboBox cmbValueFilter;
        private Button btnClearFilters;
        private DataTable clientsTable;
        private DataTable repairOrdersTable;
        private DataTable sparePartsTable;
        private DataTable repairWorksTable;
        private DataTable paymentsTable;
        private string _lastSortColumn = null;
        private bool _sortAsc = true;

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
            this.Text = "ИС Сервисный Центр - Управление базой данных";
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
            lblTitle.Text = "Информационная система\n\"СЕРВИСНЫЙ ЦЕНТР\"";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = false;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Dock = DockStyle.Fill;
            panelHeader.Controls.Add(lblTitle);

            // Панель фильтров и поиска
            panelFilters = new Panel();
            panelFilters.Dock = DockStyle.Top;
            panelFilters.Height = 50;
            panelFilters.BackColor = Color.FromArgb(236, 240, 241);

            Label lblSearch = new Label
            {
                Text = "Поиск:",
                AutoSize = true,
                Location = new Point(10, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            panelFilters.Controls.Add(lblSearch);

            txtSearch = new TextBox
            {
                Location = new Point(70, 12),
                Width = 250,
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.TextChanged += (s, e) => ApplyFilters();
            panelFilters.Controls.Add(txtSearch);

            Label lblColumn = new Label
            {
                Text = "Столбец:",
                AutoSize = true,
                Location = new Point(340, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            panelFilters.Controls.Add(lblColumn);

            cmbColumnFilter = new ComboBox
            {
                Location = new Point(410, 12),
                Width = 200,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbColumnFilter.SelectedIndexChanged += (s, e) => UpdateValueFilterOptions();
            panelFilters.Controls.Add(cmbColumnFilter);

            Label lblValue = new Label
            {
                Text = "Значение:",
                AutoSize = true,
                Location = new Point(620, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };
            panelFilters.Controls.Add(lblValue);

            cmbValueFilter = new ComboBox
            {
                Location = new Point(700, 12),
                Width = 220,
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbValueFilter.SelectedIndexChanged += (s, e) => ApplyFilters();
            panelFilters.Controls.Add(cmbValueFilter);

            btnClearFilters = new Button
            {
                Text = "Сброс",
                Location = new Point(930, 10),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(192, 57, 43),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnClearFilters.Click += (s, e) => { txtSearch.Text = string.Empty; cmbColumnFilter.SelectedIndex = -1; cmbValueFilter.DataSource = null; ApplyFilters(); };
            panelFilters.Controls.Add(btnClearFilters);

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
            tabControl.SelectedIndexChanged += (s, e) => { RefreshFilterControls(); ApplyFilters(); };

            // Добавляем элементы в правильном порядке докинга
            this.Controls.Add(tabControl);
            this.Controls.Add(panelButtons);
            this.Controls.Add(panelFilters);
            this.Controls.Add(panelHeader);

            // Вкладка "Клиенты"
            TabPage tabClients = new TabPage("Клиенты");
            tabClients.BackColor = Color.FromArgb(52, 73, 94);
            dgvClients = CreateDataGridView();
            tabClients.Controls.Add(dgvClients);
            tabControl.TabPages.Add(tabClients);

            // Вкладка "Заказы на ремонт"
            TabPage tabRepairOrders = new TabPage("Заказы на ремонт");
            tabRepairOrders.BackColor = Color.FromArgb(52, 73, 94);
            dgvRepairOrders = CreateDataGridView();
            tabRepairOrders.Controls.Add(dgvRepairOrders);
            tabControl.TabPages.Add(tabRepairOrders);

            // Вкладка "Запасные части"
            TabPage tabSpareParts = new TabPage("Запасные части");
            tabSpareParts.BackColor = Color.FromArgb(52, 73, 94);
            dgvSpareParts = CreateDataGridView();
            tabSpareParts.Controls.Add(dgvSpareParts);
            tabControl.TabPages.Add(tabSpareParts);

            // Вкладка "Ремонтные работы"
            TabPage tabRepairWorks = new TabPage("Ремонтные работы");
            tabRepairWorks.BackColor = Color.FromArgb(52, 73, 94);
            dgvRepairWorks = CreateDataGridView();
            tabRepairWorks.Controls.Add(dgvRepairWorks);
            tabControl.TabPages.Add(tabRepairWorks);

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
            dgv.ColumnHeaderMouseClick += Dgv_ColumnHeaderMouseClick;
            return dgv;
        }

        private void LoadAllData()
        {
            LoadClients();
            LoadRepairOrders();
            LoadSpareParts();
            LoadRepairWorks();
            LoadPayments();
            RefreshFilterControls();
            ApplyFilters();
        }

        private void LoadClients()
        {
            var clients = _dataService.GetAllClients();
            clientsTable = new DataTable();
            clientsTable.Columns.Add("ID", typeof(string));
            clientsTable.Columns.Add("Фамилия", typeof(string));
            clientsTable.Columns.Add("Имя", typeof(string));
            clientsTable.Columns.Add("Отчество", typeof(string));
            clientsTable.Columns.Add("Телефон", typeof(string));
            clientsTable.Columns.Add("Email", typeof(string));
            clientsTable.Columns.Add("Адрес", typeof(string));
            clientsTable.Columns.Add("ДатаРегистрации", typeof(string));
            foreach (var c in clients)
            {
                clientsTable.Rows.Add(
                    c.Id.ToString(),
                    c.LastName,
                    c.FirstName,
                    c.MiddleName,
                    c.Phone,
                    c.Email,
                    c.Address,
                    c.RegistrationDate.ToString("dd.MM.yyyy")
                );
            }
            dgvClients.DataSource = clientsTable;
        }

        private void LoadRepairOrders()
        {
            var orders = _dataService.GetAllRepairOrders();
            repairOrdersTable = new DataTable();
            repairOrdersTable.Columns.Add("ID", typeof(string));
            repairOrdersTable.Columns.Add("Клиент", typeof(string));
            repairOrdersTable.Columns.Add("ТипУстройства", typeof(string));
            repairOrdersTable.Columns.Add("МодельУстройства", typeof(string));
            repairOrdersTable.Columns.Add("ОписаниеПроблемы", typeof(string));
            repairOrdersTable.Columns.Add("ОценочнаяСтоимость", typeof(string));
            repairOrdersTable.Columns.Add("ДатаЗаказа", typeof(string));
            repairOrdersTable.Columns.Add("ДатаЗавершения", typeof(string));
            repairOrdersTable.Columns.Add("Статус", typeof(string));
            repairOrdersTable.Columns.Add("УплаченнаяСумма", typeof(string));
            foreach (var o in orders)
            {
                repairOrdersTable.Rows.Add(
                    o.Id.ToString(),
                    _dataService.GetClientById(o.ClientId)?.FullName ?? "Неизвестен",
                    o.DeviceType,
                    o.DeviceModel,
                    o.ProblemDescription,
                    o.EstimatedCost.ToString("N0") + " ₽",
                    o.OrderDate.ToString("dd.MM.yyyy"),
                    o.CompletionDate?.ToString("dd.MM.yyyy") ?? "",
                    o.Status,
                    o.PaidAmount.ToString("N0") + " ₽"
                );
            }
            dgvRepairOrders.DataSource = repairOrdersTable;

            // Цветовое выделение по статусу
            for (int i = 0; i < dgvRepairOrders.Rows.Count; i++)
            {
                if (i >= orders.Count) break;
                string status = orders[i].Status;
                if (status == "Завершен")
                {
                    dgvRepairOrders.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else if (status == "В работе")
                {
                    dgvRepairOrders.Rows[i].DefaultCellStyle.BackColor = Color.LightYellow;
                }
                else if (status == "Принят")
                {
                    dgvRepairOrders.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                }
                else if (status == "Отменен")
                {
                    dgvRepairOrders.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;
                }
            }
        }

        private void LoadSpareParts()
        {
            var parts = _dataService.GetAllSpareParts();
            sparePartsTable = new DataTable();
            sparePartsTable.Columns.Add("ID", typeof(string));
            sparePartsTable.Columns.Add("Наименование", typeof(string));
            sparePartsTable.Columns.Add("Категория", typeof(string));
            sparePartsTable.Columns.Add("Описание", typeof(string));
            sparePartsTable.Columns.Add("Стоимость", typeof(string));
            sparePartsTable.Columns.Add("Остаток", typeof(string));
            sparePartsTable.Columns.Add("Поставщик", typeof(string));
            foreach (var p in parts)
            {
                sparePartsTable.Rows.Add(
                    p.Id.ToString(),
                    p.Name,
                    p.Category,
                    p.Description,
                    p.Cost.ToString("N0") + " ₽",
                    p.StockQuantity.ToString(),
                    p.Supplier
                );
            }
            dgvSpareParts.DataSource = sparePartsTable;
        }

        private void LoadRepairWorks()
        {
            var works = _dataService.GetAllRepairWorks();
            repairWorksTable = new DataTable();
            repairWorksTable.Columns.Add("ID", typeof(string));
            repairWorksTable.Columns.Add("ЗаказНаРемонт", typeof(string));
            repairWorksTable.Columns.Add("Мастер", typeof(string));
            repairWorksTable.Columns.Add("ОписаниеРаботы", typeof(string));
            repairWorksTable.Columns.Add("СтоимостьТруда", typeof(string));
            repairWorksTable.Columns.Add("ВремяРаботы", typeof(string));
            repairWorksTable.Columns.Add("ДатаРаботы", typeof(string));
            foreach (var w in works)
            {
                var order = _dataService.GetRepairOrderById(w.RepairOrderId);
                repairWorksTable.Rows.Add(
                    w.Id.ToString(),
                    $"Заказ №{w.RepairOrderId} - {order?.DeviceType ?? "Неизвестен"}",
                    w.MasterName,
                    w.WorkDescription,
                    w.LaborCost.ToString("N0") + " ₽",
                    $"{w.WorkTime.Hours}ч {w.WorkTime.Minutes}мин",
                    w.WorkDate.ToString("dd.MM.yyyy")
                );
            }
            dgvRepairWorks.DataSource = repairWorksTable;
        }

        private void LoadPayments()
        {
            var payments = _dataService.GetAllPayments();
            paymentsTable = new DataTable();
            paymentsTable.Columns.Add("ID", typeof(string));
            paymentsTable.Columns.Add("ЗаказНаРемонт", typeof(string));
            paymentsTable.Columns.Add("ДатаПлатежа", typeof(string));
            paymentsTable.Columns.Add("Сумма", typeof(string));
            paymentsTable.Columns.Add("ТипПлатежа", typeof(string));
            foreach (var p in payments)
            {
                var order = _dataService.GetRepairOrderById(p.RepairOrderId);
                paymentsTable.Rows.Add(
                    p.Id.ToString(),
                    $"Заказ №{p.RepairOrderId} - {order?.DeviceType ?? "Неизвестен"}",
                    p.PaymentDate.ToString("dd.MM.yyyy"),
                    p.Amount.ToString("N0") + " ₽",
                    p.PaymentType
                );
            }
            dgvPayments.DataSource = paymentsTable;
        }

        private DataGridView GetCurrentDataGridView()
        {
            if (tabControl.SelectedIndex == 0) return dgvClients;
            if (tabControl.SelectedIndex == 1) return dgvRepairOrders;
            if (tabControl.SelectedIndex == 2) return dgvSpareParts;
            if (tabControl.SelectedIndex == 3) return dgvRepairWorks;
            if (tabControl.SelectedIndex == 4) return dgvPayments;
            return null;
        }

        private DataTable GetCurrentDataTable()
        {
            if (tabControl.SelectedIndex == 0) return clientsTable;
            if (tabControl.SelectedIndex == 1) return repairOrdersTable;
                        if (tabControl.SelectedIndex == 2) return sparePartsTable;
            if (tabControl.SelectedIndex == 3) return repairWorksTable;
            if (tabControl.SelectedIndex == 4) return paymentsTable;
            return null;
        }

        private void Dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var dgv = sender as DataGridView;
            if (dgv == null || dgv.Columns.Count == 0) return;
            var col = dgv.Columns[e.ColumnIndex];
            var table = GetCurrentDataTable();
            if (table == null) return;

            string colName = col.HeaderText;
            if (_lastSortColumn == colName)
                _sortAsc = !_sortAsc;
            else
            {
                _lastSortColumn = colName;
                _sortAsc = true;
            }

            table.DefaultView.Sort = $"[{colName}] {(_sortAsc ? "ASC" : "DESC")}";
        }

        private void RefreshFilterControls()
        {
            var table = GetCurrentDataTable();
            cmbColumnFilter.Items.Clear();
            cmbValueFilter.DataSource = null;
            if (table == null) return;
            foreach (DataColumn col in table.Columns)
            {
                cmbColumnFilter.Items.Add(col.ColumnName);
            }
            cmbColumnFilter.SelectedIndex = -1;
        }

        private void UpdateValueFilterOptions()
        {
            var table = GetCurrentDataTable();
            cmbValueFilter.DataSource = null;
            if (table == null || cmbColumnFilter.SelectedIndex == -1) return;
            string col = cmbColumnFilter.SelectedItem.ToString();
            var values = table.AsEnumerable()
                .Select(r => r[col]?.ToString() ?? string.Empty)
                .Distinct()
                .OrderBy(v => v)
                .ToList();
            values.Insert(0, "(Все)");
            cmbValueFilter.DataSource = values;
            cmbValueFilter.SelectedIndex = 0;
        }

        private void ApplyFilters()
        {
            var table = GetCurrentDataTable();
            if (table == null) return;

            string search = (txtSearch?.Text ?? string.Empty).Trim();
            string filterExpr = string.Empty;

            // Поиск по всем столбцам
            if (!string.IsNullOrEmpty(search))
            {
                string esc = search.Replace("'", "''");
                var colExprs = table.Columns
                    .Cast<DataColumn>()
                    .Select(c => $"Convert([{c.ColumnName}], 'System.String') LIKE '%{esc}%'");
                filterExpr = "(" + string.Join(" OR ", colExprs) + ")";
            }

            // Фильтр по выбранному столбцу/значению
            if (cmbColumnFilter != null && cmbColumnFilter.SelectedIndex != -1 &&
                cmbValueFilter != null && cmbValueFilter.SelectedItem != null &&
                cmbValueFilter.SelectedItem.ToString() != "(Все)")
            {
                string col = cmbColumnFilter.SelectedItem.ToString();
                string val = cmbValueFilter.SelectedItem.ToString().Replace("'", "''");
                string valueExpr = $"([{col}] = '{val}')";
                if (string.IsNullOrEmpty(filterExpr)) filterExpr = valueExpr; else filterExpr += " AND " + valueExpr;
            }

            table.DefaultView.RowFilter = filterExpr;
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
                            RefreshFilterControls();
                            ApplyFilters();
                            MessageBox.Show("Клиент успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                        
                    case 1: // Заказы на ремонт
                        var repairOrderForm = new RepairOrderForm(_dataService);
                        if (repairOrderForm.ShowDialog() == DialogResult.OK)
                        {
                            var newOrder = repairOrderForm.RepairOrder;
                            newOrder.Id = GetNextRepairOrderId();
                            _dataService.AddRepairOrder(newOrder);
                            _dataService.SaveData();
                            LoadRepairOrders();
                            RefreshFilterControls();
                            ApplyFilters();
                            MessageBox.Show("Заказ на ремонт успешно добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                        
                    case 2: // Запасные части
                        var sparePartForm = new SparePartForm();
                        if (sparePartForm.ShowDialog() == DialogResult.OK)
                        {
                            var newPart = sparePartForm.SparePart;
                            newPart.Id = GetNextSparePartId();
                            _dataService.AddSparePart(newPart);
                            _dataService.SaveData();
                            LoadSpareParts();
                            RefreshFilterControls();
                            ApplyFilters();
                            MessageBox.Show("Запасная часть успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                        
                    case 3: // Ремонтные работы
                        var repairWorkForm = new RepairWorkForm(_dataService);
                        if (repairWorkForm.ShowDialog() == DialogResult.OK)
                        {
                            var newWork = repairWorkForm.RepairWork;
                            newWork.Id = GetNextRepairWorkId();
                            _dataService.AddRepairWork(newWork);
                            _dataService.SaveData();
                            LoadRepairWorks();
                            RefreshFilterControls();
                            ApplyFilters();
                            MessageBox.Show("Ремонтная работа успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;
                        
                    case 4: // Платежи
                        var paymentForm = new PaymentForm(_dataService);
                        if (paymentForm.ShowDialog() == DialogResult.OK)
                        {
                            var newPayment = paymentForm.Payment;
                            newPayment.Id = GetNextPaymentId();
                            _dataService.AddPayment(newPayment);
                            _dataService.SaveData();
                            LoadPayments();
                            RefreshFilterControls();
                            ApplyFilters();
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
                                ApplyFilters();
                                MessageBox.Show("Данные клиента успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                        
                    case 1: // Заказы на ремонт
                        var order = _dataService.GetRepairOrderById(id);
                        if (order != null)
                        {
                            var orderForm = new RepairOrderForm(_dataService, order);
                            if (orderForm.ShowDialog() == DialogResult.OK)
                            {
                                _dataService.SaveData();
                                LoadRepairOrders();
                                ApplyFilters();
                                MessageBox.Show("Данные заказа на ремонт успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                        
                    case 2: // Запасные части
                        var part = _dataService.GetSparePartById(id);
                        if (part != null)
                        {
                            var partForm = new SparePartForm(part);
                            if (partForm.ShowDialog() == DialogResult.OK)
                            {
                                _dataService.SaveData();
                                LoadSpareParts();
                                ApplyFilters();
                                MessageBox.Show("Данные запасной части успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                        
                    case 3: // Ремонтные работы
                        var work = _dataService.GetAllRepairWorks().FirstOrDefault(w => w.Id == id);
                        if (work != null)
                        {
                            var workForm = new RepairWorkForm(_dataService, work);
                            if (workForm.ShowDialog() == DialogResult.OK)
                            {
                                _dataService.SaveData();
                                LoadRepairWorks();
                                ApplyFilters();
                                MessageBox.Show("Данные ремонтной работы успешно обновлены!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                        
                    case 4: // Платежи
                        var payment = _dataService.GetAllPayments().FirstOrDefault(p => p.Id == id);
                        if (payment != null)
                        {
                            var paymentForm = new PaymentForm(_dataService, payment);
                            if (paymentForm.ShowDialog() == DialogResult.OK)
                            {
                                _dataService.SaveData();
                                LoadPayments();
                                ApplyFilters();
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
                        ApplyFilters();
                        MessageBox.Show("Клиент успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        
                    case 1: // Заказы на ремонт
                        _dataService.RemoveRepairOrder(id);
                        _dataService.SaveData();
                        LoadRepairOrders();
                        ApplyFilters();
                        MessageBox.Show("Заказ на ремонт успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        
                    case 2: // Запасные части
                        _dataService.RemoveSparePart(id);
                        _dataService.SaveData();
                        LoadSpareParts();
                        ApplyFilters();
                        MessageBox.Show("Запасная часть успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        
                    case 3: // Ремонтные работы
                        _dataService.RemoveRepairWork(id);
                        _dataService.SaveData();
                        LoadRepairWorks();
                        ApplyFilters();
                        MessageBox.Show("Ремонтная работа успешно удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                        
                    case 4: // Платежи
                        _dataService.RemovePayment(id);
                        _dataService.SaveData();
                        LoadPayments();
                        ApplyFilters();
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
                _dataService.SaveData();
                MessageBox.Show("Данные успешно сохранены в файл!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        
        private int GetNextRepairOrderId()
        {
            var orders = _dataService.GetAllRepairOrders();
            return orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
        }
        
        private int GetNextSparePartId()
        {
            var parts = _dataService.GetAllSpareParts();
            return parts.Any() ? parts.Max(p => p.Id) + 1 : 1;
        }
        
        private int GetNextRepairWorkId()
        {
            var works = _dataService.GetAllRepairWorks();
            return works.Any() ? works.Max(w => w.Id) + 1 : 1;
        }
        
        private int GetNextPaymentId()
        {
            var payments = _dataService.GetAllPayments();
            return payments.Any() ? payments.Max(p => p.Id) + 1 : 1;
        }
    }
}