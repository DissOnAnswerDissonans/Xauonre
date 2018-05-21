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
    class ShopPanel : Panel
    {
        private TableLayoutPanel ItemChoosingPanel { get; set; }    
        private FlowLayoutPanel InfoPanel { get; set; }   
        private FlowLayoutPanel ExplanationPanel { get; set; }
        private Button BuyButton { get; set; }
        
        private Shop Shop { get; set; } 
        private Hero Customer { get; set; }
        private Item ChosenItem { get; set; }

        private const double CritHeight = 36;

        public ShopPanel(Hero hero, ScreenForm form, Rectangle borders)
        {
            Customer = hero;
            Shop = hero.S;
            ChosenItem = Shop.Items[0];
            Bounds = borders;
            var TotalItems = Shop.Items.Count;
            var cc = (int)(TotalItems / (Height / CritHeight)) + 1;

            ItemChoosingPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Left,
                RowCount = (TotalItems + cc - 1) / cc,
                ColumnCount = cc,
            };
            ItemChoosingPanel.Size = new Size(Width / 2, Height);
            for (int i = 0; i < ItemChoosingPanel.RowCount; i++)
                ItemChoosingPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / (ItemChoosingPanel.RowCount)));
            for (int i = 0; i < ItemChoosingPanel.ColumnCount; i++)
                ItemChoosingPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / ItemChoosingPanel.ColumnCount));
                       
            for (int i = 0; i < Shop.Items.Count; ++i)
            {
                var shopItem = Shop.Items[i];
                var button = new Button()
                {
                    Dock = DockStyle.Fill,
                    Text = shopItem.Name,
                    Font = new Font(FontFamily.GenericSansSerif, 24),
                };
                button.Click += (sender, args) =>
                {
                    ChosenItem = shopItem;
                    UpdateInfo();
                };
                ItemChoosingPanel.Controls.Add(button);      
            }

            InfoPanel = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black,
            }; 

            BuyButton = new Button()
            {
                Dock = DockStyle.Bottom,
                Font = new Font(FontFamily.GenericSansSerif, 32),
            };
            BuyButton.Size = new Size(Width / 2, 64);
            BuyButton.Click += (sender, args) =>
            {
                Customer.BuyItem(ChosenItem);
                form.StatPanelUpdate();
                form.PlayerPanelUpdate();
            };
            
            ExplanationPanel = new FlowLayoutPanel(){AutoSize = true};
            
            Controls.Add(InfoPanel);
            Controls.Add(BuyButton);
            Controls.Add(ItemChoosingPanel);
            
            UpdateInfo();

            SizeChanged += (sender, args) =>
            {
                ItemChoosingPanel.Size = new Size(Width / 2, Height);
                BuyButton.Size = new Size(Width / 2, 64);
                UpdateInfo();
            };
        }

        private void UpdateInfo()
        {
            InfoPanel.Controls.Clear();
            /*
            InfoPanel.Controls.Add(new Label());
            {
                ForeColor = Color.AliceBlue,
                Font = new Font(FontFamily.GenericSerif, 32),
                Text = ChosenItem.Explanation();
                Dock = DockStyle.Top;
            }
            */
            
            InfoPanel.Controls.Add(new Label
            {
                ForeColor = Color.AliceBlue,
                Font = new Font(FontFamily.GenericSerif, 32),
                Text = ChosenItem.Name,
                Dock = DockStyle.Top,
                Width = InfoPanel.Width,
                Height = 64,
            }); 
            
            InfoPanel.Controls.Add(ExplanationPanel);        
                
            InfoPanel.Refresh();
            
            BuyButton.Text = "Buy " + ChosenItem.Name + " (" + (int)ChosenItem.GetFinalCost(Customer) + ")";

            var itemExp = ChosenItem.GetExplanation();
            
            ExplanationPanel.Controls.Clear();

            var icons = resources.IconLoader.GetIcons(new Size(32, 32));

            foreach (var item in itemExp)
            {
                ExplanationPanel.Controls.Add(new PictureBox()
                {
                    Image = icons[item.Key],
                    Size = new Size(32, 32)
                });
                ExplanationPanel.Controls.Add(new Label()
                {
                    Text = item.Value.ToString(),
                    ForeColor = Color.White,
                    Font = new Font(FontFamily.GenericSansSerif, 24),
                    AutoSize = true,
                });
            }
        }
    }
}