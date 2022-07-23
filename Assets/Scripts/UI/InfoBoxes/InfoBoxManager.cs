using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;


/*
 * **********************************************************************
 * Project  : Basic Unity Framework
 * Author   : Laurent Montreuil
 * Date     : 07/05/2022

 * Brief    : Handles an infobox creation, positionning and deletion
 * 
 * In-depth : 
 * 
 * The corresponding BoxType and BoxTypeContent must be compatible
 *  - for example: SupraCoolBox and SupraCoolBoxContent -> Good
 *                 CoolBox and SupraCoolBoxContent      -> bad
 
 * So a script could have an instance of this class like:
 *      public InfoBoxManager<TextBox, TextBoxContent> myBoxManager;
 *      
 * **********************************************************************
*/


/// <summary>
/// To make use of this class, make sure that: <br/>
/// #1 The BoxType and BoxContent are compatible <br/>
/// #2 The InfoBoxPrefab has a component of type BoxType<br/>
/// For an example usage, see :
/// <see cref="CodeExamples.BoxRepositionExample"/><br/><br/>
/// </summary>
/// <typeparam name="BoxType">The type of box that will be instantiated by the InfoBoxManager</typeparam>
/// <typeparam name="BoxContent">and it's content</typeparam>

[Serializable]
public class InfoBoxManager<BoxType,BoxContent> 
    where BoxType : InfoBox
    where BoxContent : InfoBoxContent
{   
    /// <summary>
    /// This prefab must have the <typeparamref name="BoxType"/> script attached to it<br/>
    /// </summary>
    [SerializeField, Header("This prefab must have the correct InfoBox component")]
    GameObject _infoBoxPrefab;
    
    [SerializeField, Header("Handles the box position")]
    UIPositionHandler _textBoxPositionHandler;

    BoxType _infoBox;

    /// <summary>
    /// Validates wether the box prefab contains the appropriate script
    /// </summary>
    public void ValidatePrefab()
    {
        if (!_infoBoxPrefab)
        {
            Debug.LogError("The info box prefab requires a " + typeof(BoxType) + "Component");
            return;
        }

        var component = _infoBoxPrefab.GetComponent<BoxType>();
        if (!_infoBoxPrefab || component == null || _infoBoxPrefab.GetComponent<BoxType>().GetType() != typeof(BoxType))
        {
            Debug.LogError("The info box prefab:" + _infoBoxPrefab.name + typeof(BoxType) + " Component");
            EditorUtility.DisplayDialog("Error", "The info box prefab: " + _infoBoxPrefab.name + " requires a " + typeof(BoxType) + " Component", "Ok boyo");
        }
    }


    public BoxType InfoBox { get => _infoBox; }
    public UIPositionHandler TextBoxPositionHandler { get => _textBoxPositionHandler; }

    public void DestroyBox()
    {
        if (_infoBox == null)
        {
            return;
        }

        GameObject.Destroy(_infoBox.gameObject);
    }

    public void PositionInfoBox(BoxType infoBox)
    {
        _textBoxPositionHandler.RepositionUIElement((RectTransform)infoBox.transform);
    }

    /// <summary>
    /// Instantiate a box (if it isn't already) then set its content and position it 
    /// </summary>
    public BoxType RegenerateBox(BoxContent boxContent)
    {
        if (!_infoBox)
        {
            _infoBox = GenerateBox();
        }
        
        _infoBox.SetContent(boxContent);
        PositionInfoBox(_infoBox);
        return _infoBox;
    }

    /// <summary>
    /// Builds a box and adds it to the dynamic UI
    /// </summary>
    private BoxType GenerateBox()
    {
        var box = GameObject.Instantiate(_infoBoxPrefab);
        var infoBox = box.GetComponent<BoxType>();
        if (infoBox == null) { Debug.LogError("This prefab doesn't have your script"); }

        // Add the box to the global dynamic UI
        infoBox.transform.SetParent(DynamicUiManager.DynamicCanvas.transform, false);

        return infoBox;
    }
}




