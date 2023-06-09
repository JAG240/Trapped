using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject button;
    [SerializeField] private float pressSpeed = 1f;
    [SerializeField] private float pressDepth = -0.03f;
    public Action submit;
    private bool isPressing = false;

    void IInteractable.Interact(GameObject player)
    {
        if(!isPressing)
            StartCoroutine(pressButton(0f, pressDepth));
    }

    private IEnumerator pressButton(float start, float end)
    {
        isPressing = true;
        float t = 0f;

        while (t < pressSpeed)
        {
            float newY = Mathf.Lerp(start, end, t / pressSpeed);
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, newY, button.transform.localPosition.z);
            yield return null;
            t += Time.deltaTime;
        }

        button.transform.localPosition = new Vector3(button.transform.localPosition.x, end, button.transform.localPosition.z);

        isPressing = false;

        if(end != 0)
            submit?.Invoke();
    }

    public void ReleaseButton()
    {
        if (!isPressing)
            StartCoroutine(pressButton(pressDepth, 0f));
    }
}
