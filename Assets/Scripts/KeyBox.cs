using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBox : MonoBehaviour
{
    [SerializeField] private GameObject lid;
    [SerializeField] private GameObject key;
    [SerializeField] private float openSpeed = 3f;
    [SerializeField] private float openAngle = 200f;
    private bool opening = false;

    private void Start()
    {
        key.layer = 0;
    }

    public void Unlock()
    {
        if (!opening)
            StartCoroutine(OpenLid());
    }

    private IEnumerator OpenLid()
    {
        opening = true;
        float t = 0f;

        Quaternion end = Quaternion.Euler(openAngle, 0f, 0f);
        Quaternion start = new Quaternion(lid.transform.localRotation.x, lid.transform.localRotation.y, lid.transform.localRotation.z, lid.transform.localRotation.w);

        while (t < openSpeed)
        {
            lid.transform.localRotation = Quaternion.Slerp(start, end, t / openSpeed);
            yield return null;
            t += Time.deltaTime;
        }

        key.layer = LayerMask.NameToLayer("Interactable");
    }
}
