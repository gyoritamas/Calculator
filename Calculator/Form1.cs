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

        private string[] operators = { "+", "-", "*", "/" };

        private void button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string button = clickedButton.Text;
            // operator was pressed
            if (operators.Contains(button))
            {
                // last input also was an operator
                if (textBoxResult.Text.EndsWith(" "))
                    OverwriteLastOperator(button);
                // last input was a number
                else
                    AppendOperator(button);
            }
            // number was pressed
            else
            {
                // the very first input is number:
                // overwrite 0 the program starts with
                if (textBoxResult.Text == "0")
                    textBoxResult.Text = "";
                // don't let the user write numbers starting with 0
                // unless they follow up with ","
                // TODO: implement non-integers
                if (textBoxResult.Text.EndsWith(" 0"))
                    textBoxResult.Text = textBoxResult.Text.Substring(0, textBoxResult.Text.Length - 1);
                AppendNumber(button);
            }

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
            if (!textBoxResult.Text.EndsWith(" ") && !textBoxResult.Text.EndsWith("-"))
            {
                //this is the first number or the screen ends with a number not containing ","
                if (!textBoxResult.Text.Contains(" ") || textBoxResult.Text.LastIndexOf(" ") > textBoxResult.Text.LastIndexOf(","))
                    AppendNumber(",");
            }

        }
    }
}
