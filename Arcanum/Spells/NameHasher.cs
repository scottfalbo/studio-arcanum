using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Arcanum.Spells
{
    public class NameHasher
    {
        /// <summary>
        /// Driver method to hash a name into a unique access code.
        /// </summary>
        /// <param name="name"> string input name </param>
        /// <returns> string access code </returns>
        public static string HashNameToAccessCode(string name)
        {
            name = name.Replace(" ", String.Empty).ToLower();
            char[] chars = name.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (i % 2 == 0)
                {
                    chars[i] = CharToRandomNumber();
                }
                if (i % 3 == 0)
                {
                    chars[i] = CharToUpper(chars[i]);
                }
            }
            chars = CharArrayRandomizer(chars);
            string code = CharArrayToString(chars);
            return code;
        }

        /// <summary>
        /// Replaces the given characer with a random number between 0-9.
        /// </summary>
        /// <param name="input"> char input </param>
        /// <returns> random number as char </returns>
        private static char CharToRandomNumber()
        {
            Random random = new Random();
            int num = random.Next(0, 10);
            string numStr = num.ToString();
            return (numStr.ToCharArray())[0];
        }

        /// <summary>
        /// Converts the given character to upper case.
        /// </summary>
        /// <param name="input"> input char </param>
        /// <returns> upper case input char </returns>
        private static char CharToUpper(char input)
        {
            string letter = input.ToString();
            letter = letter.ToUpper();
            return (letter.ToCharArray()[0]);
        }

        /// <summary>
        /// Randomize the array of characters.
        /// </summary>
        /// <param name="chars"> char[] </param>
        /// <returns> randomized char[] </returns>
        private static char[] CharArrayRandomizer(char[] chars)
        {
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(1, chars.Length);
                int y = random.Next(1, chars.Length);
                char holder = chars[x];
                chars[x] = chars[y];
                chars[y] = holder;
            }
            return chars;
        }

        /// <summary>
        /// Converts the char[] back into a string.
        /// </summary>
        /// <param name="chars"> char[] </param>
        /// <returns> string hashed code </returns>
        private static string CharArrayToString(char[] chars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char item in chars)
            {
                sb.Append(item);
            }
            return sb.ToString();
        }
    }
}
