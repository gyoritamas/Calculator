using System;
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

        const int screenLimit = 88;
        const int rowLimit = 44;
        bool resultOnScreen = false;
        double lastResult = 0;

        #region button Click events
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

        #endregion

        #region input form keyboard
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

            switch (key)
            {
                case ',': Decimal(); break;
                case (char)Keys.Back: Clear(); break;
                case (char)Keys.Return: Calculate(); break;
                case (char)Keys.Escape: Application.Exit(); break;
            }

        }
        private void Calculator_KeyUp(object sender, KeyEventArgs e)
        {
            var key = e.KeyCode;
            if (key == Keys.Delete)
                Delete();
        }

        #endregion

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

            TrimUnnecessary();

            // if last input was an operator, overwrite it
            if (Screen.EndsWith(" "))
                RemoveLastChars(3);

            Append(button);
        }

        /// <summary>
        /// Append "," after digits, if last number does not contain one
        /// <para></para>
        /// Append "0," instead after operators
        /// </summary>
        private void Decimal()
        {
            if (resultOnScreen) return;
            char lastChar = Screen[Screen.Length - 1];
            if (char.IsDigit(lastChar) && Screen.LastIndexOf(" ") >= Screen.LastIndexOf(","))
                Append(",");
            if (Screen.EndsWith(" "))
                Append("0,");
        }

        /// <summary>
        /// Clear the last character
        /// <para></para>
        /// If only a one digit number left on the screen (positive or negative), set the screen to 0
        /// <para></para>
        /// Ignore whitespace while deleting
        /// <para></para>
        /// Does not work when result of last calculation is apparent (protection)
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

        /// <summary>
        /// Delete everything from the screen
        /// </summary>
        private void Delete()
        {
            Screen = "0";
        }

        /// <summary>
        /// Negate the last number on the screen
        /// </summary>
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
            TrimUnnecessary();

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
            Screen += Environment.NewLine + tempResult;
            lastResult = double.IsNaN(tempResult) || double.IsInfinity(tempResult) ? 0 : tempResult;

        }

        // misc functions
        private void Append(string button)
        {
            if (Screen.Length >= screenLimit) return;

            if (char.IsDigit(button[0]) || button.Equals(","))
            {
                if (LastNumberLength() >= rowLimit - 2) return;
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

        private void TrimUnnecessary()
        {
            if (!Screen.Equals("0") || Screen.EndsWith(" 0"))
            {
                if (Screen.LastIndexOf(" ") < Screen.LastIndexOf(","))
                    Screen = Screen.TrimEnd('0');
                Screen = Screen.TrimEnd(',');
            }
        }
        
        private int LastNumberLength()
        {
            int startsHere = Screen.Contains(" ") ? Screen.LastIndexOf(" ") + 1 : 0;
            return Screen.Length - startsHere;
        }

        
    }
}
