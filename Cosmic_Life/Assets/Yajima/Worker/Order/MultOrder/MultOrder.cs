using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultOrder : Order {
    // 命令格納配列
    protected Dictionary<OrderStatus, Order> m_MultOrders =
    new Dictionary<OrderStatus, Order>();

    // Use this for initialization
    public override void Start () {
        base.Start();

        // 配列にデータを格納させる
        //AddOrder();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}

    // 命令リストの追加
    protected virtual void AddOrder() { }
}
