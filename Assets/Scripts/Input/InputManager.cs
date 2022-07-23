using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 01/05/2022

 * Brief    : Gestion d'inputs
 * **********************************************************************
*/

public class InputManager : MonoBehaviour
{
    static InputManager _instance;
    InputSystemUIInputModule _inputSystemUIInputModule;
    PlayerInput _playerInput;

    public static InputManager Instance { get => _instance;}

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Don't instantiate this");
        }
        
        _instance = this;
        _inputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
        _playerInput = GetComponent<PlayerInput>();
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
            SceneLoader.Instance.OnSceneLoadStart += OnSceneLoadStart;
            SceneLoader.Instance.OnSceneLoadEnd += OnSceneLoadEnd;
        }
        else
        {
            SceneLoader.Instance.OnSceneLoadStart -= OnSceneLoadStart;
            SceneLoader.Instance.OnSceneLoadEnd -= OnSceneLoadEnd;
        }
    }

    private void OnSceneLoadEnd(SceneLoadData obj)
    {
        EnableInputs();
    }

    private void OnSceneLoadStart(SceneLoadData obj)
    {
        DisableInputs();
    }

    void DisableInputs()
    {
        _playerInput.SwitchCurrentActionMap(PlayerActionMapNames.LITERALLY_NOTHING);
    }

    void EnableInputs()
    {
        _playerInput.SwitchCurrentActionMap(PlayerActionMapNames.DEFAULT);
    }
}
