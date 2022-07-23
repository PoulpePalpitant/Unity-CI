using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 02/03/2022

 * Brief    : Examples d'utilisation d'attributs
 * **********************************************************************
*/

namespace CodeExamples
{

    [SelectionBase] // Quand tu click sun un sous-objet dans le preview, va select le parent à la place du sous-objet
    [HelpURL("https://www.youtube.com/watch?v=oHg5SJYRHA0&ab_channel=cotter548")]
    public class UnityAttributes : MonoBehaviour
    {
        [SerializeField]
        string thisIsNotAString;

        [Header("========Important info==========")]
        [Tooltip("Here lies important info")]
        public GameObject prefab;
        public bool gg = false;

        [Range(0, 10)]
        public int size;

        /// Add a context menu named "Do Something" in the inspector
        /// of the attached script.
        [ContextMenu("Do Something")]
        void DoSomething()
        {
            Debug.Log("Oy!");
        }
    }
}