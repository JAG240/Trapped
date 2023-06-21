using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorchTrigger : MonoBehaviour
{
    private SceneManager SceneManager;

    private void Start()
    {
        SceneManager = GetComponentInParent<SceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            SceneManager.EnterPorch();
    }
}
