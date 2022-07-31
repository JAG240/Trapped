using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTrigger : MonoBehaviour
{
    private SceneManager sceneManager;

    void Start()
    {
        sceneManager = transform.parent.GetComponent<SceneManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            sceneManager.IntroAttack();
    }
}
