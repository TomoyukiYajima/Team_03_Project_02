using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IGeneralEvent
{
    public void onDamage(int amount)
    {
        Destroy(this.gameObject);
    }

    public void onLift(GameObject obj)
    {
        throw new NotImplementedException();
    }

    public void onShock()
    {
        throw new NotImplementedException();
    }

    public void onTakeDown()
    {
        throw new NotImplementedException();
    }

    public void onThrow()
    {
        throw new NotImplementedException();
    }
}
