using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class amountOfPointPerClickDisplay : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public void UpdateCursor(GameObject obj)
    {
        GameManager gm = obj.GetComponent<GameManager>();
        tmp.text = gm.amountOfPointPerClick + " Point / Click";
    }
}
