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

        private void button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            textBoxResult.Text += clickedButton.Text;
        }

        //TODO: first character overwrites the starting 0
        //TODO: if last char was an operator, the next operator overwrites it
        //TODO: handle input from keys, not just mouse (Keypress event)
        //TODO: space before and after operators
        //TODO: negative numbers

    }
}
