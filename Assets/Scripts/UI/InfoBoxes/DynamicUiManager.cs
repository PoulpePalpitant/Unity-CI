using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * **********************************************************************
 * Project  : Laurent Montreuil
 * Date     : 07/05/2022

 * Brief    : Gestion du ui dynamique
 * 
 * TO IMPROVE: Generate all boxes on an overlay canvas
 * **********************************************************************
*/

public class DynamicUiManager : MonoBehaviour
{
    static public readonly Vector2 CANVAS_SCALE_RATIO = new Vector2(1920, 1080);

    [SerializeField] Canvas _dynamicUiCanvas;
    Camera _mainCamera;

    static private DynamicUiManager _instance;
    public static DynamicUiManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Instance doesn't exist or is being destroyed");
            return _instance;
        }
    }

    public static Canvas DynamicCanvas { get => Instance._dynamicUiCanvas; }
    public Camera MainCamera { get => _mainCamera; }

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Debug.LogError("Dont instantiate this");
        }
    }

    private void Start()
    {
        FindMainCamera();
        SetCanvasToMainCamera();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }

    void SubscribeToEvents()
    {
        SceneLoader.Instance.OnSceneActivation += OnSceneActivation;
    }

    void UnsubscribeToEvents()
    {
        SceneLoader.Instance.OnSceneActivation -= OnSceneActivation;
    }


    private void OnSceneActivation(SceneActivatedArgs args)
    {
        FindMainCamera();
        SetCanvasToMainCamera();
    }

    /// <summary>
    /// Very slow and heavy stuff here, but hey
    /// </summary>
    public void FindMainCamera()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        var cams = GameObject.FindGameObjectsWithTag(Tags.MAIN_CAMERA);
        for (int i = 0; i < cams.Length; i++)
        {
            if (cams[i].scene.name == sceneName)
            {
                _mainCamera = cams[i].GetComponent<Camera>();
                return;
            }
        }

        Debug.LogError("Main camera not found");
    }


    void SetCanvasToMainCamera()
    {
        _dynamicUiCanvas.worldCamera = _mainCamera;
        _dynamicUiCanvas.sortingLayerName = SortingLayers.UI;
    }
}



