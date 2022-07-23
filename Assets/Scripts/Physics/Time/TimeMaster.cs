using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 16/05/2022

 * Brief    : Gère le timescale du jeu
 * **********************************************************************
*/

public class TimeMaster : MonoBehaviour
{    
    private Timer restoreTimeDelay;
    private IEnumerator currentSlowMo;
    private bool timeIsPaused;
    private bool slowMoActive;

    private void Awake()
    {
        restoreTimeDelay = gameObject.AddComponent<Timer>();
        restoreTimeDelay.MakeUnscaled();
    }

    public void Pause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        timeIsPaused = pause;
    }

    /// <summary>
    /// Start un effet slow motion.
    /// </summary>
    /// <param name="slowDownFactor">La force du ralentie, de 0 à 1</param>
    /// <param name="delay">Le délay avant le que le temps retourne à la normale </param>
    /// <param name="duration">La durée à laquelle le temps retourne à la normale</param>
    public void StartSlowMotion(float slowDownFactor, float delay, float duration)
    {
        if (delay == 0 && duration == 0)
        {
            Debug.LogWarning("Un slowmo est instantanéé est useless");
            return;
        }

        if (currentSlowMo != null)
            StopCoroutine(currentSlowMo); // Stop if active

        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;  // 0.02f c'est le default fixed update speed. Ceci permet de slow down la physique au même rythme
        currentSlowMo = RestoreSpeed(delay, duration);
        StartCoroutine(currentSlowMo);
    }

    /// <summary>
    /// Restaure la vitesse originale de manière progressive 
    /// </summary>
    /// <param name="delay">Le délai avant que le time scale commence à être restauré</param>
    /// <param name="duration">La durée de temps en secondes que ça va prendre avant que le timescale soit complètement restauré. </param>
    IEnumerator RestoreSpeed(float delay, float duration)
    {
        if (delay > 0)
        {
            restoreTimeDelay.Set(delay);

            while (restoreTimeDelay.IsRunning)
            {
                yield return null;
            }
        }

        if (duration == 0)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
        else
        {
            while (Time.timeScale != 1f)
            {
                if (timeIsPaused)
                {
                    yield return null;
                    continue;
                }

                // Restore graduellement le slowmotion, à chaque frame
                if (Time.timeScale + (1f / duration) * Time.unscaledDeltaTime < 0)
                    Debug.LogError("wat?");

                Time.timeScale += (1f / duration) * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                yield return null;
            }
        }

        currentSlowMo = null;
    }
}