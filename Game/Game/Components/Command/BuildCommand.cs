using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Game.Components.Command
{
    public class BuildCommand : Command
    {
        private Player player;
        private TextBoxLogger logger;

        public BuildCommand(Player player, TextBoxLogger logger)
        {
            this.player = player;
            this.logger = logger;
        }

        public void Execute()
        {
            string colorName = Enum.GetName(typeof(PlayerColor), player.Color);
            string message = "Player " + colorName + " build fortress!";
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
