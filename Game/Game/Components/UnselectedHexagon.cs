using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class UnselectedHexagon : HexagonState
    {
        private Hexagon hexagon;

        public UnselectedHexagon(Hexagon hexagon)
        { 
            this.hexagon = hexagon;
        }

        public void AddResources(Player player)
        { 
            
        }
    }
}
