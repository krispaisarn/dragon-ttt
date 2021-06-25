using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace TTT.UI
{
    [RequireComponent(typeof(Button))]
    public class BaseButton : BaseMono
    {
        [ReadOnly] public Button button;
        public TextMeshProUGUI tmpText;
        private bool isInitialized;
        public override void Initialize()
        {
            if (isInitialized)
                return;

            button = this.GetComponent<Button>();
            button.onClick.AddListener(() => GameManager.Instance.audioManager.PlayButtonClick());

            isInitialized = true;
        }
        public void SetText(string text)
        {
            if (text == "0")
                text = "X";
            tmpText.text = text;
        }
        public void SetEvent(UnityAction action)
        {
            if (!isInitialized)
                Initialize();

            button.onClick.AddListener(action);
        }
    }
}
