using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monogame.Cyphermaze.Core.Abstractions
{
    public class Letter : IComparable<Letter>
    {
        // An array of single-channel colors
        // think of it as alpha, since we specify
        // colors of each letter later
        public byte[][] Colors;

        // a few nice accessors
        public int Width { get { return Colors.Length; } }
        public int Height { get { return Colors[0].Length; } }

        // The Value of the letter is the
        // percentage of space used by it's
        // color. e.g. if a letter was half
        // opaque, half transparent, it would
        // be 0.5
        public float Value { get; private set; }

        /// <summary>
        /// Create an empty letter
        /// </summary>
        private Letter()
        {

        }

        /// <summary>
        /// Creates a letter from some color data
        /// </summary>
        /// <param name="colors">The color data to extract from</param>
        public Letter(Color[][] colors)
        {
            Colors = new byte[colors.Length][];
            for (int i = 0; i < Colors.Length; i++)
            {
                // when copying data, we assume greyscale
                // so only take the Red channel
                Colors[i] = new byte[colors[i].Length];
                for (int k = 0; k < Colors[i].Length; k++)
                    Colors[i][k] = colors[i][k].R;
            }
        }

        /// <summary>
        /// Generates the "Value" Property for this Letter
        /// </summary>
        public void GenerateValue()
        {
            Value = 0;
            // Add up the total alpha
            for (int i = 0; i < Width; i++)
                for (int k = 0; k < Height; k++)
                    Value += Colors[i][k] / 255f;
            // Divide by the number of pixels
            Value /= (Width * Height);
        }

        /// <summary>
        /// CompareTo function for sorting
        /// </summary>
        /// <param name="other">Letter to compare to</param>
        public int CompareTo(Letter other)
        {
            return -Value.CompareTo(other.Value);
        }

        /// <summary>
        /// Basically crops this letter
        /// </summary>
        /// <returns>A cropped letter</returns>
        public Letter TrimLetter()
        {
            Letter src = this;
            int minX = src.Width - 1;
            int maxX = 0;
            int minY = src.Height - 1;
            int maxY = 0;

            // find the rectangle for cropping
            for (int i = 0; i < src.Width; i++)
                for (int k = 0; k < src.Height; k++)
                {
                    if (src.Colors[i][k] > 0)
                    {
                        if (i < minX)
                            minX = i;
                        if (i > maxX)
                            maxX = i;
                        if (k < minY)
                            minY = k;
                        if (k > maxY)
                            maxY = k;
                    }
                }

            // Create the returned letter
            Letter newLetter = new Letter();
            newLetter.Colors = new byte[maxX - minX + 1][];
            // fill in the cropped colors
            for (int x = minX; x <= maxX; x++)
            {
                newLetter.Colors[x - minX] = new byte[maxY - minY + 1];
                for (int y = minY; y <= maxY; y++)
                    newLetter.Colors[x - minX][y - minY] = src.Colors[x][y];
            }

            return newLetter;
        }

        /// <summary>
        /// Expands and centers the Letter
        /// </summary>
        /// <param name="minWidth">The new width</param>
        /// <param name="minHeight">The new Height</param>
        /// <returns>A letter sized to minWidth x minHeight</returns>
        public Letter ExpandLetter(int minWidth, int minHeight)
        {
            Letter src = this;

            // Determine how much to offset this letter by
            int addLeft = (minWidth - src.Width) / 2;
            if ((minWidth - src.Width) % 2 == 1)
                addLeft++;
            int addY = minHeight - src.Height;
            int addTop = Math.Max(0, addY - 4);

            // Create the letter and clear the values
            Letter newLetter = new Letter();
            newLetter.Colors = new byte[minWidth][];
            for (int x = 0; x < minWidth; x++)
            {
                newLetter.Colors[x] = new byte[minHeight];
                for (int y = 0; y < minHeight; y++)
                    newLetter.Colors[x][y] = 0;
            }

            // Fill in the values with the source data
            for (int x = 0; x < src.Width; x++)
                for (int y = 0; y < src.Height; y++)
                {
                    newLetter.Colors[x + addLeft][y + addTop] = src.Colors[x][y];
                }

            return newLetter;
        }
    }
}
