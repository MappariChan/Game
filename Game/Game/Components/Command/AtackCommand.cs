using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game.Components.Command
{
    public class AtackCommand : Command
    {
        private Player player;
        private int amount;
        private Hexagon section;
        private TextBoxLogger logger;

        public AtackCommand(Player player, int amount, Hexagon section, TextBoxLogger logger)
        {
            this.player = player;
            this.amount = amount;
            this.section = section;
            this.logger = logger;
        }

        public void Execute() {
            string colorName = Enum.GetName(typeof(PlayerColor), player.Color);
            string message = "Player " + colorName + " atack " + Enum.GetName(typeof(TypeOfSurfaces), section.TypeOfSurface) + " by " + amount.ToString() + " warriors!";
            Color playerColor = Color.Black;
            switch (player.Color)
            {
                case PlayerColor.Red:
                    playerColor = Color.Red;
                    break;
                case PlayerColor.Blue:
                    playerColor = Color.Blue;
                    break;
                case PlayerColor.Purple:
                    playerColor = Color.Purple;
                    break;
                case PlayerColor.Yellow:
                    playerColor = Color.Yellow;
                    break;
            }
            logger.AddColoredLine(message, playerColor);
        }
    }
}
