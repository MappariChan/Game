using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public interface TroopsPrototype
    {
        public TroopsPrototype Clone(Hexagon section, int amount);
    }
}
