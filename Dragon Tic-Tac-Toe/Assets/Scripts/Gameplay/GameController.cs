using TTT.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TTT.Character;

public class GameController : BaseMono
{
    public GameManager gameManager { get => GameManager.Instance; }
    public UIManager uiManager { get => gameManager.uIManager; }
    public CharacterManager characterManager { get => gameManager.characterManager; }

    public bool isGamePlaying;

    [Header("XO Sprite")]
    public Sprite spriteXmark;
    public Sprite spriteOmark;

    [Header("Gameplay")]
    public MarkType currentTurn;
    public int roundCount;
    public float timerCount;
    public Transform gridGroup;
    public TextMeshProUGUI debugText;

    private int xCount, oCount;

    private List<GridSpace> _gridSpaces = new List<GridSpace>();

    public override void Initialize()
    {
        _gridSpaces.Clear();
        _gridSpaces.AddRange(gridGroup.GetComponentsInChildren<GridSpace>());

        int size = gameManager.gameSettings.gameSettingsData.size;
        for (int i = 0; i < _gridSpaces.Count; i++)
        {
            _gridSpaces[i].tilePos = new Vector2(i % size, i / size);
            _gridSpaces[i].Initialize();
        }
        xCount = 0;
        oCount = 0;

        characterManager.PlayAnimation(MarkType.X, CharacterManager.CharAnim.Idle);
        characterManager.PlayAnimation(MarkType.O, CharacterManager.CharAnim.Idle);

        RandomTurn();

        if (gameManager.isSkip)
        {
            uiManager.SetTopLayout(gameManager.gameSettings.gameSettingsData);
        }
    }

    void RandomTurn()
    {
        currentTurn = (Random.Range(0, 2) == 1) ? MarkType.O : MarkType.X;
        uiManager.ChangeTurn(currentTurn);
        debugText.text = currentTurn.ToString();
    }

    private void ChangeTurn()
    {
        currentTurn = (currentTurn == MarkType.X) ? MarkType.O : MarkType.X;
        uiManager.ChangeTurn(currentTurn);
        debugText.text = currentTurn.ToString();
        timerCount = gameManager.gameSettings.gameSettingsData.time;

        characterManager.PlayAnimation(currentTurn, CharacterManager.CharAnim.OnTurn);
    }

