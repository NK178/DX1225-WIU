using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Klaus
/// <summary>
/// For Player, NPC Enemies, Boss Colliders
/// Enable and Disable Collider
/// Handling which collider does what
/// </summary>

[System.Serializable]
public struct NamingCollider
{
    public Collider obj;
    public string name;
}

public class AttackHandler : MonoBehaviour
{
    public List<NamingCollider> attackColliders;

    private enum ColliderType
    {
        Player = 0,
        NPC,
        Boss,
        Environment,
    }
    [SerializeField] private ColliderType colliderType;

    [Header("Debugging")]
    private bool DebugEnableAttack = false;
    private Transform DebugColliders;

    private void Start()
    {
        //DebugColliders = transform;
    }

    public void EnableCollider(string atkName)
    {
        Debug.Log("ENABLING " + atkName);
        for (int i = 0; i < attackColliders.Count; i++)
        {
            if (attackColliders[i].name == atkName)
            {
                attackColliders[i].obj.enabled = true;
                return;
            }
        }
    }

    public void DisableCollider(string atkName)
    {
        Debug.Log("DISABLING " + atkName);
        for (int i = 0; i < attackColliders.Count; i++)
        {
            if (attackColliders[i].name == atkName)
            {
                attackColliders[i].obj.enabled = false;
                return;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            Collider detectors = attackColliders[i].obj;

            if (!detectors.enabled) continue;

            DebugColliders = detectors.transform;
            Collider[] hitColliders = Physics.OverlapBox(detectors.transform.position, detectors.transform.localScale / 2);
            for (int j = 0; j < hitColliders.Length; j++)
            {
                // Boss hits Player
                if (hitColliders[j].TryGetComponent<PlayerController>(out var player) && (colliderType == ColliderType.Boss))
                {
                    Debug.Log(detectors.name + " HIT PLAYER");
                    BossController tempBoss = gameObject.GetComponentInParent<BossController>();

                    if (tempBoss != null)
                    {
                        player.TakeDamage(tempBoss.ActiveData.currentAttack);
                    }
                    else Debug.LogError("A Fake Boss hit Player?");

                    DisableCollider(detectors.name);

                    Vector3 contactPoint = hitColliders[j].ClosestPoint(detectors.transform.position);
                    if (tempBoss != null) tempBoss.HandleTriggerParticles(contactPoint);
                    continue;
                }
                // Enemy hits Player
                else if (hitColliders[j].TryGetComponent<PlayerController>(out var n_player) && (colliderType == ColliderType.NPC))
                {
                    Debug.Log(detectors.name + " HIT PLAYER");
                    EnemyController tempNPC = gameObject.GetComponentInParent<EnemyController>();

                    if (tempNPC != null)
                        n_player.TakeDamage(tempNPC.ActiveData.currentAttack);
                    else Debug.LogError("A Fake Enemy hit Player?");

                    DisableCollider(detectors.name);
                    continue;
                }
                // Player hits Boss
                else if (hitColliders[j].TryGetComponent<BossController>(out var Boss) && colliderType == ColliderType.Player)
                {
                    Debug.Log(detectors.name + " HIT BOSS");
                    PlayerController tempPlayer = gameObject.GetComponentInParent<PlayerController>();

                    if (tempPlayer != null)
                    {
                        float calculatedDamage = tempPlayer.ActiveData.currentAttack * tempPlayer.ActiveData.currentDamageMultiplier;
                        Boss.TakeDamage(calculatedDamage);

                        if (BattleUIManager.Instance != null)
                            BattleUIManager.Instance.AddDamage(tempPlayer.ActiveData.currentClassType, calculatedDamage);
                    }
                    else Debug.LogError("A Fake Player hit Boss?");

                    DisableCollider(detectors.name);

                    if (CineMachineImpulseMan.Instance != null)
                    {
                        CineMachineImpulseMan.Instance.GenerateEffect(EFFECT.CAMSHAKE);
                        Debug.Log("Hit Shake Screen");
                    }
                    continue;
                }
                // Player hits Enemy
                else if (hitColliders[j].TryGetComponent<EnemyController>(out var Enemy) && colliderType == ColliderType.Player)
                {
                    Debug.Log(detectors.name + " HIT ENEMY");
                    PlayerController tempPlayer = gameObject.GetComponentInParent<PlayerController>();

                    if (tempPlayer != null)
                    {
                        float calculatedDamage = tempPlayer.ActiveData.currentAttack * tempPlayer.ActiveData.currentDamageMultiplier;
                        Enemy.TakeDamage(calculatedDamage);

                        if (BattleUIManager.Instance != null)
                            BattleUIManager.Instance.AddDamage(tempPlayer.ActiveData.currentClassType, calculatedDamage);
                    }
                    else Debug.LogError("A Fake Player hit Enemy?");

                    DisableCollider(detectors.name);
                    continue;
                }
                // Player hits Spawner
                else if (hitColliders[j].TryGetComponent<EnemySpawner>(out var enemySpawner) && colliderType == ColliderType.Player)
                {
                    Debug.Log(detectors.name + " HIT SPAWNER");
                    PlayerController tempPlayer = gameObject.GetComponentInParent<PlayerController>();

                    if (tempPlayer != null)
                    {
                        enemySpawner.TakeDamage(tempPlayer.ActiveData.currentAttack * tempPlayer.ActiveData.currentDamageMultiplier);
                    }

                    DisableCollider(detectors.name);
                    continue;
                }
                // Player hits Dummy
                else if (hitColliders[j].TryGetComponent<DummyController>(out var dummy) && colliderType == ColliderType.Player)
                {
                    Debug.Log(detectors.name + " HIT DUMMY");
                    PlayerController tempPlayer = gameObject.GetComponentInParent<PlayerController>();

                    if (tempPlayer != null)
                    {
                        dummy.TakeDamage(tempPlayer.ActiveData.currentAttack * tempPlayer.ActiveData.currentDamageMultiplier);
                    }

                    DisableCollider(detectors.name);
                    continue;
                }
                // Environment Interactions
                else if (hitColliders[j].CompareTag("Environment") && colliderType == ColliderType.Environment)
                {
                    Vector3 contactPoint = hitColliders[j].ClosestPoint(detectors.transform.position);
                    Debug.Log(detectors.name + " HIT ENVIRONMENT");
                    DisableCollider(detectors.name);

                    BossController boss = detectors.GetComponentInParent<BossController>();
                    if (boss != null) boss.HandleTriggerParticles(contactPoint);
                    continue;
                }
                else if (hitColliders[j].TryGetComponent<PlayerController>(out var Player) && colliderType == ColliderType.Environment)
                {
                    // Logic here
                }
                else if (hitColliders[j].TryGetComponent<PlayerController>(out var NPC) && colliderType == ColliderType.Environment)
                {
                    // Logic here
                }
            }
        }
    }

    private IEnumerator DebugEnableCollider()
    {
        DebugEnableAttack = true;
        Collider obj = attackColliders[Random.Range(0, attackColliders.Count)].obj;
        EnableCollider(obj.name);
        yield return new WaitForSeconds(3.0f);
        DisableCollider(obj.name);
        DebugEnableAttack = false;
    }
}