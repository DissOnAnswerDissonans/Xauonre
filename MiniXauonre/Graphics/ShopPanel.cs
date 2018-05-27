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
using MiniXauonre.Graphics.resources;

namespace MiniXauonre.Graphics
{
    class ShopPanel : Panel
    {
        private TableLayoutPanel OptionsPanel { get; set; }
        private FlowLayoutPanel ItemChoosingPanel { get; set; }    
        private FlowLayoutPanel InfoPanel { get; set; }   
        private FlowLayoutPanel ExplanationPanel { get; set; }
        private TableLayoutPanel RecipePanel { get; set; }
        private TableLayoutPanel UpgradePanel { get; set; }
        private Button BuyButton { get; set; }
        
        private Shop Shop { get; set; } 
        private Hero Customer { get; set; }
        public Item ChosenItem { get; set; }
        
        private List<Item> Items { get; set; }

        private const int CritHeight = 46;
        
        private Func<int> GetOptionsWidth { get; set; }
        private Func<int> GetChoosingWidth{ get; set; }
        private Func<int> GetInfoWidth    { get; set; }

        public ShopPanel(Hero hero, ScreenForm form, Rectangle borders)
        {
            GetOptionsWidth  = () => Width * 2/10;
            GetChoosingWidth = () => Width * 3/10;
            GetInfoWidth     = () => Width * 5/10;
               
            Items = new List<Item>();
            
            Customer = hero;
            Shop = hero.S;
            ChosenItem = Shop.Items[0];
            Bounds = borders;

            OptionsPanel = new TableLayoutPanel()
            {
                Location = new Point(0, 0),
                BackColor = Color.Black,
            };
            OptionsPanel.Size = new Size(GetOptionsWidth(), Height);
            OptionsPanel.RowCount = 8;
            OptionsPanel.ColumnCount = 2;

            for (int t = 0; t <= 3; ++t)
            {
                var tier = t;
                var b = new Button()
                {
                    Size = new Size(GetOptionsWidth() / OptionsPanel.ColumnCount, Height / OptionsPanel.RowCount),
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    Text = "T" + (tier).ToString(),
                    Font = new Font(FontFamily.GenericSansSerif, 36),
                    ForeColor = Colors.ItemTierColors[tier]
                };
                b.Click += (sender, args) =>
                {
                    Items = Shop.GetItemsWithTier(tier);
                    UpdateItems();
                };
                OptionsPanel.Controls.Add(b);
            }
            
            for (var r = (int)StatType.MaxHP; r < (int)StatType.STOP; ++r)
            {
                var stat = r;
                var b = new Button()
                {
                    Size = new Size(GetOptionsWidth() / OptionsPanel.ColumnCount, Height / OptionsPanel.RowCount),
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    //Text = "+" + ((StatType)stat).ToString(),
                    Image = IconLoader.GetIcon((StatType)r, new Size(64, 64)),
                };
                b.Click += (sender, args) =>
                {
                    Items = Shop.GetItemsWithSortedStat((StatType)stat);
                    UpdateItems();
                };
                OptionsPanel.Controls.Add(b);
            }
   
            ItemChoosingPanel = new FlowLayoutPanel()
            {
                Location = new Point(GetOptionsWidth(), 0),
                FlowDirection = FlowDirection.LeftToRight,
                AutoScroll = true,
            };
            ItemChoosingPanel.Size = new Size(GetChoosingWidth(), Height);                      
            
            InfoPanel = new FlowLayoutPanel()
            {
                Location = new Point(GetOptionsWidth() + GetChoosingWidth(), 0),
                BackColor = Color.Black
            }; 
            InfoPanel.Size = new Size(GetInfoWidth(), Height - 128);  

            BuyButton = new Button()
            {
                Font = new Font(FontFamily.GenericSansSerif, 32),
            };
            BuyButton.Size = new Size(GetInfoWidth(), 128);
            BuyButton.Location = new Point(GetOptionsWidth() + GetChoosingWidth(), Height - 128);
            BuyButton.Click += (sender, args) =>
            {
                Customer.BuyItem(ChosenItem);
                form.StatPanelUpdate();
                form.PlayerPanelUpdate();
            };

            ExplanationPanel = new FlowLayoutPanel();
            ExplanationPanel.BackColor = Color.Black;
            ExplanationPanel.MinimumSize = new Size(GetInfoWidth(), 36);
            ExplanationPanel.AutoSize = true;
            
            RecipePanel = new TableLayoutPanel(){RowStyles = { new RowStyle(SizeType.AutoSize)}};
            RecipePanel.Size = new Size(GetInfoWidth() / 2, 64);
            
            UpgradePanel = new TableLayoutPanel(){RowStyles = { new RowStyle(SizeType.AutoSize)}};
            UpgradePanel.Size = new Size(GetInfoWidth() / 2, 64);

            Controls.Add(OptionsPanel);
            Controls.Add(ItemChoosingPanel);
            Controls.Add(InfoPanel);
            Controls.Add(BuyButton);
            
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

        public void UpdateItems()
        {
            ItemChoosingPanel.Controls.Clear();
            var heightExceeded = (Height / CritHeight < Items.Count);
            foreach (var shopItem in Items)
            {
                var button = new Button()
                {
                    Text = shopItem.Name,
                    Font = new Font(FontFamily.GenericSansSerif, 24),
                    BackColor = Colors.ItemTierColors[shopItem.Tier],
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    Size = heightExceeded ? 
                        new Size(GetChoosingWidth() - 17, CritHeight) : new Size(GetChoosingWidth(), CritHeight),
                };
                button.Click += (sender, args) =>
                {
                    ChosenItem = shopItem;
                    UpdateInfo();
                };
                ItemChoosingPanel.Controls.Add(button);      
            } 
        }

        public void UpdateInfo()
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
                ForeColor = Colors.ItemTierColors[ChosenItem.Tier],
                Font = new Font(FontFamily.GenericSerif, 36),
                Text = ChosenItem.Name,
                Dock = DockStyle.Top,
                Width = InfoPanel.Width,
                Height = 72,
                TextAlign = ContentAlignment.MiddleCenter,
            }); 
            
