using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : ?/?/2022

 * Brief    : Helps to vizualize the dynamic box positionning
 * 
 * NOTE     : A test prefab should be in the same folder with this script
 * 
 * In-depth :
 * Each frame, this script reposition's a box, and you can play with the 
 * values or move the gameObject around to see how the box repositions
 * itself. 
 * **********************************************************************
*/
namespace CodeExamples
{
    public class BoxRepositionExample : MonoBehaviour
    {
        [Header("Move this gameObject in the editor to test!")]

        [SerializeField] InfoBoxManager<TextBox, TextBoxContent> boxManager;

        private void Start()
        {
            boxManager.RegenerateBox(new TextBoxContent("Testitoti"));
        }
        private void Update()
        {
            if (boxManager.InfoBox != null)
                boxManager.TextBoxPositionHandler.RepositionUIElement((RectTransform)boxManager.InfoBox.transform);
        }
    }
}