using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace lab11
{
    public partial class Form1 : Form
    {
        private TabControl tabControl;
        private Button btnOpenStringWork;
        private Button btnOpenDatabase;

        public Form1()
        {
            InitializeComponent();
            SetupCustomUI();
        }

        private void SetupCustomUI()
        {
            this.Text = "Лабораторная работа №11 - Обработка исключений";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Главная панель
            Panel panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(41, 128, 185)
            };

            Label lblTitle = new Label
            {
                Text = "Лабораторная работа №11\nОбработка исключений и валидация данных",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            panelHeader.Controls.Add(lblTitle);

            // Панель навигации
            Panel panelNav = new Panel
            {
                Dock = DockStyle.Left,
                Width = 200,
                BackColor = Color.FromArgb(52, 73, 94),
                Padding = new Padding(10)
            };

            btnOpenStringWork = CreateNavButton("Работа со строками", 10);
            btnOpenStringWork.Click += (s, e) => OpenForm2();
            panelNav.Controls.Add(btnOpenStringWork);

            btnOpenDatabase = CreateNavButton("База данных\nЛомбарда", 70);
            btnOpenDatabase.Click += (s, e) => OpenForm3();
            panelNav.Controls.Add(btnOpenDatabase);

            // TabControl для математических расчётов
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            // Вкладка 1: Расчет функции
            TabPage tabCalc1 = new TabPage("Расчет функции y");
            SetupCalculationTab(tabCalc1);
            tabControl.TabPages.Add(tabCalc1);

            // Вкладка 2: Расчет суммы ряда
            TabPage tabCalc2 = new TabPage("Расчет суммы ряда");
            SetupSeriesTab(tabCalc2);
            tabControl.TabPages.Add(tabCalc2);

            this.Controls.Add(tabControl);
            this.Controls.Add(panelNav);
            this.Controls.Add(panelHeader);
        }

        private Button CreateNavButton(string text, int top)
        {
            return new Button
            {
                Text = text,
                Location = new Point(10, top),
                Size = new Size(180, 50),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
        }

        #region Вкладка расчета функции y
        private TextBox txtA, txtB, txtP, txtS, txtT, txtX0, txtXk, txtDx;
        private ListBox listBox1;
        private Button btnCalc, btnClear1;

        private void SetupCalculationTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            int labelWidth = 150;
            int textBoxWidth = 200;
            int leftMargin = 20;
            int topMargin = 20;
            int verticalSpacing = 40;

            // Заголовок
            Label lblFormula = new Label
            {
                Text = "y = √(s+t) / (√2 * sin(x)) + ln(s/(p+s)) + (a*x)/(b+s)",
                Location = new Point(leftMargin, topMargin),
                Size = new Size(700, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185)
            };
            tab.Controls.Add(lblFormula);

            int currentY = topMargin + 50;

            // Параметры
            AddLabelAndTextBox(tab, "Параметр a:", ref txtA, leftMargin, currentY);
            currentY += verticalSpacing;
            AddLabelAndTextBox(tab, "Параметр b:", ref txtB, leftMargin, currentY);
            currentY += verticalSpacing;
            AddLabelAndTextBox(tab, "Параметр p:", ref txtP, leftMargin, currentY);
            currentY += verticalSpacing;
            AddLabelAndTextBox(tab, "Параметр s:", ref txtS, leftMargin, currentY);
            currentY += verticalSpacing;
            AddLabelAndTextBox(tab, "Параметр t:", ref txtT, leftMargin, currentY);
            currentY += verticalSpacing;
            AddLabelAndTextBox(tab, "x₀ (начало):", ref txtX0, leftMargin, currentY);
            currentY += verticalSpacing;
            AddLabelAndTextBox(tab, "xₖ (конец):", ref txtXk, leftMargin, currentY);
            currentY += verticalSpacing;
            AddLabelAndTextBox(tab, "Δx (шаг):", ref txtDx, leftMargin, currentY);

            // ListBox для результатов
            listBox1 = new ListBox
            {
                Location = new Point(leftMargin + 400, topMargin + 50),
                Size = new Size(350, 400),
                Font = new Font("Consolas", 10)
            };
            tab.Controls.Add(listBox1);

            // Кнопки
            btnCalc = new Button
            {
                Text = "Вычислить",
                Location = new Point(leftMargin, currentY + 40),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCalc.Click += BtnCalc_Click;
            tab.Controls.Add(btnCalc);

            btnClear1 = new Button
            {
                Text = "Очистить",
                Location = new Point(leftMargin + 170, currentY + 40),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnClear1.Click += (s, e) =>
            {
                ClearTextBoxes(txtA, txtB, txtP, txtS, txtT, txtX0, txtXk, txtDx);
                listBox1.Items.Clear();
            };
            tab.Controls.Add(btnClear1);

            // Значения по умолчанию
            txtA.Text = "1";
            txtB.Text = "2";
            txtP.Text = "1";
            txtS.Text = "2";
            txtT.Text = "3";
            txtX0.Text = "0,1"; // поддержка запятой по умолчанию
            txtXk.Text = "3,14";
            txtDx.Text = "0,1";
        }

        private void BtnCalc_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();

                // Парсинг входных данных с обработкой исключений
                if (!TryParseDouble(txtA.Text, out double a))
                {
                    MessageBox.Show("Неверный формат параметра 'a'. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtA.Focus();
                    return;
                }

                if (!TryParseDouble(txtB.Text, out double b))
                {
                    MessageBox.Show("Неверный формат параметра 'b'. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtB.Focus();
                    return;
                }

                if (!TryParseDouble(txtP.Text, out double p))
                {
                    MessageBox.Show("Неверный формат параметра 'p'. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtP.Focus();
                    return;
                }

                if (!TryParseDouble(txtS.Text, out double s))
                {
                    MessageBox.Show("Неверный формат параметра 's'. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtS.Focus();
                    return;
                }

                if (!TryParseDouble(txtT.Text, out double t))
                {
                    MessageBox.Show("Неверный формат параметра 't'. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtT.Focus();
                    return;
                }

                if (!TryParseDouble(txtX0.Text, out double x0))
                {
                    MessageBox.Show("Неверный формат начального значения x₀. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtX0.Focus();
                    return;
                }

                if (!TryParseDouble(txtXk.Text, out double xk))
                {
                    MessageBox.Show("Неверный формат конечного значения xₖ. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtXk.Focus();
                    return;
                }

                if (!TryParseDouble(txtDx.Text, out double dx))
                {
                    MessageBox.Show("Неверный формат шага Δx. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDx.Focus();
                    return;
                }

                // Проверка корректности диапазона
                if (dx <= 0)
                {
                    MessageBox.Show("Шаг Δx должен быть положительным числом.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDx.Focus();
                    return;
                }

                if (x0 >= xk)
                {
                    MessageBox.Show("Начальное значение x₀ должно быть меньше конечного xₖ.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                listBox1.Items.Add(string.Format("{0,-15}{1,20}", "x", "y"));
                listBox1.Items.Add(new string('-', 50));

                int calculatedCount = 0;
                for (double x = x0; x <= xk + 1e-9; x += dx)
                {
                    try
                    {
                        // Проверка условий для вычисления
                        if (s + t < 0)
                            throw new ArithmeticException($"При x={x:F4}: s+t={s + t:F4} < 0. Невозможно извлечь корень.");

                        if (Math.Abs(Math.Sin(x)) < 1e-15)
                            throw new DivideByZeroException($"При x={x:F4}: sin(x) ≈ 0. Деление на ноль.");

                        if (s <= 0)
                            throw new ArithmeticException($"При x={x:F4}: s={s:F4} <= 0. Логарифм не определен.");

                        if (p + s <= 0)
                            throw new ArithmeticException($"При x={x:F4}: p+s={p + s:F4} <= 0. Логарифм не определен.");

                        if (Math.Abs(b + s) < 1e-15)
                            throw new DivideByZeroException($"При x={x:F4}: b+s ≈ 0. Деление на ноль.");

                        double term1 = Math.Sqrt(s + t) / (Math.Sqrt(2) * Math.Sin(x));
                        double term2 = Math.Log(s / (p + s));
                        double term3 = (a * x) / (b + s);

                        double y = term1 + term2 + term3;

                        listBox1.Items.Add(string.Format("{0,-15:F4}{1,20:F6}", x, y));
                        calculatedCount++;
                    }
                    catch (DivideByZeroException ex)
                    {
                        listBox1.Items.Add($"x={x:F4}  ⚠ {ex.Message}");
                    }
                    catch (ArithmeticException ex)
                    {
                        listBox1.Items.Add($"x={x:F4}  ⚠ {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        listBox1.Items.Add($"x={x:F4}  ⚠ Неизвестная ошибка: {ex.Message}");
                    }
                }

                listBox1.Items.Add(new string('-', 50));
                listBox1.Items.Add($"Успешно вычислено точек: {calculatedCount}");

                if (calculatedCount == 0)
                {
                    MessageBox.Show("Не удалось вычислить ни одной точки. Проверьте входные параметры.",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка при вычислении:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Вкладка расчета суммы ряда
        private TextBox txtN, txtX;
        private ListBox listBox2;
        private Button btnCalcSeries, btnClear2;

        private void SetupSeriesTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            int leftMargin = 20;
            int topMargin = 20;

            // Заголовок
            Label lblFormula = new Label
            {
                Text = "Вычисление суммы: S = Σ(xᵏ / k!)  где k от 1 до n",
                Location = new Point(leftMargin, topMargin),
                Size = new Size(700, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185)
            };
            tab.Controls.Add(lblFormula);

            AddLabelAndTextBox(tab, "Количество членов n:", ref txtN, leftMargin, topMargin + 60);
            AddLabelAndTextBox(tab, "Значение x:", ref txtX, leftMargin, topMargin + 110);

            listBox2 = new ListBox
            {
                Location = new Point(leftMargin + 400, topMargin + 60),
                Size = new Size(350, 400),
                Font = new Font("Consolas", 10)
            };
            tab.Controls.Add(listBox2);

            btnCalcSeries = new Button
            {
                Text = "Вычислить сумму",
                Location = new Point(leftMargin, topMargin + 160),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnCalcSeries.Click += BtnCalcSeries_Click;
            tab.Controls.Add(btnCalcSeries);

            btnClear2 = new Button
            {
                Text = "Очистить",
                Location = new Point(leftMargin + 170, topMargin + 160),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnClear2.Click += (s, e) =>
            {
                txtN.Clear();
                txtX.Clear();
                listBox2.Items.Clear();
            };
            tab.Controls.Add(btnClear2);

            // Значения по умолчанию
            txtN.Text = "10";
            txtX.Text = "1";
        }

        private void BtnCalcSeries_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();

                if (!int.TryParse(txtN.Text, out int n))
                {
                    MessageBox.Show("Неверный формат параметра 'n'. Введите целое число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtN.Focus();
                    return;
                }

                if (!TryParseDouble(txtX.Text, out double x))
                {
                    MessageBox.Show("Неверный формат параметра 'x'. Введите число.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtX.Focus();
                    return;
                }

                if (n <= 0)
                {
                    MessageBox.Show("Параметр 'n' должен быть положительным целым числом.",
                        "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtN.Focus();
                    return;
                }

                if (n > 1000)
                {
                    MessageBox.Show("Слишком большое значение 'n' (макс. 1000). Это может привести к переполнению.",
                        "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                listBox2.Items.Add($"Вычисление суммы для n={n}, x={x:F4}");
                listBox2.Items.Add(new string('-', 50));
                listBox2.Items.Add(string.Format("{0,-10}{1,-15}{2,20}", "k", "xᵏ/k!", "Сумма S"));
                listBox2.Items.Add(new string('-', 50));

                double sum = 0;
                for (int k = 1; k <= n; k++)
                {
                    try
                    {
                        double factorial = CalculateFactorial(k);
                        double term = Math.Pow(x, k) / factorial;

                        if (double.IsInfinity(term) || double.IsNaN(term))
                        {
                            throw new OverflowException($"Переполнение при k={k}");
                        }

                        sum += term;
                        listBox2.Items.Add(string.Format("{0,-10}{1,-15:E4}{2,20:F6}", k, term, sum));
                    }
                    catch (OverflowException ex)
                    {
                        listBox2.Items.Add($"k={k}  ⚠ {ex.Message}");
                        MessageBox.Show($"Переполнение при k={k}. Вычисления остановлены.",
                            "Ошибка переполнения", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        break;
                    }
                }

                listBox2.Items.Add(new string('-', 50));
                listBox2.Items.Add($"Итоговая сумма S = {sum:F6}");
                listBox2.Items.Add($"Для сравнения: eˣ - 1 = {Math.Exp(x) - 1:F6}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Критическая ошибка при вычислении:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double CalculateFactorial(int n)
        {
            if (n < 0)
                throw new ArgumentException("Факториал отрицательного числа не определен");

            if (n > 170)
                throw new OverflowException("Слишком большое значение для вычисления факториала");

            double result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        #endregion

        #region Вспомогательные методы
        private void AddLabelAndTextBox(Control parent, string labelText, ref TextBox textBox, int x, int y)
        {
            Label label = new Label
            {
                Text = labelText,
                Location = new Point(x, y + 5),
                Size = new Size(150, 20),
                Font = new Font("Segoe UI", 10)
            };
            parent.Controls.Add(label);

            textBox = new TextBox
            {
                Location = new Point(x + 160, y),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10)
            };
            parent.Controls.Add(textBox);
        }

        private void ClearTextBoxes(params TextBox[] textBoxes)
        {
            foreach (var tb in textBoxes)
            {
                if (tb != null) tb.Clear();
            }
        }

        private bool TryParseDouble(string s, out double value)
        {
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
                return true;
            if (double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                return true;
            var t = s?.Replace(',', '.');
            return double.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        private void OpenForm2()
        {
            try
            {
                Form2 form2 = new Form2();
                form2.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы работы со строками:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenForm3()
        {
            try
            {
                Form3 form3 = new Form3();
                form3.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы базы данных:\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
