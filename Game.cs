using SFML.System;
using SFML.Graphics;
using SFML.Window;
using System.Collections.Generic;
using System;

namespace DoomFire
{
    public class Game : Singleton<Game>
    {
        static Effect[] effects;
        Clock gameTime;
        RenderWindow window;

        public RenderWindow Window { get => window; private set => window = value; }
        public Vector2f MousePositionNormalized { get => new Vector2f((float)Mouse.GetPosition(window).X / window.Size.X, (float)Mouse.GetPosition(window).Y / window.Size.Y); }
        public Random Random { get; private set; }
        
        public void Init()
        {
            Random = new Random();
            gameTime = new Clock();
            window = new RenderWindow(new VideoMode(1024, 224), "DoomFire");
            effects = new Effect[]{
                new DoomFireEffect()
            };
        }

        public void Render()
        {
            window.Clear();
            foreach (var effect in effects)
                window.Draw(effect);
            window.Display();
        }

        public void Update()
        {
            foreach (var effect in effects)
            {
                effect.Update(gameTime.ElapsedTime.AsSeconds());
            }
        }
    }
}
