using TTT.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class GameController : MonoBehaviour
{
    public UIManager uiManager { get => UIManager.Instance; }
    [Header("XO Sprite")]
    public Sprite spriteXmark;
    public Sprite spriteOmark;

    public MarkType currentTurn;
    public TextMeshProUGUI debugText;
    public Transform gridGroup;

    private List<GridSpace> _gridSpaces = new List<GridSpace>();

    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        _gridSpaces.Clear();
        _gridSpaces.AddRange(gridGroup.GetComponentsInChildren<GridSpace>());

        for (int i = 0; i < _gridSpaces.Count; i++)
        {
            _gridSpaces[i].tilePos = new Vector2(i % 4, i / 4);
            _gridSpaces[i].Initialize();
        }

        RandomTurn();
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
    }

    public void CheckCondition(GridSpace gridSpace = null)
    {

        if (SearchNextNode(gridSpace))
        {
            debugText.text = "GameOver";
            return;
        }

        foreach (var node in FindNeighborGrids(gridSpace))
        {
            if (SearchNextNode(node))
            {
                debugText.text = "GameOver";
                return;
            }
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
        uiManager.ShowResult();
    }

    public void RestartGame()
    {

    }
}

public enum MarkType
{
    None,
    X,
    O
}
