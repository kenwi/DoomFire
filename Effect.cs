using SFML.Graphics;
using System.Diagnostics;

namespace DoomFire
{
    public abstract class Effect : Drawable
    {
        protected abstract void OnDraw(RenderTarget target, RenderStates states);
        protected abstract void OnUpdate(float time, float x, float y);

        public string Name { get; private set; }

        protected Effect(string name)
        {
            Name = name;
            Debug.Assert(Shader.IsAvailable);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            OnDraw(target, states);
        }

        public void Update(float time, float x, float y)
        {
            OnUpdate(time, x, y);
        }
    }
}
