using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Game.Components
{
    public class TroopMover
    {
        private static TroopMover troopMover;

        private TroopMover() {}

        private void MovePart(Troops troops,Hexagon section, int amount, Player player)
        {
            player.Army.Add(new Troops(player.Color, section, amount));
            troops.Amount -= amount;
            troops.TroopsImage.Controls[0].Text = troops.Amount.ToString();
        }

        private void MoveAll(Troops troops,Hexagon section, Player player)
        {
            Remove(troops.Section);
            player.Army.Add(new Troops(player.Color, section, troops.Amount));
        }

        private void Remove(Hexagon section)
        {
            var sectionTroops = section.SectionTroops;
            section.Player.Army.Remove(sectionTroops);
            section.SectionUI.PictureBox.Controls.Remove(sectionTroops.TroopsImage);
            section.SectionTroops = null;
        }

        private void Atack(Troops troops,Hexagon section, Player player, int amount)
        {
            int newAmount = amount - section.SectionTroops.Amount;
            if (section.SectionTroops.Amount > amount)
            {
                section.SectionTroops.Amount -= amount;
                section.SectionTroops.TroopsImage.Controls[0].Text = section.SectionTroops.Amount.ToString();
                if (amount < troops.Amount)
                {
                    troops.Amount -= amount;
                    troops.TroopsImage.Controls[0].Text = troops.Amount.ToString();
                }
                else
                {
                    Remove(troops.Section);
                }
            }
            else if (section.SectionTroops.Amount < amount)
            {
                Remove(section);
                if (troops.Amount > amount)
                {
                    MovePart(troops, section, amount, player);
                    section.SectionTroops.Amount = newAmount;
                    section.SectionTroops.TroopsImage.Controls[0].Text = newAmount.ToString();
                }
                else
                {
                    Remove(troops.Section);
                    player.Army.Add(new Troops(player.Color, section, newAmount));
                }
                if (section.Building != null)
                {
                    player.Territory.Add(new ArmyResourceDecorator(section));
                }
                else
                {
                    player.Territory.Add(new BaseResourceDecorator(section));
                }
                section.SelectBy(player);
            }
            else
            {
                Remove(section);
                Remove(troops.Section);
                if (amount < troops.Amount)
                {
                    player.Army.Add(new Troops(player.Color, troops.Section, troops.Amount - amount));
                }
            }
        }

        public void Move(Troops troops, Hexagon section, Player player, int amount)
        {
            if (section.SectionTroops == null)
            {
                if (amount < troops.Amount)
                {
                    MovePart(troops, section, amount, player);
                }
                else
                {
                    MoveAll(troops, section, player);
                }
                if (section.Building != null)
                {
                    player.Territory.Add(new ArmyResourceDecorator(section));
                }
                else
                {
                    player.Territory.Add(new BaseResourceDecorator(section));
                }
                section.SelectBy(player);
            }
            else
            {
                if (section.Player == player)
                {
                    section.SectionTroops.Amount += amount;
                    section.SectionTroops.TroopsImage.Controls[0].Text = section.SectionTroops.Amount.ToString();
                    if (amount == troops.Amount)
                    {
                        Remove(troops.Section);
                    }
                    else
                    {
                        troops.Amount -= amount;
                        troops.TroopsImage.Controls[0].Text = troops.Amount.ToString();
                    }
                    return;
                }
                Atack(troops, section, player, amount);
            }
        }


        public static TroopMover GetInstance() {
            if (troopMover == null)
            { 
                troopMover = new TroopMover();
            }
            return troopMover;
        }
    }
}
