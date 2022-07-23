using System;
using System.Collections.Generic;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : BIG MATHS
 * **********************************************************************
*/

namespace Utils
{
    static public class Math
    {
        public static Random rng = new Random();

        /// <summary>
        /// Randomization of any list
        /// </summary>
        /// <remarks> https://stackoverflow.com/questions/273313/randomize-a-listt </remarks>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }

}