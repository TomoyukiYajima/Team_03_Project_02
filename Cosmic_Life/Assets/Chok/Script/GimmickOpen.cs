using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GimmickOpen : GimmickBase{

    [SerializeField] private Transform[] m_doors;
    [SerializeField] private Transform[] m_positions;
    private List<Vector3> m_initials = new List<Vector3>();

	// Use this for initialization
	void Start () {
        foreach(var obj in m_doors)
        {
            m_initials.Add(obj.position);
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public override void onActivate()
    {
        for(int i = 0; i < m_doors.Length; ++i)
        {
            m_doors[i].DOMove(m_positions[i].position, 2.0f);
        }
    }

    public override void onReset()
    {
        for (int i = 0; i < m_doors.Length; ++i)
        {
            m_doors[i].position=m_initials[i];
        }
    }
}
