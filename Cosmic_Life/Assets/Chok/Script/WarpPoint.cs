using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour {
    [SerializeField] private StageAction action;
    [SerializeField] private GameObject m_image;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        if (m_image != null) m_image.SetActive(true);

        StageManager.GetInstance().StartAction(action);
    }


}
