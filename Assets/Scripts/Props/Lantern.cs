using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : Task
{
    new private Light light;
    [SerializeField] private float minIntensity = 0.1f;
    [SerializeField] private float maxIntensity = 0.5f;
    [SerializeField] private float minFlickerTime = 0.1f;
    [SerializeField] private float maxFlickerTime = 0.5f;

    protected override void Start()
    {
        base.Start();

        light = GetComponentInChildren<Light>();
        StartCoroutine(Flicker());
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
                intensity = Random.Range(minIntensity, maxIntensity);
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
}
