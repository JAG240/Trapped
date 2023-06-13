using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject button;
    [SerializeField] private float pressSpeed = 1f;
    [SerializeField] private float pressDepth = -0.03f;
    [SerializeField] private AudioClip success;
    [SerializeField] private AudioClip fail;
    public Action submit;
    private bool isPressing = false;
    private bool isPressed = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void IInteractable.Interact(GameObject player)
    {
        if(!isPressing && !isPressed)
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
        {
            submit?.Invoke();
            isPressed = true;
        }
        else
        {
            isPressed = false;
        }
    }

    public void PlayAudio(bool isSuccess)
    {
        if (isSuccess)
        {
            audioSource.clip = success;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = fail;
            audioSource.Play();
        }
    }

    public void ReleaseButton()
    {
        if (isPressing)
            return;

        StartCoroutine(pressButton(pressDepth, 0f));
    }
}
