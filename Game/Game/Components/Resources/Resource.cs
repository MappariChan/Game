using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components.Resources
{
    public class Resource
    {
        public ResourceType Type { get; set; }
        public int Amount { get; set; }
        public Resource(ResourceType type, int amount) {
            Type = type;
            Amount = amount;
        }
    }
}
