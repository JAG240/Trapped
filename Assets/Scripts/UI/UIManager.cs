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
    [SerializeField] private VisualTreeAsset settingsMenu;

    private SceneManager sceneManager;
    private UIDocument uiDoc;
    private Color crossHairDefaultColor = new Color(210f/255f, 210f/255f, 210f/255f, 100f/255f);
    private Color crossHairInteractColor = new Color(255f/255f, 63f/255f, 0f, 100f/255f);
    private bool canInteract = false;
    private Label crossHairText;
    private bool wasPaused = false;

    private Button playGame;
    private Button settings;
    private Button quitGame;
    private Button respawn;

    void Start()
    {
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();
        uiDoc = GetComponent<UIDocument>();
        sceneManager.playerDeath += (context) => LoadDeath();
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
        settings.clicked += LoadSettings;
        quitGame.clicked += Application.Quit;
    }

    private void StartGame()
    {
        UnloadMainMenu();

        uiDoc.visualTreeAsset = crossHair;
        LoadCrossHair();

        sceneManager.StartGame();
    }

    private void UnloadMainMenu()
    {
        var root = uiDoc.rootVisualElement;

        playGame = root.Q<Button>("start");
        settings = root.Q<Button>("settings");
        quitGame = root.Q<Button>("quit");

        playGame.clicked -= StartGame;
        settings.clicked -= LoadSettings;
        quitGame.clicked -= Application.Quit;
    }

    private void LoadSettings()
    {
        if (uiDoc.visualTreeAsset == mainMenu)
        {
            UnloadMainMenu();
            wasPaused = false;
        }
        else
        {
            UnloadPauseMenu();
            wasPaused = true;
        }

        uiDoc.visualTreeAsset = settingsMenu;

        var root = uiDoc.rootVisualElement;

        Slider camSens = root.Q<Slider>("camsens");
        Button save = root.Q<Button>("save");

        camSens.value = PlayerPrefs.GetFloat("cam_sens");

        camSens.RegisterValueChangedCallback(UpdateCameraSens);
        save.clicked += UnloadSettings;
    }

    public void UpdateCameraSens(ChangeEvent<float> newSens)
    {
        PlayerPrefs.SetFloat("cam_sens", newSens.newValue);
        sceneManager.UpdatePlayerPrefs();
    }

    private void UnloadSettings()
    {
        var root = uiDoc.rootVisualElement;

        Slider camSens = root.Q<Slider>("camsens");
        Button save = root.Q<Button>("save");

        camSens.UnregisterValueChangedCallback(UpdateCameraSens);
        save.clicked += UnloadSettings;

        if (wasPaused)
            LoadPauseMenu();
        else
            LoadMainMenu();
    }

    public void LoadPauseMenu()
    {
        uiDoc.visualTreeAsset = pauseMenu;

        var root = uiDoc.rootVisualElement;

        Button resume = root.Q<Button>("resume");
        Button settings = root.Q<Button>("settings");

        resume.clicked += sceneManager.ResumeGame;
        settings.clicked += LoadSettings;
    }

    private void UnloadPauseMenu()
    {
        var root = uiDoc.rootVisualElement;

        Button resume = root.Q<Button>("resume");
        Button settings = root.Q<Button>("settings");

        resume.clicked -= sceneManager.ResumeGame;
        settings.clicked -= LoadSettings;
    }

    public void Resume()
    {
        UnloadPauseMenu();
        uiDoc.visualTreeAsset = crossHair;
    }

    public void LoadRespawnMenu()
    {
        uiDoc.visualTreeAsset = respawnMenu;

        var root = uiDoc.rootVisualElement;

        respawn = root.Q<Button>("respawn");

        respawn.clicked += Respawn;
    }

    private void LoadDeath()
    {
        StartCoroutine(WaitForEyes());
    }

    private IEnumerator WaitForEyes()
    {
        yield return new WaitForSeconds(1.5f);
        LoadRespawnMenu();
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
