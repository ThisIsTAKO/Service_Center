using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lab11
{
    public class Form2 : Form
    {
        private TabControl tabControl;
        private TextBox txtTask1Input, txtTask2Input, txtTask3Num1, txtTask3Num2;
        private TextBox txtTask1Result, txtTask2Result, txtTask3Result;
        private RichTextBox rtbTask1Details, rtbTask2Details, rtbTask3Details;
        private Button btnTask1, btnTask2, btnTask3, btnClearAll;

        public Form2()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Лабораторная работа №11 - Работа со строками";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Заголовок
            Panel panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(142, 68, 173)
            };

            Label lblTitle = new Label
            {
                Text = "Работа со строками\nс обработкой исключений",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            panelHeader.Controls.Add(lblTitle);

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            // Вкладка 1: Поиск последовательности
            TabPage tab1 = new TabPage("Поиск «,-»");
            SetupTask1(tab1);
            tabControl.TabPages.Add(tab1);

            // Вкладка 2: ФИО в инициалы
            TabPage tab2 = new TabPage("ФИО → Инициалы");
            SetupTask2(tab2);
            tabControl.TabPages.Add(tab2);

            // Вкладка 3: Объединение чисел
            TabPage tab3 = new TabPage("Объединение чисел");
            SetupTask3(tab3);
            tabControl.TabPages.Add(tab3);

            // Кнопка "Очистить всё"
            Panel panelBottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(236, 240, 241)
            };

            btnClearAll = new Button
            {
                Text = "Очистить всё",
                Location = new Point(10, 10),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnClearAll.Click += (s, e) => ClearAllFields();
            panelBottom.Controls.Add(btnClearAll);

            this.Controls.Add(tabControl);
            this.Controls.Add(panelBottom);
            this.Controls.Add(panelHeader);
        }

        #region Задача 1: Поиск последовательности «,-»
        private void SetupTask1(TabPage tab)
        {
            tab.BackColor = Color.White;
            int margin = 20;

            Label lblDescription = new Label
            {
                Text = "Задача: Проверить, есть ли в строке последовательность «,-»",
                Location = new Point(margin, margin),
                Size = new Size(800, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            tab.Controls.Add(lblDescription);

            Label lblInput = new Label
            {
                Text = "Введите строку:",
                Location = new Point(margin, margin + 40),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblInput);

            txtTask1Input = new TextBox
            {
                Location = new Point(margin + 160, margin + 37),
                Size = new Size(650, 25),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(txtTask1Input);

            btnTask1 = new Button
            {
                Text = "Проверить",
                Location = new Point(margin + 160, margin + 75),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnTask1.Click += BtnTask1_Click;
            tab.Controls.Add(btnTask1);

            Label lblResult = new Label
            {
                Text = "Результат:",
                Location = new Point(margin, margin + 125),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblResult);

            txtTask1Result = new TextBox
            {
                Location = new Point(margin + 160, margin + 122),
                Size = new Size(650, 25),
                Font = new Font("Segoe UI", 10),
                ReadOnly = true,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            tab.Controls.Add(txtTask1Result);

            Label lblDetails = new Label
            {
                Text = "Детальная информация:",
                Location = new Point(margin, margin + 165),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblDetails);

            rtbTask1Details = new RichTextBox
            {
                Location = new Point(margin, margin + 190),
                Size = new Size(820, 300),
                Font = new Font("Consolas", 9),
                ReadOnly = true,
                BackColor = Color.FromArgb(250, 250, 250)
            };
            tab.Controls.Add(rtbTask1Details);
        }

        private void BtnTask1_Click(object sender, EventArgs e)
        {
            try
            {
                string inputString = txtTask1Input.Text;

                if (string.IsNullOrEmpty(inputString))
                {
                    MessageBox.Show("Пожалуйста, введите строку для проверки!",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTask1Input.Focus();
                    return;
                }

                bool foundSequence = false;
                StringBuilder details = new StringBuilder();
                details.AppendLine($"Анализ строки: \"{inputString}\"");
                details.AppendLine($"Длина строки: {inputString.Length} символов");
                details.AppendLine();

                // Поиск последовательности «,-»
                for (int i = 0; i < inputString.Length - 1; i++)
                {
                    if (inputString[i] == ',' && inputString[i + 1] == '-')
                    {
                        foundSequence = true;
                        details.AppendLine($"✓ Найдена последовательность «,-» на позициях {i} и {i + 1}");
                    }
                }

                if (!foundSequence)
                {
                    details.AppendLine("✗ Последовательность «,-» не найдена");
                }

                string result = foundSequence
                    ? "В строке НАЙДЕНА последовательность «,-»"
                    : "В строке НЕТ последовательности «,-»";

                txtTask1Result.Text = result;
                rtbTask1Details.Text = details.ToString();

                MessageBox.Show(result, "Результат проверки", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке строки:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Задача 2: Преобразование ФИО
        private void SetupTask2(TabPage tab)
        {
            tab.BackColor = Color.White;
            int margin = 20;

            Label lblDescription = new Label
            {
                Text = "Задача: Преобразовать ФИО в формат «Фамилия И.О.»",
                Location = new Point(margin, margin),
                Size = new Size(800, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            tab.Controls.Add(lblDescription);

            Label lblInput = new Label
            {
                Text = "Введите ФИО:",
                Location = new Point(margin, margin + 40),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblInput);

            txtTask2Input = new TextBox
            {
                Location = new Point(margin + 160, margin + 37),
                Size = new Size(650, 25),
                Font = new Font("Segoe UI", 10),
                Text = "Иванов Петр Сидорович"
            };
            // Блокировка ввода цифр
            txtTask2Input.KeyPress += (s, e) =>
            {
                try
                {
                    // Разрешаем только буквы, пробелы, Backspace, дефис
                    if (char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    {
                        e.Handled = true; // Блокируем ввод
                        MessageBox.Show("В поле ФИО нельзя вводить цифры!",
                            "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при проверке ввода:\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            tab.Controls.Add(txtTask2Input);

            btnTask2 = new Button
            {
                Text = "Преобразовать",
                Location = new Point(margin + 160, margin + 75),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnTask2.Click += BtnTask2_Click;
            tab.Controls.Add(btnTask2);

            Label lblResult = new Label
            {
                Text = "Результат:",
                Location = new Point(margin, margin + 125),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblResult);

            txtTask2Result = new TextBox
            {
                Location = new Point(margin + 160, margin + 122),
                Size = new Size(650, 25),
                Font = new Font("Segoe UI", 10),
                ReadOnly = true,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            tab.Controls.Add(txtTask2Result);

            Label lblDetails = new Label
            {
                Text = "Детальная информация:",
                Location = new Point(margin, margin + 165),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblDetails);

            rtbTask2Details = new RichTextBox
            {
                Location = new Point(margin, margin + 190),
                Size = new Size(820, 300),
                Font = new Font("Consolas", 9),
                ReadOnly = true,
                BackColor = Color.FromArgb(250, 250, 250)
            };
            tab.Controls.Add(rtbTask2Details);
        }

        private void BtnTask2_Click(object sender, EventArgs e)
        {
            try
            {
                string fullName = txtTask2Input.Text.Trim();

                if (string.IsNullOrEmpty(fullName))
                {
                    MessageBox.Show("Пожалуйста, введите ФИО!",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTask2Input.Focus();
                    return;
                }

                string[] nameParts = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (nameParts.Length < 2)
                {
                    MessageBox.Show("Введите как минимум Фамилию и Имя (через пробел)!",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTask2Input.Focus();
                    return;
                }

                StringBuilder result = new StringBuilder();
                StringBuilder details = new StringBuilder();

                details.AppendLine($"Исходное ФИО: \"{fullName}\"");
                details.AppendLine($"Количество частей: {nameParts.Length}");
                details.AppendLine();

                // Фамилия
                string surname = nameParts[0];
                result.Append(surname);
                details.AppendLine($"Фамилия: \"{surname}\"");

                // Инициалы
                for (int i = 1; i < nameParts.Length && i <= 2; i++)
                {
                    if (nameParts[i].Length > 0)
                    {
                        char initial = char.ToUpper(nameParts[i][0]);
                        result.Append($" {initial}.");
                        string partName = i == 1 ? "Имя" : "Отчество";
                        details.AppendLine($"{partName}: \"{nameParts[i]}\" → Инициал: \"{initial}.\"");
                    }
                }

                details.AppendLine();
                details.AppendLine($"Итоговый результат: \"{result}\"");

                txtTask2Result.Text = result.ToString();
                rtbTask2Details.Text = details.ToString();

                MessageBox.Show($"ФИО успешно преобразовано:\n{result}",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Ошибка при обработке частей имени. Проверьте формат ввода.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при преобразовании ФИО:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Задача 3: Объединение чисел
        private void SetupTask3(TabPage tab)
        {
            tab.BackColor = Color.White;
            int margin = 20;

            Label lblDescription = new Label
            {
                Text = "Задача: Объединить два целых числа в одну строку",
                Location = new Point(margin, margin),
                Size = new Size(800, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            tab.Controls.Add(lblDescription);

            Label lblNum1 = new Label
            {
                Text = "Первое число:",
                Location = new Point(margin, margin + 40),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblNum1);

            txtTask3Num1 = new TextBox
            {
                Location = new Point(margin + 160, margin + 37),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                Text = "123"
            };
            tab.Controls.Add(txtTask3Num1);

            Label lblNum2 = new Label
            {
                Text = "Второе число:",
                Location = new Point(margin, margin + 80),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblNum2);

            txtTask3Num2 = new TextBox
            {
                Location = new Point(margin + 160, margin + 77),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                Text = "456"
            };
            tab.Controls.Add(txtTask3Num2);

            btnTask3 = new Button
            {
                Text = "Объединить",
                Location = new Point(margin + 160, margin + 115),
                Size = new Size(150, 35),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnTask3.Click += BtnTask3_Click;
            tab.Controls.Add(btnTask3);

            Label lblResult = new Label
            {
                Text = "Результат:",
                Location = new Point(margin, margin + 165),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblResult);

            txtTask3Result = new TextBox
            {
                Location = new Point(margin + 160, margin + 162),
                Size = new Size(650, 25),
                Font = new Font("Segoe UI", 10),
                ReadOnly = true,
                BackColor = Color.FromArgb(236, 240, 241)
            };
            tab.Controls.Add(txtTask3Result);

            Label lblDetails = new Label
            {
                Text = "Детальная информация:",
                Location = new Point(margin, margin + 205),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10)
            };
            tab.Controls.Add(lblDetails);

            rtbTask3Details = new RichTextBox
            {
                Location = new Point(margin, margin + 230),
                Size = new Size(820, 260),
                Font = new Font("Consolas", 9),
                ReadOnly = true,
                BackColor = Color.FromArgb(250, 250, 250)
            };
            tab.Controls.Add(rtbTask3Details);
        }

        private void BtnTask3_Click(object sender, EventArgs e)
        {
            try
            {
                string num1Text = txtTask3Num1.Text.Trim();
                string num2Text = txtTask3Num2.Text.Trim();

                if (string.IsNullOrEmpty(num1Text) || string.IsNullOrEmpty(num2Text))
                {
                    MessageBox.Show("Пожалуйста, введите оба числа!",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(num1Text, out int number1))
                {
                    MessageBox.Show("Первое значение не является корректным целым числом!",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTask3Num1.Focus();
                    return;
                }

                if (!int.TryParse(num2Text, out int number2))
                {
                    MessageBox.Show("Второе значение не является корректным целым числом!",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTask3Num2.Focus();
                    return;
                }

                string str1 = number1.ToString();
                string str2 = number2.ToString();
                string combinedString = str1 + str2;

                StringBuilder details = new StringBuilder();
                details.AppendLine("Входные данные:");
                details.AppendLine($"  Первое число: {number1}");
                details.AppendLine($"  Второе число: {number2}");
                details.AppendLine();
                details.AppendLine("Преобразование в строки:");
                details.AppendLine($"  Строка 1: \"{str1}\"");
                details.AppendLine($"  Строка 2: \"{str2}\"");
                details.AppendLine();
                details.AppendLine("Результат объединения:");
                details.AppendLine($"  Объединенная строка: \"{combinedString}\"");
                details.AppendLine($"  Длина результата: {combinedString.Length} символов");
                details.AppendLine();
                details.AppendLine("Дополнительные операции:");
                details.AppendLine($"  Обратная строка: \"{new string(combinedString.Reverse().ToArray())}\"");
                details.AppendLine($"  Сумма чисел: {number1 + number2}");
                details.AppendLine($"  Произведение чисел: {number1 * number2}");

                txtTask3Result.Text = combinedString;
                rtbTask3Details.Text = details.ToString();

                MessageBox.Show($"Числа успешно объединены:\n{number1} + {number2} = \"{combinedString}\"",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка формата числа. Введите корректные целые числа.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (OverflowException)
            {
                MessageBox.Show("Число слишком большое или слишком маленькое для типа Int32.",
                    "Ошибка переполнения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при объединении чисел:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void ClearAllFields()
        {
            try
            {
                txtTask1Input.Clear();
                txtTask1Result.Clear();
                rtbTask1Details.Clear();

                txtTask2Input.Clear();
                txtTask2Result.Clear();
                rtbTask2Details.Clear();

                txtTask3Num1.Clear();
                txtTask3Num2.Clear();
                txtTask3Result.Clear();
                rtbTask3Details.Clear();

                MessageBox.Show("Все поля очищены!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при очистке полей:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
