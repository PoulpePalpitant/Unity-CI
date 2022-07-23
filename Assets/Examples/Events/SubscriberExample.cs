using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * **********************************************************************
 * Project  : Flask
 * Author   : Laurent Montreuil
 * DevTeam  : Antoine-Olivier Monaco, Gabriel Kirouac, Laurent Montreuil
 * Date     : 27/02/2022

 * Brief    : Example d'application de subscriber en c#
 * **********************************************************************
*/

namespace CodeExamples
{
    public class SubscriberExample : MonoBehaviour
    {
        PublisherExample eventPublisher;
        void Start()
        {
            eventPublisher = gameObject.AddComponent<PublisherExample>();

            // Subscribe 
            eventPublisher.OnEvent += OnEventAction;
            PublisherExample.OnStaticEvent += OnEventAction;
            eventPublisher.OnEventNoArgs += OnEventAction;
            eventPublisher.OnEventDelegateAction += DelegateEventMethod;

            // Unsubscribe 
            eventPublisher.OnEvent -= OnEventAction;
            PublisherExample.OnStaticEvent -= OnEventAction;
            eventPublisher.OnEventNoArgs -= OnEventAction;
            eventPublisher.OnEventDelegateAction -= DelegateEventMethod;

        }


        // EVENTS
        void OnEventAction(object o, PublisherExample.OnEventArgs args)
        {
            Debug.Log(args.param2 + ": " + args.param1);
        }
        void OnEventAction(object o, System.EventArgs empty)
        {
            Debug.Log("No args necessary");
        }

        public bool DelegateEventMethod(float param1, string param2)
        {
            Debug.Log(param2 + ": " + param1);
            return true;
        }


    }
}