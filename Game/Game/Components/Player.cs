using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Game.Components.ModalForms;
using Game.Components.Resources;

namespace Game.Components
{
    public class Player
    {
        public PlayerColor Color { get; set; }
        public Hexagon Headquater { get; set; }
        public List<Hexagon> Territory { get; set; }

        public PlayerResources Resources { get; set; }
        public List<Troops> Army { get; set; }
        public int MaxArmyCount { get; set; }

        public int BonusArmyCount { get; set; }
        public bool IsAlive { get ; set; }

        public Player(PlayerColor color, Hexagon headquater) 
        {
            BonusArmyCount = 0;
            MaxArmyCount = 5;
            Army = new List<Troops>() { new Troops(color, headquater) };
            Headquater = headquater;
            Territory = new List<Hexagon>() { Headquater };
            Color = color;
            Resources = new PlayerResources();
            headquater.SelectBy(this);
            //Headquater.SelectBy(this, Army);
            IsAlive = true;
        }

        public void AddResources() {
            foreach (var section in Territory)
            {
                var resource = section.Resource;
                if (resource != null)
                {
                    Resources.AddResource(resource.Type, resource.Amount);
                }
            }
        }

        public bool HaveEnoughResourceToBuild()
        {
            return Resources.Resources[ResourceType.Stone] >= 20 && Resources.Resources[ResourceType.Wood] >= 20 && Resources.Resources[ResourceType.Sand] >= 20;
        }

        public void ChooseSectionToBuild(Hexagon section)
        {
            section.Building = new Building(section);
            Resources.Resources[ResourceType.Stone] -= 20;
            Resources.Resources[ResourceType.Wood] -= 20;
            Resources.Resources[ResourceType.Sand] -= 20;
            BonusArmyCount += 5;
        }

        public void ChooseSection (Hexagon section)
        {
            List<Troops> armyThatCanMove = null;
            int result = 0;
            do
            {   
                using (ModalFormForMove modalForm = new ModalFormForMove())
                {
                    if (modalForm.ShowDialog() == DialogResult.OK)
                    {
                        result = modalForm.Result;
                        // Use the result as needed
                        MessageBox.Show("Result: " + result.ToString());
                    }
                }
                armyThatCanMove = Army.Where(troops => troops.Section.Neighbours.Contains(section) && troops.Amount >= result).ToList();
            } while (armyThatCanMove.Count == 0);
            armyThatCanMove[0].Move(section, this, result);
            
        }

        public void RebornDeathArmy()
        {
            var armyInHeadquater = Headquater.SectionTroops;
            int armyAmount = Army.Sum(troops => troops.Amount);
            int totalArmyCount = MaxArmyCount + BonusArmyCount;
            int armyToReborn = totalArmyCount - armyAmount;
            if (armyToReborn > 0)
            {
                if (armyInHeadquater != null)
                {
                    armyInHeadquater.Amount += armyToReborn;
                    armyInHeadquater.TroopsImage.Controls[0].Text = armyInHeadquater.Amount.ToString();
                }
                else 
                {
                    Army.Add(new Troops(Color, Headquater, armyToReborn));
                }
            }
        }

        public bool IsDead() { 
            return !(Army.Count > 0);
        }

        public void Death() {
            IsAlive = false;
            foreach (var troop in Army) { 
                troop.Unregister();
            }
            Army = null;
            foreach (var section in Territory) { 
                section.Unregister();
            }
            Territory = null;
        }
    }
}
