using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniXauonre.Graphics
{
    public class PlayerNameForm : Form
    {
        public string Player1Name { get; private set; }
        public string Player2Name { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            //WindowState = FormWindowState.Maximized;
        }

        public PlayerNameForm()
        {
            var s = new Size(ClientSize.Width, 64);
            var font = new System.Drawing.Font("Microsoft Sans Serif", 36);

            var nameBox1 = new RichTextBox
            {
                Location = new Point(0, 0),
                Size = s,
                Font = font
            };

            var nameBox2 = new TextBox
            {
                Location = new Point(0, nameBox1.Bottom),
                Size = s,
                Font = font
            };

            var sendButton = new Button
            {
                Location = new Point(0, nameBox2.Bottom + 16),
                Size = s,
                Text = "OK"
            };

            Controls.Add(nameBox1);
            Controls.Add(nameBox2);
            Controls.Add(sendButton);

            sendButton.Click += (sender, args) =>
            {
                Player1Name = (nameBox1.Text != "") ? nameBox1.Text : "FOO";
                Player2Name = (nameBox2.Text != "") ? nameBox2.Text : "BAR";
                Close();
            };
        }
        private void PlayerNameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
