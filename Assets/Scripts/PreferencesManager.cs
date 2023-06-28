using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesManager : MonoBehaviour
{
    void Start()
    {
        if(!PlayerPrefs.HasKey("cam_sens"))
            PlayerPrefs.SetFloat("cam_sens", 1f);

        if (!PlayerPrefs.HasKey("main_volume"))
            PlayerPrefs.SetFloat("main_volume", 1f);

        if (!PlayerPrefs.HasKey("brightness"))
            PlayerPrefs.SetFloat("brightness", 0.01f);

        if (!PlayerPrefs.HasKey("fullscreen"))
            PlayerPrefs.SetInt("fullscreen", 1);

        PlayerPrefs.Save();
    }
}
