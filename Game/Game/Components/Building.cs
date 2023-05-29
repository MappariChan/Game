using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class Building
    {
        public Hexagon Section { get; set; }

        public Building(Hexagon hexagon)
        {
            hexagon.SectionUI.PictureBox.Image = Image.FromFile("../../../Images/Building/BuildingWithMeadows.png");
            Section = hexagon;
        }
    }
}
