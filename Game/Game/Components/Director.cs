using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class Director
    {
        public void ConstructHexagon(IHexagonBuilder builder, TypeOfSurfaces type, ISurfaceFactory factory, Panel container, Point location, Size size)
        {
            builder.Reset();
            builder.SetSurface(type, factory);
            builder.SetSectionUI(container, location, size);
        }
    }
}
