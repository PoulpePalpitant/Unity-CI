using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Actions du bouton Credits dans le main menu
 * **********************************************************************
*/

public class MainMenuCreditsButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneLoader.Instance.ChangeScene(SceneNames.CREDITS, SceneTransitionNames.FADE_TO_BLACK);
    }
}
