using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public void UpdatePoint(GameObject obj)
    {
        GameManager gm = obj.GetComponent<GameManager>();
        tmp.text = "Point : " + gm.point;
    }
}
