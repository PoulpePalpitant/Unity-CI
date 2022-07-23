using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Un exemple d'application des event en c#
 * **********************************************************************
*/

namespace CodeExamples
{
    public class PublisherExample : MonoBehaviour
    {
        // Event version classic avec classe Event handler
        public event System.EventHandler<OnEventArgs> OnEvent;
        public static event System.EventHandler<OnEventArgs> OnStaticEvent;
        public event System.EventHandler OnEventNoArgs;
        public class OnEventArgs : System.EventArgs   
        {
            public float param1;
            public string param2;
        }

        // LESSON:
        // Il est interdit d'"Invoke" un event créé dans une autre classe.
        // L'idée est de garder le contrôle de l'event à la classe qui own l'event pour l'encapsulation.
        // Si tu veux générer un event en dehors d'une classe, tu dois créer un delegate à la place

        // Event avec delegates, plus de liberté, mais moins de sécurité
        public delegate bool OnEventDelegate(float param1, string param2);
        public event OnEventDelegate OnEventDelegateAction;

        void Start()
        {
            if (OnEvent != null)
                OnEvent(this, new OnEventArgs { param1 = 5.5f, param2 = "Value " });

            // LESSON: L'opérateur "?" remplace le  "if (x != null)"
            OnEvent?.Invoke(this, new OnEventArgs { param1 = 5.5f, param2 = "Value " });
            OnStaticEvent?.Invoke(this, new OnEventArgs { param1 = 5.5f, param2 = "Value " });
            OnEventNoArgs?.Invoke(this, System.EventArgs.Empty);

            var swag = OnEventDelegateAction?.Invoke(5.5f, "Value "); // Les delegates peuvent retourner une valeur
        }


        /// <summary>
        /// Coroutine qui attend un unity event
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/33199868/waiting-for-event-inside-unity-coroutine</remarks>
        private IEnumerator WaitUntilEvent(UnityEvent unityEvent)
        {
            var trigger = false;
            Action action = () => trigger = true;
            unityEvent.AddListener(action.Invoke);
            yield return new WaitUntil(() => trigger);
            unityEvent.RemoveListener(action.Invoke);
        }
    }
}