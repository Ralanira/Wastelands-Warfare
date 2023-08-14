using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightUnderCursor : MonoBehaviour
{
    Interactables.Interactable interactable;

    void Awake() 
    {
        interactable = GetComponent<Interactables.Interactable>();
    }
    
    private void OnMouseEnter() 
    {
        if(Time.timeScale != 0f)
        {
            interactable.ShowCursorHighlight();
        }
    }

    private void OnMouseExit() 
    {
        interactable.HideCursorHighlight();
    }
}
