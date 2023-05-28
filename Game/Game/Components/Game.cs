using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Game.Components.Resources;

namespace Game.Components
{
    public class Game
    {
        public List<Player> Players { get; set; }

        private TaskCompletionSource<Hexagon> tcs;

        public ISurfaceFactory surfaceFactory;

        public bool IsOver { get; set; }

        public Form1 Form { get; set; }

        private async Task WaitForPlayerChooseAsync()
        {
            await tcs.Task;
        }

        public Game(Form1 form, int amountOfPlayers) 
        {
            tcs = new TaskCompletionSource<Hexagon>(); 
            int[][] sections = {
                new int[]{1,},
                new int[]{1,1,},
                new int[]{1,},
            };

            surfaceFactory = new ClassicSurfaceFactory();

            Field.InitializeField(form, sections, tcs, surfaceFactory);

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

        public void ShowPotentialMoves(Player player)
        {
            foreach (var territorySection in player.Territory)
            {
                if (territorySection.SectionTroops != null)
                {
                    var notSelectedNeighbours = territorySection.Neighbours.Where(sec => sec.IsSelected == false).ToList();
                    foreach (var hexagon in notSelectedNeighbours)
                    {
                        hexagon.ShowPotenitalMove();
                    }
                }
            }
        }

        public void HidePotentialMoves(Player player)
        {
            foreach (var territorySection in player.Territory)
            {
                var notSelectedNeighbours = territorySection.Neighbours.Where(sec => sec.IsSelected == false).ToList();
                foreach (var hexagon in notSelectedNeighbours)
                {
                    hexagon.HidePotenitalMove();
                }
            }
        }

        public void ShowPlayerResources(Player player)
        {
            var searchedControlls = Form.Controls.Find("Info", true);
            Panel panel;
            if (searchedControlls.Length == 0)
            {
                panel = new Panel();
                panel.Name = "Info";
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
        }

        public async void Start() 
        {
            while (!IsOver)
            {
                foreach (var player in Players)
                {
                    if (player == null) continue;
                    player.AddResources();
                    player.RebornDeathArmy();
                    ShowPlayerResources(player);
                    ShowPotentialMoves(player);
                    await WaitForPlayerChooseAsync();
                    HidePotentialMoves(player);
                    var selectedSection = tcs.Task.Result;
                    player.ChooseSection(selectedSection);
                    foreach (var playerCheck in Players) {
                        if (playerCheck.IsDead()) { 
                            playerCheck.Death();
                            int index = Players.IndexOf(playerCheck);
                            Players[index] = null;
                            break;
                        }
                    }
                    if ((double)player.Territory.Count / Field.GetField().Hexagons.Count > 0.7 || Players.Where(playerCheck => playerCheck != null).Count() == 1)
                    {
                        IsOver = true;
                        break;
                    }
                    tcs = new TaskCompletionSource<Hexagon>();
                    Field.GetField().tcs = tcs;
                }
            }
            MessageBox.Show("End");
        }
    }
}
