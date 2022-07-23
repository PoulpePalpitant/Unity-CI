using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/*
 * **********************************************************************
 * Author   : Laurent Montreuil
 * Date     : 05/05/2022

 * Brief    : Objet qui envoie des events lorsque hovered
 * **********************************************************************
*/
[RequireComponent(typeof(Collider2D))]
public class UiHoverObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Collider2D _collider;
    public event Action<OnHoverArgs> OnHoverStart;
    public event Action<OnHoverArgs> OnHoverEnd;
    public class OnHoverArgs : EventArgs
    {
        public UiHoverObject hovered;
    }

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void EnableHover(bool enable)
    {
        _collider.enabled = enable;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverStart?.Invoke(new OnHoverArgs { hovered = this }); ;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverEnd?.Invoke(new OnHoverArgs { hovered = this }); ;
    }
}
