using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 07/5/2022

 * Brief    : Text box avec un titre
 * **********************************************************************
*/
public class TitleBox : TextBox
{
    [SerializeField] TextMeshProUGUI _title;
    public override void SetContent(InfoBoxContent content)
    {
        base.SetContent(content);
        _title.text = ((TitleBoxContent)content).title;
    }
}

public class TitleBoxContent: TextBoxContent
{
    public string title;

    public TitleBoxContent() { }
    public TitleBoxContent(string descriptionText, string title): base(descriptionText)
    {
        this.title = title;
    }
}