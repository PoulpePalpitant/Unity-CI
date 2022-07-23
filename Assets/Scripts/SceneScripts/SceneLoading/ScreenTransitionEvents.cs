using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 26/02/2022

 * Brief    : Call des events de transitions
 * 
 * Refacto  : Un peu stupide. Devrait être intégré à une classe qui fait
 * une transition à la place
 * **********************************************************************
*/


public class ScreenTransitionEvents : MonoBehaviour
{
    public void TransitionStartFinished()
    {
        SceneTransitionManager.Instance.CallOnFirstTransitionEnded();
    }

    public void TransitionEndFinished()
    {
        SceneTransitionManager.Instance.CallOnSecondTransitionEnded();
    }
}
