using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : ?/?/2022

 * Brief    : Se détruit automatiquement après X temps
 * **********************************************************************
*/

public class DestroyMe : MonoBehaviour
{
    [SerializeField] float _delay = 1.5f;

    void Awake()
    {
        Destroy(gameObject, _delay);
    }

}
