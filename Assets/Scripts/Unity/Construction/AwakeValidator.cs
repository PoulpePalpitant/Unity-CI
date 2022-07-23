using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 16/05/2022

 * Brief    : Sert à valider les composantes d'un script
 * **********************************************************************
*/

public abstract class AwakeValidator : MonoBehaviour
{
    /// <summary>
    /// Va caller automatiquement Validate. Il est donc recommandé d'appelé base.Awake() après l'initialisation  de l'objet
    /// </summary>
    protected virtual void Awake()
    {
        Validate();
    }

    /// <summary>
    /// Mettre le code de validation dans le awake 
    /// </summary>
    protected abstract void Validate();
}
