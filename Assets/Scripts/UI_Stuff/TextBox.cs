using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject text;

    void Start()
    {
        text.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        text.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        text.SetActive(false);
    }
}
