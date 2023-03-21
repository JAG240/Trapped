using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset mainMenu;
    [SerializeField] private VisualTreeAsset respawnMenu;
    private SceneManager sceneManager;
    private UIDocument uiDoc;

    private Button playGame;
    private Button settings;
    private Button quitGame;
    private Button respawn;

    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        uiDoc = GetComponent<UIDocument>();

        if (sceneManager.startIntro)
            LoadMainMenu();

        sceneManager.playerDeath += (context) => LoadRespawnMenu();
    }

    private void LoadMainMenu()
    {
        uiDoc.visualTreeAsset = mainMenu;

        var root = uiDoc.rootVisualElement;

        playGame = root.Q<Button>("start");
        settings = root.Q<Button>("settings");
        quitGame = root.Q<Button>("quit");

        playGame.clicked += StartGame;
        quitGame.clicked += Application.Quit;
    }

    private void StartGame()
    {
        var root = uiDoc.rootVisualElement;

        playGame = root.Q<Button>("start");
        settings = root.Q<Button>("settings");
        quitGame = root.Q<Button>("quit");

        playGame.clicked -= StartGame;
        quitGame.clicked -= Application.Quit;

        uiDoc.visualTreeAsset = null;

        sceneManager.StartGame();
    }


    public void LoadRespawnMenu()
    {
        uiDoc.visualTreeAsset = respawnMenu;

        var root = uiDoc.rootVisualElement;

        respawn = root.Q<Button>("respawn");

        respawn.clicked += Respawn;
    }

    private void Respawn()
    {
        uiDoc.visualTreeAsset = null;
        sceneManager.ResetLevel();
    }
}
