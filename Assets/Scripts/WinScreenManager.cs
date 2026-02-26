using UnityEngine;
using TMPro;

public class WinScreenManager : MonoBehaviour
{
    [Header("Damage Text UI")]
    [SerializeField] private TextMeshProUGUI fighterDamageText;
    [SerializeField] private TextMeshProUGUI rangerDamageText;
    [SerializeField] private TextMeshProUGUI totalDamageText;

    [Header("MVP Portraits")]
    [SerializeField] private GameObject fighterMVPPortrait; // Drag the DragonFruit Image here
    [SerializeField] private GameObject rangerMVPPortrait;  // Drag the Mandarin Image here

    private void Start()
    {
        // 1. Read the numbers from the bridge
        float fDamage = PersistentCombatStats.totalFighterDamage;
        float rDamage = PersistentCombatStats.totalRangerDamage;
        float combinedDamage = fDamage + rDamage;

        // 2. Update the Text UI
        if (fighterDamageText != null)
            fighterDamageText.text = "DragonFruit Damage: " + fDamage.ToString("F0");

        if (rangerDamageText != null)
            rangerDamageText.text = "Mandarin Damage: " + rDamage.ToString("F0");

        if (totalDamageText != null)
            totalDamageText.text = "Total Combined Damage: " + combinedDamage.ToString("F0");

        // 3. Determine the MVP!
        if (fDamage >= rDamage)
        {
            // DragonFruit dealt more damage (or they tied)
            if (fighterMVPPortrait != null) fighterMVPPortrait.SetActive(true);
            if (rangerMVPPortrait != null) rangerMVPPortrait.SetActive(false);
        }
        else
        {
            // Mandarin dealt more damage
            if (fighterMVPPortrait != null) fighterMVPPortrait.SetActive(false);
            if (rangerMVPPortrait != null) rangerMVPPortrait.SetActive(true);
        }

        // 4. Unlock the mouse so the player can click the Quit button
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Link this to your "Quit" Button's OnClick event!
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        PersistentCombatStats.ResetStats();

        // Quits the application if built, or stops playing if in the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}