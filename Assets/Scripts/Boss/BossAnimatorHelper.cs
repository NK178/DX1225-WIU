using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BossAnimatorHelper : MonoBehaviour
{
    [SerializeField] private Transform handTip;
    [SerializeField] Transform knifeHandTarget;
    [SerializeField] Transform leftHandTarget;
    [SerializeField] Transform rightHandTarget;

    [SerializeField] private ChainIKConstraint knifeHandConstraint;

    [SerializeField] private TwoBoneIKConstraint leftHandConstraint;
    [SerializeField] private TwoBoneIKConstraint rightHandConstraint;

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
            if (activeData.activeIKType == BossActiveData.IKTYPE.IK_KNIFE)
            {
                knifeHandConstraint.weight = 1f;
                knifeHandTarget.position = activeData.knifeHitPosition;
                Debug.Log("DATA POS: " + knifeHandTarget.position + " ACTUAL: " + knifeHandConstraint.data.target.position);
            }
            else if (activeData.activeIKType == BossActiveData.IKTYPE.IK_HANDSLAMS)
            {
                leftHandConstraint.weight = 1f;
                //rightHandConstraint.weight = 1f;
                leftHandTarget.position = activeData.leftHitPosition;
                //rightHandTarget.position = activeData.rightHitPosition;
            }
        }
        else
        {
            knifeHandConstraint.weight = 0f;
            leftHandConstraint.weight = 0f;
            //rightHandConstraint.weight = 0f;

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


    public void ToggleLeftHand(int condition)
    {
        AttackHandler atkHandler = GameObject.FindWithTag("Boss").GetComponent<AttackHandler>();
        if (atkHandler != null)
        {
            if (condition == 1)
                atkHandler.EnableCollider("LeftHandCollider");
            if (condition == 0)
                atkHandler.DisableCollider("LeftHandCollider");
        }
    }


    public void ToggleRightHand(int condition)
    {
        AttackHandler atkHandler = GameObject.FindWithTag("Boss").GetComponent<AttackHandler>();
        if (atkHandler != null)
        {
            if (condition == 1)
                atkHandler.EnableCollider("RightHandCollider");
            if (condition == 0)
                atkHandler.DisableCollider("RightHandCollider");
        }
    }

}
