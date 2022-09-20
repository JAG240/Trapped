using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneManager : MonoBehaviour
{
    public bool startIntro = true;
    private Car car;
    [SerializeField] private GameObject player;
    private GameObject Killer;
    private StreetBuilder streetBuilder;

    void Start()
    {
        car = GameObject.Find("Car").GetComponent<Car>();
        player = GameObject.Find("Player");
        streetBuilder = GameObject.Find("StreetBuilder").GetComponent<StreetBuilder>();
        Killer = GameObject.Find("Killer");

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
        Killer.GetComponent<NavMeshAgent>().Warp(player.transform.position - (player.transform.forward * 5f));
        Killer.GetComponent<KillerStateManager>().lookAt(player.transform.position);
    }
}
