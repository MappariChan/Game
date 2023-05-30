using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class BaseResourceDecorator : IResourceService
    {
        public IResourceService resourceService;

        public BaseResourceDecorator(IResourceService resourceService)
        {
            this.resourceService = resourceService;
        }

        public virtual void AddResource(Player player) {
            resourceService.AddResource(player);
        }
    }
}
