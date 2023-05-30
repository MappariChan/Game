using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Components.Command;
using Game.Components.ModalForms;

namespace Game.Components
{
    public class Move : ActionStrategy
    {
        private Command.Command command;
        private TextBoxLogger textBoxLogger;

        public Move(TextBoxLogger textBoxLogger)
        {
            this.textBoxLogger = textBoxLogger;
        }

        private void ShowPotentialMoves(Player player)
        {
            foreach (var territorySection in player.Territory)
            {
                if (((Hexagon)territorySection.resourceService).SectionTroops != null)
                {
                    var notSelectedNeighbours = ((Hexagon)territorySection.resourceService).Neighbours.Where(sec => sec.IsSelected == false).ToList();
                    foreach (var hexagon in notSelectedNeighbours)
                    {
                        hexagon.ShowPotenitalMove();
                    }
                }
            }
        }

        private void HidePotentialMoves(Player player)
        {
            foreach (var territorySection in player.Territory)
            {
                var notSelectedNeighbours = ((Hexagon)territorySection.resourceService).Neighbours.Where(sec => sec.IsSelected == false).ToList();
                foreach (var hexagon in notSelectedNeighbours)
                {
                    hexagon.HidePotenitalMove();
                }
            }
        }

        private int ChooseAmount()
        {
            int result = 0;
            using (ModalFormForMove modalForm = new ModalFormForMove())
            {
                if (modalForm.ShowDialog() == DialogResult.OK)
                {
                    result = modalForm.Result;
                    // Use the result as needed
                    MessageBox.Show("Result: " + result.ToString());
                }
            }
            return result;
        }

        public async Task Action(Player player, ISectionClickService sectionClickService)
        {
            ShowPotentialMoves(player);
            var result = await((SectionClickProxy)sectionClickService).Tcs.Task;
            int amount = ChooseAmount();
            command = new AtackCommand(player, amount, result, textBoxLogger);
            command.Execute();
            HidePotentialMoves(player);
            player.ChooseSection(result, amount);
        }
    }
}
