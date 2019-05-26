using System;
using System.Collections.Generic;
using SFML.Graphics;

namespace DoomFire
{
    public class DoomFireEffect : Effect
    {
        // https://github.com/chriswhocodes/DoomFire/blob/master/src/main/java/com/chrisnewland/javafx/doomfire/DoomFire.java
        int imageWidth;
        int imageHeight;
        int[] palletteReferences;
        int[] pallette;
        int[] pixelData;
        byte[] imageBytes;

        Texture texture;
        Sprite sprite;

        public DoomFireEffect() : base("DoomFireEffect")
        {
            imageWidth = (int)Game.Instance.Window.Size.X;
            imageHeight = (int)Game.Instance.Window.Size.Y;

            int pixelCount = imageWidth * imageHeight;
            palletteReferences = new int[pixelCount];
            pixelData = new int[pixelCount];
            imageBytes = new byte[pixelData.Length * sizeof(int)];
            createPallete();
            intializePalleteRefs();

            texture = new Texture(Game.Instance.Window.Size.X, Game.Instance.Window.Size.Y);
            sprite = new Sprite(texture);
        }

        private void intializePalleteRefs()
        {
            int writeIndex = 0;
            for (int y = 0; y < imageHeight; y++)
            {
                for (int x = 0; x < imageWidth; x++)
                {
                    if (y == imageHeight - 1)
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

        private void createPallete()
        {
            int[] rawPalleteRGB = new int[] {
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
            int palleteSize = rawPalleteRGB.Length / 3;
            pallette = new int[palleteSize];

            for (int i = 0; i < palleteSize; i++)
            {
                int alpha = (i == 0) ? 0 : 255;
                int red = rawPalleteRGB[3 * i + 2];
                int green = rawPalleteRGB[3 * i + 1];
                int blue = rawPalleteRGB[3 * i + 0];
                int rgba = ( alpha << 24) + (red << 16) + ( green << 8) + blue;
                pallette[i] = rgba;
            }
        }

        protected override void OnDraw(RenderTarget target, RenderStates states)
        {
            writeFireImage();
            Buffer.BlockCopy(pixelData, 0, imageBytes, 0, imageBytes.Length);
            texture.Update(imageBytes);
            target.Draw(sprite, states);
        }

        private void spreadFire(int src)
        {
            int rand = (int)Math.Round(Game.Instance.Random.NextDouble() * 3.0) & 3;
            int dst = src - rand + 1;
            palletteReferences[Math.Max(0, dst - imageWidth)] = Math.Max(0, palletteReferences[src] - (rand & 1));
        }

        private void writeFireImage()
        {
            int pos = 0;
            for (int y = 0; y < imageHeight; y++)
            {
                for (int x = 0; x < imageWidth; x++)
                {
                    var pixelColorIndex = palletteReferences[pos];
                    var pixelValue = pallette[pixelColorIndex];
                    pixelData[pos] = pixelValue;
                    pos++;
                }
            }
        }

        protected override void OnUpdate(float time)
        {
            for (int x = 0; x < imageWidth; x++)
            {
                for (int y = 1; y < imageHeight; y++)
                {
                    spreadFire(y * imageWidth + x);
                }
            }
        }
    }
}
