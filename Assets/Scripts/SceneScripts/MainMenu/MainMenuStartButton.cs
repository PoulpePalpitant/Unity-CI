using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Start button on main menu
 * **********************************************************************
*/
public class MainMenuStartButton : MonoBehaviour
{
    public void OnClick()
    {
        Game.Instance.StartNewGame();
    }
}
