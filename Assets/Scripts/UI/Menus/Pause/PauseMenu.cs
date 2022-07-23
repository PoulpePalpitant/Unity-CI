using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 16/05/2022

 * Brief    : Gestion du menu de pause
 * **********************************************************************
*/

public class PauseMenu : AwakeValidator
{
    [SerializeField] Button _quitButton;
    [SerializeField] Button _resumeButton;
    [SerializeField] Canvas _canvas;

    protected override void Awake()
    {
        base.Awake();
        _canvas.enabled = true;
    }

    private void Start()
    {
        gameObject.SetActive(false);
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
            _resumeButton.onClick.AddListener(PauseButton);
            _quitButton.onClick.AddListener(Game.Instance.EndGameAndReturnToMainMenu);
            Game.Instance.OnPause += OnPause;
        }
        else
        {
            _resumeButton.onClick.RemoveAllListeners();
            _quitButton.onClick.RemoveAllListeners();
            Game.Instance.OnPause -= OnPause;
        }
    }

    private void OnPause(Game.OnPauseArgs args)
    {
        EnablePauseMenu(args.gameIsPaused);
    }
    public void EnablePauseMenu(bool enable)
    {
        gameObject.SetActive(enable);
    }
    public void PauseButton()
    {
        Game.Instance.PauseGame(false);
    }


    protected override void Validate()
    {
        Assert.IsNotNull(_quitButton);
        Assert.IsNotNull(_resumeButton);
        Assert.IsNotNull(_canvas);
    }
}
