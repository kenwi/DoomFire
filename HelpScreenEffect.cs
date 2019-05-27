using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace DoomFire
{
    public class HelpScreenEffect : Effect
    {
        Text text;
        Font font;
        Clock clock;
        int duration = 5;
        public bool Visible { get; set; } = true;

        public HelpScreenEffect() : base("HelpScreenEffect")
        {
            clock = new Clock();
            font = new Font("cour.ttf");
            text = new Text("[H] Toggle help\n[Up/Down] Adjust alpha\n[Left/Right] Cycle palette\n[Escape] Quit", font, 15);
        }

        protected override void OnDraw(RenderTarget target, RenderStates states)
        {
            if (Visible)
            {
                target.Draw(text, states);
            }
        }

        protected override void OnUpdate(float time)
        {
            if (clock?.ElapsedTime.AsSeconds() > duration)
            {
                Visible = false;
                clock = null;
            }
        }
    }
}
