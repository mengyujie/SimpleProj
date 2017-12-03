using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Frame;

public class GameFacade : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Init();
	}
	void Init()
    {
        ManagerCenter.instance.Init(this);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
