using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public interface ActionStrategy
    {
        public Task Action(Player player, ISectionClickService sectionClickService);
    }
}
