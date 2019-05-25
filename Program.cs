using SFML.System;

namespace DoomFire
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.Instance.Init();
            InputHandler.Instance.Init();
            while (Game.Instance.Window.IsOpen)
            {
                InputHandler.Instance.Update();
                Game.Instance.Update();
                Game.Instance.Render();
            }
        }
    }
}
