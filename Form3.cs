using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using lab11.Services;
using lab11.Models;
using lab11.Forms;

namespace lab11
{
    public class Form3 : Form
    {
        private DataService? _dataService;
        private TabControl tabControl;
        private DataGridView dgvClients, dgvPledgeItems, dgvLoans;
        private Label lblTitle;
        private Panel panelHeader, panelButtons;
        private Button btnRefresh, btnSave, btnInfo;
        private Button btnAdd, btnEdit, btnDelete;

        public Form3()
        {
            try
            {
                InitializeComponent();
                _dataService = new DataService();
                LoadAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка при инициализации формы:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Лабораторная работа №11 - База данных Ломбарда";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(52, 73, 94);

            // Панель заголовка
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(52, 73, 94)
            };

            lblTitle = new Label
            {
                Text = "База данных\n\"ЛОМБАРД\"",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            panelHeader.Controls.Add(lblTitle);

            // Панель кнопок
            panelButtons = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(236, 240, 241)
            };

            btnRefresh = new Button
            {
                Text = "Обновить",
                Size = new Size(120, 40),
                Location = new Point(10, 10),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRefresh.Click += BtnRefresh_Click;
            panelButtons.Controls.Add(btnRefresh);

            btnSave = new Button
            {
                Text = "Сохранить",
                Size = new Size(120, 40),
                Location = new Point(140, 10),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;
            panelButtons.Controls.Add(btnSave);

            btnInfo = new Button
            {
                Text = "Справка",
                Size = new Size(120, 40),
                Location = new Point(270, 10),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnInfo.Click += BtnInfo_Click;
            panelButtons.Controls.Add(btnInfo);

            btnAdd = new Button
            {
                Text = "Добавить",
                Size = new Size(120, 40),
                Location = new Point(400, 10),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAdd.Click += BtnAdd_Click;
            panelButtons.Controls.Add(btnAdd);

            btnEdit = new Button
            {
                Text = "Изменить",
                Size = new Size(120, 40),
                Location = new Point(530, 10),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnEdit.Click += BtnEdit_Click;
            panelButtons.Controls.Add(btnEdit);

            btnDelete = new Button
            {
                Text = "Удалить",
                Size = new Size(120, 40),
                Location = new Point(660, 10),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnDelete.Click += BtnDelete_Click;
            panelButtons.Controls.Add(btnDelete);

            // TabControl для разных таблиц
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            // Вкладка "Клиенты"
            TabPage tabClients = new TabPage("Клиенты");
            tabClients.BackColor = Color.White;
            dgvClients = CreateDataGridView();
            tabClients.Controls.Add(dgvClients);
            tabControl.TabPages.Add(tabClients);

            // Вкладка "Залоговое имущество"
            TabPage tabPledgeItems = new TabPage("Залоговое имущество");
            tabPledgeItems.BackColor = Color.White;
            dgvPledgeItems = CreateDataGridView();
            tabPledgeItems.Controls.Add(dgvPledgeItems);
            tabControl.TabPages.Add(tabPledgeItems);

            // Вкладка "Займы"
            TabPage tabLoans = new TabPage("Займы");
            tabLoans.BackColor = Color.White;
            dgvLoans = CreateDataGridView();
            tabLoans.Controls.Add(dgvLoans);
            tabControl.TabPages.Add(tabLoans);

            this.Controls.Add(tabControl);
            this.Controls.Add(panelButtons);
            this.Controls.Add(panelHeader);
        }

        private DataGridView CreateDataGridView()
        {
            try
            {
                DataGridView dgv = new DataGridView
                {
                    Dock = DockStyle.Fill,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    ColumnHeadersDefaultCellStyle =
                    {
                        BackColor = Color.FromArgb(52, 73, 94),
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold)
                    },
                    ColumnHeadersHeight = 35,
                    RowTemplate = { Height = 30 },
                    AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(240, 240, 240) },
                    EnableHeadersVisualStyles = false
                };
                return dgv;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании таблицы:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new DataGridView(); // Возвращаем пустую таблицу
            }
        }

        private void LoadAllData()
        {
            try
            {
                if (_dataService == null)
                {
                    MessageBox.Show("Сервис данных не инициализирован!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                LoadClients();
                LoadPledgeItems();
                LoadLoans();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadClients()
        {
            try
            {
                if (_dataService == null) return;

                var clients = _dataService.GetAllClients();
                if (clients == null)
                {
                    MessageBox.Show("Не удалось загрузить клиентов",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
            catch (NullReferenceException)
            {
                MessageBox.Show("Ошибка: отсутствуют данные клиентов",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка клиентов:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPledgeItems()
        {
            try
            {
                if (_dataService == null) return;

                var items = _dataService.GetAllPledgeItems();
                if (items == null)
                {
                    MessageBox.Show("Не удалось загрузить залоговое имущество",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
            catch (NullReferenceException)
            {
                MessageBox.Show("Ошибка: отсутствуют данные залогового имущества",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке залогового имущества:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoans()
        {
            try
            {
                if (_dataService == null) return;

                var loans = _dataService.GetAllLoans();
                if (loans == null)
                {
                    MessageBox.Show("Не удалось загрузить займы",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var displayData = loans.Select(l => new
                {
                    ID = l.Id,
                    КлиентID = l.ClientId,
                    ЗалоговыйПредметID = l.PledgeItemId,
                    СуммаЗайма = l.LoanAmount.ToString("N0") + " ₽",
                    Процент = l.InterestRate.ToString("N1") + "%",
                    ДатаВыдачи = l.IssueDate.ToString("dd.MM.yyyy"),
                    СрокПогашения = l.DueDate.ToString("dd.MM.yyyy"),
                    Статус = l.Status,
                    УплаченнаяСумма = l.PaidAmount.ToString("N0") + " ₽"
                }).ToList();

                dgvLoans.DataSource = displayData;

                // Цветовое выделение по статусу
                for (int i = 0; i < dgvLoans.Rows.Count && i < loans.Count; i++)
                {
                    string status = loans[i].Status;
                    if (status == "Активный")
                        dgvLoans.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                    else if (status == "Просрочен")
                        dgvLoans.Rows[i].DefaultCellStyle.BackColor = Color.LightCoral;
                    else if (status == "Погашен")
                        dgvLoans.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Ошибка: отсутствуют данные займов",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show($"Ошибка индексации при загрузке займов:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке списка займов:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dataService == null)
                {
                    MessageBox.Show("Сервис данных не инициализирован!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _dataService.LoadData();
                LoadAllData();
                MessageBox.Show("Данные успешно обновлены из файла!",
                    "Обновление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dataService == null)
                {
                    MessageBox.Show("Сервис данных не инициализирован!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _dataService.SaveData();
                MessageBox.Show("Данные успешно сохранены в файл!",
                    "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Нет прав доступа для сохранения файла. Проверьте права доступа.",
                    "Ошибка доступа", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show($"Ошибка ввода-вывода при сохранении:\n{ex.Message}",
                    "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dataService == null)
                {
                    MessageBox.Show("Сервис данных не инициализирован!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var clients = _dataService.GetAllClients();
                var items = _dataService.GetAllPledgeItems();
                var loans = _dataService.GetAllLoans();

                string info = $"Статистика базы данных:\n\n" +
                             $"Клиентов: {clients?.Count ?? 0}\n" +
                             $"Залоговых предметов: {items?.Count ?? 0}\n" +
                             $"Займов: {loans?.Count ?? 0}\n\n" +
                             $"Данная форма демонстрирует обработку исключений\n" +
                             $"при работе с базой данных.";

                MessageBox.Show(info, "Справка",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении информации:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dataService == null)
                {
                    MessageBox.Show("Сервис данных не инициализирован!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                            MessageBox.Show("Клиент успешно добавлен!", "Успех", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            MessageBox.Show("Залоговое имущество успешно добавлено!", "Успех",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        break;

                    case 2: // Займы
                        MessageBox.Show("Добавление займов доступно через диалоговую форму (добавьте LoanForm.cs)",
                            "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка валидации:\n{ex.Message}",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dataService == null)
                {
                    MessageBox.Show("Сервис данных не инициализирован!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dgv = GetCurrentDataGridView();
                if (dgv == null || dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите строку для редактирования",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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
                                MessageBox.Show("Данные клиента успешно обновлены!", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                                MessageBox.Show("Данные залогового имущества обновлены!", "Успех",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        break;
                }
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Ошибка преобразования данных. Проверьте выбранную строку.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_dataService == null)
                {
                    MessageBox.Show("Сервис данных не инициализирован!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dgv = GetCurrentDataGridView();
                if (dgv == null || dgv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите строку для удаления",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?",
                    "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                int tabIndex = tabControl.SelectedIndex;
                var selectedRow = dgv.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);

                switch (tabIndex)
                {
                    case 0: // Клиенты
                        _dataService.RemoveClient(id);
                        _dataService.SaveData();
                        LoadClients();
                        MessageBox.Show("Клиент успешно удалён!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case 1: // Залоговое имущество
                        _dataService.RemovePledgeItem(id);
                        _dataService.SaveData();
                        LoadPledgeItems();
                        MessageBox.Show("Залоговое имущество успешно удалено!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case 2: // Займы
                        _dataService.RemoveLoan(id);
                        _dataService.SaveData();
                        LoadLoans();
                        MessageBox.Show("Займ успешно удалён!", "Успех",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataGridView? GetCurrentDataGridView()
        {
            try
            {
                if (tabControl.SelectedIndex == 0) return dgvClients;
                if (tabControl.SelectedIndex == 1) return dgvPledgeItems;
                if (tabControl.SelectedIndex == 2) return dgvLoans;
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении текущей таблицы:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private int GetNextClientId()
        {
            try
            {
                var clients = _dataService?.GetAllClients();
                return clients != null && clients.Any() ? clients.Max(c => c.Id) + 1 : 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации ID:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
        }

        private int GetNextPledgeItemId()
        {
            try
            {
                var items = _dataService?.GetAllPledgeItems();
                return items != null && items.Any() ? items.Max(p => p.Id) + 1 : 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации ID:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }
        }
    }
}
