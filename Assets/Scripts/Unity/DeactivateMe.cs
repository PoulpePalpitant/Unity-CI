using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : ?/?/2022

 * Brief    : Se désactive automatiquement après X temps
 * **********************************************************************
*/

public class DeactivateMe : MonoBehaviour
{
    [SerializeField] float _delay = 1.5f;

    void Awake()
    {
        gameObject.AddComponent<Timer>().Set(() => { 
            gameObject.SetActive(false); 
        }, _delay);
    }

}
