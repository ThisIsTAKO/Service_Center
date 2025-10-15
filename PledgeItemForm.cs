using System;
using System.Drawing;
using System.Windows.Forms;
using Lab678.Models;

namespace Lab678.Forms
{
    public class PledgeItemForm : Form
    {
        private TextBox txtName;
        private ComboBox cmbCategory;
        private TextBox txtDescription;
        private NumericUpDown numEstimatedValue;
        private ComboBox cmbCondition;
        private Button btnSave;
        private Button btnCancel;
        
        public PledgeItem PledgeItem { get; private set; }
        
        public PledgeItemForm(PledgeItem pledgeItem = null)
        {
            PledgeItem = pledgeItem ?? new PledgeItem();
            InitializeComponents();
            if (pledgeItem != null)
            {
                LoadPledgeItemData();
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = PledgeItem.Id == 0 ? "Добавить залоговое имущество" : "Редактировать залоговое имущество";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            int labelWidth = 150;
            int textBoxWidth = 300;
            int leftMargin = 20;
            int topMargin = 20;
            int verticalSpacing = 45;
            
            // Наименование
            Label lblName = new Label
            {
                Text = "Наименование:",
                Location = new Point(leftMargin, topMargin),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblName);
            
            txtName = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtName);
            
            // Категория
            Label lblCategory = new Label
            {
                Text = "Категория:",
                Location = new Point(leftMargin, topMargin + verticalSpacing),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblCategory);
            
            cmbCategory = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.Items.AddRange(new object[] { "Ювелирные изделия", "Электроника", "Часы", "Антиквариат", "Бытовая техника", "Другое" });
            this.Controls.Add(cmbCategory);
            
            // Описание
            Label lblDescription = new Label
            {
                Text = "Описание:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 2),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblDescription);
            
            txtDescription = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 2),
                Size = new Size(textBoxWidth, 60),
                Font = new Font("Segoe UI", 10),
                Multiline = true
            };
            this.Controls.Add(txtDescription);
            
            // Оценочная стоимость
            Label lblEstimatedValue = new Label
            {
                Text = "Оценочная стоимость:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 3 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblEstimatedValue);
            
            numEstimatedValue = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 3 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 10000000,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            this.Controls.Add(numEstimatedValue);
            
            // Состояние
            Label lblCondition = new Label
            {
                Text = "Состояние:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblCondition);
            
            cmbCondition = new ComboBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCondition.Items.AddRange(new object[] { "Отличное", "Хорошее", "Удовлетворительное", "Плохое" });
            this.Controls.Add(cmbCondition);
            
            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 5 + 30),
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
                Location = new Point(leftMargin + 320, topMargin + verticalSpacing * 5 + 30),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            this.Controls.Add(btnCancel);
        }
        
        private void LoadPledgeItemData()
        {
            txtName.Text = PledgeItem.Name;
            if (cmbCategory.Items.Contains(PledgeItem.Category))
                cmbCategory.SelectedItem = PledgeItem.Category;
            txtDescription.Text = PledgeItem.Description;
            numEstimatedValue.Value = PledgeItem.EstimatedValue;
            if (cmbCondition.Items.Contains(PledgeItem.Condition))
                cmbCondition.SelectedItem = PledgeItem.Condition;
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || 
                cmbCategory.SelectedIndex == -1 ||
                cmbCondition.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все обязательные поля", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            PledgeItem.Name = txtName.Text.Trim();
            PledgeItem.Category = cmbCategory.SelectedItem.ToString();
            PledgeItem.Description = txtDescription.Text.Trim();
            PledgeItem.EstimatedValue = numEstimatedValue.Value;
            PledgeItem.Condition = cmbCondition.SelectedItem.ToString();
            
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
