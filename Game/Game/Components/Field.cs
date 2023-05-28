using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Game.Components
{
    //singleton
    public class Field
    {
        public Panel FieldUI { get; set; }
        private static Field? field;
        public List<Hexagon> Hexagons { get; set; }
        public TaskCompletionSource<Hexagon> tcs { get; set; }
        public static void SectionClick(object sender, EventArgs e)
        {
            Hexagon hexagon = null;
            if (sender is Panel)
            {
                hexagon = field.Hexagons.Where(hexagon => hexagon.SectionUI.Section == (Panel)sender).First();
            }
            else 
            {
                hexagon = field.Hexagons.Where(hexagon => hexagon.SectionUI.Section == (Panel)((PictureBox)sender).Parent).First();
            }
            field.tcs.TrySetResult(hexagon);
        }

        private void CreateNewSection(int x, int y, int hexagonWidth, int hexagonHeight, ISurfaceFactory surfaceFactory)
        {
            int max = Enum.GetValues(typeof(TypeOfSurfaces)).Length;
            TypeOfSurfaces randomSurface = (TypeOfSurfaces)new Random().Next(0, max);
            var newSection = new Hexagon(FieldUI, x, y, hexagonWidth, hexagonHeight, randomSurface, surfaceFactory);
            foreach (var hexagon in Hexagons)
            {
                hexagon.AddNeigbourIfNeighbour(newSection);
            }
            Hexagons.Add(newSection);
        }

        private Field(Form1 form, int[][] sections, ISurfaceFactory surfaceFactory)
        {
            Hexagons = new List<Hexagon>();
            FieldUI = new Panel();
            FieldUI.ControlAdded += FieldUI_ControlAdded;

            int hexagonWidth = 120;
            int hexagonHeight = 120;
            int marginX = 12;
            int marginY = 12;
            int hexagonSectionWidth = hexagonHeight + marginX;
            int hexagonSectionHeight = hexagonWidth + marginY;

            int x;
            int y = 0;

            int maxSectionsAmount = sections.Max(row => row.Length);

            foreach (var row in sections)
            {
                x = (int)((maxSectionsAmount - row.Length) / 2.0 * hexagonSectionWidth);
                foreach (var section in row)
                {
                    if (section == 1)
                    {
                        CreateNewSection(x, y, hexagonWidth, hexagonHeight, surfaceFactory);
                    }
                    x += hexagonSectionWidth;
                }
                y += hexagonSectionHeight * 3 / 4;
            }

            int fieldWidth = maxSectionsAmount * hexagonSectionWidth;
            int fieldHeight = hexagonSectionHeight + (sections.Length - 1) * hexagonSectionHeight * 3/4;

            FieldUI.Size = new Size(fieldWidth, fieldHeight);
            FieldUI.Location = new Point(100, 0);
            FieldUI.BackColor = Color.FromArgb(255,48,48,48);
            
            form.Controls.Add(FieldUI);
        }

        private void FieldUI_ControlAdded(object? sender, ControlEventArgs e)
        {
            e.Control.BringToFront();
        }

        public static void InitializeField(Form1 form, int[][] sections, TaskCompletionSource<Hexagon> tcs, ISurfaceFactory surfaceFactory)
        {
            if (field == null)
            {
                field = new Field(form, sections, surfaceFactory);
                field.tcs = tcs;
            }
        }

        public static Field GetField()
        {
            return field;
        }
    }
}
