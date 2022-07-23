using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 27/06/2022

 * Brief    : Use case for the UiHoverBoxMaker
 * 
 * 
 * 1 - How to use the example: 
 * 
 * To test this, drop the example prefab onto a canvas on your scene.
 * When you hover on the prefab, it should be able to spawn a box with a
 * simple text that displays the image color value.
 * 
 * 
 * 2 - How to use the class UIHoverBoxMaker<T, TT>: 
 * 
 * 
 * **********************************************************************
*/
namespace CodeExamples
{
    public class HoverInfoBoxExample : UIHoverBoxMaker<TextBox, TextBoxContent>
    {
        Image image;
        protected override void Start()
        {
            base.Start();
            image = GetComponent<Image>();
        }

        protected override TextBoxContent MakeBoxContent()
        {
            return new TextBoxContent(image.color.ToString());
        }
    }
}