using System;
using SFML.Graphics;
using System.Numerics;

namespace DoomFire
{
    public class MapEffect : Effect
    {
        byte[] map ={
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1,
            1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1,
            1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        };

        byte[] palette = {
            0xFF, 0x77, 0x00,
            0xAB, 0xBB, 0xBB
        };

        int SquareMapSize => (int)Math.Sqrt(map.Length);
        int width => (int)Game.Instance.Window.Size.X;
        int height => (int)Game.Instance.Window.Size.Y;
        Texture texture;
        Sprite sprite;

        public MapEffect() : base("MapTestEffect")
        {
            texture = new Texture(Game.Instance.Window.Size.X, Game.Instance.Window.Size.Y);
            Vector2 cellSize = new Vector2(width / SquareMapSize, height / SquareMapSize);
            int[] pixels = new int[width * height];
            int cellId = 0;
            for (int y = 0; y < SquareMapSize; y++)
            {
                for (int x = 0; x < SquareMapSize; x++)
                {
                    var cellColorId = map[cellId++];
                    var cellColor = palette.AsSpan(cellColorId * 3, 3);
                    drawRectangle(pixels, width, height, x * (int)cellSize.X, y * (int)cellSize.Y, (int)cellSize.X, (int)cellSize.Y, pack_color(cellColor[0], cellColor[1], cellColor[2]));
                }
            }
            var bytes = new byte[width * height * 4];
            Buffer.BlockCopy(pixels, 0, bytes, 0, bytes.Length);
            texture.Update(bytes);
            sprite = new Sprite(texture);
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

        protected override void OnDraw(RenderTarget target, RenderStates states)
        {
            target.Draw(sprite, states);
        }

        protected override void OnUpdate(float time)
        {
        }

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
    }
}
