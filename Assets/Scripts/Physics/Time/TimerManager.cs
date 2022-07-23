using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 26/02/2022

 * Brief    : Gère la création et l'éxécutions de timers non-lié à des GameObjects
 * 
 * (DERECATED): Il est recommandé d'utiliser la class Timer à la place.
 * 
 * **********************************************************************
*/

/// <summary>
/// WARNING  : Les objets créant les timers doivent s'assurer de les 
/// désactiver si ceux-ci ne doivent plus être utilisés. 
/// Ex: void OnDestroy() { myTimer.Deactivate()}
/// </summary>
public class TimerManager : MonoBehaviour
{
    static TimerManager _instance;
    static readonly int INITIAL_CAPACITY = 50;
    List<_timer> _timers = new List<_timer>(INITIAL_CAPACITY);

    static public TimerManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Instance doesn't exist");

            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    void Update()
    {
        var i = 0;
        var len = _timers.Count;
        while (i < len)
        {
            _timers[i]._Tick();
            ++i;
        }
    }

    public _timer Add(float duration, bool infinite = false, float interval = 1)
    {
        foreach (var timer in _timers)
        {
            if (timer.Deactivated)
            {
                return timer.Set(duration, infinite, interval);
            }
        }

        var t = new _timer();
        _timers.Add(t);
        return t.Set(duration, infinite, interval);

    }
    public _timer Add(System.Action action, float duration, bool infinite = false, float interval = 1)
    {
        foreach (var timer in _timers)
        {
            if (timer.Deactivated)
            {
                return timer.Set(action, duration, infinite, interval);
            }
        }

        var t = new _timer();
        _timers.Add(t);
        return t.Set(action, duration, infinite, interval);
    }
    public _timer AddUnscaled(System.Action action, float duration, bool infinite = false, float interval = 1)
    {
        foreach (var timer in _timers)
        {
            if (timer.Deactivated)
            {
                timer.MakeUnscaled();
                return timer.Set(action, duration, infinite, interval);
            }
        }

        var t = new _timer();
        _timers.Add(t);
        return t.SetUnscaled(action, duration, infinite, interval);
    }


    /// <summary>
    /// Un timer simple qui fait une action après un certains temps.
    /// WARNING: Utilisez la classe "Timer" si vous souhaitez un timer lié par un GameObject
    /// </summary>
    public class _timer
    {
        bool _running;
        bool _deactivated = true;  // Si le timer n'est plus utilisé
        float _interval;
        float _duration;
        float _timeLeft;
        bool _unscaled = false;  // Peut fonctionner même quand le jeu est en pause
        System.Action _action;

        bool _infinite;

        // GET/SETS
        public bool IsRunning { get => _running; set => _running = value; }
        public bool Deactivated { get => _deactivated; }

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

        public void Deactivate()
        {
            _running = false;
            _deactivated = true;
        }

        /// <summary>
        /// DO NOT USE THIS. Only TimerManager is allowed to tick timers
        /// </summary>
        /// <returns></returns>
        public void _Tick()
        {
            if (_deactivated || !_running)
                return;

            _timeLeft -= _interval * (_unscaled ? Time.unscaledDeltaTime : Time.deltaTime);

            if (_timeLeft <= 0)
            {
                if (_action != null)
                    _action();

                if (!_infinite)
                {
                    Deactivate();
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
                Deactivate();
            }

            // Un reset exact
            _timeLeft = _duration;
        }

        public bool Refresh()
        {
            if (_deactivated)
            {
                Debug.LogWarning("Cannot refresh this timer, it was not active");
                return false;
            }

            if (_duration <= 0)
            {
                Debug.LogWarning("Cannot refresh this timer, it had no duration");
                return false;
            }

            _timeLeft = _duration;
            return true;
        }

        public _timer Set(float duration, bool infinite = false, float interval = 1)
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
            _deactivated = false;

            return this;
        }
        public _timer Set(System.Action action, float duration, bool infinite = false, float interval = 1)
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
            _deactivated = false;

            return this;
        }
        public _timer SetUnscaled(System.Action action, float duration, bool infinite = false, float interval = 1)
        {
            _unscaled = true;
            return Set(action, duration, infinite, interval);
        }

        /// <summary>
        ///  Un timer unscaled peut fonctionner même quand le jeu est sur pause.
        /// </summary>
        public _timer MakeUnscaled()
        {
            _unscaled = true;
            return this;
        }
    }

}
