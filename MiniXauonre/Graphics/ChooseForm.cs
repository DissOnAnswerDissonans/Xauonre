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
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DoubleBuffered = true;
            //WindowState = FormWindowState.Maximized;
        }

        public int Answer { get; set; }

        public ChooseForm(List<string> head, List<string> variants)
        {
            var s = new Size(ClientSize.Width, ClientSize.Height / (head.Count + variants.Count));
            var font = new System.Drawing.Font("Microsoft Sans Serif", ClientSize.Height / (head.Count + variants.Count) / 2);

            var labels = new List<Label>();
            foreach (var lb in head)
            {
                labels.Add(new Label()
                {
                    Location = new System.Drawing.Point(0, s.Height * labels.Count),
                    Size = s,
                    Text = head[labels.Count],
                    Font = font,
                });
            }
            foreach (var l in labels)
                Controls.Add(l);

            var buttons = new List<Button>();
            foreach (var cmd in variants)
            {
                buttons.Add(new Button()
                {
                    Location = new System.Drawing.Point(0, s.Height * (buttons.Count + head.Count)),
                    Size = s,
                    Text = cmd,
                    Font = font,
                });
            }

            for (int i = 0; i < buttons.Count; ++i)
            {
                var cyka = i;
                Controls.Add(buttons[i]);
                buttons[cyka].Click += (sender, args) => {
                    Answer = cyka;
                    Close();
                };
            }

            SizeChanged += (sender, args) =>
            {
                s = new Size(ClientSize.Width, ClientSize.Height / (head.Count + variants.Count));
                font = new System.Drawing.Font("Microsoft Sans Serif", ClientSize.Height / (head.Count + variants.Count) / 2);
                for (var v = 0; v < labels.Count; ++v)
                {
                    labels[v].Size = s;
                    labels[v].Font = font;
                    labels[v].Location = new System.Drawing.Point(0, s.Height * v);
                }
                for (var v = 0; v < buttons.Count; ++v)
                {
                    buttons[v].Size = s;
                    buttons[v].Font = font;
                    buttons[v].Location = new System.Drawing.Point(0, s.Height * (v + labels.Count));
                }
            };
        }
 
        private void PlayerNameForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}