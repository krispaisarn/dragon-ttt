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
        public override void Initialize()
        {
            button = this.GetComponent<Button>();
        }
        public void SetText(string text)
        {
            tmpText.text = text;
        }
        public void SetEvent(UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }
    }
}
