using Game.Components.ClassicSurfaces;
using Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class SectionClickProxy : ISectionClickService
    {
        public TaskCompletionSource<Hexagon> Tcs { get; set; }
        private SectionClickService sectionClickService;
        private Func<Player> getCurrentPlayer;
        private Func<string> getCurrentMode;
        public SectionClickProxy(Func<Player> getCurrentPlayer, Func<string> getCurrentMode)
        {
            Tcs = new TaskCompletionSource<Hexagon>();
            sectionClickService = new SectionClickService(Tcs);
            this.getCurrentPlayer = getCurrentPlayer;
            this.getCurrentMode = getCurrentMode;
        }

        private bool Validate(Hexagon section)
        {
            if (getCurrentMode() == "ATACK")
            {
                if (section.Neighbours.Count(sect => sect.Player == getCurrentPlayer() && sect.SectionTroops != null) > 0)
                {
                    return true;
                }
                MessageBox.Show("You can't atack this section!\nTry to choose green section!");
            }
            else
            {
                if (section.Surface is Meadows)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("You can build only on Meadows!\nTry to choose green section!");
                }
            }
            return false;
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
            if (Validate(hexagon))
            { 
                sectionClickService.Option(sender, e);
                Tcs = new TaskCompletionSource<Hexagon>();
                sectionClickService.tcs = Tcs;
            }
        }
    }
}
