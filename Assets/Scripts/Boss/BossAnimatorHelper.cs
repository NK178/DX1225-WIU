using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BossAnimatorHelper : MonoBehaviour
{
    [SerializeField] private Transform handTip;
    [SerializeField] Transform knifeHandTarget;
    [SerializeField] private TwoBoneIKConstraint knifeHandConstraint;
    [SerializeField] private DataHolder dataHolder;

    private bool IKEnabled = false;
    private BossActiveData activeData;

    private float rotationForKnife = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activeData = (BossActiveData)dataHolder.activeData;

    }

    // Update is called once per frame
    void Update()
    {
        if (IKEnabled)
        {
            knifeHandConstraint.weight = 1f;
            knifeHandTarget.position = activeData.knifeHitPosition; 
            Debug.Log("DATA POS: " + knifeHandTarget.position + " ACTUAL: " + knifeHandConstraint.data.target.position);

        }
        else
        {
            knifeHandConstraint.weight = 0f;
        }
    }


    public void ToggleIK(int condition)
    {
        if (condition == 1)
        {
            IKEnabled = true;
            
            //if (activeData.BAnimState == BossActiveData.BossAnimStates.KNIFE_ATTACK)
            //{
            //    Debug.Log("ROTATE KNIFE");
            //    Vector3 directionVector = (handTip.transform.position - knifeHandTarget.position).normalized;
            //    knifeHandTarget.rotation = Quaternion.LookRotation(directionVector);
            //}

        }
        else if (condition == 0)
            IKEnabled = false;
        Debug.Log("TOGGLE IK: " + IKEnabled);
    }


    public void SetAnimBackIdle()
    {
        activeData.BAnimState = BossActiveData.BossAnimStates.IDLE;
        activeData.isAttacking = false;
    }
}
