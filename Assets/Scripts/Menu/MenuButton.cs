using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image hoverTriangle;

    private void OnEnable()
    {
        hoverTriangle.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverTriangle.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverTriangle.enabled = false;
    }
}
