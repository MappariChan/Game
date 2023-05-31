using Game.Components.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class SelectedHexagon : HexagonState
    {
        private Hexagon hexagon;

        public SelectedHexagon(Hexagon hexagon)
        { 
            this.hexagon = hexagon;
        }

        public void AddResources(Player player)
        {
            if (hexagon.Resource != null)
            {
                player.Resources.AddResource(hexagon.Resource.Type, hexagon.Resource.Amount);
            }
        }
    }
}
