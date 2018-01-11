using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKSample : MonoBehaviour {

    private bool m_IsIK = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnAnimatorIK(int layerIndex)
    {
        //print("IKの設定");

        // IKの設定
        if (m_IsIK)
        {
            //// 左手
            //m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            //m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            //m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftHandPoint.position);
            //m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, m_LeftHandPoint.rotation);
            //// 右手
            //m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            //m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            //m_Animator.SetIKPosition(AvatarIKGoal.RightHand, m_RightHandPoint.position);
            //m_Animator.SetIKRotation(AvatarIKGoal.RightHand, m_RightHandPoint.rotation);
        }
        else {
            //// 左手
            //m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            //m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            //// 右手
            //m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            //m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            //m_Animator.SetLookAtWeight(0);
        }
    }
}
