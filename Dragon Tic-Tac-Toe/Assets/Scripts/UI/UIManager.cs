using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT.Settings;
using TMPro;
using UnityEngine.UI;

namespace TTT.UI
{
    public class UIManager : BaseMono
    {
        [SerializeField] private Animator _loadingAnimator;
        [SerializeField] private GameObject _gameSettingsBoard;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;

        [Header("Top Layout")]
        [SerializeField] private Transform _xRoundGroup;
        [SerializeField] private Transform _oRoundGroup;
        [SerializeField] private TextMeshProUGUI _timerTmp;

        [Header("Result")]
        [SerializeField] private CanvasGroup _resultCanvas;

        private RoundSlot[] _xRoundSlots;
        private RoundSlot[] _oRoundSlots;
        public GameManager gameManager { get => GameManager.Instance; }

        public override void Initialize()
        {
            _xRoundSlots = _xRoundGroup.GetComponentsInChildren<RoundSlot>();
            _oRoundSlots = _oRoundGroup.GetComponentsInChildren<RoundSlot>();

        }

        public void ShowResult()
        {
            _resultCanvas.gameObject.SetActive(true);
        }

        public void SetTopLayout(GameSettingsData settingsData)
        {
            gameManager.gameController.roundCount = settingsData.round;
            gameManager.gameController.timerCount = settingsData.time;

            Initialize(_xRoundSlots);
            Initialize(_oRoundSlots);

            SetTimer(gameManager.gameController.timerCount);

            void Initialize(RoundSlot[] slots)
            {
                foreach (var slot in slots)
                {
                    slot.ToggleSlotObject(false);
                }

                for (int i = 0; i < settingsData.round; i++)
                {
                    slots[i].ToggleSlotObject(true);
                }
            }
        }

        public void SetRoundSlot(MarkType type, int count)
        {
            if (type == MarkType.X)
            {
                SetSlotIconOnActive(_xRoundSlots);
            }
            else if (type == MarkType.O)
            {
                SetSlotIconOnActive(_oRoundSlots);
            }
            else
            {
                Debug.Log("Incorrect Type!!!");
                return;
            }

            void SetSlotIconOnActive(RoundSlot[] slots)
            {
                for (int i = 0; i < count; i++)
                {
                    slots[i].ToggleSlotActive(true);
                }
            }
        }

        public void SetTimer(float timerCount)
        {
            _timerTmp.text = timerCount.ToString();
        }

        public void SetBoardSize(int size)
        {
            for (int i = 0; i < _gridLayoutGroup.transform.childCount; i++)
            {
                GameObject gridObj = _gridLayoutGroup.transform.GetChild(i).gameObject;
                if (size * size > i)
                {
                    gridObj.SetActive(true);
                }
                else
                {
                    gridObj.SetActive(false);
                }
            }

            _gridLayoutGroup.constraintCount = size;
            _gridLayoutGroup.cellSize = new Vector2(360 - (size * 30), 360 - (size * 30));
            _gridLayoutGroup.spacing = new Vector2(45 - (size * 5), 45 - (size * 5));
        }

        public void ShowLoading()
        {
            _loadingAnimator.SetTrigger("Play");
            StartCoroutine(OnShowLoading());
        }

        IEnumerator OnShowLoading()
        {
            float animationDuration = _loadingAnimator.runtimeAnimatorController.animationClips[0].averageDuration / 2f;
            yield return new WaitForSeconds(animationDuration);
            _gameSettingsBoard.SetActive(false);
            yield return new WaitForSeconds(animationDuration);
            gameManager.gameController.isGamePlaying = true;
        }

    }
}
