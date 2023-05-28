using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Components.Resources;

namespace Game.Components
{
    public abstract class ISurface
    {
        public Image SurfaceImage { get; set; }

        public ISurface(string imageUrl) {
            SurfaceImage = Image.FromFile(imageUrl);
        }
    }
}
