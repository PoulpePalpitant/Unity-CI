using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Handles the credit scene
 * **********************************************************************
*/

public class CreditsSceneController : MonoBehaviour
{
    public void GoBackToMainMenu()
    {
        SceneLoader.Instance.ChangeScene(SceneNames.MAIN_MENU, SceneTransitionNames.FADE_TO_BLACK);
    }
}
