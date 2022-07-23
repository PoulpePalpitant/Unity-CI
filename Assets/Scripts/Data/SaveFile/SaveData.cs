using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Project  : ...
 * Author   : Laurent Montreuil
 * Date     : 29/06/2022

 * Brief    : Donnée engregistrées dans un fichier pour le jeu
 * **********************************************************************
*/

[Serializable]
public class SaveData
{
    public string playerName;
    /*
        YOUR DATA HERE
    */

    public SaveData(string playerName)
    {
        this.playerName = playerName;
        /*
        
            YOUR DATA HERE


        */
    }
}


// WARNING : Tentative infructueuse de rendre générique l'enregistrement des données

// ************************************
// Les différents types de data entrys
// *************************
public abstract class SaveDataEntry<T>
{
    protected T _value;
    public T Value { get => _value; }
}
public abstract class ValueDataEntry<T> : SaveDataEntry<T>
{
    public abstract void Register(T value);
}
public abstract class CollectionDataEntry<C, V> : SaveDataEntry<C>
{
    public abstract void Register(V value);

}

// **************************************************
// Types de manipulation sur les data entrys
// **************************************************

[Serializable]
public class AddIntValueDataEntry : ValueDataEntry<int>
{
    public override void Register(int value)
    {
        _value += value;
    }
}
[Serializable]
public class AddFloatValueDataEntry : ValueDataEntry<float>
{
    public override void Register(float value)
    {
        _value += value;
    }
}
[Serializable]
public class AddListDataEntry<V> : CollectionDataEntry<List<V>, V>
{
    public AddListDataEntry()
    {
        _value = new List<V>();
    }
    public override void Register(V value)
    {
        _value.Add(value);
    }
}


