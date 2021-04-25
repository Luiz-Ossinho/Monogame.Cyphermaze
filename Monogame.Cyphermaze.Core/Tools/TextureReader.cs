using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monogame.Cyphermaze.Core.Tools
{
    public static class TextureReader
    {
        public static void ReadTexture(Texture2D texture)
        {
            using (var stream = new MemoryStream())
            {
                texture.SaveAsPng(stream, texture.Width, texture.Height);
                _ = Convert.ToBase64String(stream.ToArray());
            }
        }
    }
}
