using Game.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Game.Components.Resources;

namespace Game.Components
{
    public class Hexagon
    {
        public HexagonUI SectionUI { get; set; }
        public List<Hexagon> Neighbours { get; set; }
        public bool IsSelected { get; set; }
        public PlayerColor Color { get; set; }
        public ISurface Surface { get; set; }
        public TypeOfSurfaces TypeOfSurface { get; set; }
        public Resource Resource { get; set; }
        public Image SurfaceImage { get; set; }
        public Troops SectionTroops { get; set; }
        public Player Player { get; set; }

        public Building Building { get; set; }

        public Hexagon() {
            Neighbours = new List<Hexagon>();
            IsSelected = false;
            SectionTroops = null;
            Player = null;
            Building = null;
            Color = PlayerColor.None;
        }

        public void AddNeigbourIfNeighbour(Hexagon potentialNeighbour)
        {
            Point location = SectionUI.Section.Location;
            Point locationOfNeighbour = potentialNeighbour.SectionUI.Section.Location;
            Vector2 locationVec = new Vector2(location.X, location.Y);
            Vector2 locationOfNeighbourVec = new Vector2(locationOfNeighbour.X, locationOfNeighbour.Y);
            double distance = Vector2.Distance(locationVec, locationOfNeighbourVec);
            if (distance < SectionUI.Section.Size.Width * 1.5)
            {
                Neighbours.Add(potentialNeighbour);
                potentialNeighbour.Neighbours.Add(this);
            }
        }

        public void SelectBy(Player player)
        {
            if (Player != null) { 
                Player.Territory.Remove(this);
                if (Building != null) {
                    Player.BonusArmyCount -= 5;
                    player.BonusArmyCount += 5;
                }
            }
            Color = player.Color;
            IsSelected = true;
            SectionUI.ChangeSectionColor(player.Color);
            Player = player;
        }

        public void ShowPotenitalMove()
        {
            SectionUI.ChangeSectionColor(PlayerColor.Potential);
        }

        public void HidePotenitalMove()
        {
            SectionUI.ChangeSectionColor(PlayerColor.None);
        }

        public void HidePotentialBuilding()
        {
            SectionUI.ChangeSectionColor(Player.Color);
        }

        public void Unregister() {
            Player = null;
            Color = PlayerColor.None;
            IsSelected = false;
            SectionUI.Section.BackColor = System.Drawing.Color.Black;
        }
    }
}
