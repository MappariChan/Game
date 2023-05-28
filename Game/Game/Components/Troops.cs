using System;
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
    public class Troops { 
        public PictureBox TroopsImage { get; set; }
        public int Amount { get; set; }
        public Hexagon Section { get; set; }
        public Troops(PlayerColor color, Hexagon section) 
        {
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
            Amount = 5;
            System.Windows.Forms.Label l1 = new System.Windows.Forms.Label();
            TroopsImage.Controls.Add(new System.Windows.Forms.Label()
            {
                Text = Amount.ToString(),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Height = 40,
                Font = new Font(l1.Font.FontFamily, 14f, FontStyle.Bold),
                Location = new Point(TroopsImage.Size.Width - 20, TroopsImage.Size.Height - 30)
            });
            sectionUI.Controls.Add(TroopsImage);
            TroopsImage.BackColor = Color.Transparent;
            
        }

        public Troops(PlayerColor color, Hexagon section, int amount):this(color, section)
        {
            Amount = amount;
            TroopsImage.Controls[0].Text = Amount.ToString();
        }

        private void MovePart(Hexagon section, int amount, Player player) {
            player.Army.Add(new Troops(player.Color, section, amount));
            Amount -= amount;
            TroopsImage.Controls[0].Text = Amount.ToString();
        }

        private void MoveAll(Hexagon section, Player player) {
            Remove(Section);
            player.Army.Add(new Troops(player.Color, section, Amount));
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

        private void Atack(Hexagon section, Player player, int amount)
        {
            int newAmount = amount - section.SectionTroops.Amount;
            if (section.SectionTroops.Amount > amount)
            {
                section.SectionTroops.Amount -= amount;
                section.SectionTroops.TroopsImage.Controls[0].Text = section.SectionTroops.Amount.ToString();
                if (amount < Amount)
                {
                    Amount -= amount;
                    TroopsImage.Controls[0].Text = Amount.ToString();
                }
                else {
                    Remove(Section);
                }
            }
            else if (section.SectionTroops.Amount < amount)
            {
                Remove(section);
                if (Amount > amount)
                {
                    MovePart(section, amount, player);
                    section.SectionTroops.Amount = newAmount;
                    section.SectionTroops.TroopsImage.Controls[0].Text = newAmount.ToString();
                }
                else
                {
                    Remove(Section);
                    player.Army.Add(new Troops(player.Color, section, newAmount));
                }
                player.Territory.Add(section);
                section.SelectBy(player);
            }
            else {
                Remove(section);
                Remove(Section);
                player.Army.Add(new Troops(player.Color, Section, Amount - amount));
            }
        }

        public void Move(Hexagon section, Player player, int amount) 
        {
            if (section.SectionTroops == null)
            {
                if (amount < Amount)
                {
                    MovePart(section, amount, player);
                }
                else
                {
                    MoveAll(section, player);
                }
                player.Territory.Add(section);
                section.SelectBy(player);
            }
            else
            {
                Atack(section, player, amount);
            }
        }
    }
}
