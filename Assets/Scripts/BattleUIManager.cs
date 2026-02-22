using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    [Header("Boss UI")]
    public Slider bossHealthSlider;

    [Header("Player HP Bars")]
    public RectTransform activePos;
    public RectTransform inactivePos;

    public CanvasGroup mandarinHPGroup;
    public CanvasGroup dragonFruitHPGroup;

    [Header("UI Animation Settings")]
    public float swapSpeed = 10f;
    private bool isMandarinActive = false;

    [Header("Cooldowns")]
    public Image rollCooldownImage;
    public Image laserCooldownImage;

    [Header("Damage Stats")]
    public TextMeshProUGUI damageTrackerText;
    private float mandarinDamage = 0;
    private float dragonFruitDamage = 0;

    private void Update()
    {
        AnimateHPBars();
    }

    private void AnimateHPBars()
    {
        if (mandarinHPGroup == null || dragonFruitHPGroup == null) return;

        RectTransform mandarinRect = mandarinHPGroup.GetComponent<RectTransform>();
        RectTransform dragonFruitRect = dragonFruitHPGroup.GetComponent<RectTransform>();

        Vector3 mandarinTargetPos = isMandarinActive ? activePos.anchoredPosition : inactivePos.anchoredPosition;
        Vector3 dragonTargetPos = isMandarinActive ? inactivePos.anchoredPosition : activePos.anchoredPosition;

        float mandarinTargetAlpha = isMandarinActive ? 1f : 0.5f;
        float dragonTargetAlpha = isMandarinActive ? 0.5f : 1f;

        // Lerp everything smooth slide and fade effect
        mandarinRect.anchoredPosition = Vector3.Lerp(mandarinRect.anchoredPosition, mandarinTargetPos, Time.deltaTime * swapSpeed);
        dragonFruitRect.anchoredPosition = Vector3.Lerp(dragonFruitRect.anchoredPosition, dragonTargetPos, Time.deltaTime * swapSpeed);

        mandarinHPGroup.alpha = Mathf.Lerp(mandarinHPGroup.alpha, mandarinTargetAlpha, Time.deltaTime * swapSpeed);
        dragonFruitHPGroup.alpha = Mathf.Lerp(dragonFruitHPGroup.alpha, dragonTargetAlpha, Time.deltaTime * swapSpeed);
    }

    // Call this whenever the player presses 1 or 2 to switch characters
    public void SwapActivePlayerUI(CLASSTYPE activeClass)
    {
        isMandarinActive = (activeClass == CLASSTYPE.RANGED);
    }

    // Call these to update the visuals
    public void UpdateDamageTracker(float mandarinDmg, float dragonDmg)
    {
        mandarinDamage = mandarinDmg;
        dragonFruitDamage = dragonDmg;
        float total = mandarinDamage + dragonFruitDamage;

        damageTrackerText.text = $"Mandarin: {mandarinDamage}\nDragonFruit: {dragonFruitDamage}\n<color=yellow>TOTAL: {total}</color>";
    }

    public void UpdateCooldownUI(Image abilityIcon, float currentCooldownTimer, float maxCooldown)
    {
        // Fills the radial circle from 0 to 1 based on the timer
        if (abilityIcon != null)
        {
            abilityIcon.fillAmount = 1f - (currentCooldownTimer / maxCooldown);
        }
    }
}