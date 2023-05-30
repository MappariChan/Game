using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class SectionClickService:ISectionClickService
    {
        public TaskCompletionSource<Hexagon> tcs;

        public SectionClickService(TaskCompletionSource<Hexagon> tcs)
        {
            this.tcs = tcs;
        }

        public void Option(object sender, EventArgs e)
        {
            Hexagon hexagon = null;
            if (sender is Panel)
            {
                hexagon = Field.GetField().Hexagons.Where(hexagon => hexagon.SectionUI.Section == (Panel)sender).First();
            }
            else
            {
                hexagon = Field.GetField().Hexagons.Where(hexagon => hexagon.SectionUI.Section == (Panel)((PictureBox)sender).Parent).First();
            }
            tcs.TrySetResult(hexagon);
        }
    }
}
