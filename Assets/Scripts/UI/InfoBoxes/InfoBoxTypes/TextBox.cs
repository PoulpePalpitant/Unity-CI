using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 07/05/2022

 * Brief    : Une boîte de texte
 * **********************************************************************
*/
public class TextBox : InfoBox
{
    [SerializeField] TextMeshProUGUI _text;
    public override void SetContent(InfoBoxContent content)
    {
        SetText(((TextBoxContent)content).text);
    }
    public void SetText(string text)
    {
        _text.text = text;
    }
}

public class TextBoxContent: InfoBoxContent
{
    public string text;
    public TextBoxContent() { }
    public TextBoxContent(string text)
    {
        this.text = text;
    }
}

