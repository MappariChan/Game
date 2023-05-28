using Game.Components.ClassicSurfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Components.FantasySurfaces;

namespace Game.Components
{
    public class FantasySurfaceFactory : ISurfaceFactory
    {
        public ISurface CreateDesert()
        {
            return new MummyDesert();
        }

        public ISurface CreateForest()
        {
            return new SpiderForest();
        }

        public ISurface CreateMeadows()
        {
            return new FairyMeadows();
        }

        public ISurface CreateMountains()
        {
            return new DragonMountains();
        }

        public ISurface CreateSwamp()
        {
            return new DeadSwamp();
        }
    }
}
