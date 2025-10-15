using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Lab678.Models;
using Lab678.Services;

namespace Lab678.Forms
{
    public class LoanForm : Form
    {
        private ComboBox cmbClient;
        private ComboBox cmbPledgeItem;
        private NumericUpDown numLoanAmount;
        private NumericUpDown numInterestRate;
        private DateTimePicker dtpIssueDate;
        private DateTimePicker dtpDueDate;
        private ComboBox cmbStatus;
        private NumericUpDown numPaidAmount;
        private Button btnSave;
        private Button btnCancel;
        
        public Loan Loan { get; private set; }
        private DataService _dataService;
        
        public LoanForm(DataService dataService, Loan loan = null)
        {
            _dataService = dataService;
            Loan = loan ?? new Loan 
            { 
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddMonths(3),
                Status = "Активный",
                PaidAmount = 0
            };
            InitializeComponents();
            LoadComboBoxData();
            if (loan != null)
            {
                LoadLoanData();
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = Loan.Id == 0 ? "Добавить займ" : "Редактировать займ";
            this.Size = new Size(500, 480);
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
            
            // Залоговый предмет
            Label lblPledgeItem = new Label
            {
                Text = "Залоговый предмет:",
                Location = new Point(leftMargin, topMargin + verticalSpacing),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblPledgeItem);
            
            cmbPledgeItem = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbPledgeItem);
            
            // Сумма займа
            Label lblLoanAmount = new Label
            {
                Text = "Сумма займа:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 2),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblLoanAmount);
            
            numLoanAmount = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 2),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 10000000,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            this.Controls.Add(numLoanAmount);
            
            // Процентная ставка
            Label lblInterestRate = new Label
            {
                Text = "Процент (%):",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 3),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblInterestRate);
            
            numInterestRate = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 3),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 100,
                Minimum = 0,
                DecimalPlaces = 1
            };
            this.Controls.Add(numInterestRate);
            
            // Дата выдачи
            Label lblIssueDate = new Label
            {
                Text = "Дата выдачи:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 4),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblIssueDate);
            
            dtpIssueDate = new DateTimePicker
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 4),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpIssueDate);
            
            // Срок погашения
            Label lblDueDate = new Label
            {
                Text = "Срок погашения:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 5),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblDueDate);
            
            dtpDueDate = new DateTimePicker
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 5),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpDueDate);
            
            // Статус
            Label lblStatus = new Label
            {
                Text = "Статус:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 6),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblStatus);
            
            cmbStatus = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 6),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "Активный", "Погашен", "Просрочен" });
            this.Controls.Add(cmbStatus);
            
            // Уплаченная сумма
            Label lblPaidAmount = new Label
            {
                Text = "Уплаченная сумма:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 7),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblPaidAmount);
            
            numPaidAmount = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 7),
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
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 8),
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
                Location = new Point(leftMargin + 320, topMargin + verticalSpacing * 8),
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
            
            var pledgeItems = _dataService.GetAllPledgeItems();
            foreach (var item in pledgeItems)
            {
                cmbPledgeItem.Items.Add(new ComboBoxItem { Value = item.Id, Text = item.Name });
            }
        }
        
        private void LoadLoanData()
        {
            var clientItem = cmbClient.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Value == Loan.ClientId);
            if (clientItem != null) cmbClient.SelectedItem = clientItem;
            
            var pledgeItem = cmbPledgeItem.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Value == Loan.PledgeItemId);
            if (pledgeItem != null) cmbPledgeItem.SelectedItem = pledgeItem;
            
            numLoanAmount.Value = Loan.LoanAmount;
            numInterestRate.Value = Loan.InterestRate;
            dtpIssueDate.Value = Loan.IssueDate;
            dtpDueDate.Value = Loan.DueDate;
            if (cmbStatus.Items.Contains(Loan.Status))
                cmbStatus.SelectedItem = Loan.Status;
            numPaidAmount.Value = Loan.PaidAmount;
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbClient.SelectedIndex == -1 || 
                cmbPledgeItem.SelectedIndex == -1 ||
                cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все обязательные поля", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            Loan.ClientId = ((ComboBoxItem)cmbClient.SelectedItem).Value;
            Loan.PledgeItemId = ((ComboBoxItem)cmbPledgeItem.SelectedItem).Value;
            Loan.LoanAmount = numLoanAmount.Value;
            Loan.InterestRate = numInterestRate.Value;
            Loan.IssueDate = dtpIssueDate.Value;
            Loan.DueDate = dtpDueDate.Value;
            Loan.Status = cmbStatus.SelectedItem.ToString();
            Loan.PaidAmount = numPaidAmount.Value;
            
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
