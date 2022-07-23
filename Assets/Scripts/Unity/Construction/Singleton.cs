using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Author   : Glen Stevens
 * Date     : 04/03/2022

 * Brief    : Generic pour un singleton
 * **********************************************************************
*/


public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    Debug.LogError("An instance of " + typeof(T) +
                       " is needed in the scene, but there is none.");
                }
            }

            return instance;
        }
    }
}