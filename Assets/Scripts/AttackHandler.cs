using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Klaus
// For Player, NPC Enemies, Boss Colliders
// Enable and Disable Collider
// Handling which collider does what

[System.Serializable]
public struct NamingCollider
{
    public GameObject obj;
    //public Collider obj;
    public string name;
}

public class AttackHandler : MonoBehaviour
{
    [SerializeField] private List<NamingCollider> attackColliders; // For now just set to GameObject. CHANGE TO COLLIDERS WHEN MODEL ARE HERE
    [SerializeField] private enum colliderType
    {
        Player = 0,
        NPC,
        Boss,
    }

    public void EnableCollider(string atkName)
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            if (attackColliders[i].obj.name == atkName)
            {
                attackColliders[i].obj.SetActive(true);
                return;
            }
            //if (attackColliders[i].name == atkName)
            //{
            //    attackColliders[i].obj.SetActive(true);
            //    return;
            //}
        }
    }

    public void DisableCollider(string atkName)
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            if (attackColliders[i].obj.name == atkName)
            {
                attackColliders[i].obj.SetActive(true);
                return;
            }
            //if (attackColliders[i].name == atkName)
            //{
            //    attackColliders[i].obj.SetActive(true);
            //    return;
            //}
        }
    }

    private void Update()
    {
        StartCoroutine(DebugEnableCollider());
        for (int i = 0; i < attackColliders.Count; i++)
        {
            GameObject detectors = attackColliders[i].obj;
            //Collider detectors = attackColliders[i].obj;

            if (!detectors.activeSelf) return;
            //if (!detectors.enabled) return;

            //Collider[] hitColliders = Physics.OverlapSphere(detector.transform.position, detector.radius, targetLayer);


            //for (int j = 0; j < hitColliders.Length; j++)
            //{
            //    HandleHit(hitColliders[j], detector);

            //}
        }
    }

    private IEnumerator DebugEnableCollider()
    {
        yield return new WaitForSeconds(3.0f);
        GameObject obj = attackColliders[Random.Range(0,attackColliders.Count)].obj;
        EnableCollider(obj.name);
        yield return new WaitForSeconds(3.0f);
        obj.SetActive(false);
    }
}
