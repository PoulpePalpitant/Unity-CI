using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 09/05/2022

 * Brief    : Utility class qui permet de créer des info box selon un hover
 * **********************************************************************
*/

/// <summary>
/// Will create a box of a certain type whenever you hover on any of the _uiHoverObjects. <br/>
/// You just need to implement the method that will generate the content of the Infobox <br/>
/// </summary>
/// <typeparam name="BoxType">The type of box that will be instantiated by the InfoBoxManager</typeparam>
/// <typeparam name="BoxContent">and it's content</typeparam>
public abstract class UIHoverBoxMaker<BoxType, BoxContent> : MonoBehaviour
    where BoxType : InfoBox             
    where BoxContent : InfoBoxContent    
{
    [SerializeField] protected UiHoverObject[] _uiHoverObjects;

    [Header("Ne peut générer qu'une seule box à la fois")]
    [SerializeField] protected InfoBoxManager<BoxType, BoxContent> _boxManager;

    public InfoBoxManager<BoxType, BoxContent> BoxHandler { get => _boxManager; }

    private void OnValidate()
    {
        if(_boxManager != null)
            _boxManager.ValidatePrefab();
    }

    protected virtual void Start()
    {
        SubscribeToEvents(true);
    }

    public void OnDisable()
    {
        if (_boxManager.InfoBox)
            _boxManager.DestroyBox();
    }

    protected virtual void OnDestroy()
    {
        _boxManager.DestroyBox();
        SubscribeToEvents(false);
    }

    void SubscribeToEvents(bool sub)
    {
        if (sub)
        {
            for (int i = 0; i < _uiHoverObjects.Length; i++)
            {
                _uiHoverObjects[i].OnHoverStart += OnHoverStart;
                _uiHoverObjects[i].OnHoverEnd += OnHoverEnd;
            }
        }
        else
        {
            for (int i = 0; i < _uiHoverObjects.Length; i++)
            {
                _uiHoverObjects[i].OnHoverStart -= OnHoverEnd;
                _uiHoverObjects[i].OnHoverEnd -= OnHoverEnd;
            }
        }
    }

    protected virtual void OnHoverStart(UiHoverObject.OnHoverArgs obj)
    {
        MakeInfoBox();
    }

    protected virtual void OnHoverEnd(UiHoverObject.OnHoverArgs obj)
    {
        _boxManager.DestroyBox();
    }

    void MakeInfoBox()
    {
        _boxManager.RegenerateBox(MakeBoxContent());
    }

    /// <summary>
    /// This is where you build the content for your box. 
    /// </summary>
    protected abstract BoxContent MakeBoxContent();
}
