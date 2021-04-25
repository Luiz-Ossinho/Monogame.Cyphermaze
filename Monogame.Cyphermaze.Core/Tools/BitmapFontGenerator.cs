using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monogame.Cyphermaze.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monogame.Cyphermaze.Core.Tools
{
    public static class BitmapFontGenerator
    {
        public static Texture2D GenerateTextTexture(Texture2D texSrcLetters, int numSlots, GraphicsDevice graphics)
        {
            // get the manual color data
            Color[] clrSrcLetters = new Color[texSrcLetters.Width * texSrcLetters.Height];
            texSrcLetters.GetData(clrSrcLetters);
            // Create the array to hold our current letter
            Color[][] cols = new Color[32][];
            for (int i = 0; i < cols.Length; i++)
                cols[i] = new Color[32];
            // there are 64 letters in the source texture
            // generate the letter data for each
            Letter[] letters = new Letter[64];
            for (int i = 0; i < 64; i++)
            {
                for (int x = 0; x < 32; x++)
                    for (int y = 0; y < 32; y++)
                    {
                        cols[x][y] = clrSrcLetters[((i * 32) + x) + (y * texSrcLetters.Width)];
                    }
                letters[i] = new Letter(cols);
            }
            // Tell the letters to trim themselves
            // This removes bordering alpha-space
            for (int i = 0; i < 64; i++)
                letters[i] = letters[i].TrimLetter();
            // find the maximum dimensions for all 
            // the letters. Surely they arent 32x32?
            int maxW = 0, maxH = 0;
            for (int i = 0; i < 64; i++)
            {
                if (letters[i].Width > maxW)
                    maxW = letters[i].Width;
                if (letters[i].Height > maxH)
                    maxH = letters[i].Height;
            }
            // Tell the letters to expand to the max
            // This makes all letters the same size
            // and centers them
            for (int i = 0; i < 64; i++)
            {
                letters[i] = letters[i].ExpandLetter(maxW, maxH);
                // also generate their "value" or how much
                // of their allotted space is letter, and
                // how much is empty space
                letters[i].GenerateValue();
            }
            // Generate some lists
            List<Letter> letterList = new List<Letter>();
            for (int i = 0; i < 64; i++)
                letterList.Add(letters[i]);
            Letter[] slotList = new Letter[numSlots];
            // Filter through the letters and pick the best ones
            for (int i = 0; i < letterList.Count; i++)
            {
                for (int k = 0; k < numSlots; k++)
                    if (slotList[k] == null || Math.Abs((letterList[i].Value * numSlots) - (k)) < Math.Abs((slotList[k].Value * numSlots) - (k)))
                        slotList[k] = letterList[i];
            }
            // And put them all in one pretty texture
            Texture2D text = new Texture2D(graphics, maxW * numSlots, maxH);
            Color[] newCols = new Color[text.Width * text.Height];
            for (int i = 0; i < numSlots; i++)
                for (int x = 0; x < maxW; x++)
                    for (int y = 0; y < maxH; y++)
                        newCols[((i * maxW) + x) + (y * text.Width)] = new Color(slotList[numSlots - i - 1].Colors[x][y], slotList[numSlots - i - 1].Colors[x][y], slotList[numSlots - i - 1].Colors[x][y]);
            text.SetData(newCols);
            // Yay! Done.
            return text;
        }
    }
}
