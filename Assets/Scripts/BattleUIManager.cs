using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    public static BattleUIManager Instance;

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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateDamageTrackerUI();
    }

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

        mandarinRect.anchoredPosition = Vector3.Lerp(mandarinRect.anchoredPosition, mandarinTargetPos, Time.deltaTime * swapSpeed);
        dragonFruitRect.anchoredPosition = Vector3.Lerp(dragonFruitRect.anchoredPosition, dragonTargetPos, Time.deltaTime * swapSpeed);

        mandarinHPGroup.alpha = Mathf.Lerp(mandarinHPGroup.alpha, mandarinTargetAlpha, Time.deltaTime * swapSpeed);
        dragonFruitHPGroup.alpha = Mathf.Lerp(dragonFruitHPGroup.alpha, dragonTargetAlpha, Time.deltaTime * swapSpeed);
    }

    public void SwapActivePlayerUI(CLASSTYPE activeClass)
    {
        isMandarinActive = (activeClass == CLASSTYPE.RANGED);
    }

    public void UpdateCooldownUI(Image abilityIcon, float currentCooldownTimer, float maxCooldown)
    {
        if (abilityIcon != null)
        {
            abilityIcon.fillAmount = 1f - (currentCooldownTimer / maxCooldown);
        }
    }
    public void AddDamage(CLASSTYPE classSource, float amount)
    {
        if (classSource == CLASSTYPE.RANGED)
        {
            mandarinDamage += amount;
        }
        else if (classSource == CLASSTYPE.MELEE)
        {
            dragonFruitDamage += amount;
        }

        UpdateDamageTrackerUI();
    }

    private void UpdateDamageTrackerUI()
    {
        if (damageTrackerText != null)
        {
            float total = mandarinDamage + dragonFruitDamage;
            damageTrackerText.text = $"Mandarin: {mandarinDamage}\nDragonFruit: {dragonFruitDamage}\n<color=yellow>TOTAL: {total}</color>";
        }
    }
}