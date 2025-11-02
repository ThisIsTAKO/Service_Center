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
    public partial class RepairWorkForm : Form
    {
        private ComboBox cmbRepairOrder;
        private TextBox txtMasterName;
        private TextBox txtWorkDescription;
        private NumericUpDown numLaborCost;
        private NumericUpDown numWorkHours;
        private NumericUpDown numWorkMinutes;
        private DateTimePicker dtpWorkDate;
        private Button btnSave;
        private Button btnCancel;
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RepairWork RepairWork { get; private set; }
        private DataService _dataService;
        
        public RepairWorkForm(DataService dataService, RepairWork work = null)
        {
            _dataService = dataService;
            RepairWork = work ?? new RepairWork { WorkDate = DateTime.Now, WorkTime = TimeSpan.Zero };
            InitializeComponents();
            LoadComboBoxData();
            if (work != null)
            {
                LoadRepairWorkData();
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = RepairWork.Id == 0 ? "Добавить ремонтную работу" : "Редактировать ремонтную работу";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            int labelWidth = 150;
            int textBoxWidth = 300;
            int leftMargin = 20;
            int topMargin = 20;
            int verticalSpacing = 45;
            
            // Заказ на ремонт
            Label lblRepairOrder = new Label
            {
                Text = "Заказ на ремонт:",
                Location = new Point(leftMargin, topMargin),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblRepairOrder);
            
            cmbRepairOrder = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbRepairOrder);
            
            // Имя мастера
            Label lblMasterName = new Label
            {
                Text = "Имя мастера:",
                Location = new Point(leftMargin, topMargin + verticalSpacing),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblMasterName);
            
            txtMasterName = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtMasterName);
            
            // Описание работы
            Label lblWorkDescription = new Label
            {
                Text = "Описание работы:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 2),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblWorkDescription);
            
            txtWorkDescription = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 2),
                Size = new Size(textBoxWidth, 60),
                Font = new Font("Segoe UI", 10),
                Multiline = true
            };
            this.Controls.Add(txtWorkDescription);
            
            // Стоимость труда
            Label lblLaborCost = new Label
            {
                Text = "Стоимость труда:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 3 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblLaborCost);
            
            numLaborCost = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 3 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 10000000,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            this.Controls.Add(numLaborCost);
            
            // Время работы (часы)
            Label lblWorkTime = new Label
            {
                Text = "Время работы:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblWorkTime);
            
            numWorkHours = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 100,
                Minimum = 0,
                DecimalPlaces = 0
            };
            this.Controls.Add(numWorkHours);
            
            Label lblHours = new Label
            {
                Text = "ч",
                Location = new Point(leftMargin + 270, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(20, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblHours);
            
            numWorkMinutes = new NumericUpDown
            {
                Location = new Point(leftMargin + 300, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 59,
                Minimum = 0,
                DecimalPlaces = 0
            };
            this.Controls.Add(numWorkMinutes);
            
            Label lblMinutes = new Label
            {
                Text = "мин",
                Location = new Point(leftMargin + 410, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(30, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblMinutes);
            
            // Дата работы
            Label lblWorkDate = new Label
            {
                Text = "Дата работы:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 5 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblWorkDate);
            
            dtpWorkDate = new DateTimePicker
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 5 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpWorkDate);
            
            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 6 + 30),
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
                Location = new Point(leftMargin + 320, topMargin + verticalSpacing * 6 + 30),
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
            var orders = _dataService.GetAllRepairOrders();
            foreach (var order in orders)
            {
                var client = _dataService.GetClientById(order.ClientId);
                string orderText = $"Заказ №{order.Id} - {client?.FullName ?? "Неизвестен"} - {order.DeviceType}";
                cmbRepairOrder.Items.Add(new ComboBoxItem { Value = order.Id, Text = orderText });
            }
        }
        
        private void LoadRepairWorkData()
        {
            var orderItem = cmbRepairOrder.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Value == RepairWork.RepairOrderId);
            if (orderItem != null) cmbRepairOrder.SelectedItem = orderItem;
            
            txtMasterName.Text = RepairWork.MasterName;
            txtWorkDescription.Text = RepairWork.WorkDescription;
            numLaborCost.Value = RepairWork.LaborCost;
            numWorkHours.Value = RepairWork.WorkTime.Hours;
            numWorkMinutes.Value = RepairWork.WorkTime.Minutes;
            dtpWorkDate.Value = RepairWork.WorkDate;
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbRepairOrder.SelectedIndex == -1 || 
                string.IsNullOrWhiteSpace(txtMasterName.Text))
            {
                MessageBox.Show("Заполните все обязательные поля", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            RepairWork.RepairOrderId = ((ComboBoxItem)cmbRepairOrder.SelectedItem).Value;
            RepairWork.MasterName = txtMasterName.Text.Trim();
            RepairWork.WorkDescription = txtWorkDescription.Text.Trim();
            RepairWork.LaborCost = numLaborCost.Value;
            RepairWork.WorkTime = new TimeSpan((int)numWorkHours.Value, (int)numWorkMinutes.Value, 0);
            RepairWork.WorkDate = dtpWorkDate.Value;
            
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