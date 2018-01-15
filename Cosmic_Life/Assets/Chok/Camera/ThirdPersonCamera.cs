using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField, Tooltip("高さ調整")] private float m_height = 1.0f;
    [SerializeField, Tooltip("ターゲットとの距離")] private float m_distance = 2.0f;
    [SerializeField, Tooltip("感度")] private float m_sensitivity = 100.0f;
    [SerializeField, Tooltip("横ずらし")] private float m_slide = 0.0f;

    private GameObject m_player;
    private GameObject m_target;
    private bool m_isLockOn;

    private CameraRay m_cameraRay;

    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        // ターゲットをプレイヤーに設定します
        m_target = m_player;



        m_isLockOn = false;

        m_cameraRay = GetComponent<CameraRay>();
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
                    m_target = m_cameraRay.CollideObj.transform.Find("CenterPoint").gameObject;
                    return;
                }
            }

            // 中心点を設定します
            var lookAt = m_player.transform.position + Vector3.up * m_height;

            // ターゲットの
            transform.RotateAround(lookAt, Vector3.up, x);

            if (transform.forward.y > 0.3f && y < 0.0f) y = 0.0f;
            if (transform.forward.y < -0.9f && y > 0.0f) y = 0.0f;

            transform.RotateAround(lookAt, transform.right, y);

            //Vector3 dir = lookAt - transform.position;
            //dir.y = 0;
            //dir.Normalize();

            Vector3 target = lookAt - transform.forward * m_distance;

            RaycastHit wallHit = new RaycastHit();
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

                //var direction = target - m_player.transform.position;
                ////float degree = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                ////if (degree < 0) degree += 360.0f;

                ////Debug.Log("Degree :" + degree);

                //var model = m_player.transform.Find("Model");

                //Vector3 perp = Vector3.Cross(model.transform.forward, direction);
                //float D = Vector3.Dot(perp, model.transform.up);

                //if (D < 0f)
                //{
                //    if (m_slide < 0.8f) m_slide += 0.4f;
                //}
                //else if (D > 0f)
                //{
                //    if (m_slide > -0.8f) m_slide -= 0.4f;
                //}
                //else
                //{
                //    if (m_slide > 0f) m_slide -= 0.4f;
                //    else if (m_slide < 0f) m_slide += 0.4f;
                //}

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
            transform.position = target;

            transform.LookAt(lookAt);

            var dir = m_player.transform.position - target;
            dir.y = 0;
            dir.Normalize();
            target = target + dir * 0.2f;

            transform.position = target + transform.right * m_slide;

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
            // 中心点を設定します
            var lookAt = m_player.transform.position + Vector3.up * m_height;

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
            transform.position = target;

            transform.LookAt(lookAt);

            var dir = m_player.transform.position - target;
            dir.y = 0;
            dir.Normalize();
            target = target + dir * 0.2f;

            transform.position = target + transform.right * m_slide;
        }
    }

}
