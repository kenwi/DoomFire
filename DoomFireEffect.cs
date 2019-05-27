using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace DoomFire
{
    public class DoomFireEffect : Effect
    {
        int width;
        int height;
        int[] palletteReferences;
        int[] pallette;
        int[] pixelData;
        byte[] imageBytes;

        Texture texture;
        Sprite sprite;

        public DoomFireEffect() : base("DoomFireEffect")
        {
            width = (int)Game.Instance.Window.Size.X;
            height = (int)Game.Instance.Window.Size.Y;

            int pixelCount = width * height;
            palletteReferences = new int[pixelCount];
            pixelData = new int[pixelCount];
            imageBytes = new byte[pixelData.Length * sizeof(int)];
            createPallette();
            intializePalletteRefs();

            texture = new Texture(Game.Instance.Window.Size.X, Game.Instance.Window.Size.Y);
            sprite = new Sprite(texture);
        }

        private void intializePalletteRefs()
        {
            for (int y = 0, writeIndex = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (y == height - 1)
                    {
                        palletteReferences[writeIndex++] = pallette.Length - 1;
                    }
                    else
                    {
                        palletteReferences[writeIndex++] = 0;
                    }
                }
            }
        }

        private void createPallette()
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
            int palleteSize = rgb.Length / 3;
            pallette = new int[palleteSize];
            for (int i = 0; i < palleteSize; i++)
            {
                byte alpha = (i == 0) ? (byte)0x00 : (byte)0xFF;
                byte red = rgb[3 * i + 0];
                byte green = rgb[3 * i + 2];
                byte blue = rgb[3 * i + 1];
                var color = new Color(red, green, blue, alpha);
                pallette[i] = (int)color.ToInteger();
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
            for (int i = 0; i < height * width; pixelData[i] = pallette[palletteReferences[i++]]) ;
            Buffer.BlockCopy(pixelData, 0, imageBytes, 0, imageBytes.Length);
            texture.Update(imageBytes);
        }

        private void spreadFire(int src)
        {
            int rand = (int)Math.Round(Game.Instance.Random.NextDouble() * 3.0) & 3;
            int dst = src - rand + 1;
            palletteReferences[Math.Max(0, dst - width)] = Math.Max(0, palletteReferences[src] - (rand & 1));
        }

        protected override void OnUpdate(float time)
        {
            for (int i = 0; i < width * height; spreadFire(i++)) ;
        }
    }
}
