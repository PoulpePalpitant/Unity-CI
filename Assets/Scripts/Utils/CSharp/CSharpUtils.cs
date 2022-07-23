using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 10/05/2022

 * Brief    : Utils pour des trucs funky en c#
 * **********************************************************************
*/

namespace Utils
{
    public class CSharpUtils
    {
        /// <summary>
        /// Génère une liste d'enum randomly, en excluant certaines valeurs
        /// </summary>
        /// <typeparam name="T">Le enum</typeparam>
        /// <param name="enumType"></param>
        /// <param name="toExclude">Si non null, va exclude certains éléments du enum</param>
        /// <returns></returns>
        static public List<T> GenerateRandomListOfEnums<T>(int ammountToGenerate, List<T> toExclude )
        {
            List<T> randomEnumsList = new List<T>(ammountToGenerate);
            T[] array = (T[])Enum.GetValues(typeof(T));

            var candidates = toExclude != null ? array.Except(toExclude) : array;
            var count = candidates.Count();
            for (int i = 0; i < ammountToGenerate; i++)
            {
                randomEnumsList.Add(candidates.ElementAt<T>(UnityEngine.Random.Range(0, count)));
            }

            return randomEnumsList;
        }
        static public List<T> GenerateRandomListOfEnums<T>(int ammountToGenerate, T [] toExclude)
        {
            List<T> randomEnumsList = new List<T>(ammountToGenerate);
            T[] array = (T[])Enum.GetValues(typeof(T));

            var candidates = toExclude != null ? array.Except(toExclude) :array;
            var count = candidates.Count();
            for (int i = 0; i < ammountToGenerate; i++)
            {
                randomEnumsList.Add(candidates.ElementAt<T>(UnityEngine.Random.Range(0, count)));
            }

            return randomEnumsList;
        }

    }
}