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

        public string Screen {
            get => textBoxResult.Text;
            set => textBoxResult.Text = value;
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
            if (Screen == "0" || calculationDone)
            {
                Screen = "";
                calculationDone = false;
            }

            // don't let the user write numbers starting with 0
            // unless they follow up with ","                
            if (Screen.EndsWith(" 0"))
                Screen = Screen.Substring(0, Screen.Length - 1);
            AppendNumber(button);
        }

        private void buttonOperator_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string button = clickedButton.Text;

            if (calculationDone)
            {
                Screen = lastResult.ToString();
                calculationDone = false;
            }

            // last input also was an operator
            if (Screen.EndsWith(" "))
                OverwriteLastOperator(button);
            // last input was a number
            else
                AppendOperator(button);
        }

        private string OverwriteLastOperator(string operation)
        {
            //clear last operator and whitespace
            Screen = Screen.Substring(0, Screen.Length - 3);
            return AppendOperator(operation);
        }
        //TODO: handle input from keys, not just mouse (Keypress event)
        private string AppendOperator(string operation)
        {
            Screen = Screen.TrimEnd(',');
            return Screen += " " + operation + " ";
        }

        private string AppendNumber(string number)
        {
            return Screen += number;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            if (calculationDone) return;
            // if only a one digit number left (positive or negative) set the screen to 0
            if (Screen.Length == 1 || (Screen.Length == 2 && Screen.StartsWith("-")))
            {
                Screen = "0";
            }
            // if more than one digit left, remvove the last one
            // ignore whitespace
            if (Screen.Length > 1)
            {
                if (Screen.EndsWith(" "))
                    Screen = Screen.Substring(0, Screen.Length - 3);
                else
                {
                    Screen = Screen.Substring(0, Screen.Length - 1);
                }
            }
        }

        private void buttonNegate_Click(object sender, EventArgs e)
        {
            if (calculationDone)
            {
                Screen = lastResult.ToString();
                calculationDone = false;
            }

            if (Screen != "0" && !Screen.EndsWith(" "))
            {
                string lastNumber = Screen;
                string restOfScreen = "";
                if (Screen.Contains(" "))
                {
                    int cutHere = Screen.LastIndexOf(" ");
                    lastNumber = Screen.Substring(cutHere + 1);
                    restOfScreen = Screen.Substring(0, cutHere + 1);
                }

                if (lastNumber.StartsWith("-"))
                {
                    lastNumber = lastNumber.TrimStart('-');
                }
                else
                {
                    lastNumber = "-" + lastNumber;
                }
                Screen = restOfScreen + lastNumber;
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            Screen = "0";
        }

        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            if (calculationDone) return;
            if (!Screen.EndsWith(" ") && !Screen.EndsWith("-"))
            {
                //this is the first number or the screen ends with a number not containing ","
                if (!Screen.Contains(" ") || Screen.LastIndexOf(" ") > Screen.LastIndexOf(","))
                    AppendNumber(",");
            }

        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            if (calculationDone) return;
            string[] calcString = Screen.Split(' ');
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
            Screen += Environment.NewLine + tempResult;
        }

        //TODO: after calculate, at next click clean everything and put result in the first line
        //TODO: if lastresult was NaN, set lastresult to 0 before calculating with it
    }
}
