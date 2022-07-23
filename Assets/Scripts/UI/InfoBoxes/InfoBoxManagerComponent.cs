using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Project  : ...
 * Author   : Laurent Montreuil
 * Date     : 09/05/2022

 * Brief    : Wrapper pour infobox Manager
 * 
 * Utile pour utiliser un infobox manager en temps que component
 * **********************************************************************
*/

public class InfoBoxManagerComponent<BoxType, BoxContent> : MonoBehaviour
    where BoxType : InfoBox
    where BoxContent : InfoBoxContent
{
    [SerializeField] protected InfoBoxManager<BoxType, BoxContent> _infoBoxManager;

    public BoxType InfoBox { get => _infoBoxManager.InfoBox; }
    public UIPositionHandler TextBoxPositionHandler { get => _infoBoxManager.TextBoxPositionHandler; }

    private void OnValidate()
    {
        if (_infoBoxManager != null)
            _infoBoxManager.ValidatePrefab();
    }

    private void OnDisable()
    {
        _infoBoxManager.DestroyBox();
    }

    private void OnDestroy()
    {
        _infoBoxManager.DestroyBox();
    }

    public void DestroyBox()
    {
        _infoBoxManager.DestroyBox();
    }

    public void PositionInfoBox(BoxType infoBox)
    {
       _infoBoxManager.TextBoxPositionHandler.RepositionUIElement((RectTransform)infoBox.transform);
    }

    public BoxType RegenerateBox(BoxContent boxContent)
    {
        return _infoBoxManager.RegenerateBox(boxContent);
    }
}
