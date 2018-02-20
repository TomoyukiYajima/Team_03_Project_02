﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IOrderEvent : IEventSystemHandler {
    // 命令の変更
    void onOrder(OrderStatus order);
    // 命令の変更(方向指定)
    void onOrder(OrderStatus order, OrderDirection direction);
    // 命令の停止
    void stopOrder();
    // 命令の停止
    void stopOrder(OrderStatus order);
    // 命令の終了
    void endOrder(OrderNumber number, bool isStop = false);
    // 参照するオブジェクトの設定
    void setObject(GameObject obj);
    // アニメーションの変更
    void changeAnimation(UndroidAnimationStatus state);
}
