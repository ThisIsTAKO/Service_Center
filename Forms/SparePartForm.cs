#nullable disable
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System;
using Lab678.Models;
namespace Lab678.Forms
{
    public partial class SparePartForm : Form
    {
        private TextBox txtName;
        private ComboBox cmbCategory;
        private TextBox txtDescription;
        private NumericUpDown numCost;
        private NumericUpDown numStockQuantity;
        private TextBox txtSupplier;
        private Button btnSave;
        private Button btnCancel;
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SparePart SparePart { get; private set; }
        
        public SparePartForm(SparePart sparePart = null)
        {
            SparePart = sparePart ?? new SparePart();
            InitializeComponents();
            if (sparePart != null)
            {
                LoadSparePartData();
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = SparePart.Id == 0 ? "Добавить запасную часть" : "Редактировать запасную часть";
            this.Size = new Size(500, 450);
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
            cmbCategory.Items.AddRange(new object[] { "Электроника", "Механика", "Корпус", "Другое" });
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
            
            // Стоимость
            Label lblCost = new Label
            {
                Text = "Стоимость:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 3 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblCost);
            
            numCost = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 3 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 10000000,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            this.Controls.Add(numCost);
            
            // Остаток на складе
            Label lblStockQuantity = new Label
            {
                Text = "Остаток на складе:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblStockQuantity);
            
            numStockQuantity = new NumericUpDown
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 4 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 100000,
                Minimum = 0,
                DecimalPlaces = 0
            };
            this.Controls.Add(numStockQuantity);
            
            // Поставщик
            Label lblSupplier = new Label
            {
                Text = "Поставщик:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 5 + 15),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblSupplier);
            
            txtSupplier = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 5 + 15),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtSupplier);
            
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
        
        private void LoadSparePartData()
        {
            txtName.Text = SparePart.Name;
            if (cmbCategory.Items.Contains(SparePart.Category)) cmbCategory.SelectedItem = SparePart.Category;
            txtDescription.Text = SparePart.Description;
            numCost.Value = SparePart.Cost;
            numStockQuantity.Value = SparePart.StockQuantity;
            txtSupplier.Text = SparePart.Supplier;
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || 
                cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все обязательные поля", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            SparePart.Name = txtName.Text.Trim();
            SparePart.Category = cmbCategory.SelectedItem.ToString();
            SparePart.Description = txtDescription.Text.Trim();
            SparePart.Cost = numCost.Value;
            SparePart.StockQuantity = (int)numStockQuantity.Value;
            SparePart.Supplier = txtSupplier.Text.Trim();
            
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}