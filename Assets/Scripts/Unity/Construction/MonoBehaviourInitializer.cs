using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 21/04/2022

 * Brief    : Permet de d'initialiser des scripts Monobehaviour plus facilement
 * **********************************************************************
*/

/// <summary>
/// Permet d'utiliser un Initializer pour classes de types monobehaviour <br/><br/>
/// WARNING: Pour faire des Initialiser fonctionnant avec héritage, utilisez plutôt 
/// <see cref="MonoBehaviourInheritanceInitializer{T}"/> et <see cref="MonoBehaviourInitializerArgs"/><br/><br/>
/// <typeparam name="T">T: L'objet qui contiendra les paramètres de l'Initializer</typeparam>
/// </summary>
public abstract class MonoBehaviourInitializer<T> : MonoBehaviour
{
    private bool _initialized;

    public bool Initialized { get => _initialized; }

    public void Intialize(T initializerArgs)
    {
        if (_initialized)
        {
            Debug.LogError("Object was already initialized.");
            return;
        }

        DoInitialize(initializerArgs);
        _initialized = true;
    }

    protected abstract void DoInitialize(T initializerArgs);
}


/// <summary>
/// Utilisation : faire héritage de cette classe + <see cref="MonoBehaviourInitializerArgs"/>  <br/><br/>
/// <example>Pour des exemples d'implémentation, voir <see cref="CodeExamples.FooBarFactory"/></example>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoBehaviourInheritanceInitializer<T> : MonoBehaviourInitializer<T> where T : MonoBehaviourInitializerArgs { }

/// <summary>
///  Args pour l'initializer. 
///  Utilisez l'héritage pour fournir les paramètres voulu à l'initializer
/// </summary>
public abstract class MonoBehaviourInitializerArgs { }
