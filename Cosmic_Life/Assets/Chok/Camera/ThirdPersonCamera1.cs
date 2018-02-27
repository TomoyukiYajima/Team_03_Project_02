﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirdPersonCamera1 : MonoBehaviour
{
    [SerializeField, Tooltip("高さ調整")] private float m_height = 1.0f;
    [SerializeField, Tooltip("ターゲットとの距離")] private float m_distance = 2.0f;
    [SerializeField, Tooltip("感度")] private float m_sensitivity = 100.0f;
    [SerializeField, Tooltip("横ずらし")] private float m_slide = 0.0f;
    [SerializeField] private GameObject m_lockOnUI;

    [SerializeField] private Image[] m_images;

    private GameObject m_player;
    private GameObject m_target;
    private bool m_isLockOn;

    private CameraRay m_cameraRay;

    private Transform m_parent;

    // Use this for initialization
    void Start()
    {
        m_parent = transform.parent;
        m_player = GameObject.FindGameObjectWithTag("Player");
        // ターゲットをプレイヤーに設定します
        m_target = m_player;



        m_isLockOn = false;

        m_cameraRay = m_parent.GetComponent<CameraRay>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(m_player==null) m_player = GameObject.FindGameObjectWithTag("Player");

        if (!m_isLockOn)
        {
            // コントローラー右スティックで回転
            var y = Input.GetAxis("VerticalR") * Time.deltaTime * m_sensitivity;
            var x = Input.GetAxis("HorizontalR") * Time.deltaTime * m_sensitivity;

            if (m_cameraRay.CollideObj != null)
            {
                if (Input.GetButtonDown("Triggrt_Left"))
                {
                    m_isLockOn = true;
                    m_lockOnUI.SetActive(true);
                    m_target = m_cameraRay.CollideObj;
                    m_lockOnUI.GetComponent<LockOnUi>().m_Target = m_target;

                    foreach(var m  in m_images)
                    {
                        m.enabled = false;
                    }
                    return;
                }
            }

            // 中心点を設定します
            var lookAt = m_player.transform.position + Vector3.up * m_height;

            // ターゲットの
            m_parent.RotateAround(lookAt, Vector3.up, x);

            if (transform.forward.y > 0.3f && y < 0.0f) y = 0.0f;
            if (transform.forward.y < -0.5f && y > 0.0f) y = 0.0f;

            m_parent.RotateAround(lookAt, transform.right, y);

            //Vector3 dir = lookAt - transform.position;
            //dir.y = 0;
            //dir.Normalize();

            Vector3 target = lookAt - m_parent.forward * m_distance;

            RaycastHit wallHit = new RaycastHit();


            Vector3 target2 = target;
            if (Physics.Linecast(lookAt, target, out wallHit, 1 << 8))
            {
                #region Unused
                //Vector3 euler = transform.eulerAngles;
                //transform.eulerAngles = new Vector3(0, euler.y, euler.z);
                //if (Physics.Linecast(lookAt, target, out wallHit, 1 << 8))
                //{
                //    target = new Vector3(wallHit.point.x, target.y, wallHit.point.z);
                //}
                #endregion
                target = new Vector3(wallHit.point.x, transform.position.y, wallHit.point.z);

                if(Physics.Linecast(lookAt,target2,out wallHit, 1 << 8))
                {
                    target2 += m_parent.forward;
                }


                #region Unused
                //if (degree > 140.0f)
                //{
                //    m_slide = 0.6f;
                //}
                //else 
                //{
                //    m_slide = -0.6f;
                //}
                #endregion
            }

            //var dir = m_player.transform.position - target;
            //dir.y = 0;
            //dir.Normalize();
            //target = target + dir * 0.2f;
            m_parent.position = target2;

            m_parent.LookAt(lookAt);

            var dir = m_player.transform.position - target;
            dir.y = 0;
            dir.Normalize();
            target = target + dir * 0.2f;

            m_parent.position = target + m_parent.right * m_slide;

            #region Unused
            //transform.Rotate(y, x, 0);

            //transform.position = m_target.transform.position - transform.forward * 2.0f;
            //transform.Translate(0, 1.0f, 0);

            //Ray ray = new Ray(transform.position, m_target.transform.position - transform.position);
            //RaycastHit hitInfo;

            //Ray rayBack = new Ray(transform.position, transform.position - m_target.transform.position);
            //Physics.Raycast(rayBack, out hitInfo, 1.25f);
            //if (hitInfo.collider == null)
            //{
            //    m_distance += 0.1f;
            //    m_distance = Mathf.Min(m_distance, 1.25f);
            //}


            //Physics.Raycast(ray, out hitInfo, 10.0f);

            //if (hitInfo.collider.tag != "Player")
            //{
            //    m_distance -= 0.1f;
            //}


            //if(Physics.Raycast(transform.position,m_target.transform.position - transform.position,out hitInfo, (m_target.transform.position - transform.position).magnitude,LayerMask.NameToLayer("WallLayer")))
            //{
            //    transform.position = (hitInfo.point - m_target.transform.position) * 0.8f + m_target.transform.position;
            //}
            #endregion
        }
        else
        {
            if (Input.GetButtonDown("Triggrt_Left") || m_target == null)
            {
                m_isLockOn = false;
                m_lockOnUI.SetActive(false);
                foreach (var m in m_images)
                {
                    m.enabled = true;
                }

                return;
            }
            // 中心点を設定します
            var lookAt = m_player.transform.position + Vector3.up * m_height;

            //m_lockOnUI.GetComponent<LockOnUi>().m_Target = m_target ;
            
            Vector3 playerTotarget = m_target.transform.position - m_player.transform.position;
            playerTotarget.y = 0;
            playerTotarget.Normalize();
            Vector3 target = lookAt - playerTotarget * m_distance;

            m_slide = 0.0f;

            RaycastHit wallHit = new RaycastHit();
            if (Physics.Linecast(lookAt, target, out wallHit, 1 << 8))
            {
                target = new Vector3(wallHit.point.x, transform.position.y, wallHit.point.z);
            }
            m_parent.position = target;

            m_parent.LookAt(lookAt);

            var dir = m_player.transform.position - target;
            dir.y = 0;
            dir.Normalize();
            target = target + dir * 0.2f;

            m_parent.position = target + m_parent.right * m_slide;
        }
    }

}
