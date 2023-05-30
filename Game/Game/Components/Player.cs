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
        public List<BaseResourceDecorator> Territory { get; set; }

        public PlayerResources Resources { get; set; }
        public List<Troops> Army { get; set; }
        public int MaxArmyCount { get; set; }

        public int BonusArmyCount { get; set; }
        public bool IsAlive { get ; set; }

        public Player(PlayerColor color, Hexagon headquater) 
        {
            MaxArmyCount = 5;
            Army = new List<Troops>() { new Troops(color, headquater) };
            Headquater = headquater;
            Territory = new List<BaseResourceDecorator>() { new BaseResourceDecorator(Headquater) };
            Color = color;
            Resources = new PlayerResources();
            headquater.SelectBy(this);
            //Headquater.SelectBy(this, Army);
            IsAlive = true;
        }

        public void AddResources() {
            foreach (var section in Territory)
            {
                section.AddResource(this);
            }
        }

        public bool HaveEnoughResourceToBuild()
        {
            return Resources.Resources[ResourceType.Stone] >= 5 && Resources.Resources[ResourceType.Wood] >= 5 && Resources.Resources[ResourceType.Sand] >= 5;
        }

        public void ChooseSectionToBuild(Hexagon section)
        {
            section.Building = new Building(section);
            Territory.Remove(Territory.Find(sect => sect.resourceService == section));
            var sectionWithBuilding = new ArmyResourceDecorator(section);
            Territory.Add(sectionWithBuilding);
            Resources.Resources[ResourceType.Stone] -= 5;
            Resources.Resources[ResourceType.Wood] -= 5;
            Resources.Resources[ResourceType.Sand] -= 5;
        }

        public void ChooseSection (Hexagon section, int amount)
        {
            List<Troops> armyThatCanMove = Army.Where(troops => troops.Section.Neighbours.Contains(section) && troops.Amount >= amount).ToList();
            armyThatCanMove[0].Move(section, this, amount);
            
        }

        public void RebornDeathArmy()
        {
            var armyInHeadquater = Headquater.SectionTroops;
            int armyAmount = Army.Sum(troops => ((Troops)troops).Amount);
            int totalArmyCount = MaxArmyCount;
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
                ((Troops)troop).Unregister();
            }
            Army = null;
            foreach (var section in Territory) { 
                ((Hexagon)((BaseResourceDecorator)section).resourceService).Unregister();
            }
            Territory = null;
        }
    }
}
