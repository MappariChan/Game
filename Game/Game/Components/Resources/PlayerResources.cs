using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components.Resources
{
    public class PlayerResources
    {
        public Dictionary<ResourceType, int> Resources { get; set; }

        public PlayerResources() {
            Resources = new Dictionary<ResourceType, int>() {
                { ResourceType.Wood, 0 },
                { ResourceType.Stone, 0 },
                { ResourceType.Sand, 0 }
            };
        }

        public void AddResource(ResourceType type, int amount) {
            Resources[type] += amount;
        }
    }
}
