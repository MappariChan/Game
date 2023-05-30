using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game.Components
{
    public class TextBoxLogger
    {
        private RichTextBox _textBox;

        public TextBoxLogger(Form1 form)
        {
            _textBox = new RichTextBox();
            _textBox.Location = new Point(0, 250);
            _textBox.Size = new Size(200, 300);
            form.Controls.Add(_textBox);
        }

        public void AddColoredLine(string text, Color color)
        {
            _textBox.SelectionStart = _textBox.TextLength;
            _textBox.SelectionLength = 0;
            _textBox.SelectionColor = color;
            _textBox.AppendText(text + Environment.NewLine);
            _textBox.SelectionColor = _textBox.ForeColor;
        }
    }
}
