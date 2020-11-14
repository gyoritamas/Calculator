using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();
        }

        bool calculationDone = false;
        double lastResult = 0;
        private void buttonNumber_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string button = clickedButton.Text;

            // the very first input is number:
            // overwrite 0 the program starts with
            // also reset the result of the previous calculation
            if (textBoxResult.Text == "0" || calculationDone)
            {
                textBoxResult.Text = "";
                calculationDone = false;
            }
            
            // don't let the user write numbers starting with 0
            // unless they follow up with ","                
            if (textBoxResult.Text.EndsWith(" 0"))
                textBoxResult.Text = textBoxResult.Text.Substring(0, textBoxResult.Text.Length - 1);
            AppendNumber(button);
        }

        private void buttonOperator_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string button = clickedButton.Text;

            if (calculationDone)
            {
                textBoxResult.Text = lastResult.ToString();
                calculationDone = false;
            }

            // last input also was an operator
            if (textBoxResult.Text.EndsWith(" "))
                OverwriteLastOperator(button);
            // last input was a number
            else
                AppendOperator(button);
        }

        private string OverwriteLastOperator(string operation)
        {
            //clear last operator and whitespace
            textBoxResult.Text = textBoxResult.Text.Substring(0, textBoxResult.Text.Length - 3);
            return AppendOperator(operation);
        }
        //TODO: handle input from keys, not just mouse (Keypress event)
        private string AppendOperator(string operation)
        {
            textBoxResult.Text = textBoxResult.Text.TrimEnd(',');
            return textBoxResult.Text += " " + operation + " ";
        }

        private string AppendNumber(string number)
        {
            return textBoxResult.Text += number;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (calculationDone) return;
            // if only a one digit number left (positive or negative) set the screen to 0
            if (textBoxResult.TextLength == 1 || (textBoxResult.TextLength == 2 && textBoxResult.Text.StartsWith("-")))
            {
                textBoxResult.Text = "0";
            }
            // if more than one digit left, remvove the last one
            // ignore whitespace
            if (textBoxResult.TextLength > 1)
            {
                if (textBoxResult.Text.EndsWith(" "))
                    textBoxResult.Text = textBoxResult.Text.Substring(0, textBoxResult.Text.Length - 3);
                else
                {
                    textBoxResult.Text = textBoxResult.Text.Substring(0, textBoxResult.Text.Length - 1);
                }
            }
        }

        private void buttonNegate_Click(object sender, EventArgs e)
        {
            if (calculationDone)
            {
                textBoxResult.Text = lastResult.ToString();
                calculationDone = false;
            }
            
            if (textBoxResult.Text != "0" && !textBoxResult.Text.EndsWith(" "))
            {
                string lastNumber = textBoxResult.Text;
                string restOfScreen = "";
                if (textBoxResult.Text.Contains(" "))
                {
                    int cutHere = textBoxResult.Text.LastIndexOf(" ");
                    lastNumber = textBoxResult.Text.Substring(cutHere + 1);
                    restOfScreen = textBoxResult.Text.Substring(0, cutHere + 1);
                }

                if (lastNumber.StartsWith("-"))
                {
                    lastNumber = lastNumber.TrimStart('-');
                }
                else
                {
                    lastNumber = "-" + lastNumber;
                }
                textBoxResult.Text = restOfScreen + lastNumber;
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            textBoxResult.Text = "0";
        }

        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            if (calculationDone) return;
            if (!textBoxResult.Text.EndsWith(" ") && !textBoxResult.Text.EndsWith("-"))
            {
                //this is the first number or the screen ends with a number not containing ","
                if (!textBoxResult.Text.Contains(" ") || textBoxResult.Text.LastIndexOf(" ") > textBoxResult.Text.LastIndexOf(","))
                    AppendNumber(",");
            }

        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (calculationDone) return;
            string[] calcString = textBoxResult.Text.Split(' ');
            double tempResult = 0;
            double nextDouble = 0;
            for (int i = 0; i < calcString.Length;)
            {
                if (Double.TryParse(calcString[i], out double result))
                {
                    if (i == 0)
                    {
                        tempResult = result;
                        i += 2;
                        continue;
                    }
                    nextDouble = result;
                    i--;
                }
                else
                {
                    switch (calcString[i])
                    {
                        case "+":
                            tempResult += nextDouble;
                            break;
                        case "-":
                            tempResult -= nextDouble;
                            break;
                        case "*":
                            tempResult *= nextDouble;
                            break;
                        case "/":
                            tempResult /= nextDouble;
                            break;
                    }
                    i += 3;
                }
            }

            calculationDone = true;
            lastResult = tempResult;
            textBoxResult.Text += Environment.NewLine + tempResult;
        }

        //TODO: after calculate, at next click clean everything and put result in the first line

    }
}
