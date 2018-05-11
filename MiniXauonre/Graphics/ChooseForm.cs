using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Controller;
using Xauonre.Core;

namespace MiniXauonre.Graphics
{
    public class ChooseForm : Form
    {
        public Command FormCommand { get; private set; }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            //WindowState = FormWindowState.Maximized;
        }

        public ChooseForm(string text, List<Command> commands)
        {
            var s = new Size(ClientSize.Width, 64);
            var font = new System.Drawing.Font("Microsoft Sans Serif", 36);
            
            var playerLabel = new Label()
            {
                Location = new System.Drawing.Point(0, 0),
                Size = s,
                Text = text
            };

            var commandButtons = new List<CommandButton>();
            foreach (var cmd in commands)
            {
                commandButtons.Add(new CommandButton()
                {
                    Location = new System.Drawing.Point(0, s.Height * commandButtons.Count + 1),
                    Size = s,
                    Text = cmd.ToString(),
                    Command = cmd
                });
            }

            foreach (var b in commandButtons)
            {
                Controls.Add(b);
                b.Click += (sender, args) =>
                {
                    FormCommand = b.Command;
                    Close();
                };
            }
        }
        
        private void PlayerNameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }

    public class CommandButton : Button
    {
        public Command Command { get; set; }
    }
}