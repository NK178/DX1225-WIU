using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;



// Klaus Phase 1: Mechanical Knife Attack & Hand Swipe Attack
// Ainsley Phase 2: Hand Slam, Fly Swatter Attack, Claw Grab, Sugarcane Missiles and Fruit Air Strike

[System.Serializable]
struct AttackPhaseData
{
    // Phase 1 2 3
    // Attack Scriptable Object
    public float healthPercentage;
    public int phaseNo;
    public List<BossAttacks> _atks;
    //public BossAttacks _atks;
}

public class BossController : MonoBehaviour
{
    [SerializeField] private DataHolder dataHolder;
    [SerializeField] private BossAnimator animator;
    [SerializeField] private BossClassData bossData;

    [SerializeField] private List<AttackPhaseData> attackPhaseData;
    private BossActiveData activeData;
    public BossActiveData ActiveData
    {
        get { return activeData; }
        private set { activeData = value; }
    }

    [Header("Debugging")]
    [SerializeField] private BossAttacks DEBUGAttackData;
    [SerializeField] private AttackHandler DEBUGAttackHandler;

    public float HP;
    public float ATK;
    private bool DebugEnableAttack;

    public bool debugRunning = false;

    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private Color damageColor;
    [SerializeField] private float damageEffectDuration;
    private Color originalColor;

    [SerializeField] private float minIdleTime; 
    [SerializeField] private float maxIdleTime; 

    //the actual phases
    private int waveIndex = 0;
    private bool isBossActive = false;
    private bool shouldRandomizeAttack = false;

    private BossAttacks activeBossAttack;

    private bool IKEnabled = false;


    private void Start()
    {
        if (dataHolder.activeData == null)
        {
            Debug.LogError("NO ACTIVE DATA HELD");
            return;
        }

        activeData = (BossActiveData)dataHolder.activeData;

        if (activeData == null)
        {
            Debug.LogError("NO BOSS ACTIVE DATA FOUND");
            return;
        }

        originalColor = objectRenderer.material.color;
        //Set to idle 
        activeData.BAnimState = BossActiveData.BossAnimStates.IDLE;
        activeData.isMoving = false;


        //Debug 
        DEBUGAttackData.UpdateAttack(activeData);
        debugRunning = true;
        DebugEnableAttack = false;
        activeData.currentHealth = bossData.maxHealth;
        activeData.currentAttack = bossData.damage;

        // Set true for now     
        isBossActive = true;

        waveIndex = 0;
        activeData.BossPhase = 0;
        shouldRandomizeAttack = true;

    }



    private void Update()
    {

        if (!isBossActive)
            return;


        if (shouldRandomizeAttack)
        {
            SelectAttackPhase();
            StartCoroutine(AttackDurationCoroutine());
            activeBossAttack.ExecuteAttack(activeData);
        }

        if (activeBossAttack != null)
        {
            activeBossAttack.UpdateAttack(activeData);
        }
        //if (activeData.isAttacking)
        //{
        //    if (shouldRandomizeAttack)
        //    {
        //        SelectAttackPhase();
        //        StartCoroutine(AttackDurationCoroutine());
        //        activeBossAttack.ExecuteAttack(activeData);
        //    }

        //    if (activeBossAttack != null)
        //    {
        //        activeBossAttack.UpdateAttack(activeData);
        //    }
        //}


        float bossHealthPercentage = activeData.currentHealth / bossData.maxHealth;
        Debug.Log("HP: " + bossHealthPercentage + "CURR: " + activeData.currentHealth + " MAX: " + bossData.maxHealth);

        if (bossHealthPercentage <= attackPhaseData[activeData.BossPhase].healthPercentage)
        {
            Debug.Log("NEXT PHASE");
            activeData.BossPhase++;
        }

        if (activeData.currentHealth == 0)
            isBossActive = false;
    }

    int debugAttackInt = 0;

    private void SelectAttackPhase()
    {
        Debug.Log("SELECTED ATTACK");
        //randomize the attack 
        int attackListCount = attackPhaseData[activeData.BossPhase]._atks.Count;
        int randomAttackIndex = Random.Range(0, attackListCount);
        shouldRandomizeAttack = false;

        //activeBossAttack = attackPhaseData[activeData.BossPhase]._atks[randomAttackIndex];
        activeBossAttack = attackPhaseData[activeData.BossPhase]._atks[debugAttackInt];

        if (debugAttackInt < attackListCount - 1)
            debugAttackInt++;
        else
            debugAttackInt = 0;

    }

    private IEnumerator AttackDurationCoroutine()
    {
        //shld prob set a attack duration somewhere here 
        yield return new WaitForSeconds(5f);
        Debug.Log("RANDOMIZE ATTACK AGAIN");
        shouldRandomizeAttack = true;

    }


    public void TakeDamage(float damage)
    {
        activeData.currentHealth -= damage;
        StartCoroutine(TakeDamageEffect());
        if (AudioManager.instance != null) AudioManager.instance.Play("BossTakeDamage");
        if (BattleUIManager.Instance != null && bossData != null)
        {
            BattleUIManager.Instance.bossHealthSlider.value = activeData.currentHealth / bossData.maxHealth;
        }
    }

    private IEnumerator TakeDamageEffect()
    {
        // Set to damage color instantly
        objectRenderer.material.color = damageColor;
        // Gradually transition back to the original color over time
        float elapsedTime = 0f;
        while (elapsedTime < damageEffectDuration)
        {
            objectRenderer.material.color = Color.Lerp(damageColor,
            originalColor, elapsedTime / damageEffectDuration);
            elapsedTime += Time.deltaTime;
            yield return null;

            //Color lerpDamageColor = Color.Lerp(damageColor,
            //originalColor, elapsedTime / damageEffectDuration);
            //elapsedTime += Time.deltaTime;

            //objectRenderer.material.SetColor("_BaseColor", lerpDamageColor);
        }
        // Ensure the final color is reset to the original
        objectRenderer.material.color = originalColor;
    }

    public void HandleTriggerParticles(Vector3 hitPoint)
    {
        Debug.Log("ANIM: " + activeData.BAnimState);
        switch (activeData.BAnimState)
        {
            case BossActiveData.BossAnimStates.FLYSWATTER_ATTACK:
                Debug.Log("FLY SWATTER PARTICLE");
                activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_ELECTRICSPARK;
                activeData.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
                activeData.isObjectPoolTriggered = true;
                ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
                break;
            case BossActiveData.BossAnimStates.HANDSLAM_ATTACK:
                Debug.Log("HAND SLAM ATTACK");
                activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_WOODSPLINTER;
                activeData.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
                activeData.isObjectPoolTriggered = true;
                activeData.isObjectPoolTriggered = false;

                activeData.spawnableType = ObjectPoolManager.SPAWNABLE_TYPES.PARTICLE_DUSTSPLASH;
                activeData.objectPoolSpawnData = new ObjectPoolSpawnData(hitPoint, Vector3.up);
                activeData.isObjectPoolTriggered = true;
                ObjectPoolManager.Instance.HandleSpawnRequest(activeData);
                break;

        }
    }
}
