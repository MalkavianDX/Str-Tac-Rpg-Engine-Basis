using UnityEngine;
using System.Collections;

public abstract class AITemplate : MonoBehaviour {

    // This File is extremely basic thought of an abstract, to be revamped on a later date

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public abstract void loadAI();
	public abstract void defenceAI();
	public abstract void offenceAI();
	public abstract void reconAI();
	public abstract void nodeControlAI();

}
