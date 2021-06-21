using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    public GameController gameController { get => GameManager.Instance.gameController; }
    public MarkType mark;
    public Vector2 tilePos;
    public Alignment alignment;
    [SerializeField] private Image markImage;
    private Button _button;

    public void Initialize()
    {
        _button = this.GetComponent<Button>();
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => SetGridMark());

        markImage.gameObject.SetActive(false);
    }

    public void SetGridMark(MarkType markType = MarkType.None)
    {
        mark = gameController.currentTurn;
        SetMarkImage(mark);
        gameController.CheckCondition(this);

        _button.interactable = false;

        void SetMarkImage(MarkType markType = MarkType.None)
        {
            Sprite markSprite = null;
            switch (markType)
            {
                case MarkType.X:
                    markSprite = gameController.spriteXmark;
                    break;
                case MarkType.O:
                    markSprite = gameController.spriteOmark;
                    break;
            }
            markImage.sprite = markSprite;
            markImage.gameObject.SetActive(true);
        }
    }

    public enum Alignment
    {
        None,
        UpperLeft,
        UpperCenter,
        UpperRight,
        MiddleLeft,
        MiddleRight,
        LowerLeft,
        LowerCenter,
        LowerRight
    }
}