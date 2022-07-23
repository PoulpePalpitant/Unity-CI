using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Project  : Flask
 * Author   : https://monoflauta.com/2021/07/27/11-useful-unity-c-extension-methods/
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 30/03/2022

 * Brief    : Extensions methods of C# Librairies
 * **********************************************************************
*/

public static class CSharpExtensions
{
    /// Ref: https://monoflauta.com/2021/07/27/11-useful-unity-c-extension-methods/
    public static T GetRandomItem<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    /// Ref: https://monoflauta.com/2021/07/27/11-useful-unity-c-extension-methods/
    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = list.Count - 1; i > 1; i--)
        {
            var j = Random.Range(0, i + 1);
            var value = list[j];
            list[j] = list[i];
            list[i] = value;
        }
    }
}