    public void CheckCondition(GridSpace gridSpace = null)
    {
        characterManager.PlayAnimation(currentTurn, CharacterManager.CharAnim.Mark);

        bool isGameEnd = false;
        if (SearchNextNode(gridSpace))
        {
            isGameEnd = true;
        }

        foreach (var node in FindNeighborGrids(gridSpace))
        {
            if (SearchNextNode(node))
            {
                isGameEnd = true;
            }
        }

        if (isGameEnd)
        {
            debugText.text = "GameOver";
            GameEnd();
            return;
        }

        if (CheckGridFull())
        {
            GameEnd(true);//Set draw
            return;
        }


        ChangeTurn();

        bool SearchNextNode(GridSpace nodex)
        {
            foreach (var anode in FindNeighborGrids(nodex))
            {
                foreach (var sNode in FindNeighborGrids(anode))
                {
                    if (anode.alignment == sNode.alignment)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool CheckGridFull()
        {
            foreach (var grid in _gridSpaces)
            {
                if (grid.mark == MarkType.None)
                {
                    return false;
                }
            }
            return true;
        }
    }

    List<GridSpace> FindNeighborGrids(GridSpace gridSpace)
    {
        Vector2 pos = gridSpace.tilePos;

        Vector2 upperLeft = new Vector2(pos.x - 1, pos.y - 1);
        Vector2 upperCenter = new Vector2(pos.x, pos.y - 1);
        Vector2 upperRight = new Vector2(pos.x + 1, pos.y - 1);
        Vector2 middleLeft = new Vector2(pos.x - 1, pos.y);
        Vector2 middleRight = new Vector2(pos.x + 1, pos.y);
        Vector2 lowerLeft = new Vector2(pos.x - 1, pos.y + 1);
        Vector2 lowerCenter = new Vector2(pos.x, pos.y + 1);
        Vector2 lowerRight = new Vector2(pos.x + 1, pos.y + 1);

        List<GridSpace> gridNeighbors = new List<GridSpace>();
        foreach (var grid in _gridSpaces)
        {
            if (grid.tilePos != gridSpace.tilePos)
            {
                grid.alignment = GridSpace.Alignment.None;
                if (grid.mark == gridSpace.mark)
                {
                    if (grid.tilePos == upperLeft)
                        grid.alignment = GridSpace.Alignment.UpperLeft;
                    else if (grid.tilePos == upperCenter)
                        grid.alignment = GridSpace.Alignment.UpperCenter;
                    else if (grid.tilePos == upperRight)
                        grid.alignment = GridSpace.Alignment.UpperRight;
                    else if (grid.tilePos == middleLeft)
                        grid.alignment = GridSpace.Alignment.MiddleLeft;
                    else if (grid.tilePos == middleRight)
                        grid.alignment = GridSpace.Alignment.MiddleRight;
                    else if (grid.tilePos == lowerLeft)
                        grid.alignment = GridSpace.Alignment.LowerLeft;
                    else if (grid.tilePos == lowerCenter)
                        grid.alignment = GridSpace.Alignment.LowerCenter;
                    else if (grid.tilePos == lowerRight)
                        grid.alignment = GridSpace.Alignment.LowerRight;

                    if (grid.alignment != GridSpace.Alignment.None)
                        gridNeighbors.Add(grid);
                }
            }
        }
        return gridNeighbors;
    }

    private void GameEnd(bool isDraw = false)
    {
        int roundWinnerCount = 0;
        if (!isDraw)
        {
            if (currentTurn == MarkType.X)
            {
                xCount++;
                roundWinnerCount = xCount;

            }
            else if (currentTurn == MarkType.O)
            {
                oCount++;
                roundWinnerCount = oCount;
            }
            else
                Debug.Log("Incorrect Type!!!");

            uiManager.ShowRoundResult(currentTurn);
        }
        else
        {
            uiManager.ShowRoundResult(currentTurn, true); // Show draw text
        }

        uiManager.SetRoundSlot(currentTurn, roundWinnerCount);

        if (xCount < roundCount && oCount < roundCount)
        {
            characterManager.PlayAnimation(currentTurn, CharacterManager.CharAnim.WinRound);
            characterManager.PlayAnimation(GetOppositePlayer(currentTurn), CharacterManager.CharAnim.LoseRound);

            RestartRound();
        }
        else
        {
            characterManager.PlayAnimation(currentTurn, CharacterManager.CharAnim.WinGame);
            characterManager.PlayAnimation(GetOppositePlayer(currentTurn), CharacterManager.CharAnim.LoseGame);

            uiManager.ShowResult(currentTurn);
            isGamePlaying = false;
        }
    }

    public void RestartRound()
    {
        StartCoroutine(OnRestartRound());
    }

    IEnumerator OnRestartRound()
    {
        debugText.text = currentTurn + " Win round";
        isGamePlaying = false;
        yield return new WaitForSeconds(1.5f);
        isGamePlaying = true;
        uiManager.ShowRoundResult(MarkType.None);

        characterManager.PlayAnimation(currentTurn, CharacterManager.CharAnim.Idle);
        characterManager.PlayAnimation(currentTurn, CharacterManager.CharAnim.Idle);

        foreach (var grid in _gridSpaces)
        {
            grid.Reset();
        }

        ChangeTurn();
    }

    public void RestartGame()
    {
        isGamePlaying = false;
        uiManager.ShowLoading(UIManager.GameScreen.Gameplay);
        uiManager.SetTopLayout(gameManager.gameSettings.gameSettingsData);
        uiManager.SetBoardSize(gameManager.gameSettings.gameSettingsData.size);
        Initialize();
    }

    public void GameUpdate()
    {
        if (!isGamePlaying)
            return;

        if (timerCount == 0)
            return;

        uiManager.SetTimer((int)timerCount);
        timerCount -= Time.deltaTime;

        if (timerCount <= 0)
        {

            int randIndex = Random.Range(0, _gridSpaces.Count);
            while (_gridSpaces[randIndex].mark != MarkType.None)
            {
                randIndex = Random.Range(0, _gridSpaces.Count);
            }

            _gridSpaces[randIndex].SetGridMark(currentTurn);
        }
    }

    private MarkType GetOppositePlayer(MarkType markType)
    {
        return (markType == MarkType.X) ? MarkType.O : MarkType.X;
    }
}

public enum MarkType
{
    None,
    X,
    O
}
