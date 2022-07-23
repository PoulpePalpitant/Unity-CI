using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Handles option menu scene
 * **********************************************************************
*/

public class OptionsMenuSceneController : MonoBehaviour
{
    public void GoBackToMainMenu()
    {
        SceneLoader.Instance.ChangeScene(SceneNames.MAIN_MENU, SceneTransitionNames.FADE_TO_BLACK);
    }
}
