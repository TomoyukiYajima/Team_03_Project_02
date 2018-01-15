using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChecker : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        // プレイヤーに衝突したら削除する
        if (other.tag != "Player") return;
        Destroy(gameObject);
    }
}
