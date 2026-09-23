using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public string unitUniqueDialog;
	// public int unitLevel;

	public float damage;

	public float maxHP;
	public float currentHP;

	public bool TakeDamage(float dmg)
	{
        currentHP = Mathf.Clamp(currentHP -= dmg, 0, maxHP);
		if (currentHP == 0)
			return true;
		else
			return false;
	}

	public void Heal(float amount)
	{
		currentHP += amount;
		if (currentHP > maxHP)
			currentHP = maxHP;
	}
}
