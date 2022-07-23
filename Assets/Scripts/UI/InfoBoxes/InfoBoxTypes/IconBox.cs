using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
* **********************************************************************
* Author   : Laurent Montreuil
* Date     : 07/5/2022

* Brief    : Text box avec un titre, et une icône
* **********************************************************************
*/

public class IconBox : TitleBox
{
    [SerializeField] Image _icon;
    public override void SetContent(InfoBoxContent content)
    {
        base.SetContent(content);
        _icon.sprite = ((IconBoxContent)content).icon;
    }
}

public class IconBoxContent : TitleBoxContent
{
    public Sprite icon;

    public IconBoxContent(){}
    public IconBoxContent(string descriptionText, string title, Sprite icon) : base(title, descriptionText)
    {
        this.icon = icon;
    }
}