using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTextColor : MonoBehaviour
{
    public List<Button> buttonList;
    public Button button;

    public void onButtonPressed()
    {
        if (button.colors.normalColor == new Color32(0x65, 0x43, 0xE1,0xFF)) return;
        foreach (var butt in buttonList)
        {
            if (butt.colors.normalColor == new Color32(0x65, 0x43, 0xE1,0xFF))
            {
                ColorBlock cb = butt.colors;
                cb.normalColor = new Color32(0xEB, 0xEB, 0xEB,0xFF);
                cb.highlightedColor = new Color32(0xEB, 0xEB, 0xEB, 0xFF);
                cb.selectedColor = new Color32(0xEB, 0xEB, 0xEB, 0xFF);
                cb.pressedColor = new Color32(0x9B, 0x8E, 0xCA, 0xFF);
                butt.colors = cb;
                TextMeshProUGUI text = butt.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.black;
            }
            else if (butt == button)
            {
                ColorBlock cb = butt.colors;
                cb.normalColor = new Color32(0x65, 0x43, 0xE1, 0xFF);
                cb.highlightedColor = new Color32(0x65, 0x43, 0xE1, 0xFF);
                cb.selectedColor = new Color32(0x65, 0x43, 0xE1, 0xFF);
                cb.pressedColor = new Color32(0x9B, 0x8E, 0xCA, 0xFF);
                butt.colors = cb;
                TextMeshProUGUI text = butt.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.white;
            }
        }
    }
}
