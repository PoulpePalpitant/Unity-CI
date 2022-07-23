using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/*
 * **********************************************************************
 * Project  : ...
 * Author   : Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Gère le jeu. 
 * **********************************************************************
*/

public class Game : MonoBehaviour
{
    static Game _instance;

    private SaveData _playerProfile; // Ne doit jamais être exposé

    TimeMaster _timeMaster;
    
    bool _gameInProgress = false;
    bool _paused = false;

    public event Action<OnPauseArgs> OnPause;

    public class OnPauseArgs: EventArgs
    {
        public bool gameIsPaused;
    }

    static public Game Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Instance doesn't exist");

            return _instance;
        }
    }

    public bool GameInProgress { get => _gameInProgress; }
    public SaveData PlayerProfile { get => _playerProfile;}
    private void Awake()
    {
        if (_instance != null)
            Debug.LogError("Instance already exist");

        _instance = this;
        DontDestroyOnLoad(gameObject);

        _timeMaster = GetComponent<TimeMaster>();
        Assert.IsNotNull(_timeMaster);

        LoadGameDataIfExists();
    }

    static public bool IsInitialized()
    {
        return _instance != null;
    }

    /// <summary>
    /// Commence une nouvelle partie
    /// </summary>
    public void StartNewGame()
    {
        _gameInProgress = true;
        SceneLoader.Instance.ChangeScene(SceneNames.SCENE_1, SceneTransitionNames.FADE_TO_BLACK);
        
        /*
            The things you do when you start an adventure
        */
    }

    /// <summary>
    /// Pause ou resume le jeu
    /// </summary>
    public void TogglePauseGame()
    {
        if (!_gameInProgress)
            return;

        _paused = !_paused;

        _timeMaster.Pause(_paused);
        OnPause?.Invoke(new OnPauseArgs { gameIsPaused = _paused });
    }
    public void PauseGame(bool pause)
    {
        _paused = pause;
        _timeMaster.Pause(pause);
        OnPause?.Invoke(new OnPauseArgs { gameIsPaused = pause });
    }

    public void EndGameAndReturnToMainMenu()
    {
        _gameInProgress = false;

        PauseGame(false);
        SceneLoader.Instance.ChangeScene(SceneNames.MAIN_MENU, SceneTransitionNames.FADE_TO_BLACK);
    }

    public void LoadGameDataIfExists()
    {
        SaveManager.LoadGame();
    }
}
