using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public interface IHexagonBuilder
    {
        public void Reset();
        public void SetSurface(TypeOfSurfaces type, ISurfaceFactory factory);
        public void SetSectionUI(Panel container, Point location, Size size);
    }
}
