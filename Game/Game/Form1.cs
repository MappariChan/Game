using Game.Components;

namespace Game
{
    public partial class Form1 : Form {

        TaskCompletionSource task;

        public Form1()
        {
           
            InitializeComponent();
            task = new TaskCompletionSource();
            Shown += Form1_Shown;
            Load += FormLoad;
            
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            task.TrySetResult();
        }

        private async void FormLoad(object sender, EventArgs e)
        {
            Components.Game game = new Components.Game(this, 3);
            await task.Task;
            game.Start();
        }
    }
}