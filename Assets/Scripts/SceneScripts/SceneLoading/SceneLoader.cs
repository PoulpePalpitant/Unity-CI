using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil (Baby Boom 2021)
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Fait les changements de sc�nes, avec transitions
 * **********************************************************************
*/

public class SceneLoadData : EventArgs
{
    public string sceneOrigin;
    public string sceneDestination;
}
public class SceneActivatedArgs : EventArgs
{
    public string sceneActivated;
}

public class SceneLoader : MonoBehaviour
{
    static private SceneLoader _instance;

    [SerializeField] SceneTransitionManager _screenTransitions;
    bool _firstTransitionEnded = false;
    bool _isChangingScene = false;

    /// <summary>
    /// Call� lorsque la transition de changement de sc�ne est compl�t�
    /// </summary>
    public event Action<SceneLoadData> OnSceneLoadEnd;
    /// <summary>
    /// Call� lorsque le changement de sc�ne est commenc�
    /// </summary>
    public event Action<SceneLoadData> OnSceneLoadStart;
    /// <summary>
    /// Call� lorsque une sc�ne est activ�. Il faut attendre cet event avant d'instancier
    /// des GameObject lors d'un changement de sc�ne
    /// </summary>
    public event Action<SceneActivatedArgs> OnSceneActivation;

