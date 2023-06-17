using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    [SerializeField][TextArea] private string text;
    [SerializeField] private float textSize;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void IInteractable.Interact(GameObject player)
    {
        uiManager.LoadNote(text, textSize);
    }

}
