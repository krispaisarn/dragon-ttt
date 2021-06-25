using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TTT.Settings;
using TTT.UI;

public class GameManager : BaseMono
{
    public static GameManager Instance;
    // Start is called before the first frame update
    public GameController gameController;
    public GameSettings gameSettings;
    public UIManager uIManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Initialize();
    }

    public override void Initialize()
    {
        gameSettings.Initialize();
        gameController.Initialize();
        uIManager.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();

        gameController.GameUpdate();

    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
