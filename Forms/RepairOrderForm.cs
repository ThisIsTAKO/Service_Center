#nullable disable
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Linq;
using Lab678.Models;
using Lab678.Services;
namespace Lab678.Forms
{
    public partial class RepairOrderForm : Form
    {
        private ComboBox cmbClient;
        private TextBox txtDeviceType;
        private TextBox txtDeviceModel;
        private TextBox txtProblemDescription;
        private NumericUpDown numEstimatedCost;
        private DateTimePicker dtpOrderDate;
        private DateTimePicker dtpCompletionDate;
        private ComboBox cmbStatus;
        private NumericUpDown numPaidAmount;
        private Button btnSave;
        private Button btnCancel;
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RepairOrder RepairOrder { get; private set; }
        private DataService _dataService;
        
        public RepairOrderForm(DataService dataService, RepairOrder order = null)
        {
            _dataService = dataService;
            RepairOrder = order ?? new RepairOrder { OrderDate = DateTime.Now, Status = "Принят", PaidAmount = 0 };
            InitializeComponents();
            LoadComboBoxData();
            if (order != null)
            {
                LoadRepairOrderData();
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = RepairOrder.Id == 0 ? "Добавить заказ на ремонт" : "Редактировать заказ на ремонт";
            this.Size = new Size(500, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            int labelWidth = 150;
            int textBoxWidth = 300;
            int leftMargin = 20;
            int topMargin = 20;
            int verticalSpacing = 45;
            
            // Клиент
            Label lblClient = new Label
            {
                Text = "Клиент:",
                Location = new Point(leftMargin, topMargin),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblClient);
            
            cmbClient = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbClient);
            
            // Тип устройства
            Label lblDeviceType = new Label
            {
                Text = "Тип устройства:",
                Location = new Point(leftMargin, topMargin + verticalSpacing),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblDeviceType);
            
            txtDeviceType = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtDeviceType);
            
            // Модель устройства
            Label lblDeviceModel = new Label
            {
                Text = "Модель устройства:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 2),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblDeviceModel);
            
            txtDeviceModel = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 2),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtDeviceModel);
            
            // Описание проблемы
            Label lblProblemDescription = new Label
            {
                Text = "Описание проблемы:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 3),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblProblemDescription);
            
            txtProblemDescription = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 3),
                Size = new Size(textBoxWidth, 60),
                Font = new Font("Segoe UI", 10),
                Multiline = true
            };
            this.Controls.Add(txtProblemDescription);
            
            // Оценочная стоимость
            Label lblEstimatedCost = new Label
            {
                Text = "Оценочная стоимость:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblEstimatedCost);
            
            numEstimatedCost = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 10000000,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            this.Controls.Add(numEstimatedCost);
            
            // Дата заказа
            Label lblOrderDate = new Label
            {
                Text = "Дата заказа:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 5 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblOrderDate);
            
            dtpOrderDate = new DateTimePicker
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 5 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpOrderDate);
            
            // Дата завершения
            Label lblCompletionDate = new Label
            {
                Text = "Дата завершения:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 6 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblCompletionDate);
            
            dtpCompletionDate = new DateTimePicker
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 6 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpCompletionDate);
            
            // Статус
            Label lblStatus = new Label
            {
                Text = "Статус:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 7 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblStatus);
            
            cmbStatus = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 7 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "Принят", "В работе", "Завершен", "Отменен" });
            this.Controls.Add(cmbStatus);
            
            // Уплаченная сумма
            Label lblPaidAmount = new Label
            {
                Text = "Уплаченная сумма:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 8 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblPaidAmount);
            
            numPaidAmount = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 8 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 10000000,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            this.Controls.Add(numPaidAmount);
            
            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 9 + 30),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);
            
            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(leftMargin + 320, topMargin + verticalSpacing * 9 + 30),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            this.Controls.Add(btnCancel);
        }
        
        private void LoadComboBoxData()
        {
            var clients = _dataService.GetAllClients();
            foreach (var client in clients)
            {
                cmbClient.Items.Add(new ComboBoxItem { Value = client.Id, Text = client.FullName });
            }
        }
        
        private void LoadRepairOrderData()
        {
            var clientItem = cmbClient.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Value == RepairOrder.ClientId);
            if (clientItem != null) cmbClient.SelectedItem = clientItem;
            
            txtDeviceType.Text = RepairOrder.DeviceType;
            txtDeviceModel.Text = RepairOrder.DeviceModel;
            txtProblemDescription.Text = RepairOrder.ProblemDescription;
            numEstimatedCost.Value = RepairOrder.EstimatedCost;
            dtpOrderDate.Value = RepairOrder.OrderDate;
            if (RepairOrder.CompletionDate.HasValue) dtpCompletionDate.Value = RepairOrder.CompletionDate.Value;
            if (cmbStatus.Items.Contains(RepairOrder.Status)) cmbStatus.SelectedItem = RepairOrder.Status;
            numPaidAmount.Value = RepairOrder.PaidAmount;
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbClient.SelectedIndex == -1 || 
                string.IsNullOrWhiteSpace(txtDeviceType.Text) ||
                cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все обязательные поля", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            RepairOrder.ClientId = ((ComboBoxItem)cmbClient.SelectedItem).Value;
            RepairOrder.DeviceType = txtDeviceType.Text.Trim();
            RepairOrder.DeviceModel = txtDeviceModel.Text.Trim();
            RepairOrder.ProblemDescription = txtProblemDescription.Text.Trim();
            RepairOrder.EstimatedCost = numEstimatedCost.Value;
            RepairOrder.OrderDate = dtpOrderDate.Value;
            RepairOrder.CompletionDate = dtpCompletionDate.Value;
            RepairOrder.Status = cmbStatus.SelectedItem.ToString();
            RepairOrder.PaidAmount = numPaidAmount.Value;
            
            DialogResult = DialogResult.OK;
            Close();
        }
        
        private class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }
    }
}