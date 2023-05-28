using Game.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Windows.Forms;

namespace Game.UI
{
    public class HexagonUI
    {
        public Panel Section { get; set; }
        public PictureBox PictureBox { get; set; }
        public PictureBox Clouds { get; set; }

        public HexagonUI(Panel container, int x, int y, int width, int height, Image SurfaceImage) {
            Section = new Panel();

            Section.Click += Field.SectionClick;

            Section.Size = new Size(width, height);
            Section.Location = new Point(x, y);

            Section.BackColor = System.Drawing.Color.Black;

            var panelRegion = new Region();

            var hexPath = new GraphicsPath();
            hexPath.AddPolygon(new[] {
                new Point(0, height / 4),
                new Point(width / 2, 0),
                new Point(width, height / 4),
                new Point(width, height * 3 / 4),
                new Point(width / 2, height),
                new Point(0, height * 3 / 4),
            });

            panelRegion.MakeEmpty();
            panelRegion.Union(hexPath);
            Section.Region = panelRegion;

            PictureBox = new PictureBox();
            PictureBox.Location = new Point(width * 1 / 10, height * 1 / 10);
            PictureBox.Size = new Size(width * 8 / 10, height * 8 / 10);
            PictureBox.Image = SurfaceImage;
            PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            PictureBox.BackColor = System.Drawing.Color.LightBlue;

            PictureBox.Click += Field.SectionClick;

            int picHeight = PictureBox.Height;
            int picWidth = PictureBox.Width;

            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(new[] {
                new Point(0, picHeight / 4),
                new Point(picWidth / 2, 0),
                new Point(picWidth, picHeight / 4),
                new Point(picWidth, picHeight * 3 / 4),
                new Point(picWidth / 2, picHeight),
                new Point(0, picHeight * 3 / 4),
            });
            PictureBox.Region = new Region(path);

            Clouds = new PictureBox();
            Clouds.Size = new Size(width, height);
            Clouds.SizeMode = PictureBoxSizeMode.StretchImage;
            Clouds.Image = Image.FromFile("../../../Images/Clouds.png");

            //Clouds.BackColor = Color.Transparent;

            Section.Controls.Add(PictureBox);

            Section.Controls.Add(Clouds);

            Section.Controls.SetChildIndex(Clouds, 0);

            container.Controls.Add(Section);
        }
        public void ChangeSectionColor(PlayerColor color)
        {
            Clouds.Visible = false;
            switch (color)
            {
                case PlayerColor.Red:
                    Section.BackColor = System.Drawing.Color.Red;
                    break;
                case PlayerColor.Purple:
                    Section.BackColor = System.Drawing.Color.Purple;
                    break;
                case PlayerColor.Blue:
                    Section.BackColor = System.Drawing.Color.Blue;
                    break;
                case PlayerColor.Yellow:
                    Section.BackColor = System.Drawing.Color.Yellow;
                    break;
                case PlayerColor.Potential:
                    Section.BackColor = System.Drawing.Color.FromArgb(0, 255, 4);
                    break;
                case PlayerColor.None:
                    Section.BackColor = System.Drawing.Color.Black;
                    break;
            }
        }
    }
}
