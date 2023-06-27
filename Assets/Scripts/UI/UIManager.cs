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
    [SerializeField] private VisualTreeAsset credits;

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

        SliderInt camSens = root.Q<SliderInt>("camSens");
        SliderInt masterAudio = root.Q<SliderInt>("masterAudio");
        SliderInt brightness = root.Q<SliderInt>("brightness");

        Button save = root.Q<Button>("save");

        camSens.value = Mathf.RoundToInt(PlayerPrefs.GetFloat("cam_sens") * 100);
        masterAudio.value = Mathf.RoundToInt(PlayerPrefs.GetFloat("main_volume") * 100);
        brightness.value = Mathf.RoundToInt(PlayerPrefs.GetFloat("brightness") * 100);

        camSens.RegisterValueChangedCallback(UpdateCameraSens);
        masterAudio.RegisterValueChangedCallback(UpdateMasterAudio);
        brightness.RegisterValueChangedCallback(UpdateBrightness);

        save.clicked += UnloadSettings;
    }

    private void UnloadSettings()
    {
        var root = uiDoc.rootVisualElement;

        SliderInt camSens = root.Q<SliderInt>("camSens");
        SliderInt masterAudio = root.Q<SliderInt>("masterAudio");
        SliderInt brightness = root.Q<SliderInt>("brightness");

        Button save = root.Q<Button>("save");

        camSens.UnregisterValueChangedCallback(UpdateCameraSens);
        masterAudio.UnregisterValueChangedCallback(UpdateMasterAudio);
        brightness.UnregisterValueChangedCallback(UpdateBrightness);

        save.clicked += UnloadSettings;

        if (wasPaused)
            LoadPauseMenu();
        else
            LoadMainMenu();
    }

    private void UpdateCameraSens(ChangeEvent<int> newSens)
    {
        float sens = newSens.newValue / 100f;

        PlayerPrefs.SetFloat("cam_sens", sens);
        sceneManager.UpdatePlayerPrefs();
    }

    private void UpdateMasterAudio(ChangeEvent<int> newAudio)
    {
        float vol = newAudio.newValue / 100f;

        PlayerPrefs.SetFloat("main_volume", vol);
        sceneManager.UpdatePlayerPrefs();
    }

    private void UpdateBrightness(ChangeEvent<int> newBrightness)
    {
        float bright = newBrightness.newValue / 100f;

        PlayerPrefs.SetFloat("brightness", bright);
        sceneManager.UpdatePlayerPrefs();
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
        LoadCrossHair();
    }

    public void LoadRespawnMenu()
    {
        if (sceneManager.introDeath)
        {
            Respawn();
            return;
        }
        else if(sceneManager.finalDeath)
        {
            LoadCredits();
            return;
        }

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
        if (!sceneManager.introDeath)
        {
            var root = uiDoc.rootVisualElement;

            respawn = root.Q<Button>("respawn");
            respawn.clicked -= Respawn;

            uiDoc.visualTreeAsset = crossHair;
        }

        sceneManager.introDeath = false;
        LoadCrossHair();
        sceneManager.ResetLevel();
    }

    private void LoadCredits()
    {
        uiDoc.visualTreeAsset = credits;

        var root = uiDoc.rootVisualElement;

        Button reload = root.Q<Button>("mainmenu");
        Button quit = root.Q<Button>("quit");

        reload.clicked += sceneManager.ReloadGame;
        quit.clicked += Application.Quit;
    }
}
