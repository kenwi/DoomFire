using System;
using SFML.Graphics;

namespace DoomFire
{
    public class MapEffect : Effect
    {
        Texture texture;
        Sprite sprite;

        string mapString = "0000000000000000" +
                           "0   0   0      0" +
                           "0   0          0" +
                           "0   0   0      0" +
                           "0   0   0      0" +
                           "0   0   0      0" +
                           "0   0   0      0" +
                           "00 000 000000000" +
                           "0              0" +
                           "0              0" +
                           "0              0" +
                           "0              0" +
                           "0              0" +
                           "0              0" +
                           "0              0" +
                           "0000000000000000";

        int pack_color(byte r, byte g, byte b, byte a = 255)
        {
            return (a << 24) + (b << 16) + (g << 8) + r;
        }

        Tuple<byte, byte, byte, byte> unpack_color(int color)
        {
            return Tuple.Create<byte, byte, byte, byte>(
                (byte)((color << 0) & 255),
                (byte)((color << 8) & 255),
                (byte)((color << 16) & 255),
                (byte)((color << 24) & 255)
            );
        }

        void drawRectangle(int[] img, int width, int height, int x, int y, int w, int h, int color)
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    var cx = x + i;
                    var cy = y + j;
                    img[cx + cy * width] = color;
                }
            }
        }

        public MapEffect() : base("MapTestEffect")
        {
            texture = new Texture(Game.Instance.Window.Size.X, Game.Instance.Window.Size.Y);
            int[] pixelData = new int[texture.Size.X * texture.Size.Y];
            
            var mapSize = Math.Sqrt(mapString.Length);
            int rect_w = (int)(Game.Instance.Window.Size.X / mapSize);
            int rect_h = (int)(Game.Instance.Window.Size.Y / mapSize);
            int width = (int)Game.Instance.Window.Size.X;
            int height = (int)Game.Instance.Window.Size.Y;

            int mapStringId = 0;
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    if (mapString.Substring(mapStringId++, 1) == "0")
                        continue;
                    byte r = (byte)(255 * x / texture.Size.X);
                    byte g = (byte)(255 * y / texture.Size.Y);
                    byte b = 0;
                    int rect_X = x * rect_w;
                    int rect_y = y * rect_h;
                    drawRectangle(pixelData, width, height, rect_X, rect_y, rect_w, rect_h, pack_color(255, 255, b));
                }
            }
            var byteBuffer = new byte[pixelData.Length * 4];
            Buffer.BlockCopy(pixelData, 0, byteBuffer, 0, byteBuffer.Length);
            texture.Update(byteBuffer);
            sprite = new Sprite(texture);
        }

        protected override void OnDraw(RenderTarget target, RenderStates states)
        {
            target.Draw(sprite, states);
        }

        protected override void OnUpdate(float time)
        {

        }
    }
}
