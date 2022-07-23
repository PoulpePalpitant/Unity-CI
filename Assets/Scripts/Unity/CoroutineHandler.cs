using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 14/04/2022

 * Brief    : Sert à déléguer la gestion d'une coroutine
 * 
 * WARNING: En utilisant cette classe, vous vous engagez à ne jamais
 * utiliser les méthodes suivantes dans votre script:
 * 
        public Coroutine StartCoroutine(string methodName);
        public Coroutine StartCoroutine(IEnumerator routine);
        public Coroutine StartCoroutine_Auto(IEnumerator routine);
        public void StopAllCoroutines();
        public void StopCoroutine(IEnumerator routine);
        public void StopCoroutine(Coroutine routine);
        public void StopCoroutine(string methodName);

 * **********************************************************************
*/

public class CoroutineHandler
{
    bool _isRunning = false;
    Coroutine _coroutine = null;
    Coroutine _coroutineWrapper = null;

    Func<IEnumerator> _coroutineMethod;
    MonoBehaviour _parentScript;
    public bool Started{ get => _coroutineWrapper != null; }
    public bool MustStop{ get => _coroutine == null && _coroutineWrapper == null; }
    public bool IsRunning { get => _isRunning; }
    public MonoBehaviour ParentScript { get => _parentScript; }
    public Func<IEnumerator> CoroutineMethod { get => _coroutineMethod; }

    public CoroutineHandler(MonoBehaviour parentScript, Func<IEnumerator> coroutineMethod)
    {
        _parentScript = parentScript;
        _coroutineMethod = coroutineMethod;
    }

    public void Start()
    {
        _isRunning = true;
        if (_coroutine != null)
        {
            Debug.LogError("Coroutine is already running");
        }

        _coroutineWrapper =_parentScript.StartCoroutine(CoroutineWrapper());
    }
    public void Stop()
    {
        if (_coroutine != null)
        {
            _parentScript.StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutineWrapper = null;
    }

    IEnumerator CoroutineWrapper()
    {
        // Nécessaire pour assigner le wrapper avant l'éxécution de la _coroutineMethod
        yield return null; 

        _coroutine = _parentScript.StartCoroutine(_coroutineMethod());
        
        while (!MustStop)
        {
            yield return null;
        }
        
        _isRunning = false;
        _coroutineWrapper = null;
    }
}
