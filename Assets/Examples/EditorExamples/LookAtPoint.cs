using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 2/03/2022

 * Brief    : Tourne un objet vers un point
 * **********************************************************************
*/

namespace CodeExamples
{
    [ExecuteInEditMode]
    public class LookAtPoint : MonoBehaviour
    {
        public Vector3 lookAtPoint = Vector3.zero;

        void Update()
        {
            transform.LookAt(lookAtPoint);
        }
    }
}