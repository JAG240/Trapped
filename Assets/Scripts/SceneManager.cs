using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField] public Vector3 playerRespawnPoint { get; private set; }
    [field: SerializeField] public Vector3 playerReturnPoint { get; private set; }
    [field: SerializeField] public Vector3 playerReturnLookPoint { get; private set; }
    [field: SerializeField] public Vector3 killerRespawnPoint { get; private set; }

    public bool startIntro = true;
    public bool introDeath = true;
    public bool finalDeath = false;
    private Car car;
    private StreetBuilder streetBuilder;
    private GameObject player;
    private bool mainMenu = false;
    private bool paused = false;

    public Action<KillerStateManager> playerDeath;
    public Action resetLevel;

    public Action<Car> enterCar;
    public Action<Car> exitCar;

    public Action introKill;

    public Action readNote;
    public Action exitNote;

    public Action pauseGame;
    public Action resumeGame;

    public Action playerPrefsUpdated;

    public Action playerEneteredPorch;

    void Start()
    {
        car = GameObject.Find("Car").GetComponent<Car>();
        player = GameObject.Find("Player");
        streetBuilder = GameObject.Find("StreetBuilder").GetComponent<StreetBuilder>();

        if(startIntro)
        {
            mainMenu = true;
            player.GetComponent<PlayerAudio>().StartIntroMusic();
            car.EnterCar();
        }

        playerPrefsUpdated += UpdateLighting;
        UpdatePlayerPrefs();
    }

    public void StartGame()
    {
        mainMenu = false;
        StartCoroutine(streetBuilder.StopMovement());
        StartCoroutine(StallCar());
    }

    private IEnumerator StallCar()
    {
        car.Stall();
        yield return new WaitForSeconds(car.needleSpeed);
        car.ExitCar();
    }

    public void IntroAttack()
    {
        if(introDeath)
            introKill?.Invoke();
    }

    public void KillPlayer(KillerStateManager killer)
    {
        playerDeath?.Invoke(killer);
    }

    public void ResetLevel()
    {
        resetLevel?.Invoke();
    }

    public void EnterCar(Car car)
    {
        enterCar?.Invoke(car);
    }

    public void ExitCar(Car car)
    {
        exitCar?.Invoke(car);
    }

    public void ReadNote()
    {
        readNote?.Invoke();
    }

    public void CloseNote()
    {
        exitNote?.Invoke();
    }

    public void PauseGame()
    {
        if (mainMenu || paused)
            return;

        paused = true;
        pauseGame?.Invoke();
    }

    public void ResumeGame()
    {
        if (!paused)
            return;

        paused = false;
        resumeGame?.Invoke();
    }

    public void UpdatePlayerPrefs()
    {
        playerPrefsUpdated?.Invoke();
    }

    public void EnterPorch()
    {
        playerEneteredPorch?.Invoke();
    }

    public void ReloadGame()
    {
        Scene game = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

        UnityEngine.SceneManagement.SceneManager.LoadScene(game.name);
    }

    private void UpdateLighting()
    {
        RenderSettings.ambientIntensity = PlayerPrefs.GetFloat("brightness");
    }
}
