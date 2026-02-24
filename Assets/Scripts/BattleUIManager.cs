using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    // Singleton instance for easy global access
    public static BattleUIManager Instance;
    public CanvasGroup mainCanvasGroup;
    [Header("Boss UI")]
    public Slider bossHealthSlider;

    [Header("Player Radial HP (In/Out Animation)")]
    public Image fighterRadialHP;
    public CanvasGroup fighterRadialGroup;
    public RectTransform fighterRadialRect;

    public Image rangerRadialHP;
    public CanvasGroup rangerRadialGroup;
    public RectTransform rangerRadialRect;

    public float activeRingScale = 1f;
    public float inactiveRingScale = 0.85f;
    public float slideSpeed = 12f;
    private bool isRangerActive = false;

    [Header("Cooldowns (Static Grid)")]
    public Image swordCooldownImage;
    public Image parryCooldownImage;
    public Image rollCooldownImage;
    public Image laserCooldownImage;

    [Header("Damage Stats")]
    public TextMeshProUGUI damageTrackerText;
    private float fighterDamage = 0;
    private float rangerDamage = 0;

    [Header("Player Portraits")]
    public Image centerPortraitImage;
    public Sprite fighterPortrait;   
    public Sprite rangerPortrait;    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start() => UpdateDamageTrackerUI();

    private void Update() => AnimateRadialRings();

    private void AnimateRadialRings()
    {
        if (fighterRadialGroup == null || rangerRadialGroup == null) return;

        float fighterTargetAlpha = isRangerActive ? 0.4f : 1f;
        float rangerTargetAlpha = isRangerActive ? 1f : 0.4f;

        Vector3 fighterTargetScale = isRangerActive ? new Vector3(inactiveRingScale, inactiveRingScale, 1f) : new Vector3(activeRingScale, activeRingScale, 1f);
        Vector3 rangerTargetScale = isRangerActive ? new Vector3(activeRingScale, activeRingScale, 1f) : new Vector3(inactiveRingScale, inactiveRingScale, 1f);

        fighterRadialGroup.alpha = Mathf.Lerp(fighterRadialGroup.alpha, fighterTargetAlpha, Time.deltaTime * slideSpeed);
        rangerRadialGroup.alpha = Mathf.Lerp(rangerRadialGroup.alpha, rangerTargetAlpha, Time.deltaTime * slideSpeed);

        fighterRadialRect.localScale = Vector3.Lerp(fighterRadialRect.localScale, fighterTargetScale, Time.deltaTime * slideSpeed);
        rangerRadialRect.localScale = Vector3.Lerp(rangerRadialRect.localScale, rangerTargetScale, Time.deltaTime * slideSpeed);

        if (isRangerActive) rangerRadialRect.SetAsLastSibling();
        else fighterRadialRect.SetAsLastSibling();
    }

    // Called by PlayerInputController when swapping characters
    public void SwapActivePlayerUI(CLASSTYPE activeClass)
    {
        isRangerActive = (activeClass == CLASSTYPE.RANGED);

        if (centerPortraitImage != null)
        {
            centerPortraitImage.sprite = isRangerActive ? rangerPortrait : fighterPortrait;
        }
    }

    // Called by PlayerController when taking damage
    public void UpdatePlayerHealthUI(float currentHP, float maxHP, CLASSTYPE classType)
    {
        // Fills the full 360-degree white circle based on damage
        float fillRatio = currentHP / maxHP;

        if (classType == CLASSTYPE.MELEE && fighterRadialHP != null) fighterRadialHP.fillAmount = fillRatio;
        else if (classType == CLASSTYPE.RANGED && rangerRadialHP != null) rangerRadialHP.fillAmount = fillRatio;
    }

    // Called by FighterMechanics and RangerMechanics in their Update loops
    public void UpdateCooldownUI(Image abilityIcon, float currentTimer, float maxCooldown)
    {
        if (abilityIcon != null) abilityIcon.fillAmount = 1f - (currentTimer / maxCooldown);
    }

    // Called by GenericProjectile and AttackHandler when hitting enemies
    public void AddDamage(CLASSTYPE classSource, float amount)
    {
        if (classSource == CLASSTYPE.RANGED) rangerDamage += amount;
        else if (classSource == CLASSTYPE.MELEE) fighterDamage += amount;

        UpdateDamageTrackerUI();
    }

    private void UpdateDamageTrackerUI()
    {
        if (damageTrackerText != null)
        {
            float total = fighterDamage + rangerDamage;
            damageTrackerText.text = $"Fighter: {fighterDamage}\nRanger: {rangerDamage}\n<color=yellow>TOTAL: {total}</color>";
        }
    }

    public void ToggleUI(bool isVisible)
    {
        if (mainCanvasGroup != null)
        {
            mainCanvasGroup.alpha = isVisible ? 1f : 0f;
            mainCanvasGroup.interactable = isVisible;
            mainCanvasGroup.blocksRaycasts = isVisible;
        }
    }
}