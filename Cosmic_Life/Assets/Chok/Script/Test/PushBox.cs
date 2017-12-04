using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class PushBox : MonoBehaviour,IGeneralEvent {
    [SerializeField] private int m_interactiveAngle = 90;
    [SerializeField] private GameObject m_interactText;

    private void Start()
    {
        if (m_interactText == null)
        {
            m_interactText = GameObject.Find("Canvas").transform.FindChild("InteractText").gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        // プレイヤーが正面向いているなら起動可能
        Vector3 directionFromPlayer = transform.position - other.transform.position;
        directionFromPlayer.Normalize();

        float angleFromPlayer = Vector3.Angle(other.transform.forward, directionFromPlayer);

        if (angleFromPlayer <= m_interactiveAngle)
        {
            other.GetComponent<Player>().LiftObject = this.gameObject;
            m_interactText.SetActive(true);
        }
        else
        {
            other.GetComponent<Player>().LiftObject = null;
            m_interactText.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        m_interactText.SetActive(false);
        other.GetComponent<Player>().LiftObject = null;
    }

    private IEnumerator Lifted(GameObject obj)
    {
        while (true)
        {
            Vector3 position = transform.position;
            transform.position = obj.transform.position + obj.transform.forward * 1.5f;
            transform.position = new Vector3(transform.position.x, position.y, transform.position.z);
            yield return null;
        }
    }

    public void onDamage(int amount)
    {
        throw new NotImplementedException();
    }

    public void onShock()
    {
        throw new NotImplementedException();
    }

    public void onThrow()
    {
        throw new NotImplementedException();
    }

    public void onLift(GameObject obj)
    {
        StartCoroutine(Lifted(obj));
    }

    public void onTakeDown()
    {
        StopAllCoroutines();
    }
}
