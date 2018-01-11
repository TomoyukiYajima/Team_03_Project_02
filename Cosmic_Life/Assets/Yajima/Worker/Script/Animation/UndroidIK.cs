using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndroidIK : MonoBehaviour {

    // アニメーター
    private Animator m_Animator;
    // IKを設定するのか
    private bool m_IsIK = false;

    // 左手のIKポイント
    private Transform m_LeftHandPoint;
    // 右手のIKポイント
    private Transform m_RightHandPoint;

    // Use this for initialization
    void Start () {
        m_Animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // IK
    public void OnAnimatorIK(int layerIndex)
    {
        //print("IKの設定");

        // IKの設定
        if (m_IsIK)
        {
            // 左手
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            m_Animator.SetIKPosition(AvatarIKGoal.LeftHand, m_LeftHandPoint.position);
            m_Animator.SetIKRotation(AvatarIKGoal.LeftHand, m_LeftHandPoint.rotation);
            // 右手
            m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            m_Animator.SetIKPosition(AvatarIKGoal.RightHand, m_RightHandPoint.position);
            m_Animator.SetIKRotation(AvatarIKGoal.RightHand, m_RightHandPoint.rotation);
        }
        else {
            // 左手
            m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            // 右手
            m_Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            m_Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            m_Animator.SetLookAtWeight(0);
        }
    }

    // IKの設定
    public void SetHandIK(Transform left, Transform right)
    {
        // 手のポイントの設定
        m_LeftHandPoint = left;
        m_RightHandPoint = right;
        m_IsIK = true;
    }

    // IKの初期化
    public void InitIK()
    {
        m_IsIK = false;
    }
}
