using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using lab11.Models;

namespace lab11.Forms
{
    public class PledgeItemForm : Form
    {
        private TextBox txtName, txtDescription;
        private ComboBox cmbCategory, cmbCondition;
        private NumericUpDown numEstimatedValue;
        private Button btnSave, btnCancel;
        
        public PledgeItem PledgeItem { get; private set; }
        
        public PledgeItemForm(PledgeItem? pledgeItem = null)
        {
            try
            {
                PledgeItem = pledgeItem ?? new PledgeItem();
                InitializeComponents();
                if (pledgeItem != null) LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации:\n{ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void InitializeComponents()
        {
            this.Text = PledgeItem.Id == 0 ? "Добавить имущество" : "Редактировать имущество";
            this.Size = new Size(520, 420);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;
            
            int leftMargin = 20, topMargin = 50, spacing = 55;
            
            AddField("Наименование:*", ref txtName, topMargin);
            AddNameValidation(txtName);
            AddComboField("Категория:*", ref cmbCategory, topMargin + spacing,
                new[] { "Ювелирные изделия", "Электроника", "Часы", "Антиквариат", "Бытовая техника", "Другое" });
            AddField("Описание:", ref txtDescription, topMargin + spacing * 2, true);
            AddNumericField("Стоимость:*", ref numEstimatedValue, topMargin + spacing * 3);
            AddComboField("Состояние:*", ref cmbCondition, topMargin + spacing * 4,
                new[] { "Отличное", "Хорошее", "Удовлетворительное", "Плохое" });
            
            btnSave = CreateButton("Сохранить", 190, topMargin + spacing * 5 + 20, Color.FromArgb(46, 204, 113));
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);
            
            btnCancel = CreateButton("Отмена", 360, topMargin + spacing * 5 + 20, Color.FromArgb(231, 76, 60));
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            this.Controls.Add(btnCancel);
        }
        
        private void AddField(string label, ref TextBox textBox, int y, bool multiline = false)
        {
            this.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(20, y + 5),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            });
            
            textBox = new TextBox
            {
                Location = new Point(190, y),
                Size = new Size(300, multiline ? 50 : 30),
                Font = new Font("Segoe UI", 10),
                Multiline = multiline
            };
            this.Controls.Add(textBox);
        }
        
        private void AddComboField(string label, ref ComboBox combo, int y, string[] items)
        {
            this.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(20, y + 5),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            });
            
            combo = new ComboBox
            {
                Location = new Point(190, y),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            combo.Items.AddRange(items);
            this.Controls.Add(combo);
        }
        
        private void AddNumericField(string label, ref NumericUpDown numeric, int y)
        {
            this.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(20, y + 5),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            });
            
            numeric = new NumericUpDown
            {
                Location = new Point(190, y),
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 10),
                Maximum = 10000000,
                Minimum = 0,
                DecimalPlaces = 0,
                ThousandsSeparator = true
            };
            this.Controls.Add(numeric);
        }
        
        private Button CreateButton(string text, int x, int y, Color color)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(150, 40),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
        }
        
        private void LoadData()
        {
            try
            {
                txtName.Text = PledgeItem.Name;
                if (cmbCategory.Items.Contains(PledgeItem.Category))
                    cmbCategory.SelectedItem = PledgeItem.Category;
                txtDescription.Text = PledgeItem.Description;
                numEstimatedValue.Value = PledgeItem.EstimatedValue;
                if (cmbCondition.Items.Contains(PledgeItem.Condition))
                    cmbCondition.SelectedItem = PledgeItem.Condition;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Заполните наименование!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }
                
                if (cmbCategory.SelectedIndex == -1 || cmbCondition.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите категорию и состояние!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (numEstimatedValue.Value <= 0)
                {
                    MessageBox.Show("Стоимость должна быть больше нуля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    numEstimatedValue.Focus();
                    return;
                }
                
                PledgeItem.Name = txtName.Text.Trim();
                PledgeItem.Category = cmbCategory.SelectedItem?.ToString() ?? string.Empty;
                PledgeItem.Description = txtDescription.Text.Trim();
                PledgeItem.EstimatedValue = numEstimatedValue.Value;
                PledgeItem.Condition = cmbCondition.SelectedItem?.ToString() ?? string.Empty;
                
                PledgeItem.Validate();
                
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка валидации:\n{ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения:\n{ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Валидация названия: предупреждение о недопустимых символах
        private void AddNameValidation(TextBox textBox)
        {
            textBox.KeyPress += (s, e) =>
            {
                try
                {
                    // Запрещаем недопустимые спецсимволы для названия
                    char[] forbiddenChars = { '<', '>', '|', '\\', '/', ':', '*', '?', '"' };
                    if (forbiddenChars.Contains(e.KeyChar) && !char.IsControl(e.KeyChar))
                    {
                        e.Handled = true;
                        MessageBox.Show("В названии нельзя использовать символы: < > | \\ / : * ? \"\n(Эти символы запрещены в именах файлов)",
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
