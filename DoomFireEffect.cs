using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace DoomFire
{
    public class DoomFireEffect : Effect
    {
        int width;
        int height;
        int[] paletteReferences;
        int[] palette;
        int[] pixelData;
        byte[] imageBytes;

        Texture texture;
        Sprite sprite;

        public DoomFireEffect() : base("DoomFireEffect")
        {
            width = (int)Game.Instance.Window.Size.X;
            height = (int)Game.Instance.Window.Size.Y;

            int pixelCount = width * height;
            paletteReferences = new int[pixelCount];
            pixelData = new int[pixelCount];
            imageBytes = new byte[pixelData.Length * sizeof(int)];
            createpalette();
            intializepaletteRefs();

            texture = new Texture(Game.Instance.Window.Size.X, Game.Instance.Window.Size.Y);
            sprite = new Sprite(texture);
        }

        private void intializepaletteRefs()
        {
            for (int y = 0, writeIndex = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == height - 1)
                    {
                        paletteReferences[writeIndex++] = palette.Length - 1;
                    }
                    else
                    {
                        paletteReferences[writeIndex++] = 0;
                    }
                }
            }
        }

        private void createpalette()
        {
            byte[] rgb = new byte[] {
                0x07,0x07,0x07,
                0x1F,0x07,0x07,
                0x2F,0x0F,0x07,
                0x47,0x0F,0x07,
                0x57,0x17,0x07,
                0x67,0x1F,0x07,
                0x77,0x1F,0x07,
                0x8F,0x27,0x07,
                0x9F,0x2F,0x07,
                0xAF,0x3F,0x07,
                0xBF,0x47,0x07,
                0xC7,0x47,0x07,
                0xDF,0x4F,0x07,
                0xDF,0x57,0x07,
                0xDF,0x57,0x07,
                0xD7,0x5F,0x07,
                0xD7,0x5F,0x07,
                0xD7,0x67,0x0F,
                0xCF,0x6F,0x0F,
                0xCF,0x77,0x0F,
                0xCF,0x7F,0x0F,
                0xCF,0x87,0x17,
                0xC7,0x87,0x17,
                0xC7,0x8F,0x17,
                0xC7,0x97,0x1F,
                0xBF,0x9F,0x1F,
                0xBF,0x9F,0x1F,
                0xBF,0xA7,0x27,
                0xBF,0xA7,0x27,
                0xBF,0xAF,0x2F,
                0xB7,0xAF,0x2F,
                0xB7,0xB7,0x2F,
                0xB7,0xB7,0x37,
                0xCF,0xCF,0x6F,
                0xDF,0xDF,0x9F,
                0xEF,0xEF,0xC7,
                0xFF,0xFF,0xFF
            };

            int paletteSize = rgb.Length / 3;
            palette = new int[paletteSize];
            for (int i = 0; i < paletteSize; i++)
            {
                int alpha = (i == 0) ? 0 : 255;
                int red = rgb[3 * i + 0];
                int green = rgb[3 * i + 1];
                int blue = rgb[3 * i + 2];
                palette[i] = (red << 24) + (blue << 16) + (green << 8) + alpha;
            }
        }

        protected override void OnDraw(RenderTarget target, RenderStates states)
        {
            updateTexture();
            target.Draw(sprite, states);
            System.Threading.Thread.Sleep(1000 / 60);
        }

        private void updateTexture()
        {
            for (int i = 0; i < height * width; pixelData[i] = palette[paletteReferences[i++]]) ;
            Buffer.BlockCopy(pixelData, 0, imageBytes, 0, imageBytes.Length);
            texture.Update(imageBytes);
        }

        private void spreadFire(int src)
        {
            int rand = (int)Math.Round(Game.Instance.Random.NextDouble() * 3.0) & 3;
            int dst = src - rand + 1;
            paletteReferences[Math.Max(0, dst - width)] = Math.Max(0, paletteReferences[src] - (rand & 1));
        }

        protected override void OnUpdate(float time)
        {
            for (int i = 0; i < width * height; spreadFire(i++)) ;
        }
    }
}
