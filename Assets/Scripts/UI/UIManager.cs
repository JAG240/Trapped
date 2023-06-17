using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset crossHair;
    [SerializeField] private VisualTreeAsset mainMenu;
    [SerializeField] private VisualTreeAsset respawnMenu;
    [SerializeField] private VisualTreeAsset note;
    [SerializeField] private VisualTreeAsset pauseMenu;

    private SceneManager sceneManager;
    private UIDocument uiDoc;
    private Color crossHairDefaultColor = new Color(210f/255f, 210f/255f, 210f/255f, 100f/255f);
    private Color crossHairInteractColor = new Color(255f/255f, 63f/255f, 0f, 100f/255f);
    private bool canInteract = false;
    private Label crossHairText;

    private Button playGame;
    private Button settings;
    private Button quitGame;
    private Button respawn;

    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        uiDoc = GetComponent<UIDocument>();
        sceneManager.playerDeath += (context) => LoadRespawnMenu();
        sceneManager.pauseGame += LoadPauseMenu;
        sceneManager.resumeGame += Resume;
        sceneManager.exitNote += CloseNote;

        if (sceneManager.startIntro)
        {
            LoadMainMenu();
            return;
        }

        LoadCrossHair();
    }

    public void LoadCrossHair()
    {
        var root = uiDoc.rootVisualElement;
        crossHairText = root.Q<Label>("CrossHair");
    }

    public void SetCrossHairColor(bool canInteract)
    {
        if (canInteract == this.canInteract)
            return;

        crossHairText.style.color = canInteract ? crossHairInteractColor : crossHairDefaultColor;
        this.canInteract = canInteract;
    }

    public void LoadNote(string noteText, float textSize)
    {
        uiDoc.visualTreeAsset = note;
        sceneManager.ReadNote();

        var root = uiDoc.rootVisualElement;

        Button close = root.Q<Button>("Close");
        Label textField = root.Q<Label>("Text");

        textField.text = noteText;
        textField.style.fontSize = textSize;

        close.clicked += sceneManager.CloseNote;
    }

    private void CloseNote()
    {
        var root = uiDoc.rootVisualElement;
        Button close = root.Q<Button>("Close");
        close.clicked -= sceneManager.CloseNote;

        uiDoc.visualTreeAsset = crossHair;
        LoadCrossHair();
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

        uiDoc.visualTreeAsset = crossHair;
        LoadCrossHair();

        sceneManager.StartGame();
    }

    public void LoadPauseMenu()
    {
        uiDoc.visualTreeAsset = pauseMenu;

        var root = uiDoc.rootVisualElement;

        Button resume = root.Q<Button>("resume");

        resume.clicked += sceneManager.ResumeGame;
    }

    public void Resume()
    {
        var root = uiDoc.rootVisualElement;

        Button resume = root.Q<Button>("resume");

        resume.clicked -= sceneManager.ResumeGame;

        uiDoc.visualTreeAsset = crossHair;
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
        var root = uiDoc.rootVisualElement;

        respawn = root.Q<Button>("respawn");
        respawn.clicked -= Respawn;

        uiDoc.visualTreeAsset = crossHair;
        LoadCrossHair();
        sceneManager.ResetLevel();
    }
}
