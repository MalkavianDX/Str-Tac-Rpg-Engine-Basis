using UnityEngine;
using System.Collections;

public abstract class UITemplate : MonoBehaviour {

    //Completely Abstract

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void LoadUI ();
	public abstract void UpdateUI ();
	public abstract void UsageUI ();
}
