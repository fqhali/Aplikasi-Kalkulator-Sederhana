using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace CalculatorApp;

public partial class Form1 : Form
{
    private double firstNumber = 0;
    private double secondNumber = 0;
    private double result = 0;
    private string operation = "";
    private bool isNewEntry = true;

    private Label lblTitle;
    private TextBox txtDisplay;

    public Form1()
    {
        InitializeComponent();

        KeyPreview = true;
        KeyDown += Form1_KeyDown;

        // Label Judul
        lblTitle = new Label();
        lblTitle.Text = "Kalkulator";
        lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(30, 41, 59);
        lblTitle.AutoSize = true;
        lblTitle.Location = new Point(20, 15);
        Controls.Add(lblTitle);

        // Layar Display Angka / Hasil
        txtDisplay = new TextBox();
        txtDisplay.Text = "0";
        txtDisplay.ReadOnly = true;
        txtDisplay.Font = new Font("Segoe UI", 18, FontStyle.Regular);
        txtDisplay.TextAlign = HorizontalAlignment.Right;
        txtDisplay.BackColor = Color.White;
        txtDisplay.Location = new Point(20, 48);
        txtDisplay.Width = 275;
        Controls.Add(txtDisplay);

        // Baris 1: 7, 8, 9, ÷
        AddButton("btn7", "7", 20, 100, NumberButton_Click);
        AddButton("btn8", "8", 90, 100, NumberButton_Click);
        AddButton("btn9", "9", 160, 100, NumberButton_Click);
        AddButton("btnDivide", "÷", 230, 100, OperatorButton_Click, isOperator: true);

        // Baris 2: 4, 5, 6, ×
        AddButton("btn4", "4", 20, 160, NumberButton_Click);
        AddButton("btn5", "5", 90, 160, NumberButton_Click);
        AddButton("btn6", "6", 160, 160, NumberButton_Click);
        AddButton("btnMultiply", "×", 230, 160, OperatorButton_Click, isOperator: true);

        // Baris 3: 1, 2, 3, -
        AddButton("btn1", "1", 20, 220, NumberButton_Click);
        AddButton("btn2", "2", 90, 220, NumberButton_Click);
        AddButton("btn3", "3", 160, 220, NumberButton_Click);
        AddButton("btnMinus", "-", 230, 220, OperatorButton_Click, isOperator: true);

        // Baris 4: 0, ., C, +
        AddButton("btn0", "0", 20, 280, NumberButton_Click);
        AddButton("btnDecimal", ".", 90, 280, btnDecimal_Click);
        AddButton("btnClear", "C", 160, 280, btnClear_Click, isClear: true);
        AddButton("btnPlus", "+", 230, 280, OperatorButton_Click, isOperator: true);

        // Baris 5: = (Sama Dengan)
        AddButton("btnEquals", "=", 20, 340, btnEquals_Click, width: 275, isEqual: true);
    }

    private void AddButton(
        string name,
        string text,
        int x,
        int y,
        EventHandler clickHandler,
        int width = 65,
        int height = 50,
        bool isOperator = false,
        bool isClear = false,
        bool isEqual = false)
    {
        Button btn = new Button();
        btn.Name = name;
        btn.Text = text;
        btn.Location = new Point(x, y);
        btn.Width = width;
        btn.Height = height;
        btn.Font = new Font("Segoe UI", 13, FontStyle.Bold);
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 1;
        btn.Cursor = Cursors.Hand;

        if (isEqual)
        {
            btn.BackColor = Color.FromArgb(22, 163, 74);
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderColor = Color.FromArgb(21, 128, 61);
        }
        else if (isClear)
        {
            btn.BackColor = Color.FromArgb(220, 38, 38);
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderColor = Color.FromArgb(185, 28, 28);
        }
        else if (isOperator)
        {
            btn.BackColor = Color.FromArgb(2, 132, 199);
            btn.ForeColor = Color.White;
            btn.FlatAppearance.BorderColor = Color.FromArgb(3, 105, 161);
        }
        else
        {
            btn.BackColor = Color.FromArgb(241, 245, 249);
            btn.ForeColor = Color.FromArgb(15, 23, 42);
            btn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);
        }

        btn.Click += clickHandler;
        Controls.Add(btn);
    }

    private void NumberButton_Click(object? sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            InputDigit(btn.Text);
        }
    }

    private void InputDigit(string digit)
    {
        if (isNewEntry || txtDisplay.Text == "0")
        {
            txtDisplay.Text = digit;
            isNewEntry = false;
        }
        else
        {
            txtDisplay.Text += digit;
        }
    }

    private void OperatorButton_Click(object? sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            ApplyOperator(btn.Text);
        }
    }

    private void ApplyOperator(string op)
    {
        if (double.TryParse(txtDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out double currentVal))
        {
            firstNumber = currentVal;
            operation = op;
            isNewEntry = true;
        }
    }

    private void btnEquals_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(operation))
        {
            return;
        }

        if (double.TryParse(txtDisplay.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out secondNumber))
        {
            switch (operation)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "-":
                    result = firstNumber - secondNumber;
                    break;
                case "×":
                case "x":
                case "*":
                    result = firstNumber * secondNumber;
                    break;
                case "÷":
                case "/":
                    if (Math.Abs(secondNumber) < double.Epsilon)
                    {
                        MessageBox.Show("Tidak dapat membagi dengan nol!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnClear_Click(sender, e);
                        return;
                    }
                    result = firstNumber / secondNumber;
                    break;
                default:
                    return;
            }

            txtDisplay.Text = result.ToString(CultureInfo.InvariantCulture);
            firstNumber = result;
            operation = "";
            isNewEntry = true;
        }
    }

    private void btnClear_Click(object? sender, EventArgs e)
    {
        firstNumber = 0;
        secondNumber = 0;
        result = 0;
        operation = "";
        isNewEntry = true;
        txtDisplay.Text = "0";
    }

    private void btnDecimal_Click(object? sender, EventArgs e)
    {
        if (isNewEntry)
        {
            txtDisplay.Text = "0.";
            isNewEntry = false;
        }
        else if (!txtDisplay.Text.Contains('.'))
        {
            txtDisplay.Text += ".";
        }
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && !e.Shift)
        {
            InputDigit(((int)e.KeyCode - (int)Keys.D0).ToString());
            e.Handled = true;
        }
        else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
        {
            InputDigit(((int)e.KeyCode - (int)Keys.NumPad0).ToString());
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Add || (e.KeyCode == Keys.Oemplus && e.Shift))
        {
            ApplyOperator("+");
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
        {
            ApplyOperator("-");
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Multiply || (e.KeyCode == Keys.D8 && e.Shift))
        {
            ApplyOperator("×");
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Divide || e.KeyCode == Keys.OemQuestion)
        {
            ApplyOperator("÷");
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Enter)
        {
            btnEquals_Click(this, EventArgs.Empty);
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Escape)
        {
            btnClear_Click(this, EventArgs.Empty);
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod)
        {
            btnDecimal_Click(this, EventArgs.Empty);
            e.Handled = true;
        }
    }
}
