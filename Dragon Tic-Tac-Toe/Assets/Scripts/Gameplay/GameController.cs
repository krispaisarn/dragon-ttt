using TTT.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameController : BaseMono
{
    public GameManager gameManager { get => GameManager.Instance; }
    public UIManager uiManager { get => gameManager.uIManager; }

    public bool isGamePlaying;

    [Header("XO Sprite")]
    public Sprite spriteXmark;
    public Sprite spriteOmark;

    public MarkType currentTurn;
    public TextMeshProUGUI debugText;
    public Transform gridGroup;

    private List<GridSpace> _gridSpaces = new List<GridSpace>();

    // For debugging
    public bool isSkip;

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

        RandomTurn();

        if (isSkip)
        {
            uiManager.SetTopLayout(gameManager.gameSettings.gameSettingsData);
        }
    }

    void RandomTurn()
    {
        currentTurn = (Random.Range(0, 2) == 1) ? MarkType.O : MarkType.X;
        debugText.text = currentTurn.ToString();
    }

    private void ChangeTurn()
    {
        currentTurn = (currentTurn == MarkType.X) ? MarkType.O : MarkType.X;
        debugText.text = currentTurn.ToString();
        timerCount = gameManager.gameSettings.gameSettingsData.time;
    }

    public void CheckCondition(GridSpace gridSpace = null)
    {
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

    private void GameEnd()
    {
        //uiManager.ShowResult();

        int roundWinnderCount = 0;
        if (currentTurn == MarkType.X)
        {
            xCount++;
            roundWinnderCount = xCount;

        }
        else if (currentTurn == MarkType.O)
        {
            oCount++;
            roundWinnderCount = oCount;
        }
        else
            Debug.Log("Incorrect Type!!!");

        uiManager.SetRoundSlot(currentTurn, roundWinnderCount);

        if (xCount < roundCount && oCount < roundCount)
        {
            RestartRound();
        }
        else
        {
        
        }
    }
    int xCount, oCount;
    public int roundCount;
    public float timerCount;

    IEnumerator OnRestartRound()
    {
        debugText.text = currentTurn + " Win round";

        yield return new WaitForSeconds(1);

        foreach (var grid in _gridSpaces)
        {
            grid.Reset();
        }

        RandomTurn();
    }

    public void RestartRound()
    {
        StartCoroutine(OnRestartRound());
    }

    public void GameUpdate()
    {
        if (!isGamePlaying)
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
}

public enum MarkType
{
    None,
    X,
    O
}
