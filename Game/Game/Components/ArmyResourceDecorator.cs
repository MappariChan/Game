using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    internal class ArmyResourceDecorator : BaseResourceDecorator
    {
        public ArmyResourceDecorator(IResourceService resourceService) : base(resourceService) { }

        public void AddArmy(Player player)
        {
            var section = (Hexagon)resourceService;
            player.Army.Add(new Troops(player.Color, section, 1));
        }

        public override void AddResource(Player player)
        {
            base.AddResource(player);
            AddArmy(player);
        }
    }
}
