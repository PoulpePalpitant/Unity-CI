using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Option button in the main menu
 * **********************************************************************
*/

public class MainMenuOptionsButtons : MonoBehaviour
{
    public void OnClick()
    {
        SceneLoader.Instance.ChangeScene(SceneNames.OPTIONS_MENU, SceneTransitionNames.FADE_TO_BLACK);
    }
}
