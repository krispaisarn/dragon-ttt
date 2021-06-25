using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT.UI;
using UnityEngine.Events;
using TMPro;

namespace TTT.Settings
{
    public class GameSettings : BaseMono
    {
        [SerializeField] private GameSettingsData defaultSettingsData;
        [ReadOnly] public GameSettingsData gameSettingsData;
        [Header("Settings Group")]
        [SerializeField] private SettingsButton _roundSettingsButton;
        [SerializeField] private SettingsButton _timeSettingsButton;
        [Header("Size Group")]
        [SerializeField] private SettingsButton _addSizeButton;
        [SerializeField] private SettingsButton _substractSizeButton;
        [SerializeField] private TextMeshProUGUI _sizeTmp;

        [Header("Button Colors")]
        [SerializeField] private Color _settingOffColor;
        [SerializeField] private Color _settingOnColor;
        [Header("Etc.")]
        [SerializeField] private BaseButton _startButton;

        private List<SettingsButton> _roundSettingsButtons = new List<SettingsButton>();
        private List<SettingsButton> _timeSettingsButtons = new List<SettingsButton>();

        private UIManager _uiManager { get => GameManager.Instance.uIManager; }
        private GameController _gameController { get => GameManager.Instance.gameController; }

        public override void Initialize()
        {
            // If no save
            UseDefaultSettings();

            /// Setup Sections
            SetUpRoundSection();
            SetUpTimeSection();
            SetUpSizeSection();

            _startButton.Initialize();
            _startButton.SetEvent(() => StartGame());
        }

        private void SetUpRoundSection()
        {
            for (int i = 0; i < gameSettingsData.rounds.Length; i++)
            {
                int roundCount = gameSettingsData.rounds[i];
                SettingsButton button = null;
                if (i == 0)
                {
                    button = _roundSettingsButton;
                }
                else
                {
                    button = Instantiate(_roundSettingsButton, Vector3.zero, Quaternion.identity, _roundSettingsButton.transform.parent);
                }
                button.Initialize();
                button.SetText(roundCount.ToString());
                button.SetEvent(() => SetRound(roundCount));
                button.SetEvent(() => button.SetButtonOn(_settingOnColor));
                _roundSettingsButtons.Add(button);

                if (roundCount == gameSettingsData.round)
                    button.SetButtonOn(_settingOnColor);
                else
                    button.SetButtonOn(_settingOffColor);
            }
        }

        private void SetUpTimeSection()
        {
            for (int i = 0; i < gameSettingsData.times.Length; i++)
            {
                int timeCount = gameSettingsData.times[i];
                SettingsButton button = null;
                if (i == 0)
                {
                    button = _timeSettingsButton;
                }
                else
                {
                    button = Instantiate(_timeSettingsButton, Vector3.zero, Quaternion.identity, _timeSettingsButton.transform.parent);
                }
                button.Initialize();
                button.SetText(timeCount.ToString());
                button.SetEvent(() => SetTime(timeCount));
                button.SetEvent(() => button.SetButtonOn(_settingOnColor));
                _timeSettingsButtons.Add(button);

                if (timeCount == gameSettingsData.time)
                    button.SetButtonOn(_settingOnColor);
                else
                    button.SetButtonOn(_settingOffColor);
            }
        }

        private void SetUpSizeSection()
        {
            _addSizeButton.Initialize();
            _addSizeButton.SetEvent(() => ChangeSize(1));

            _substractSizeButton.Initialize();
            _substractSizeButton.SetEvent(() => ChangeSize(-1));

            SizeConiditon(gameSettingsData.size);
        }

        public void SetRound(int roundCount)
        {
            gameSettingsData.round = roundCount;
            ToggleButtonsOff(_roundSettingsButtons);
        }

        public void SetTime(int timeCount)
        {
            gameSettingsData.time = timeCount;
            ToggleButtonsOff(_timeSettingsButtons);
        }

        public void ChangeSize(int value)
        {
            int newSize = gameSettingsData.size + value;
            gameSettingsData.size = newSize;

            SizeConiditon(newSize);
        }

        private void SizeConiditon(int size)
        {
            _addSizeButton.ToggleButtonVisibility(true);
            _substractSizeButton.ToggleButtonVisibility(true);

            if (size >= gameSettingsData.maxSize)
                _addSizeButton.ToggleButtonVisibility(false);
            else if (size <= gameSettingsData.minSize)
                _substractSizeButton.ToggleButtonVisibility(false);

            _sizeTmp.text = size.ToString();
        }

        public void UseDefaultSettings()
        {
            gameSettingsData = defaultSettingsData;
        }

        private void ToggleButtonsOff(List<SettingsButton> buttons)
        {
            foreach (var button in buttons)
            {
                button.SetButtonOff(_settingOffColor);
            }
        }

        public void StartGame()
        {
            _gameController.RestartGame();
        }

    }
}