using Game.Components;

namespace Game
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            
            InitializeComponent();
            Load += FormLoad;
        }

        private void FormLoad(object sender, EventArgs e)
        {
            Components.Game game = new Components.Game(this, 2);
            game.Start();
        }
    }
}