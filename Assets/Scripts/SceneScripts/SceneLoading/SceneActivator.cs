using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 25/04/2022

 * Brief    : Permet d'activer un script juste après un changement de scène
 * 
 * In-depth : Ce script peut être bien utile pour les controllr de scène qui 
 * nécessite d'être activer avant tout les autres.
 * 
 * La méthode d'activation sera toujours appelé après
 * le Awake(si l'objet est enabled dans l'éditeur dans la scène) et 
 * avant le Start:
 * 
 * Awake() - SceneActivation() - Start()
 * **********************************************************************
*/

public abstract class SceneActivator : MonoBehaviour
{
    protected virtual void Awake()
    {
        SceneLoader.Instance.OnSceneActivation += OnSceneActivation;
    }

    protected virtual void OnDestroy()
    {
        if (SceneLoader.NoInstance)
        {
            return;
        }

        SceneLoader.Instance.OnSceneActivation -= OnSceneActivation;
    }

    protected void OnSceneActivation(SceneActivatedArgs obj)
    {
        SceneLoader.Instance.OnSceneActivation -= OnSceneActivation;
        SceneActivation();
    }

    protected abstract void SceneActivation();
}
