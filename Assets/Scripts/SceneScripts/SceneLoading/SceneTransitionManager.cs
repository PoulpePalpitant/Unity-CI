using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine.Rendering;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil Baby Boom (2021)
 * Date     : 26/02/2022

 * Brief    : Gère les transitions entre les scènes.
 * **********************************************************************
*/

public enum SceneTransitionNames
{
    RANDOM = 0,
    FADE_TO_BLACK,
    WIPE_LEFT,
    WIPE_RIGHT,
    WIPE_DOWN,
    WIPE_UP,
    FANCYSTUF,

    NONE = -1
}

public class SceneTransitionManager : MonoBehaviour 
{
    static private SceneTransitionManager _instance;

    private Animator _currentTransition;
    private SceneTransitionNames _currentSceneTransitionName;
    private SceneLoadData _currentSceneData = null;

    [SerializeField] Animator _crossfadeTransition;
    [SerializeField] Animator _wipeLeftTransition;
    [SerializeField] Animator _wipeRightTransition;
    [SerializeField] Animator _wipeUpTransition;
    [SerializeField] Animator _wipeDownTransition;

    public event Action<SceneLoadData> OnHalfTransitionComplete;
    public event Action<SceneLoadData> OnFullTransitionComplete;

    /// <summary>
    /// Servent à enregistrer des lambda pour 1 un seul call. Vidé à chaque invoke.
    /// </summary>
    private List<Action<SceneLoadData>> _OnHalfTransitionSubscribersFor1Call = new List<Action<SceneLoadData>>();
    private List<Action<SceneLoadData>> _OnFullTransitionSubscribersFor1Call = new List<Action<SceneLoadData>>();

    static public SceneTransitionManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Instance doesn't exist");
            return _instance;
        }
    }
    public bool ScreenTransitionStarted { get => _currentTransition != null; }
    public SceneTransitionNames CurrentSceneTransition { get => _currentSceneTransitionName; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    /// <param name="name">Le nom de la transition</param>
    /// <param name="scenes">Les scènes qui sont relié par la transition.
    public void StartTransition(SceneTransitionNames transition, SceneLoadData scenes)
    {
        if (_currentTransition != null)
        {
            Debug.LogError("Une transition à déjà lieu; des choses imprévisibles vont survenir...");
            CallOnSecondTransitionEnded();
        }
        
        switch (transition)
        {
            case SceneTransitionNames.FADE_TO_BLACK:
                _currentTransition = _crossfadeTransition;
                break;
            case SceneTransitionNames.WIPE_LEFT:
                _currentTransition = _wipeLeftTransition;
                break;
            case SceneTransitionNames.WIPE_RIGHT:
                _currentTransition = _wipeRightTransition;
                break;
            case SceneTransitionNames.WIPE_UP:
                _currentTransition = _wipeUpTransition;
                break;
            case SceneTransitionNames.WIPE_DOWN:
                _currentTransition = _wipeDownTransition;
                break;
        
            default:
                Debug.LogError("Screen transition does not exist. Maybe a typo? Animator trigger name must be the same as screen transition name");
                break;
        }

        _currentTransition.gameObject.SetActive(true);

        _currentSceneData = scenes;
        _currentSceneTransitionName = transition;
        _currentTransition.SetTrigger(GetStartTrigger(transition));
    }

    public void EndCurrentTransition()
    {
        _currentTransition.SetTrigger(GetEndTrigger(_currentSceneTransitionName));
    }

    public void SubToOnHalfTransitionCompleteOnce(Action<SceneLoadData> a)
    {
        OnHalfTransitionComplete += a;
        _OnHalfTransitionSubscribersFor1Call.Add(a);
    }
    public void SubToOnFullTransitionCompleteOnce(Action<SceneLoadData> a)
    {
        OnFullTransitionComplete += a;
        _OnFullTransitionSubscribersFor1Call.Add(a);
    }

    public void CallOnFirstTransitionEnded()
    {
        var sceneData = _currentSceneData != null ? _currentSceneData :
            new SceneLoadData
            {
                sceneOrigin = SceneManager.GetActiveScene().name,
                sceneDestination = SceneManager.GetActiveScene().name
            };

        OnHalfTransitionComplete?.Invoke(_currentSceneData);

        for (int i = 0; i < _OnHalfTransitionSubscribersFor1Call.Count; i++)
            OnHalfTransitionComplete -= _OnHalfTransitionSubscribersFor1Call[i];
        _OnHalfTransitionSubscribersFor1Call.Clear();
    }

    public void CallOnSecondTransitionEnded()
    {
        SceneLoadData screenDataToSend = null;
        if (_currentSceneData == null)
        {
            screenDataToSend = new SceneLoadData
            {
                sceneOrigin = SceneManager.GetActiveScene().name,
                sceneDestination = SceneManager.GetActiveScene().name
            };
        }
        else
            screenDataToSend = _currentSceneData;

        OnFullTransitionComplete?.Invoke(screenDataToSend);

        for (int i = 0; i < _OnFullTransitionSubscribersFor1Call.Count; i++)
            OnFullTransitionComplete -= _OnFullTransitionSubscribersFor1Call[i];
        _OnFullTransitionSubscribersFor1Call.Clear();

        _currentTransition.gameObject.SetActive(false);
        _currentTransition = null;
        _currentSceneData = null;
        _currentSceneTransitionName = SceneTransitionNames.NONE;
    }

    private string GetStartTrigger(SceneTransitionNames transition)
    {
        switch (transition)
        {
            case SceneTransitionNames.FADE_TO_BLACK:
                return "StartCrossfade";
            case SceneTransitionNames.WIPE_LEFT:
                return "StartScreenWipeLeft";
            case SceneTransitionNames.WIPE_RIGHT:
                return "StartScreenWipeRight";
            case SceneTransitionNames.WIPE_DOWN:
                return "StartScreenWipeDown";
            case SceneTransitionNames.WIPE_UP:
                return "StartScreenWipeUp";

        }

        Debug.LogError("The transition start trigger was not added here");
        return "";
    }

    private string GetEndTrigger(SceneTransitionNames transition)
    {
        switch (transition)
        {
            case SceneTransitionNames.FADE_TO_BLACK:
                return "EndCrossfade";
            case SceneTransitionNames.WIPE_LEFT:
                return "EndScreenWipeLeft";
            case SceneTransitionNames.WIPE_RIGHT:
                return "EndScreenWipeRight";
            case SceneTransitionNames.WIPE_DOWN:
                return "EndScreenWipeDown";
            case SceneTransitionNames.WIPE_UP:
                return "EndScreenWipeUp";
        }

        Debug.LogError("The transition end trigger was not added here");
        return "";
    }

    public SceneTransitionNames GetRandomWipe()
    {
        Predicate<int> isSwipe = swipe =>
        {
            switch ((SceneTransitionNames)swipe)
            {
                case SceneTransitionNames.WIPE_LEFT:
                case SceneTransitionNames.WIPE_RIGHT:
                case SceneTransitionNames.WIPE_DOWN:
                case SceneTransitionNames.WIPE_UP:
                    return true;
            }
            return false;
        };

        SceneTransitionNames randomWipe;
        Array allTransitions = Enum.GetValues(typeof(SceneTransitionNames));

        do
        {
            randomWipe = (SceneTransitionNames)allTransitions.GetValue(Utils.Math.rng.Next(allTransitions.Length));
        } while (!isSwipe((int)randomWipe));

        return randomWipe;
    }

}
