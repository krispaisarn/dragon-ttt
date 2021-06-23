
using UnityEngine;
using UnityEngine.UI;

namespace TTT.UI
{
    public class SettingsButton : BaseButton
    {
        public Image bgImg;
        public Image iconImg;

        public void SetButtonOff(Color offColor)
        {
            bgImg.color = offColor;
        }

        public void SetButtonOn(Color onColor)
        {
            bgImg.color = onColor;
        }

        public void ToggleButtonVisibility(bool isVisible)
        {
            bgImg.enabled = isVisible;
            iconImg.enabled = isVisible;
            button.enabled = isVisible;
        }
    }
}
