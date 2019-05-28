using System;
using SFML.System;

namespace DoomFire
{
    class Program
    {
        static void Main(string[] args)
        {
            int targetfps = 10;
            float targetUpdateRate = 1f / targetfps;

            Game.Instance.Init();
            InputHandler.Instance.Init();
            while (Game.Instance.Window.IsOpen)
            {
                var dt = Game.Instance.DeltaTime;
                while (dt < targetUpdateRate)
                {
                    dt += Game.Instance.DeltaTime;
                    System.Threading.Thread.Sleep(1000 / 60);
                }
                InputHandler.Instance.Update();
                Game.Instance.Update();
                Game.Instance.Render();
            }
        }
    }
}