using UnityEngine;
using System.Collections;

public class GenerateControllers : MonoBehaviour {



    // Use this for initialization
    void Start () {
        // Create Game Controller
        GameObject Controllers = GameObject.FindGameObjectWithTag("GameController");

        RectTransform rt = Controllers.AddComponent<RectTransform>();

        rt.sizeDelta = new Vector2(0,0);

        // Add CodeComponents
        Controllers.AddComponent<PlayerTemplate>();
        Controllers.AddComponent<MapRulesExample1>();
        Controllers.AddComponent<MouseExample>();
        Controllers.AddComponent<SelectiveUI>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
