using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneManager : MonoBehaviour
{
    [field: SerializeField] public Vector3 playerRespawnPoint { get; private set; }
    [field: SerializeField] public Vector3 killerRespawnPoint { get; private set; }

    public bool startIntro = true;
    private Car car;
    private StreetBuilder streetBuilder;
    private GameObject player;

    public Action<KillerStateManager> playerDeath;
    public Action resetLevel;

    public Action<Car> enterCar;
    public Action<Car> exitCar;

    public Action introKill;

    void Start()
    {
        car = GameObject.Find("Car").GetComponent<Car>();
        player = GameObject.Find("Player");
        streetBuilder = GameObject.Find("StreetBuilder").GetComponent<StreetBuilder>();

        if(startIntro)
        {
            player.GetComponent<PlayerAudio>().StartIntroMusic();
            car.EnterCar();
        }
    }

    public void StartGame()
    {
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
}
