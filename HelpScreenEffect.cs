using SFML.Graphics;
using SFML.Window;

namespace DoomFire
{
    public class HelpScreenEffect : Effect
    {
        Text text;
        Font font;
        int duration = 3;
        public bool Visible { get; set; } = true;

        public HelpScreenEffect(int duration) : base("HelpScreenEffect")
        {
            this.duration = duration;
            font = new Font("cour.ttf");
            text = new Text("[H] Toggle help\n[Up/Down] Adjust alpha\n[Left/Right] Cycle palette\n[Escape] Quit", font, 18);
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
            if (Game.Instance.GameTime.ElapsedTime.AsSeconds() > duration)
                Visible = false;
        }
    }
}
