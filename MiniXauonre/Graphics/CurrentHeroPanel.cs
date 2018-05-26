using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Controller;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;

namespace MiniXauonre.Graphics
{
    class CurrentHeroPanel : Panel
    {
        private Hero Hero { get; set; }
        
        private ScreenForm ParentForm { get; set; }
        
        private TableLayoutPanel ItemsPanel { get; set; }

        public CurrentHeroPanel(Hero h, ScreenForm f)
        {
            Hero = h;
            ParentForm = f;
            AutoSize = true;
            
            Padding = new Padding(2);
            
            //BackColor = Color.AliceBlue;
            
            var b = new Button()
            {
                Image = Hero.GetImage(),
                Dock = DockStyle.Bottom,
                AutoSize = false,
                BackColor = Colors.PlayerDarkColors[h.P.Game.Players.IndexOf(h.P) % Colors.count],
                Margin = new Padding(4),
                Size = new Size(128, 128),
            };

            b.Click += (sender, args) =>
            {
                h.P.Game.ChosenHero = h;
                ParentForm.CenterOnPoint(h.M.UnitPositions[h]);
                ParentForm.ControlPanelUpdate();
                ParentForm.StatPanelUpdate();
            };

            Controls.Add(b);

            ItemsPanel = new TableLayoutPanel()
            {
                Padding = new Padding(0,2,0,0),
                RowCount = 4,
                RowStyles = { new RowStyle(SizeType.AutoSize)},
                Dock = DockStyle.Top,
                Size = new Size(128,0),
                AutoSize = true,
                MaximumSize = new Size(128, 1000),
            };
            
            Controls.Add(ItemsPanel);

            UpdateItems();
        }

        public void UpdateItems()
        {
            ItemsPanel.Controls.Clear();

            ItemsPanel.RowCount = Hero.Items.Count;

            foreach (var item in Hero.Items)
            {
                var loabel = new Label
                {
                    Margin = new Padding(0, 0, 0, 2),
                    Size = new Size(128, 24),
                    BackColor = Color.Black,
                    ForeColor = Colors.ItemTierColors[item.Tier],
                    Text = item.Name,
                    Font = new Font(FontFamily.GenericSansSerif, 12),
                    TextAlign = ContentAlignment.MiddleCenter,
                };

                loabel.DoubleClick += (sender, args) =>
                {
                    var dialogResult = MessageBox.Show(
                        "Are you sure you want to throw away a " + item.Name + "?",
                        "Delete item?",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation
                    );
                    if (dialogResult == DialogResult.Yes)
                    {
                        item.Remove(Hero);
                        ParentForm.ControlPanelUpdate();
                        ParentForm.StatPanelUpdate();
                        UpdateItems();
                    }
                };
                
                ItemsPanel.Controls.Add(loabel);
            }
        }
    }
}