using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Toggle fullscreen
 * **********************************************************************
*/


public class ToggleFullscreenButton : MonoBehaviour
{
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
