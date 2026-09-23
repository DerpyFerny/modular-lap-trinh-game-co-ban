using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllowAttack : MonoBehaviour
{
    public BossManager bm;
    public Button AttackButton;
    void Start()
    {
        bm = gameObject.GetComponent<BossManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bm.Attacking)
        {
            AttackButton.interactable = false;
        }
        else
        {
            AttackButton.interactable = true;
        }
    }
}
