using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 26/02/2022

 * Brief    : Un timer qui peut être attaché à un gameObject
 * **********************************************************************
*/

public class Timer : MonoBehaviour
{
    bool _running;
    float _interval;
    float _duration;
    float _timeLeft;
    bool _unscaled = false;  // Peut fonctionner même quand le jeu est en pause
    System.Action _action;

    bool _infinite;

    public bool IsRunning { get => _running; set => _running = value; }

    public float GetElapsedTime()
    {
        return _duration - _timeLeft;
    }

    public void Pause()
    {
        _running = false;
    }

    public void Resume()
    {
        _running = true;
    }


    public void Update()
    {
        if (!_running)
            return;

        _timeLeft -= _interval * (_unscaled ? Time.unscaledDeltaTime : Time.deltaTime);

        if (_timeLeft <= 0)
        {
            if (_action != null)
                _action();

            if (!_infinite)
            {
                _running = false;
                return;
            }

            // timeLeft peux très bien être plus petit que 0, il faut compenser en additionnant
            _timeLeft += _duration;
        }
    }

    /// <summary>
    /// Termine la loop actuelle, de ce fait trigger l'action immédiatement 
    /// </summary>
    public void EndCurrentLoop()
    {
        if (_action != null)
            _action();

        if (!_infinite)
        {
            _running = false;
            return;
        }

        // Un reset exact
        _timeLeft = _duration;
    }

    public bool Refresh()
    {
        if (_duration <= 0)
        {
            Debug.LogWarning("Cannot refresh this timer, it had no duration");
            return false;
        }

        _timeLeft = _duration;
        return true;
    }

    public Timer Set(float duration, bool infinite = false, float interval = 1)
    {
        if (duration <= 0)
        {
            Debug.LogWarning("Invalid Timer");
            return null;
        }

        _interval = interval;
        _infinite = infinite;
        _duration = duration;
        _timeLeft = duration;
        _running = true;

        return this;
    }
    public Timer Set(System.Action action, float duration, bool infinite = false, float interval = 1)
    {
        if (duration <= 0)
        {
            Debug.LogWarning("You made an invalid timer");
            return null;
        }

        _action = action;
        _interval = interval;
        _infinite = infinite;
        _duration = duration;
        _timeLeft = duration;
        _running = true;

        return this;
    }
    public Timer SetUnscaled(System.Action action, float duration, bool infinite = false, float interval = 1)
    {
        _unscaled = true;
        return Set(action, duration, infinite, interval);
    }

    /// <summary>
    ///  Un timer unscaled peut fonctionner même quand le jeu est sur pause.
    /// </summary>
    public Timer MakeUnscaled()
    {
        _unscaled = true;
        return this;
    }
}