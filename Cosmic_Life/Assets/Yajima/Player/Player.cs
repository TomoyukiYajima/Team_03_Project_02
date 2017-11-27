﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class Player : MonoBehaviour, IGeneralEvent
{
    [SerializeField, Tooltip("移動速度")] private float m_Speed;
    [SerializeField, Tooltip("攻撃判定")] private GameObject m_attackCollision;
    [SerializeField, Tooltip("攻撃生成位置")] private GameObject m_attackPos;

    [SerializeField] private PlayerStatus m_status = new PlayerStatus(3, 100);

    private Transform m_camera;
    private Animator m_animator;
    private Rigidbody m_rigidbody;
    private PlayerState m_state;
    private Vector3 m_velocity;
    private Vector3 m_groundNormal;

    private int m_maxHp;

    private bool m_isDamaged;
    private bool m_isCanWalk;
    private bool m_isGrounded;
    private float m_groundCheckDistance;
    private float m_origGroundCheckDistance;

    public delegate void OnCollide(int hp);
    public event OnCollide onCollide;

    // Use this for initialization
    void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_camera = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        m_animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_state = PlayerState.IDLE;
        m_velocity = Vector3.zero;
        m_isDamaged = false;
        m_isCanWalk = true;
        //ChangeState(Move());
        m_groundCheckDistance = 0.1f;
        m_origGroundCheckDistance = m_groundCheckDistance;

        m_maxHp = m_status.hp;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("X")) { onDamage(1); }

        //m_animator.SetFloat("Forward", m_velocity.z, 0.1f, Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (!m_isCanWalk) return;

        if (Input.GetButtonDown("OK"))
        {
            ChangeState(Attack());
            return;
        }

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        float hR = Input.GetAxis("HorizontalR");

        if (h < 0.0f) transform.FindChild("Model").transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - 90, 0);
        else if (h > 0.0f) transform.FindChild("Model").transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 90, 0);
        else if (h == 0.0f && v != 0.0f) transform.FindChild("Model").transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        Vector3 velocity = Vector3.zero;

        //velocity = new Vector3(x, 0, z);
        //if (m_camera != null)
        //{
        //var forward = Vector3.Scale(m_camera.forward, new Vector3(1, 0, 1)).normalized;
        velocity = transform.forward * v + transform.right * hR;
        //}

        if (velocity.magnitude > 1f) velocity.Normalize();
        velocity = transform.InverseTransformDirection(velocity);
        CheckGroundStatus();
        velocity = Vector3.ProjectOnPlane(velocity, m_groundNormal);

        float turnZ = Mathf.Clamp(velocity.z, 0.1f, 1.0f);

        float m_TurnAmount = Mathf.Atan2(velocity.x, turnZ);

        float turnSpeed = Mathf.Lerp(180.0f, 360.0f, velocity.z);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.fixedDeltaTime, 0);

        float factor = 1.0f;
        if (v < 0.5f) factor = 0.3f;

        //Vector3 ve = (m_animator.deltaPosition * m_Speed) / Time.fixedDeltaTime;
        Vector3 ve = (transform.forward * v * factor + transform.right * h) * 2.5f;

        // we preserve the existing y part of the current velocity.
        ve.y = m_rigidbody.velocity.y;
        m_rigidbody.velocity = ve;

        if (m_isGrounded)
        {

            if (Input.GetButtonDown("Cancel"))
            {
                m_isGrounded = false;
                ve.y = 12f;
            }


            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0, m_rigidbody.velocity.z);

            m_isGrounded = false;
            m_groundCheckDistance = 0.1f;

            //transform.position += transform.right * h * m_Speed * 2.5f * Time.fixedDeltaTime;

            //if (v <= 0.0f)
            //{
            //    Vector3 vec = transform.forward * v * m_Speed * 2.5f * Time.fixedDeltaTime;
            //   m_rigidbody.velocity = new Vector3(0,0,vec.z);
            //}
        }
        else
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * 2f) - Physics.gravity;
            m_rigidbody.AddForce(extraGravityForce);

            m_groundCheckDistance = m_rigidbody.velocity.y < 0 ? m_origGroundCheckDistance : 0.01f;

        }

        m_animator.SetFloat("Speed", v, 0.1f, Time.fixedDeltaTime);
        m_animator.SetFloat("SideSpeed", h, 0.1f, Time.fixedDeltaTime);
    }

    private void UpdateState()
    {
        //switch (m_state)
        //{
        //    case PlayerState.IDLE:
        //        break;
        //    case PlayerState.ATTACK:
        //        Attack();
        //        break;
        //    default:
        //        break;
        //}
    }

    private void ChangeState(IEnumerator coroutine)
    {
        m_rigidbody.velocity = Vector3.zero;
        m_isCanWalk = false;
        m_animator.SetFloat("Speed", 0);
        StopAllCoroutines();
        StartCoroutine(coroutine);
    }

    private IEnumerator Move()
    {
        while (true)
        {
            // デルタタイムの取得
            float time = Time.deltaTime;

            m_velocity = Vector3.zero;

            if (Input.GetButtonDown("OK"))
            {
                yield return null;
            }

            //float angle = Input.GetAxis("HorizontalR");

            //transform.Rotate(Vector3.up, angle);

            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            Vector2 velocity = new Vector2(h, v);

            //velocity = new Vector3(x, 0, z);
            if (m_camera != null)
            {
                var forward = Vector3.Scale(m_camera.forward, new Vector3(1, 0, 1)).normalized;
                m_velocity = forward * velocity.y + m_camera.transform.right * velocity.x;
            }

            if (m_velocity.magnitude > 1f) m_velocity.Normalize();
            m_velocity = transform.InverseTransformDirection(m_velocity);

            float m_TurnAmount = Mathf.Atan2(m_velocity.x, m_velocity.z);

            float turnSpeed = Mathf.Lerp(180.0f, 360.0f, m_velocity.z);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

            Vector3 ve = (m_animator.deltaPosition * m_Speed) / Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            ve.y = m_rigidbody.velocity.y;
            m_rigidbody.velocity = ve;

            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0, m_rigidbody.velocity.z);

            //this.transform.position += m_velocity * m_Speed * time;

            m_animator.SetFloat("Forward", m_velocity.z, 0.1f, Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        m_animator.SetBool("Attack", true);

        yield return new WaitForEndOfFrame();
        // 攻撃コリジョン生成
        GameObject attack = Instantiate(m_attackCollision, m_attackPos.transform.position, m_attackPos.transform.rotation) as GameObject;
        DestroyObject(attack, 0.5f);
        // モーション変更

        yield return new WaitForSeconds(1.0f);

        EndState();
        yield return null;
    }

    private IEnumerator Lifted(GameObject crane)
    {
        while (true)
        {
            transform.position = crane.transform.position + (transform.FindChild("LiftPoint").transform.position - transform.position) * 2;
            if (Input.GetButtonDown("Cancel"))
            {
                EndState();
            }
            yield return null;
        }
    }


    private IEnumerator Damaged()
    {
        m_animator.SetTrigger("Damaged");

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(DamageWait());
        StartCoroutine(DamageInput());

        yield return new WaitWhile(() => m_isDamaged);

        EndState();

        yield return null;
    }

    private IEnumerator DamageInput()
    {
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        m_isDamaged = false;
    }

    private IEnumerator DamageWait()
    {
        yield return new WaitForSeconds(5.0f);
        m_isDamaged = false;
    }

    private IEnumerator Dead()
    {
        // DeadMotion

        // Wait 
        yield return new WaitForSeconds(1.0f);

        // GameOverUI
        SceneMgr.Instance.SceneTransition(SceneType.Title);
        yield return null;
    }

    public void onDamage(int amount)
    {
        m_status.hp = Mathf.Max(0, --m_status.hp);
        if (onCollide != null) onCollide(m_status.hp);

        m_velocity = Vector3.zero;

        StopAllCoroutines();
        m_isDamaged = true;

        IEnumerator nextState = m_status.hp > 0 ? Damaged() : Dead();

        ChangeState(nextState);
    }

    public void onLift(GameObject crane)
    {
        // モーション
        ChangeState(Lifted(crane));
    }

    public void onTakeDown()
    {
        EndState();
    }


    public void EndState()
    {
        StopAllCoroutines();
        m_animator.SetTrigger("MotionEnd");
        m_animator.SetBool("Attack", false);

        m_isCanWalk = true;
    }

    //public void OnAnimatorMove()
    //{
    //    Vector3 v = (m_animator.deltaPosition * m_Speed) / Time.fixedDeltaTime;

    //    // we preserve the existing y part of the current velocity.
    //    v.y = m_rigidbody.velocity.y;
    //    m_rigidbody.velocity = v;

    //    m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0, m_rigidbody.velocity.z);

    //}

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_groundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_groundCheckDistance))
        {
            m_groundNormal = hitInfo.normal;
            m_isGrounded = true;
            //m_animator.applyRootMotion = true;
        }
        else
        {
            m_isGrounded = false;
            m_groundNormal = Vector3.up;
            //m_animator.applyRootMotion = false;
        }
    }

    public void onShock()
    {
        throw new NotImplementedException();
    }

    public void onThrow()
    {
        throw new NotImplementedException();
    }
}
