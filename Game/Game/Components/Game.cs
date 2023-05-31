using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Game.Components.ClassicSurfaces;
using Game.Components.Resources;

namespace Game.Components
{
    public class Game
    {
        public List<Player> Players { get; set; }

        private Player currentPlayer { get; set; }

        private TaskCompletionSource<string> tcsMode;

        public ISurfaceFactory surfaceFactory;

        private ISectionClickService sectionClickService;

        private ActionStrategy actionStrategy;

        public bool IsOver { get; set; }

        public Form1 Form { get; set; }

        private string Mode { get; set; }

        private TextBoxLogger textBoxLogger;

        private Player getCurrentPlayer()
        { 
            return currentPlayer;
        }

        private string getCurrentMode()
        {
            return Mode;
        }
        private bool isModeSelected;

        //private void SectionClick(object sender, EventArgs e)
        //{
        //    Hexagon hexagon = null;
        //    if (sender is Panel)
        //    {
        //        hexagon = Field.GetField().Hexagons.Where(hexagon => hexagon.SectionUI.Section == (Panel)sender).First();
        //    }
        //    else
        //    {
        //        hexagon = Field.GetField().Hexagons.Where(hexagon => hexagon.SectionUI.Section == (Panel)((PictureBox)sender).Parent).First();
        //    }
        //    if (Mode == "ATACK")
        //    {
        //        if (hexagon.Neighbours.Count(sect => sect.Player == currentPlayer && sect.SectionTroops != null) > 0)
        //        {
        //            tcs.TrySetResult(hexagon);
        //        }
        //        else
        //        {
        //            MessageBox.Show("You can't atack this section!\nTry to choose green section!");
        //        }
        //    }
        //    else
        //    {
        //        if (hexagon.Surface is Meadows)
        //        {
        //            tcs.TrySetResult(hexagon);
        //        }
        //        else
        //        {
        //            MessageBox.Show("You can build only on Meadows!\nTry to choose green section!");
        //        }
        //    }
        //}

        public Game(Form1 form, int amountOfPlayers) 
        {
            Mode = "ATACK";
            textBoxLogger = new TextBoxLogger(form);
            tcsMode = new TaskCompletionSource<string>();
            int[][] sections = {
                new int[]{1,1,1,1},
                new int[]{1,1,1,1,1},
                new int[]{1,1,1,1,1,1},
                new int[]{1,1,1,1,1,1,1},
                new int[]{1,1,1,1,1,1},
                new int[]{1,1,1,1,1},
                new int[]{1,1,1,1},
            };

            surfaceFactory = new ClassicSurfaceFactory();

            sectionClickService = new SectionClickProxy(getCurrentPlayer, getCurrentMode);

            Field.InitializeField(form, sections, surfaceFactory, sectionClickService.Option);

            Players = new List<Player>();
            for (int i = 0; i < amountOfPlayers; i++)
            {
                Player player = null;
                int random = -1;
                while (true)
                {
                    random = new Random().Next() % Field.GetField().Hexagons.Count();
                    var randomSection = Field.GetField().Hexagons[random];
                    if (randomSection.IsSelected == false)
                    {
                        player = new Player((PlayerColor)i, randomSection);
                        break;
                    }
                    
                }
                Players.Add(player);
            }

            Form = form;

            IsOver = false;
        }

        public void ShowPlayerResources(Player player)
        {
            var searchedControlls = Form.Controls.Find("Info", true);
            Panel panel;
            if (searchedControlls.Length == 0)
            {
                panel = new Panel();
                panel.Name = "Info";
                panel.Size = new Size(100, 300);
                Form.Controls.Add(panel);
            }
            else {
                panel = (Panel)searchedControlls[0];
                panel.Controls.Clear();
            }
            int top = 20;
            var playerLabel = new Label();
            playerLabel.Text = "Player " + Enum.GetName(player.Color);
            playerLabel.ForeColor = player.Headquater.SectionUI.Section.BackColor;
            panel.Controls.Add(playerLabel);
            foreach (var resource in player.Resources.Resources) { 
                var label = new Label();
                label.Text = Enum.GetName(resource.Key) + " " + resource.Value.ToString();
                label.Location = new Point(0, top);
                panel.Controls.Add(label);
                top += 20;
            }
            var actionPanel = new Panel();
            actionPanel.Height = 150;
            actionPanel.Location = new Point(0, top + 40);
            var actionLabel = new Label();
            actionLabel.Text = "Modes";
            var atackButton = new Button();
            atackButton.Click += AtackButton_Click;
            atackButton.Text = "Atack";
            atackButton.Location = new Point(0, 40);
            atackButton.Height = 40;
            var buildButton = new Button();
            buildButton.Click += BuildButton_Click;
            buildButton.Text = "Build";
            buildButton.Location = new Point(0, 80);
            buildButton.Height = 40;
            actionPanel.Controls.Add(actionLabel);
            actionPanel.Controls.Add(atackButton);
            actionPanel.Controls.Add(buildButton);
            panel.Controls.Add(actionPanel);
        }

        private void BuildButton_Click(object? sender, EventArgs e)
        {
            if (currentPlayer.HaveEnoughResourceToBuild())
            {
                if (currentPlayer.Territory.Count(sect =>   ((Hexagon)sect.resourceService).Surface is Meadows) > 0)
                {
                    if (!isModeSelected)
                    {
                        tcsMode.TrySetResult("BUILD");
                        isModeSelected = true;
                    }
                }
                else {
                    MessageBox.Show("You haven`t meadows to build fortress!\nFirstly, try to find meadows!");
                }
            }
            else {
                MessageBox.Show("You haven't enough resource to build.\nYou need 5 stones, 5 wood, 5 sand to build!");
            }
        }

        private void AtackButton_Click(object? sender, EventArgs e)
        {
            if (!isModeSelected)
            {
                tcsMode.TrySetResult("ATACK");
                isModeSelected = true;
            }
        }

        public async void Start() 
        {
            while (!IsOver)
            {
                foreach (var player in Players)
                {
                    isModeSelected = false;
                    currentPlayer = player;
                    if (!player.IsAlive) continue;
                    player.AddResources();
                    player.RebornDeathArmy();
                    ShowPlayerResources(player);
                    MessageBox.Show("Choose mode");
                    Mode = await tcsMode.Task;
                    tcsMode = new TaskCompletionSource<string> ();
                    if (Mode == "ATACK")
                    {
                        actionStrategy = new Move(textBoxLogger);
                        
                    }
                    else if (Mode == "BUILD")
                    { 
                        actionStrategy= new Build(textBoxLogger);
                    }
                    await actionStrategy.Action(player, sectionClickService);
                    foreach (var playerCheck in Players)
                    {
                        if (playerCheck.IsAlive != false && playerCheck.IsDead())
                        {
                            playerCheck.Death();
                            break;
                        }
                    }
                    if ((double)player.Territory.Count / Field.GetField().Hexagons.Count > 0.7 || Players.Where(playerCheck => playerCheck.IsAlive).Count() == 1)
                    {
                        IsOver = true;
                        break;
                    }
                }
            }
            MessageBox.Show("End");
        }
    }
}
