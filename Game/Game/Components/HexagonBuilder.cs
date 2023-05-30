using Game.Components.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class HexagonBuilder : IHexagonBuilder
    {
        private Hexagon section;
        public void Reset() 
        {
            section = new Hexagon();
        }
        public void SetSurface(TypeOfSurfaces type, ISurfaceFactory factory)
        {
            section.TypeOfSurface = type;
            int randomResourceAmount = new Random().Next(1, 4);
            switch (type)
            {
                case TypeOfSurfaces.Meadows:
                    section.Surface = factory.CreateMeadows();
                    section.Resource = null;
                    break;
                case TypeOfSurfaces.Desert:
                    section.Surface = factory.CreateDesert();
                    section.Resource = new Resource(ResourceType.Sand, randomResourceAmount);
                    break;
                case TypeOfSurfaces.Mountains:
                    section.Surface = factory.CreateMountains();
                    section.Resource = new Resource(ResourceType.Stone, randomResourceAmount);
                    break;
                case TypeOfSurfaces.Swamp:
                    section.Surface = factory.CreateSwamp();
                    break;
                case TypeOfSurfaces.Forest:
                    section.Surface = factory.CreateForest();
                    section.Resource = new Resource(ResourceType.Wood, randomResourceAmount);
                    break;
            }
        }

        public void SetSectionUI(Panel container, Point location, Size size) 
        {
            section.SectionUI = new UI.HexagonUI(container, location, size, section.Surface.SurfaceImage);
        }


        public Hexagon GetResult()
        { 
            return section;
        }
    }
}
