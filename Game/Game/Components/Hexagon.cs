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

        private void setSurface(TypeOfSurfaces typeOfSurface, ISurfaceFactory surfaceFactory)
        {
            int randomResourceAmount = new Random().Next(5, 11);
            switch (typeOfSurface)
            {
                case TypeOfSurfaces.Meadows:
                    Surface = surfaceFactory.CreateMeadows();
                    Resource = null;
                    break;
                case TypeOfSurfaces.Desert:
                    Surface = surfaceFactory.CreateDesert();
                    Resource = new Resource(ResourceType.Sand, randomResourceAmount);
                    break;
                case TypeOfSurfaces.Mountains:
                    Surface = surfaceFactory.CreateMountains();
                    Resource = new Resource(ResourceType.Stone, randomResourceAmount);
                    break;
                case TypeOfSurfaces.Swamp:
                    Surface = surfaceFactory.CreateSwamp();
                    break;
                case TypeOfSurfaces.Forest:
                    Surface = surfaceFactory.CreateForest();
                    Resource = new Resource(ResourceType.Wood, randomResourceAmount);
                    break;
            }
        }

        public Hexagon(Panel container, int x, int y, int width, int height, TypeOfSurfaces typeOfSurface, ISurfaceFactory surfaceFactory)
        {
            setSurface(typeOfSurface, surfaceFactory);
            SectionUI = new HexagonUI(container, x, y, width, height, Surface.SurfaceImage);
            Neighbours = new List<Hexagon>();
            IsSelected = false;
            Color = PlayerColor.None;
            TypeOfSurface = typeOfSurface;
            SectionTroops = null;
            Player = null;
            Building = null;
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
