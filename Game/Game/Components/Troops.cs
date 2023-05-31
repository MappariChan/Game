﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Game.Components.ModalForms;
using static System.Collections.Specialized.BitVector32;

namespace Game.Components
{
    public class Troops{ 
        public PictureBox TroopsImage { get; set; }
        public int Amount { get; set; } = 0;
        public Hexagon Section { get; set; }
        public Troops(PlayerColor color, Hexagon section)
        {
            if (section.SectionTroops != null)
            {
                Amount += section.SectionTroops.Amount;
                Remove(section);
            }
            Section = section;
            section.SectionTroops = this;
            var sectionUI = section.SectionUI.PictureBox;
            TroopsImage = new PictureBox();
            var size = sectionUI.Size;
            var troopsLocation = new Point(size.Width / 6, size.Height / 6);
            TroopsImage.Size = new Size(size.Width/6*4, size.Height/6*4);
            TroopsImage.Location = troopsLocation;
            TroopsImage.SizeMode = PictureBoxSizeMode.StretchImage;
            switch (color)
            {
                case PlayerColor.Blue:
                    TroopsImage.Image = Image.FromFile("../../../Images/KnightOnTheHorse/blue.png");
                    break;
                case PlayerColor.Red:
                    TroopsImage.Image = Image.FromFile("../../../Images/KnightOnTheHorse/red.png");
                    break;
                case PlayerColor.Yellow:
                    TroopsImage.Image = Image.FromFile("../../../Images/KnightOnTheHorse/yellow.png");
                    break;
                case PlayerColor.Purple:
                    TroopsImage.Image = Image.FromFile("../../../Images/KnightOnTheHorse/Purple.png");
                    break;
            }
            Amount += 5;
            System.Windows.Forms.Label l1 = new System.Windows.Forms.Label();
            TroopsImage.Controls.Add(new System.Windows.Forms.Label()
            {
                Text = Amount.ToString(),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Height = 40,
                Font = new Font(l1.Font.FontFamily, 14f, FontStyle.Bold),
                Width = TroopsImage.Size.Width,
                Location = new Point(0, TroopsImage.Size.Height - 30),
                RightToLeft = RightToLeft.Yes
            });
            sectionUI.Controls.Add(TroopsImage);
            TroopsImage.BackColor = Color.Transparent;
            TroopsImage.BringToFront();
        }

        public Troops(PlayerColor color, Hexagon section, int amount):this(color, section)
        {
            Amount += amount - 5;
            TroopsImage.Controls[0].Text = Amount.ToString();
        }
        private void Remove(Hexagon section) {
            var sectionTroops = section.SectionTroops;
            section.Player.Army.Remove(sectionTroops);
            section.SectionUI.PictureBox.Controls.Remove(sectionTroops.TroopsImage);
            section.SectionTroops = null;
        }

        public void Unregister() {
            Section.SectionUI.PictureBox.Controls.Remove(TroopsImage);
            Section = null;
        }

        public void Move(Hexagon section, Player player, int amount) 
        {
            var troopMover = TroopMover.GetInstance();
            troopMover.Move(this, section, player, amount);
        }
    }
}
