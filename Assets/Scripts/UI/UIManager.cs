using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset mainMenu;
    private SceneManager sceneManager;
    private UIDocument uiDoc;

    private Button playGame;
    private Button settings;
    private Button quitGame;

    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        uiDoc = GetComponent<UIDocument>();

        if (sceneManager.startIntro)
            LoadMainMenu();

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
        uiDoc.visualTreeAsset = null;
        sceneManager.StartGame();
    }

}
