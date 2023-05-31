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
    public class Hexagon : IResourceService
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
        public bool IsHeadquater { get; set; }

        public Building Building { get; set; }

        private HexagonState hexagonState;

        public Hexagon() {
            Neighbours = new List<Hexagon>();
            IsSelected = false;
            SectionTroops = null;
            Player = null;
            Building = null;
            Color = PlayerColor.None;
            IsHeadquater = false;
            hexagonState = new UnselectedHexagon(this);
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

        public void AddResource(Player player)
        {
            hexagonState.AddResources(player);
        }

        public void SelectBy(Player player)
        {
            if (Player != null) { 
                Player.Territory.Remove(Player.Territory.Find(sect => sect.resourceService == this));
                if (Building != null) {
                    Player.BonusArmyCount -= 5;
                    player.BonusArmyCount += 5;
                }
            }
            Color = player.Color;
            IsSelected = true;
            SectionUI.ChangeSectionColor(player.Color);
            Player = player;
            hexagonState = new SelectedHexagon(this);
        }

        public void SelectHeadquaterBy(Player player)
        {
            SelectBy(player);
            IsHeadquater = true;
            string playerColor = Enum.GetName(typeof(PlayerColor), player.Color);
            PictureBox flag = new PictureBox();
            flag.SizeMode = PictureBoxSizeMode.StretchImage;
            flag.BackColor = System.Drawing.Color.Transparent;
            flag.Image = Image.FromFile("../../../Images/Flags/" + playerColor + ".png");
            int size = SectionUI.PictureBox.Width;
            flag.Location = new Point(size / 5, size / 5);
            flag.Size = new Size(size/5*3, size/5*3);
            SectionUI.PictureBox.Controls.Add(flag);
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