    public static SceneLoader Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Instance doesn't exist or is being destroyed");
            return _instance;
        }
    }

    public bool IsChangingScene { get => _isChangingScene; }
    static public bool NoInstance { get => _instance == null; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    private void Start()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        OnSceneActivation?.Invoke(new SceneActivatedArgs { sceneActivated = SceneManager.GetActiveScene().name });
        OnSceneLoadEnd?.Invoke(new SceneLoadData { sceneOrigin = sceneName, sceneDestination = sceneName });

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
            _screenTransitions.OnFullTransitionComplete += OnFullTransitionComplete;
        }
        else
        {
            _screenTransitions.OnFullTransitionComplete -= OnFullTransitionComplete;
        }
    }

    private void OnFullTransitionComplete(SceneLoadData obj)
    {
        _isChangingScene = false;
        OnSceneLoadEnd?.Invoke(obj);
    }

    /// <summary>
    /// Change la sc�ne actuelle pour une autre. D�clanche une transition de changement de sc�ne
    /// </summary>
    /// <param name="scene">La sc�ne de destination</param>
    /// <param name="transitionName">La transition qui aura lieu durant le changement de sc�ne, pour masquer le changement</param>
    public void ChangeScene(string scene, SceneTransitionNames transition)
    {
        _isChangingScene = true;
        StartCoroutine(ChangeSceneAsync(scene, transition));
    }

    /// <summary>
    /// Load une sc�ne suppl�mentaire et change la sc�ne active pour celle-ci
    /// </summary>
    public void AddSceneOnTop(string scene, SceneTransitionNames transition)
    {
        _isChangingScene = true;
        StartCoroutine(LoadSceneOnTopAsync(scene, transition));
    }

    /// <summary>
    /// Unload une sc�ne suppl�mentaire et change la sc�ne active
    /// </summary>
    /// <param name="scene">La sc�ne de destination. Celle-ci doit d�j� �tre load� </param>
    public void RemoveSceneOnTop(string scene, SceneTransitionNames transition)
    {
        if (SceneManager.sceneCount < 2)
            Debug.LogError("There is no scene on top boyo!");

        if (!SceneManager.GetSceneByName(scene).IsValid())
            Debug.LogError("This scene is not currently loaded");

        _isChangingScene = true;
        StartCoroutine(UnloadSceneOnTopAsync(scene, transition));
    }

    /// <summary>
    /// Unload toutes les sc�nes et load une nouvelle
    /// </summary>
    public void ChangeAllScenes(string scene, SceneTransitionNames transition)
    {
        _isChangingScene = true;
        StartCoroutine(UnloadAllAndChangeSceneAsync(scene, transition));
    }

    /// <summary>
    /// #1 Start une screen transition 
    /// #2 Une fois termin�, load une nouvelle sc�ne par-dessus et ensuite Unload l'ancienne sc�ne
    /// #3 Termine la deuxi�me partie de la screen transition
    /// </summary>
    /// <param name="scene">Nouvelle sc�ne � loader</param>
    /// <param name="transitionName">Le nom de la transition � initialiser pour cacher le loading</param>
    /// <remarks> https://www.youtube.com/watch?v=CE9VOZivb3I&ab_channel=Brackeys </remarks>
    private IEnumerator ChangeSceneAsync(string scene, SceneTransitionNames transition)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        var screenData = new SceneLoadData
        {
            sceneOrigin = currentScene.name,
            sceneDestination = scene
        };

        InitSceneChange(transition, screenData);

        // wait the first part of the transition to finish before we load anything
        while (!_firstTransitionEnded)
        {
            yield return null;
        }

        // Adds new scene on top
        SceneManager.sceneLoaded += MakeActiveSceneImmediatelyAfterLoad;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Unload the previous Scene
        var asyncUnload = SceneManager.UnloadSceneAsync(currentScene);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        _screenTransitions.EndCurrentTransition();
    }
    private IEnumerator LoadSceneOnTopAsync(string scene, SceneTransitionNames transition)
    {
        var screenData = new SceneLoadData
        {
            sceneOrigin = SceneManager.GetActiveScene().name,
            sceneDestination = scene
        };

        // Start screen transition.
        InitSceneChange(transition, screenData);

        // wait the first part of the transition to finish before we load anything
        while (!_firstTransitionEnded)
        {
            yield return null;
        }

        // Adds new scene on top
        SceneManager.sceneLoaded += MakeActiveSceneImmediatelyAfterLoad;
        var asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        _screenTransitions.EndCurrentTransition();
    }
    private IEnumerator UnloadSceneOnTopAsync(string scene, SceneTransitionNames transition)
    {
        var screenData = new SceneLoadData
        {
            sceneOrigin = scene,
            sceneDestination = SceneManager.GetSceneAt(SceneManager.sceneCount - 2).name    // L'avant derni�re sc�ne sera load�
        };

        // Start screen transition.
        InitSceneChange(transition, screenData);

        // wait the first part of the transition to finish before we unload anything
        while (!_firstTransitionEnded)
        {
            yield return null;
        }

        var asyncUnload = SceneManager.UnloadSceneAsync(scene);

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        _screenTransitions.EndCurrentTransition();
    }
    private IEnumerator UnloadAllAndChangeSceneAsync(string sceneToLoad, SceneTransitionNames transition)
    {
        var screenData = new SceneLoadData
        {
            sceneOrigin = SceneManager.GetActiveScene().name,
            sceneDestination = sceneToLoad
        };

        // Start screen transition.
        InitSceneChange(transition, screenData);

        // wait the first part of the transition to finish before we unload anything
        while (!_firstTransitionEnded)
        {
            yield return null;
        }

        // Adds new scene on top
        SceneManager.sceneLoaded += MakeActiveSceneImmediatelyAfterLoad;
        var asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        // Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        int c = SceneManager.sceneCount;
        List<AsyncOperation> scenesToUnload = new List<AsyncOperation>();
        for (int i = 0; i < c; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            if (s.name != sceneToLoad)
            {
                scenesToUnload.Add(SceneManager.UnloadSceneAsync(s));
            }
        }

        // Attente que chacune des sc�nes soient unload�es
        for (int i = scenesToUnload.Count - 1; i >= 0; i--)
        {
            while (!scenesToUnload[i].isDone)
            {
                yield return null;
            }
        }

        _screenTransitions.EndCurrentTransition();
    }

    void InitSceneChange(SceneTransitionNames transition, SceneLoadData data)
    {
        _isChangingScene = true;
        _firstTransitionEnded = false;
        _screenTransitions.SubToOnHalfTransitionCompleteOnce((SceneData) =>
        {
            if (_firstTransitionEnded == true)
                Debug.LogError("huuu, you might have more than 1 active transitions");

            _firstTransitionEnded = true;
            _isChangingScene = false;
        });

        OnSceneLoadStart?.Invoke(data);
        _screenTransitions.StartTransition(transition, data);
    }
    void MakeActiveSceneImmediatelyAfterLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        OnSceneActivation?.Invoke(new SceneActivatedArgs { sceneActivated = scene.name });
        SceneManager.sceneLoaded -= MakeActiveSceneImmediatelyAfterLoad;
    }


}
