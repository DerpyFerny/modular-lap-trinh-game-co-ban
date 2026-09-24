using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visually represents player Unit HP using a standard Unity UI Slider.
/// Reads directly from the existing Unit's currentHP and maxHP.
/// </summary>
public class PlayerHealthBar : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private Slider hpSlider;

    [Header("Target Unit")]
    [SerializeField] private Unit playerUnit;

    private void Awake()
    {
        if (hpSlider == null)
        {
            hpSlider = GetComponent<Slider>();
        }
    }

    /// <summary>
    /// Binds the health bar to the spawned player Unit.
    /// </summary>
    public void SetTargetUnit(Unit unit)
    {
        playerUnit = unit;
        UpdateHealthBar();
    }

    /// <summary>
    /// Updates the slider value and bounds.
    /// </summary>
    public void UpdateHealthBar()
    {
        if (playerUnit == null || hpSlider == null) return;

        hpSlider.maxValue = playerUnit.maxHP;
        hpSlider.value = playerUnit.currentHP;
    }

    private void Update()
    {
        if (playerUnit != null && hpSlider != null)
        {
            if (hpSlider.value != playerUnit.currentHP || hpSlider.maxValue != playerUnit.maxHP)
            {
                UpdateHealthBar();
            }
        }
    }
}
