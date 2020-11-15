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
                
        private string Screen
        {
            get => textBoxResult.Text;
            set => textBoxResult.Text = value;
        }
        private void Append(string button)
        {
            if (char.IsDigit(button[0]) || button.Equals(","))
            {
                Screen += button;
            }
            else
            {
                Screen = Screen.TrimEnd(',');
                Screen += " " + button + " ";
            }
        }
        private void RemoveLastChars(int numberOfChars)
        {
            Screen = Screen.Substring(0, Screen.Length - numberOfChars);
        }

        private char LastChar()
        {
            return Screen[Screen.Length - 1];
        }

        bool resultOnScreen = false;
        double lastResult = 0;

        // button Click events
        private void buttonNumber_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string button = clickedButton.Text;

            Number(button);
        }

        private void buttonOperator_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string button = clickedButton.Text;

            Operator(button);
        }
        
        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void buttonNegate_Click(object sender, EventArgs e)
        {
            Negate();
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void buttonDecimal_Click(object sender, EventArgs e)
        {
            Decimal();
        }

        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        // Keypress events
        private void Calculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            char key = e.KeyChar;
            
            if (char.IsDigit(key))
            {
                Number(key.ToString());
            }

            if ("+-*/".Contains(key.ToString()))
            {
                Operator(key.ToString());
            }

            if (key.ToString().Equals(","))
            {
                Decimal();
            }
            
            if(key == (char)Keys.Back)
            {
                Clear();
            }

            if(key== (char)Keys.Return)
            {
                Calculate();
            }
            
            if (key == (char)Keys.Escape)
            {
                Application.Exit();
            }
            
        }

        private void Number(string button)
        {
            // the very first input is number:
            // overwrite 0 the program starts with
            // also reset the result of the previous calculation
            if (Screen == "0" || resultOnScreen)
            {
                Screen = "";
                resultOnScreen = false;
            }

            // don't let the user write numbers starting with 0
            // unless they follow up with ","                
            if (Screen.EndsWith(" 0"))
                RemoveLastChars(1);
            Append(button);
        }
        private void Operator(string button)
        {
            if (resultOnScreen)
            {
                Screen = lastResult.ToString();
                resultOnScreen = false;
            }

            // last input also was an operator
            if (Screen.EndsWith(" "))
                RemoveLastChars(3);

            Append(button);
        }

        /// <summary>
        /// Append "," after digits, if last number does not contain one
        /// Append "0," instead after operators
        /// </summary>
        private void Decimal()
        {
            if (resultOnScreen) return;
            if (char.IsDigit(LastChar()) && Screen.LastIndexOf(" ") >= Screen.LastIndexOf(","))
                Append(",");
            if (Screen.EndsWith(" "))
                Append("0,");
        }

        /// <summary>
        /// Clear the last character
        /// If only a one digit number left on the screen (positive or negative), set the screen to 0
        /// Ignore whitespace while deleting
        /// Does not work when result of last calculation is appearent (protection)
        /// </summary>
        private void Clear()
        {
            if (resultOnScreen) return;
            if (Screen.Length == 1 || (Screen.Length == 2 && Screen.StartsWith("-")))
            {
                Screen = "0";
            }
            if (Screen.Length > 1)
            {
                if (Screen.EndsWith(" "))
                    RemoveLastChars(3);
                else
                {
                    RemoveLastChars(1);
                }
            }
        }

        private void Delete()
        {
            Screen = "0";
        }

        private void Negate()
        {
            if (resultOnScreen)
            {
                Screen = lastResult.ToString();
                resultOnScreen = false;
            }

            if (Screen != "0" && !Screen.EndsWith(" "))
            {
                string lastNumber = Screen;
                string restOfScreen = "";
                if (Screen.Contains(" "))
                {
                    int cutHere = Screen.LastIndexOf(" ") + 1;
                    lastNumber = Screen.Substring(cutHere);
                    restOfScreen = Screen.Substring(0, cutHere);
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

        private void Calculate()
        {
            if (resultOnScreen) return;
            Screen = Screen.Trim(',');
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

            resultOnScreen = true;
            lastResult = tempResult;
            Screen += Environment.NewLine + tempResult;
        }

        //TODO: if lastresult was NaN of infinity, set lastresult to 0 before calculating with it
    }
}