            InfoPanel.Controls.Add(ExplanationPanel);

            var shit = new Panel { AutoSize = true };
             
            InfoPanel.Controls.Add(shit);
                       
            shit.Controls.Add(RecipePanel);
            shit.Controls.Add(UpgradePanel);
            
            InfoPanel.Controls.Add(new Label
            {
                AutoSize = true,
                MinimumSize = new Size(GetInfoWidth(), 0),
                MaximumSize = new Size(GetInfoWidth(), 1000),
                Text = ChosenItem.Explanation(Customer),
                ForeColor = Color.Red,
                Font = new Font(FontFamily.GenericSansSerif, 20),
            });
            
            InfoPanel.Refresh();
            
            BuyButton.Text =  ChosenItem.Name + ":\n Buy for (" + (int)ChosenItem.GetFinalCost(Customer) + ")";

            var itemExp = ChosenItem.GetExplanation();  
            ExplanationPanel.Controls.Clear();
            var icons = resources.IconLoader.GetIcons(new Size(32, 32));
            foreach (var item in itemExp)   
            {
                var micropanel = new Panel()
                {
                    AutoSize = true,
                };
                
                micropanel.Controls.Add(new PictureBox()
                {
                    Image = icons[item.Key],
                    Size = new Size(32, 32)
                });
                micropanel.Controls.Add(new Label()
                {
                    Location = new Point(32,0),
                    Text = item.Value.ToString(),
                    ForeColor = Color.White,
                    Font = new Font(FontFamily.GenericSansSerif, 24),
                    AutoSize = true,
                });
                ExplanationPanel.Controls.Add(micropanel);
            }
            
            RecipePanel.Controls.Clear();
            RecipePanel.RowCount = ChosenItem.Parts.Count;
            RecipePanel.Width = GetInfoWidth() / 2;
            RecipePanel.Height = ChosenItem.Parts.Count * 48;
            var kik = new List<Item>(Customer.Items);
            foreach (var part in ChosenItem.Parts)
            {
                var b = new Label()
                {
                    Font = new Font(FontFamily.GenericSerif, 24),
                    Text = "+ " + part.Name,
                    BackColor = Color.Azure,
                    ForeColor = Color.Black,
                    Size = new Size(RecipePanel.Width - 6, 44),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 2, 6, 2)
                };
                
                b.Click += (sender, args) =>
                {
                    ChosenItem = part;
                    UpdateInfo();
                };
                
                RecipePanel.Controls.Add(b);
                if (kik.Contains(part))
                {
                    kik.Remove(part);
                    RecipePanel.Controls[RecipePanel.Controls.Count - 1].BackColor = Color.Chartreuse;
                }              
            }
            
            UpgradePanel.Controls.Clear();
            var upgrades = Shop.GetUpgrades(ChosenItem);
            UpgradePanel.Location = new Point(GetInfoWidth() / 2, 0);
            UpgradePanel.Width = GetInfoWidth() / 2;
            UpgradePanel.Height = upgrades.Count * 48;
            foreach (var part in upgrades)
            {
                var b = new Label()
                {
                    Font = new Font(FontFamily.GenericSerif, 24),
                    Text = "> " + part.Name,
                    BackColor = Color.Azure,
                    ForeColor = Color.Black,
                    Size = new Size(UpgradePanel.Width - 6, 44),
                    TextAlign = ContentAlignment.MiddleRight,
                    Margin = new Padding(0, 2, 6, 2)
                };

                b.Click += (sender, args) =>
                {
                    ChosenItem = part;
                    UpdateInfo();
                };
                
                UpgradePanel.Controls.Add(b);             
            }
        }
    }
}