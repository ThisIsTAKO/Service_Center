#nullable disable
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
using Lab678.Models;
namespace Lab678.Forms
{
    public partial class ClientForm : Form
    {
        private TextBox txtLastName;
        private TextBox txtFirstName;
        private TextBox txtMiddleName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private DateTimePicker dtpRegistrationDate;
        private Button btnSave;
        private Button btnCancel;
        
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Client Client { get; private set; }
        
        public ClientForm(Client client = null)
        {
            Client = client ?? new Client { RegistrationDate = DateTime.Now };
            InitializeComponents();
            if (client != null)
            {
                LoadClientData();
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = Client.Id == 0 ? "Добавить клиента" : "Редактировать клиента";
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
            
            // Фамилия
            Label lblLastName = new Label
            {
                Text = "Фамилия:",
                Location = new Point(leftMargin, topMargin),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblLastName);
            
            txtLastName = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtLastName);
            
            // Имя
            Label lblFirstName = new Label
            {
                Text = "Имя:",
                Location = new Point(leftMargin, topMargin + verticalSpacing),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblFirstName);
            
            txtFirstName = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtFirstName);
            
            // Отчество
            Label lblMiddleName = new Label
            {
                Text = "Отчество:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 2),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblMiddleName);
            
            txtMiddleName = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 2),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtMiddleName);
            
            // Телефон
            Label lblPhone = new Label
            {
                Text = "Телефон:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 3),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblPhone);
            
            txtPhone = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 3),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtPhone);
            
            // Email
            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 4),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblEmail);
            
            txtEmail = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 4),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtEmail);
            
            // Адрес
            Label lblAddress = new Label
            {
                Text = "Адрес:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 5),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblAddress);
            
            txtAddress = new TextBox
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 5),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtAddress);
            
            // Дата регистрации
            Label lblRegistrationDate = new Label
            {
                Text = "Дата регистрации:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 6),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblRegistrationDate);
            
            dtpRegistrationDate = new DateTimePicker
            {
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 6),
                Size = new Size(textBoxWidth, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpRegistrationDate);
            
            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(leftMargin + 160, topMargin + verticalSpacing * 7),
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
                Location = new Point(leftMargin + 320, topMargin + verticalSpacing * 7),
                Size = new Size(140, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            this.Controls.Add(btnCancel);
        }
        
        private void LoadClientData()
        {
            txtLastName.Text = Client.LastName;
            txtFirstName.Text = Client.FirstName;
            txtMiddleName.Text = Client.MiddleName;
            txtPhone.Text = Client.Phone;
            txtEmail.Text = Client.Email;
            txtAddress.Text = Client.Address;
            dtpRegistrationDate.Value = Client.RegistrationDate;
        }
        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) || 
                string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Заполните обязательные поля: Фамилия и Имя", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            Client.LastName = txtLastName.Text.Trim();
            Client.FirstName = txtFirstName.Text.Trim();
            Client.MiddleName = txtMiddleName.Text.Trim();
            Client.Phone = txtPhone.Text.Trim();
            Client.Email = txtEmail.Text.Trim();
            Client.Address = txtAddress.Text.Trim();
            Client.RegistrationDate = dtpRegistrationDate.Value;
            
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}