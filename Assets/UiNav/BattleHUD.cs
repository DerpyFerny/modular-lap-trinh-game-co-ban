using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI uniqueDialogueText;
    // public Slider hpSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        hpText.text = unit.currentHP + " / " + unit.maxHP;
        uniqueDialogueText.text = unit.unitUniqueDialog;
        // hpSlider.maxValue = unit.maxHP;
        // hpSlider.value = unit.currentHP;
    }
//for the hp bar i never had
    public void SetHP(float hp, float maxHP)
    {
        hpText.text = hp + " / " + maxHP;
        // hpSlider.value = hp;
    }
}
