using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 16/05/2022

 * Brief    : Gère les actions d'inputs du joueur
 * **********************************************************************
*/

public class PlayerInputActions : AwakeValidator
{
    PlayerInput _playerInput;

    InputActionMap _UIActionsMap;

    protected override void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _UIActionsMap = _playerInput.actions.FindActionMap(PlayerActionMapNames.DEFAULT);
        base.Awake();
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
            _UIActionsMap.FindAction(PlayerActionMaps.UI.PAUSE).performed += PauseAction;  
        }
        else
        {
            _UIActionsMap.FindAction(PlayerActionMaps.UI.PAUSE).performed -= PauseAction;
        }
    }
    protected override void Validate()
    {
        Assert.IsNotNull(_playerInput);
        Assert.IsNotNull(_UIActionsMap);
    }

    /// PUT ALL ACTIONS HERE 
    /// ********************

    public void PauseAction(InputAction.CallbackContext context)
    {
        if (context.performed )
        {
            Game.Instance.TogglePauseGame();
        }
    }

    /// ********************


}
