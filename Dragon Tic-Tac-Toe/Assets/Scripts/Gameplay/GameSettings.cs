using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TTT.UI;
using UnityEngine.Events;

namespace TTT.Settings
{
    public class GameSettings : BaseMono
    {
        [SerializeField] private GameSettingsData defaultSettingsData;
        [ReadOnly] public GameSettingsData gameSettingsData;
        [Header("Settings Group")]
        [SerializeField] private SettingsButton _roundSettingsButton;
        [SerializeField] private SettingsButton _timeSettingsButton;

        public override void Initialize()
        {
            // If no save
            UseDefaultSettings();

            /// Setup Sections
            SetUpRoundSection();
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
            }
        }

        public void SetRound(int roundCount)
        {
            gameSettingsData.round = roundCount;
        }

        public void SetTime(int timeCount)
        {
            gameSettingsData.time = timeCount;
        }

        public void ChangeSize(int value)
        {
            gameSettingsData.size = gameSettingsData.size + value;
        }

        public void UseDefaultSettings()
        {
            gameSettingsData = defaultSettingsData;
        }
    }
}