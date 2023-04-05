using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private Key key;
    [SerializeField] private AudioClip unlock;
    [SerializeField] private AudioClip failedUnlock;
    private string keyID;
    private AudioSource audioSource;

    private void Start()
    {
        keyID = key.keyID;
        audioSource = GetComponent<AudioSource>();
    }

    public bool Unlock(PlayerInventory playerInventory)
    {
        if(playerInventory.ConsumeKey(keyID))
        {
            StartCoroutine(SFXUnlock(unlock, true));
            return true;
        }

        StartCoroutine(SFXUnlock(failedUnlock, false));
        return false;
    }

    private IEnumerator SFXUnlock(AudioClip clip, bool unlock)
    {
        audioSource.clip = clip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        if(unlock)
            Destroy(gameObject);
    }
}
