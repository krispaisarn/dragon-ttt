using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundSlot : BaseMono
{
    public Image onIcon;
    public override void Initialize()
    {
        onIcon.enabled = false;
    }
    public void ToggleSlotActive(bool isActive)
    {
        onIcon.enabled = isActive;
    }

    public void ToggleSlotObject(bool isActive)
    {
        this.gameObject.SetActive(isActive);
        onIcon.enabled = false;
    }
}
