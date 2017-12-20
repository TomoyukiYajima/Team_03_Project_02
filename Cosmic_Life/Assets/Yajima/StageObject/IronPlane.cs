using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronPlane : StageObject
{
    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    protected override void CollideEnter(Collision collision)
    {
        //base.CollideEnter();
        if (collision.transform.tag == "AttackColliderE")
        {
            SoundManager.Instance.PlaySe("SE_Bullet_Reflect");
            GameObject.Destroy(collision.gameObject);
        }
    }
}
