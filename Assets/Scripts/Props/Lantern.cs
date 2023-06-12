using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : Task, IInteractable
{
    new private Light light;
    [SerializeField] private float minIntensity = 0.6f;
    [SerializeField] private float maxIntensity = 1f;
    [SerializeField] private float minFlickerTime = 0.1f;
    [SerializeField] private float maxFlickerTime = 0.5f;
    [field: SerializeField] public bool lit { get; private set; } = true;
    [SerializeField] private Material litMat;
    [SerializeField] private Material unlitMat;
    [SerializeField] private bool canInteract = true;
    private float currentMinIntensity;
    private float currentMaxIntensity;
    private MeshRenderer lightMat;
    private MeshCollider meshCollider;
    protected AudioSource audioSource;

    protected override void Start()
    {
        base.Start();

        light = GetComponentInChildren<Light>();
        StartCoroutine(Flicker());

        lightMat = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        audioSource = GetComponent<AudioSource>();

        meshCollider.enabled = canInteract;
        currentMaxIntensity = lit ? maxIntensity : 0;
        currentMinIntensity = lit ? minIntensity : 0;
        ChangeLanternMat();
    }

    override public Vector3 GetTaskPosition()
    {
        return transform.position + (-transform.right * taskPosOffset);
    }

    private IEnumerator Flicker()
    {
        bool inFlicker = false;
        float intensity = 0.3f;
        float flickerTime = 0.3f;
        float totalTime = 0f;
        float currentLight = 0f;

        while (true)
        {
            if(!inFlicker)
            {
                inFlicker = true;
                intensity = Random.Range(currentMinIntensity, currentMaxIntensity);
                flickerTime = Random.Range(minFlickerTime, maxFlickerTime);
                totalTime = 0f;
                currentLight = light.intensity;
            }

            while(totalTime < flickerTime && inFlicker)
            {
                float t = totalTime / flickerTime;
                light.intensity = Mathf.Lerp(currentLight, intensity, t);
                totalTime += Time.deltaTime;
                yield return null;
            }

            if (totalTime >= flickerTime)
                inFlicker = false;

            yield return null;
        }
    }

    public void Interact(GameObject player)
    {
        lit = !lit;

        currentMaxIntensity = lit ? maxIntensity : 0;
        currentMinIntensity = lit ? minIntensity : 0;
        ChangeLanternMat();
        audioSource.Play();
    }

    private void ChangeLanternMat()
    {
        Material[] mats = lightMat.materials;
        mats[3] = lit ? litMat : unlitMat;
        lightMat.materials = mats;
    }

    public void UpdateInteractable(bool state)
    {
        canInteract = state;
        meshCollider.enabled = canInteract;
    }
}
