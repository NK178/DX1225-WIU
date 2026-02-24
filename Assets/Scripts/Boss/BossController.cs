using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klaus Phase 1: Mechanical Knife Attack & Hand Swipe Attack
// Ainsley Phase 2: Hand Slam, Fly Swatter Attack, Claw Grab, Sugarcane Missiles and Fruit Air Strike

[System.Serializable]
struct AttackPhaseData
{
    // Phase 1 2 3
    // Attack Scriptable Object
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


    //the actual phases
    private int waveIndex = 0;
    private bool shouldStartBoss = false;
    private bool shouldRandomizeAttack = false;

    private BossAttacks activeBossAttack; 

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
        shouldStartBoss = true;

        waveIndex = 0;
        activeData.BossPhase = 0;
        shouldRandomizeAttack = true;

        // Debug to check what phases have what attacks
        //for (int i = 0; i < attackPhaseData.Count; i++)
        //{
        //    for (int j = 0; j < attackPhaseData[i]._atks.Count; j++)
        //    {
        //        Debug.Log("Phase " + attackPhaseData[i].phaseNo + " : " + attackPhaseData[i]._atks[j].name);
        //    }
        //}
        //HandleAttack();
    }



    private void Update()
    {
        if (shouldStartBoss)
        {
            Debug.LogWarning("SHLD RANDOM: " + shouldRandomizeAttack);
            if (activeData.BossPhase == waveIndex && shouldRandomizeAttack)
            {
                SelectAttackPhase();
                StartCoroutine(AttackDurationCoroutine());
                activeBossAttack.ExecuteAttack(activeData);
            }

            if (activeBossAttack != null)
            {
                activeBossAttack.UpdateAttack(activeData);
            }


            if (HP <= 70 && activeData.BossPhase == 0)
            {
                activeData.BossPhase++;
            }
        }
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
    }

    private IEnumerator AttackDurationCoroutine()
    {
        //shld prob set a attack duration somewhere here 
        yield return new WaitForSeconds(5f);
        Debug.Log("RANDOMIZE ATTACK AGAIN");
        shouldRandomizeAttack = true;

        int attackListCount = attackPhaseData[activeData.BossPhase]._atks.Count;

        if (debugAttackInt - 1 != attackListCount)
            debugAttackInt++;
    }



    //private void Update()
    //{
    //    //for (int i = 0; i < attackPhaseData[0]._atks.Count; i++)
    //    //{
    //    //    //Debug.Log(attackPhaseData[0]._atks[i]);
    //    //}

    //    if (HP <= 70 && activeData.BossPhase == 0)
    //    {
    //        activeData.BossPhase++;
    //    }

    //    if (debugRunning) {

    //        //DEBUGAttackData.UpdateAttack(activeData);
    //        //DEBUGAttackData.ExecuteAttack(activeData);
    //    }
    //    //HandleAttack();

    //    attackPhaseData[activeData.BossPhase]._atks[0].UpdateAttack(activeData);
    //    attackPhaseData[activeData.BossPhase]._atks[1].UpdateAttack(activeData);
    //}


    public void HandleMove()
    {

        //Debug.Log((BossActiveData.BossAnimStates)animator.GetAnimState()); // Check what Anim it is at
    }

    public void HandleAttack()
    {
        if (!DebugEnableAttack)
            StartCoroutine(DebugAttacking());
    }

    private IEnumerator DebugAttacking()
    {
        DebugEnableAttack = true;
        //GameObject obj = attackColliders[Random.Range(0,attackColliders.Count)].obj;
        //Collider obj = attackColliders[Random.Range(0, attackColliders.Count)].obj;
        //Collider obj = attackColliders[Random.Range(0, attackColliders.Count)].obj;
        //attackPhaseData[activeData.BossPhase]._atks[Random.Range(0, attackPhaseData[activeData.BossPhase]._atks.Count)].ExecuteAttack(activeData);
        attackPhaseData[activeData.BossPhase]._atks[0].ExecuteAttack(activeData);
        attackPhaseData[activeData.BossPhase]._atks[1].ExecuteAttack(activeData);
        //EnableCollider(obj.name);
        yield return new WaitForSeconds(3.0f);
        //DisableCollider(obj.name);
        DebugEnableAttack = false;
    }

    public void TakeDamage(float damage)
    {
        activeData.currentHealth -= damage;
        StartCoroutine(TakeDamageEffect());
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
