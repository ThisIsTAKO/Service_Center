using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using lab11.Models;

namespace lab11.Forms
{
    public class ClientForm : Form
    {
        private TextBox txtLastName, txtFirstName, txtMiddleName;
        private TextBox txtPassportNumber, txtPhone, txtAddress;
        private DateTimePicker dtpRegistrationDate;
        private Button btnSave, btnCancel;
        
        public Client Client { get; private set; }
        
        public ClientForm(Client? client = null)
        {
            try
            {
                Client = client ?? new Client { RegistrationDate = DateTime.Now };
                InitializeComponents();
                if (client != null)
                {
                    LoadClientData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации формы:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = Client.Id == 0 ? "Добавить клиента" : "Редактировать клиента";
            this.Size = new Size(520, 470);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            
            int labelWidth = 150;
            int textBoxWidth = 320;
            int leftMargin = 20;
            int topMargin = 20;
            int verticalSpacing = 50;
            
            // Заголовок
            Label lblHeader = new Label
            {
                Text = Client.Id == 0 ? "Новый клиент" : "Редактирование клиента",
                Location = new Point(leftMargin, topMargin - 10),
                Size = new Size(480, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            this.Controls.Add(lblHeader);
            
            topMargin += 30;
            
            // Фамилия
            AddLabelAndTextBox("Фамилия:*", ref txtLastName, leftMargin, topMargin, labelWidth, textBoxWidth);
            AddLettersOnlyValidation(txtLastName, "Фамилия");
            
            // Имя
            AddLabelAndTextBox("Имя:*", ref txtFirstName, leftMargin, topMargin + verticalSpacing, labelWidth, textBoxWidth);
            AddLettersOnlyValidation(txtFirstName, "Имя");
            
            // Отчество
            AddLabelAndTextBox("Отчество:", ref txtMiddleName, leftMargin, topMargin + verticalSpacing * 2, labelWidth, textBoxWidth);
            AddLettersOnlyValidation(txtMiddleName, "Отчество");
            
            // Паспорт
            AddLabelAndTextBox("Паспорт:*", ref txtPassportNumber, leftMargin, topMargin + verticalSpacing * 3, labelWidth, textBoxWidth);
            AddPassportValidation(txtPassportNumber);
            
            // Телефон
            AddLabelAndTextBox("Телефон:*", ref txtPhone, leftMargin, topMargin + verticalSpacing * 4, labelWidth, textBoxWidth);
            AddPhoneValidation(txtPhone);
            
            // Адрес
            AddLabelAndTextBox("Адрес:", ref txtAddress, leftMargin, topMargin + verticalSpacing * 5, labelWidth, textBoxWidth);
            
            // Дата регистрации
            Label lblRegistrationDate = new Label
            {
                Text = "Дата регистрации:",
                Location = new Point(leftMargin, topMargin + verticalSpacing * 6 + 5),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(lblRegistrationDate);
            
            dtpRegistrationDate = new DateTimePicker
            {
                Location = new Point(leftMargin + 170, topMargin + verticalSpacing * 6),
                Size = new Size(textBoxWidth, 30),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };
            this.Controls.Add(dtpRegistrationDate);
            
            // Кнопки
            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(leftMargin + 170, topMargin + verticalSpacing * 7),
                Size = new Size(150, 40),
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
                Location = new Point(leftMargin + 340, topMargin + verticalSpacing * 7),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            this.Controls.Add(btnCancel);
        }
        
        private void AddLabelAndTextBox(string labelText, ref TextBox textBox, int x, int y, int labelWidth, int textBoxWidth)
        {
            Label label = new Label
            {
                Text = labelText,
                Location = new Point(x, y + 5),
                Size = new Size(labelWidth, 20),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(label);
            
            textBox = new TextBox
            {
                Location = new Point(x + 170, y),
                Size = new Size(textBoxWidth, 30),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(textBox);
        }
        
        private void LoadClientData()
        {
            try
            {
                if (Client == null)
                {
                    throw new InvalidOperationException("Объект клиента не инициализирован");
                }
                
                txtLastName.Text = Client.LastName ?? string.Empty;
                txtFirstName.Text = Client.FirstName ?? string.Empty;
                txtMiddleName.Text = Client.MiddleName ?? string.Empty;
                txtPassportNumber.Text = Client.PassportNumber ?? string.Empty;
                txtPhone.Text = Client.Phone ?? string.Empty;
                txtAddress.Text = Client.Address ?? string.Empty;
                dtpRegistrationDate.Value = Client.RegistrationDate;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных клиента:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                // Валидация обязательных полей
                if (string.IsNullOrWhiteSpace(txtLastName.Text))
                {
                    MessageBox.Show("Заполните поле \"Фамилия\"!", 
                        "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLastName.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                {
                    MessageBox.Show("Заполните поле \"Имя\"!", 
                        "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFirstName.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtPassportNumber.Text))
                {
                    MessageBox.Show("Заполните поле \"Паспорт\"!", 
                        "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassportNumber.Focus();
                    return;
                }
                
                if (txtPassportNumber.Text.Trim().Length < 10)
                {
                    MessageBox.Show("Номер паспорта должен содержать минимум 10 символов!", 
                        "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassportNumber.Focus();
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(txtPhone.Text))
                {
                    MessageBox.Show("Заполните поле \"Телефон\"!", 
                        "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.Focus();
                    return;
                }
                
                // Сохранение данных
                Client.LastName = txtLastName.Text.Trim();
                Client.FirstName = txtFirstName.Text.Trim();
                Client.MiddleName = txtMiddleName.Text.Trim();
                Client.PassportNumber = txtPassportNumber.Text.Trim();
                Client.Phone = txtPhone.Text.Trim();
                Client.Address = txtAddress.Text.Trim();
                Client.RegistrationDate = dtpRegistrationDate.Value;
                
                // Валидация через метод модели
                Client.Validate();
                
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка валидации данных:\n{ex.Message}",
                    "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных клиента:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Валидация: только буквы
        private void AddLettersOnlyValidation(TextBox textBox, string fieldName)
        {
            textBox.KeyPress += (s, e) =>
            {
                try
                {
                    // Разрешаем буквы, пробел, дефис, Backspace
                    if (char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    {
                        e.Handled = true;
                        MessageBox.Show($"В поле \"{fieldName}\" нельзя вводить цифры!",
                            "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при проверке ввода:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }

        // Валидация паспорта: только цифры и пробелы
        private void AddPassportValidation(TextBox textBox)
        {
            textBox.KeyPress += (s, e) =>
            {
                try
                {
                    // Разрешаем только цифры, пробелы и Backspace
                    if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
                    {
                        e.Handled = true;
                        MessageBox.Show("В поле \"Паспорт\" можно вводить только цифры и пробелы!\n(Пример: 4512 123456)",
                            "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при проверке ввода:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }

        // Валидация телефона: цифры, пробелы, +, -, (, )
        private void AddPhoneValidation(TextBox textBox)
        {
            textBox.KeyPress += (s, e) =>
            {
                try
                {
                    // Разрешаем цифры, пробелы, +, -, (, ), Backspace
                    char[] allowedChars = { ' ', '+', '-', '(', ')' };
                    if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !allowedChars.Contains(e.KeyChar))
                    {
                        e.Handled = true;
                        MessageBox.Show("В поле \"Телефон\" можно вводить только цифры и символы: + - ( ) пробел\n(Пример: +7 (912) 345-67-89)",
                            "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при проверке ввода:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
        }
    }
}
