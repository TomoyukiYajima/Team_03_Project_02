using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCheck : MonoBehaviour,IOrderEvent {
    [SerializeField] private StageAction m_action;
    [SerializeField] private GameObject m_image;
    [SerializeField] private GameObject m_takeDown;

    private bool m_isEnd;

    public void changeAnimation(UndroidAnimationStatus state)
    {
        throw new NotImplementedException();
    }

    public void endOrder(OrderNumber number)
    {
        throw new NotImplementedException();
    }

    public void onOrder(OrderStatus order)
    {
        if (m_isEnd) return;
        if (order == OrderStatus.MOVE)
        {
            m_isEnd = true;
            m_takeDown.SetActive(true);
            if (m_image != null) m_image.SetActive(true);
            StageManager.GetInstance().StartAction(m_action);

        }
    }

    public void onOrder(OrderStatus order, OrderDirection direction)
    {
        throw new NotImplementedException();
    }

    public void setObject(GameObject obj)
    {
        throw new NotImplementedException();
    }

    public void stopOrder()
    {
        throw new NotImplementedException();
    }

    public void stopOrder(OrderStatus order)
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
