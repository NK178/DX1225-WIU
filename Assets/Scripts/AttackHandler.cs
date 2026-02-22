using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Klaus
// For Player, NPC Enemies, Boss Colliders
// Enable and Disable Collider
// Handling which collider does what

[System.Serializable]
public struct NamingCollider
{
    //public GameObject obj;
    public Collider obj;
    public string name;
}

public class AttackHandler : MonoBehaviour
{
    //[SerializeField] private List<NamingCollider> attackColliders; // For now just set to GameObject. CHANGE TO COLLIDERS WHEN MODEL ARE HERE
    public List<NamingCollider> attackColliders; // For now just set to GameObject. CHANGE TO COLLIDERS WHEN MODEL ARE HERE

    private enum ColliderType
    {
        Player = 0,
        NPC,
        Boss,
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
                //attackColliders[i].obj.SetActive(true);
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
                //attackColliders[i].obj.SetActive(false);
                attackColliders[i].obj.enabled = false;
                return;
            }
        }
    }

    // DISABLE ERROR PAUSE
    // Debugging tool to see if there the PhysicsOverlap is exists at that area
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawWireCube(DebugColliders.position, DebugColliders.localScale);   
    //}
    private void Update()
    {
        // For Testing if the attacks can be enabled
        //if (!DebugEnableAttack)
        //    StartCoroutine(DebugEnableCollider());
        for (int i = 0; i < attackColliders.Count; i++)
        {
            //GameObject detectors = attackColliders[i].obj;
            Collider detectors = attackColliders[i].obj;

            //if (!detectors.activeSelf) continue;
            if (!detectors.enabled) continue;

            //Collider[] hitColliders = Physics.OverlapSphere(detector.transform.position, detector.radius, targetLayer);
            DebugColliders = detectors.transform;
            Collider[] hitColliders = Physics.OverlapBox(detectors.transform.position, detectors.transform.localScale / 2);
            for (int j = 0; j < hitColliders.Length; j++)
            {
                if (hitColliders[j].TryGetComponent<PlayerController>(out var player) && (colliderType == ColliderType.Boss || colliderType == ColliderType.NPC))
                {
                    Debug.Log(detectors.name + " HIT PLAYER");
                    DisableCollider(detectors.name);

                    //handle stuff like particles and whatnot 
                    //detectors.GetComponent<BossController>().HandleTriggerParticles(hitColliders[j].gameObject);
                    continue;
                }
                else if (hitColliders[j].TryGetComponent<BossController>(out var Boss) &&  colliderType == ColliderType.Player)
                {
                    Debug.Log(detectors.name + " HIT BOSS");
                    DisableCollider(detectors.name);
                    continue;
                }
                else if (hitColliders[j].TryGetComponent<EnemyController>(out var Enemy) && colliderType == ColliderType.Player)
                {
                    Debug.Log(detectors.name + " HIT ENEMY");
                    DisableCollider(detectors.name);
                    continue;
                }

                //need one for environment
            }


            //for (int j = 0; j < hitColliders.Length; j++)
            //{
            //    HandleHit(hitColliders[j], detector);

            //}
        }
        //if (DebugEnableAttack) return;
    }

    private IEnumerator DebugEnableCollider()
    {
        DebugEnableAttack = true;
        //GameObject obj = attackColliders[Random.Range(0,attackColliders.Count)].obj;
        Collider obj = attackColliders[Random.Range(0,attackColliders.Count)].obj;
        EnableCollider(obj.name);
        yield return new WaitForSeconds(3.0f);
        DisableCollider(obj.name);
        DebugEnableAttack = false;
    }
}
