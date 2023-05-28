using Game.Components.ClassicSurfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public interface ISurfaceFactory
    {
        ISurface CreateDesert();
        ISurface CreateForest();
        ISurface CreateMeadows();
        ISurface CreateMountains();
        ISurface CreateSwamp();
    }
}
