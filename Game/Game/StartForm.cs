using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class StartForm : Form
    {
        public static string option;
        public static int amountint;

        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            amountint = int.Parse(amount.Text);
            option = options.Text;
            Hide();
            Form1 form = new Form1();
            form.Show();
        }
    }
}
