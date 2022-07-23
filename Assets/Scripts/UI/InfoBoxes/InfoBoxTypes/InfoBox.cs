using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 07/05/2022

 * Brief    : Sert à afficher des info
 * **********************************************************************
*/

public abstract class InfoBox : MonoBehaviour
{
    [SerializeField] Image box;

    public abstract void SetContent(InfoBoxContent content);
}

public class InfoBoxContent { }
