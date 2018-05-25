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
        private TableLayoutPanel RecipePanel { get; set; }
        private Button BuyButton { get; set; }
        
        private Shop Shop { get; set; } 
        private Hero Customer { get; set; }
        private Item ChosenItem { get; set; }

        private const double CritHeight = 48;

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
            ItemChoosingPanel.Size = new Size(Width * 2 / 3, Height);
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
                    Margin = new Padding(0),
                    Padding = new Padding(0)
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
            BuyButton.Size = new Size(Width * 1 / 3, 128);
            BuyButton.Click += (sender, args) =>
            {
                Customer.BuyItem(ChosenItem);
                form.StatPanelUpdate();
                form.PlayerPanelUpdate();
            };

            ExplanationPanel = new FlowLayoutPanel();
            ExplanationPanel.BackColor = Color.Black;
            ExplanationPanel.MinimumSize = new Size(Width * 1 / 3, 36);
            ExplanationPanel.AutoSize = true;
            
            RecipePanel = new TableLayoutPanel(){RowStyles = { new RowStyle(SizeType.AutoSize)}};
            RecipePanel.Size = new Size(Width * 1 / 3, 64);
            
            Controls.Add(InfoPanel);
            Controls.Add(BuyButton);
            Controls.Add(ItemChoosingPanel);
            Controls.Add(RecipePanel);
            
            UpdateInfo();

            /*
            SizeChanged += (sender, args) =>
            {
                RecipePanel.Size = new Size(Width * 3 / 10, 64);
                ExplanationPanel.Size = new Size(Width * 3 / 10, 36);
                ItemChoosingPanel.Size = new Size(Width * 7 / 10, Height);
                BuyButton.Size = new Size(Width * 3 / 10, 128);
                UpdateInfo();
            };
            */
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
                Font = new Font(FontFamily.GenericSerif, 36),
                Text = ChosenItem.Name,
                Dock = DockStyle.Top,
                Width = InfoPanel.Width,
                Height = 72,
                TextAlign = ContentAlignment.MiddleCenter,
            }); 
            
            InfoPanel.Controls.Add(ExplanationPanel);            
            InfoPanel.Controls.Add(RecipePanel);               
            InfoPanel.Refresh();
            
            BuyButton.Text =  ChosenItem.Name + ":\n Buy for (" + (int)ChosenItem.GetFinalCost(Customer) + ")";

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
            
            RecipePanel.Controls.Clear();
            RecipePanel.RowCount = ChosenItem.Parts.Count;
            RecipePanel.Width = Width * 1 / 3;
            RecipePanel.Height = ChosenItem.Parts.Count * 64;
            var kik = new List<Item>(Customer.Items);
            foreach (var part in ChosenItem.Parts)
            {
                RecipePanel.Controls.Add(new Label()
                {
                    Font = new Font(FontFamily.GenericSerif, 32),
                    Text = part.Name,
                    BackColor = Color.Azure,
                    ForeColor = Color.Black,
                    Size = new Size(RecipePanel.Width, 60),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 2, 0, 2)
                });
                if (kik.Contains(part))
                {
                    kik.Remove(part);
                    RecipePanel.Controls[RecipePanel.Controls.Count - 1].BackColor = Color.Chartreuse;
                }              
            }
        }
    }
}