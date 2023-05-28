using Game.Components.ClassicSurfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class ClassicSurfaceFactory : ISurfaceFactory
    {
        public ISurface CreateDesert()
        {
            return new Desert();
        }

        public ISurface CreateForest()
        {
            return new Forest();
        }

        public ISurface CreateMeadows()
        {
            return new Meadows();
        }

        public ISurface CreateMountains()
        {
            return new Mountains();
        }

        public ISurface CreateSwamp()
        {
            return new Swamp();
        }
    }
}
