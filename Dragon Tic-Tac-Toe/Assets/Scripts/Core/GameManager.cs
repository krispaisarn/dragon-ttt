using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TTT.Settings;
using TTT.UI;
using TTT.Character;
using TTT.Audio;

public class GameManager : BaseMono
{
    public static GameManager Instance;
    // Start is called before the first frame update

    // For debugging
    public bool isSkip;

    public GameController gameController;
    public GameSettings gameSettings;
    public UIManager uIManager;
    public CharacterManager characterManager;
    public AudioManager audioManager;
    public bool isGamePause;
    public bool isReleaseUI;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Initialize();
    }

    public override void Initialize()
    {
        characterManager.Initialize();
        gameSettings.Initialize();
        uIManager.Initialize();

        if (isReleaseUI)
            uIManager.SetUpReleaseUI();

        if (isSkip)
            gameController.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();

        if (isGamePause)
            return;

        gameController.GameUpdate();

    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
