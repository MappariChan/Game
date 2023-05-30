using Game.Components.ClassicSurfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components
{
    public class Build : ActionStrategy
    {
        private Command.Command command;
        private TextBoxLogger textBoxLogger;

        public Build(TextBoxLogger textBoxLogger)
        { 
            this.textBoxLogger = textBoxLogger;
        }

        private void ShowPotentialBuildings(Player player)
        {
            foreach (var section in player.Territory.Where(ter => ((Hexagon)ter.resourceService).Surface is Meadows))
            {
                ((Hexagon)section.resourceService).ShowPotenitalMove();
            }
        }

        private void HidePotentialBuildings(Player player)
        {
            foreach (var section in player.Territory.Where(ter => ((Hexagon)ter.resourceService).Surface is Meadows))
            {
                ((Hexagon)section.resourceService).HidePotentialBuilding();
            }
        }

        public async Task Action(Player player, ISectionClickService sectionClickService) {
            ShowPotentialBuildings(player);
            var result = await ((SectionClickProxy)sectionClickService).Tcs.Task;
            command = new Command.BuildCommand(player, textBoxLogger);
            command.Execute();
            player.ChooseSectionToBuild(result);
            HidePotentialBuildings(player);
        }
    }
}
