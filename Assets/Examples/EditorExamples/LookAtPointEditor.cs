using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 03/02/2022

 * Brief    : Example d'editor custom. 
 * 
 * Warning  : Doit être placé dans l'Editor
 * **********************************************************************
*/

// TODO: DELETE ME


//namespace CodeExample
//{

//    ////[CustomEditor(typeof(LookAtPoint))]
//    ////[CanEditMultipleObjects]
//    //public class LookAtPointEditor : Editor
//    //{
//    //    SerializedProperty lookAtPoint;

//    //    void OnEnable()
//    //    {
//    //        lookAtPoint = serializedObject.FindProperty("lookAtPoint");
//    //    }

//    //    public override void OnInspectorGUI()
//    //    {
//    //        serializedObject.Update();
//    //        EditorGUILayout.PropertyField(lookAtPoint);
//    //        serializedObject.ApplyModifiedProperties();

//    //        if (lookAtPoint.vector3Value.y > (target as LookAtPoint).transform.position.y)
//    //        {
//    //            EditorGUILayout.LabelField("(Above this object)");
//    //        }
//    //        if (lookAtPoint.vector3Value.y < (target as LookAtPoint).transform.position.y)
//    //        {
//    //            EditorGUILayout.LabelField("(Below this object)");
//    //        }

//    //        EditorGUILayout.LabelField("La mère à William", EditorStyles.boldLabel);


//    //    }


//    //}
//}