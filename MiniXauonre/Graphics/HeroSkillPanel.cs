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
    class HeroSkillPanel : FlowLayoutPanel
    {
        private Game Game { get; set; }
        private Hero Hero { get; set; }
        
        private Dictionary<Skill, Panel> Skills { get; set; }

        public HeroSkillPanel(Game g, Hero h, ScreenForm form, int width)
        {
            Game = g;
            Hero = h;
            
            Skills = new Dictionary<Skill, Panel>();

            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowOnly;
            Width = width - 6;

            FlowDirection = FlowDirection.TopDown;

            foreach (var skill in Hero.Skills)
            {
                var panel = new Panel();
                panel.Width = width - 12;
                panel.BackColor = skill.Availiable(Hero) ? Color.Black : Color.DarkSlateGray;

                var skillName = new Label()
                {
                    Dock = DockStyle.Top,
                    Text = skill.Name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.Aquamarine,
                    Font = new Font(FontFamily.GenericSerif, 24),
                    Width = width - 12,
                    BorderStyle = BorderStyle.None,
                    AutoSize = true,
                };
                
                var skillExp = new Label()
                {
                    Dock = DockStyle.Top,
                    Text = skill.Explanation(),
                    TextAlign = ContentAlignment.MiddleLeft,
                    ForeColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    MaximumSize = new Size(width-12, 1000),
                    AutoSize = true,
                };
         
                panel.Controls.Add(skillExp);
                panel.Controls.Add(skillName);
      
                if (skill.Timer > 0)
                {
                    panel.Controls.Add(new PictureBox
                    {
                        Size = new Size(32, 32),
                        Image = resources.IconLoader.GetIcons(new Size(32, 32))["CDR"],
                        Dock = DockStyle.Right,
                    });
                    
                    panel.Controls.Add(new Label
                    {
                        Text = skill.Timer.ToString(),
                        AutoSize = true,
                        Dock = DockStyle.Right,
                        Font = new Font(FontFamily.GenericSansSerif, 24),
                        ForeColor = Color.Yellow,
                    });
                }

                panel.Height = skillName.Height + skillExp.Height;
                
                panel.BringToFront();

                if (Game.CurrentHero == Hero)
                {
                    void OnPanelOnClick(object sender, EventArgs args)
                    {
                        Hero.UseSkill(Hero.Skills.IndexOf(skill));
                        form.ControlPanelUpdate();
                        form.StatPanelUpdate();
                        form.MapUpdate();
                        form.PlayerPanelUpdate();
                    }

                    panel.Click += OnPanelOnClick;
                    skillName.Click += OnPanelOnClick;
                    skillExp.Click += OnPanelOnClick;
                }
                Controls.Add(panel);
                Skills.Add(skill, panel);
            }
        }
    }
}