using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game.Components.ModalForms
{
    public partial class ModalFormForMove : Form
    {
        public int Result { get; private set; }
        public ModalFormForMove()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int parsedValue;
            if (int.TryParse(textBox1.Text, out parsedValue))
            {
                Result = parsedValue;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter a valid integer.");
            }

        }
    }
}
