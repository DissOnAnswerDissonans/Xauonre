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
        public string PlayerName { get; private set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            //WindowState = FormWindowState.Maximized;
        }

        public PlayerNameForm(int someNumber)
        {
            var s = new Size(ClientSize.Width, 64);
            var font = new System.Drawing.Font("Microsoft Sans Serif", 36);

            var nameBox = new RichTextBox
            {
                Location = new Point(0, 0),
                Size = s,
                Font = font
            };

            var sendButton = new Button
            {
                Location = new Point(0, nameBox.Bottom + 16),
                Size = s,
                Text = "OK"
            };

            Controls.Add(nameBox);
            Controls.Add(sendButton);

            sendButton.Click += (sender, args) =>
            {
                PlayerName = (nameBox.Text != "") ? nameBox.Text : "Player "+someNumber;
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
