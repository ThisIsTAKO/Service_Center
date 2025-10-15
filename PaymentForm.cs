using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Lab678.Models;
using Lab678.Services;

namespace Lab678.Forms
{
    public class PaymentForm : Form
    {
        private ComboBox cmbLoan;
        private DateTimePicker dtpPaymentDate;
        private NumericUpDown numAmount;
        private ComboBox cmbPaymentType;
        private Button btnSave;
        private Button btnCancel;
        
        public Payment Payment { get; private set; }
        private DataService _dataService;
        
        public PaymentForm(DataService dataService, Payment payment = null)
        {
            _dataService = dataService;
            Payment = payment ?? new Payment 
            { 
                PaymentDate = DateTime.Now
            };
            InitializeComponents();
            LoadComboBoxData();
            if (payment != null)
            {
                LoadPaymentData();
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = Payment.Id == 0 ? "Добавить платеж" : "Редактировать платеж";
            this.Size = new Size(500, 330);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            int labelWidth = 150;
            int textBoxWidth = 300;
            int leftMargin = 20;
            int topMargin = 20;
            int verticalSpacing = 45;
            
            // Займ
            Label lblLoan = new Label
            {
                Text = "Займ:",
                Location = new Point(leftMargin, topMargin),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblLoan);
            
            cmbLoan = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbLoan);
            
            // Дата платежа
            Label lblPaymentDate = new Label
            {
                Text = "Дата платежа:",
                Location = new Point(leftMargin, topMargin + verticalSpacing),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblPaymentDate);
            
            dtpPaymentDate = new DateTimePicker
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpPaymentDate);
            
            // Сумма
            Label lblAmount = new Label
            {
                Text = "Сумма:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 2),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblAmount);
            
            numAmount = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 2),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 10000000,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            this.Controls.Add(numAmount);
            
            // Тип платежа
            Label lblPaymentType = new Label
            {
                Text = "Тип платежа:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 3),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblPaymentType);
            
            cmbPaymentType = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 3),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPaymentType.Items.AddRange(new object[] { "Основной долг", "Проценты", "Штраф" });
            this.Controls.Add(cmbPaymentType);
            
            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 4 + 15),
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
                Location = new Point(leftMargin + 320, topMargin + verticalSpacing * 4 + 15),
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
            var loans = _dataService.GetAllLoans();
            foreach (var loan in loans)
            {
                var client = _dataService.GetClientById(loan.ClientId);
                string loanText = $"Займ №{loan.Id} - {client?.FullName ?? "Неизвестен"} - {loan.LoanAmount:N0} ₽";
                cmbLoan.Items.Add(new ComboBoxItem { Value = loan.Id, Text = loanText });
            }
        }
        
        private void LoadPaymentData()
        {
            var loanItem = cmbLoan.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Value == Payment.LoanId);
            if (loanItem != null) cmbLoan.SelectedItem = loanItem;
            
            dtpPaymentDate.Value = Payment.PaymentDate;
            numAmount.Value = Payment.Amount;
            if (cmbPaymentType.Items.Contains(Payment.PaymentType))
                cmbPaymentType.SelectedItem = Payment.PaymentType;
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbLoan.SelectedIndex == -1 || 
                cmbPaymentType.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все обязательные поля", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            Payment.LoanId = ((ComboBoxItem)cmbLoan.SelectedItem).Value;
            Payment.PaymentDate = dtpPaymentDate.Value;
            Payment.Amount = numAmount.Value;
            Payment.PaymentType = cmbPaymentType.SelectedItem.ToString();
            
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
