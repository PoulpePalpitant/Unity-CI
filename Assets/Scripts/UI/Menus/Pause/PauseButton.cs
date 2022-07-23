using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 16/05/2022

 * **********************************************************************
*/
[RequireComponent(typeof(Button))]
public class PauseButton : MonoBehaviour
{
    Button _button;
    protected void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        SubscribeToEvents(true);
    }

    private void OnDestroy()
    {
        SubscribeToEvents(false);
    }

    void SubscribeToEvents(bool sub)
    {
        if (sub)
        {
            _button.onClick.AddListener(PauseGame);
        }
        else
        {
            _button.onClick.AddListener(PauseGame);
        }
    }
    
    /// <summary>
    /// Le menu de pause devrait s'ouvrir automatiquement avec ça
    /// </summary>
    private void PauseGame()
    {
        Game.Instance.PauseGame(true);
    }
}
