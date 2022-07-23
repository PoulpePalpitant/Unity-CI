using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 26/02/2022

 * Brief    : Instantie le jeu si n'est pas présent dans une scène.
 * **********************************************************************
*/

public class GameInitializer : MonoBehaviour
{
    [SerializeField] GameObject gamePrefab;

    private void Awake()
    {
        if (!Game.IsInitialized())
        {
            Instantiate(gamePrefab);
        }
        Destroy(gameObject);
    }
}
