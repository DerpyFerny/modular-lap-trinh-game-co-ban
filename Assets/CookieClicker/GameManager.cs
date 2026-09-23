using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnScoreChange;
    public int point;
    public int amountOfPointPerClick = 1;

    public Button CursorUpgradeButton;

    public void addPointFromCursor()
    {
        point += amountOfPointPerClick;
        OnScoreChange.Invoke();
    }
    void Update()
    {
        CanUpgradeCursor();   
    }

    public int CursorUpgradePrice;
    void CanUpgradeCursor()
    {
        if(point < CursorUpgradePrice)
        {
            CursorUpgradeButton.interactable = false;
        }
        else
        {
            CursorUpgradeButton.interactable = true;
        }
    }
    
    public void UpgradeCursor()
    {
        point -= CursorUpgradePrice;
        amountOfPointPerClick++;
        OnScoreChange.Invoke();
    }
}
