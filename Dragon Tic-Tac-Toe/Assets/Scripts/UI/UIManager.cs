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
        [SerializeField] private GameObject _gameplayGroup;
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;

        [Header("Player Tag")]
        [SerializeField] private GameObject _xTag;
        [SerializeField] private GameObject _oTag;
        [SerializeField] private TextMeshProUGUI _xRoundResultTmp;
        [SerializeField] private TextMeshProUGUI _oRoundResultTmp;

        [Header("Top Layout")]
        [SerializeField] private Transform _xRoundGroup;
        [SerializeField] private Transform _oRoundGroup;
        [SerializeField] private TextMeshProUGUI _timerTmp;

        [Header("Result")]
        [SerializeField] private CanvasGroup _resultCanvas;
        [SerializeField] private GameObject _xResultTag;
        [SerializeField] private GameObject _oResultTag;

        [SerializeField] private BaseButton _playAgainButton;
        [SerializeField] private BaseButton _resultMenuButton;

        [Header("Pause Menu")]
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private BaseButton _pauseButton;
        [SerializeField] private BaseButton _continueButton;
        [SerializeField] private BaseButton _restartButton;
        [SerializeField] private BaseButton _menuButton;

        private RoundSlot[] _xRoundSlots;
        private RoundSlot[] _oRoundSlots;
        public GameManager gameManager { get => GameManager.Instance; }

        public enum GameScreen
        {
            Menu,
            Gameplay
        }

        private bool isInitialized;
        public override void Initialize()
        {
            if (isInitialized)
                return;

            _xRoundSlots = _xRoundGroup.GetComponentsInChildren<RoundSlot>();
            _oRoundSlots = _oRoundGroup.GetComponentsInChildren<RoundSlot>();

            _pauseButton.SetEvent(() => ShowPauseMenu());
            _continueButton.SetEvent(() => HidePauseMenu());
            _restartButton.SetEvent(() => RestartGame());
            _menuButton.SetEvent(() => GoToMenu());

            _playAgainButton.SetEvent(() => RestartGame());
            _resultMenuButton.SetEvent(() => GoToMenu());

            isInitialized = true;
        }

        public void SetUpReleaseUI()
        {
            _gameplayGroup.SetActive(false);
            _gameSettingsBoard.SetActive(true);
            _pauseButton.gameObject.SetActive(false);
            _resultCanvas.gameObject.SetActive(false);
            _pauseMenu.SetActive(false);
            _xRoundResultTmp.text = "";
            _oRoundResultTmp.text = "";
        }

        public void ShowResult(MarkType winnerMark)
        {
            _xResultTag.SetActive(false);
            _oResultTag.SetActive(false);

            if (winnerMark == MarkType.X)
            {
                _xResultTag.SetActive(true);
            }
            else if (winnerMark == MarkType.O)
            {
                _oResultTag.SetActive(true);
            }

            _resultCanvas.gameObject.SetActive(true);
        }

        public void HideResult()
        {
            _resultCanvas.gameObject.SetActive(false);
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

            float screenRatio = (float)Screen.width / (float)Screen.height;

            _gridLayoutGroup.constraintCount = size;

            _gridLayoutGroup.cellSize = new Vector2(410 - (size * 25) - (screenRatio * 50), 410 - (size * 25) - (screenRatio * 50));
            _gridLayoutGroup.spacing = new Vector2(45 - (size * 3) - (screenRatio * 5), 45 - (size * 3) - (screenRatio * 5));
        }

        public void ShowLoading(GameScreen gameScreen = GameScreen.Menu)
        {
            _loadingAnimator.SetTrigger("Play");
            StartCoroutine(OnShowLoading(gameScreen));
        }

        IEnumerator OnShowLoading(GameScreen gameScreen = GameScreen.Menu)
        {
            float animationDuration = 2.5f;
            yield return new WaitForSeconds(animationDuration / 2f);

            if (gameScreen == GameScreen.Gameplay)
            {
                _gameSettingsBoard.SetActive(false);
                _gameplayGroup.SetActive(true);
                _pauseButton.gameObject.SetActive(true);
            }
            else if (gameScreen == GameScreen.Menu)
            {
                _gameSettingsBoard.SetActive(true);
                _gameplayGroup.SetActive(false);
                _pauseButton.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(animationDuration / 2f);
            gameManager.gameController.isGamePlaying = true;
            gameManager.isGamePause = false;
        }



        public void ChangeTurn(MarkType markType)
        {
            if (markType == MarkType.X)
            {
                _xTag.SetActive(true);
                _oTag.SetActive(false);
            }
            else if (markType == MarkType.O)
            {
                _oTag.SetActive(true);
                _xTag.SetActive(false);
            }
        }

        public void ShowPauseMenu()
        {
            gameManager.isGamePause = true;
            _pauseMenu.SetActive(true);
        }

        public void HidePauseMenu()
        {
            gameManager.isGamePause = false;
            _pauseMenu.SetActive(false);
        }

        public void RestartGame()
        {
            _pauseMenu.SetActive(false);
            HideResult();
            gameManager.gameController.RestartGame();
        }

        public void GoToMenu()
        {
            HidePauseMenu();
            HideResult();
            ShowLoading(UIManager.GameScreen.Menu);
            gameManager.characterManager.Reset();
            SetUpReleaseUI();
        }

        public void ShowRoundResult(MarkType winnerMark, bool isDraw = false)
        {
            _oRoundResultTmp.text = "";
            _xRoundResultTmp.text = "";

            if (winnerMark == MarkType.None)
                return;

            if (isDraw)
            {
                _oRoundResultTmp.text = "Draw..";
                _xRoundResultTmp.text = "Draw..";
                return;
            }

            if (winnerMark == MarkType.X)
            {
                _xRoundResultTmp.text = "Wins!";
            }
            else if (winnerMark == MarkType.O)
            {
                _oRoundResultTmp.text = "Wins!";
            }
        }
    }
}
