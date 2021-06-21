using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseMono
{

    public static GameManager Instance;
    // Start is called before the first frame update
    public GameController gameController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        Initialize();
    }

    public override void Initialize()
    {
        //   gameController.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            RestartGame();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
