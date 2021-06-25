using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpace : MonoBehaviour
{
    [ReadOnly] public MarkType mark;
    [ReadOnly] public Vector2 tilePos;
    [ReadOnly] public Alignment alignment;

    [SerializeField] private Image markImage;
    private GameController _gameController { get => GameManager.Instance.gameController; }
    private Button _button;
    private bool isInitialized;
    public void Initialize()
    {
        if (!isInitialized)
        {
            _button = this.GetComponent<Button>();
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => SetGridMark());
            _button.onClick.AddListener(() => GameManager.Instance.audioManager.PlayMark());

            isInitialized = true;
        }

        Reset();
    }

    public void Reset()
    {
        mark = MarkType.None;
        markImage.sprite = null;
        _button.interactable = true;
        markImage.gameObject.SetActive(false);
    }

    public void SetGridMark(MarkType markType = MarkType.None)
    {
        if (!_gameController.isGamePlaying)
            return;

        if (mark != MarkType.None)//Prevent button spamming
            return;

        mark = _gameController.currentTurn;
        SetMarkImage(mark);
        _gameController.CheckCondition(this);

        _button.interactable = false;

        void SetMarkImage(MarkType markType = MarkType.None)
        {
            Sprite markSprite = null;
            switch (markType)
            {
                case MarkType.X:
                    markSprite = _gameController.spriteXmark;
                    break;
                case MarkType.O:
                    markSprite = _gameController.spriteOmark;
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