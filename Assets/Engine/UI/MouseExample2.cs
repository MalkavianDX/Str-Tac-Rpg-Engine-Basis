using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

public class MouseExample2 : MouseTemplate {

	bool click = false;
	bool hold = false;
	Vector3 prevCursorPos;
	Rect selectionBox;
	public GameObject selectionHighlightPrefab;
	RaycastHit hit;
	MapRulesExample1 mapR;
	List<UnitTemplate> UnitL;

	// Use this for initialization
	void Start () {
		mapR = (MapRulesExample1) GameObject.FindGameObjectWithTag("GameController").GetComponent(typeof(MapRulesExample1));
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		if (Input.GetMouseButtonDown (0)) {
			click = true;
			prevCursorPos = Input.mousePosition;
			hold = true;
			
			Physics.Raycast (Camera.main.ScreenPointToRay (prevCursorPos), out hit, Mathf.Infinity);

			if (hit.collider.gameObject.layer == 10) {
				mapR.EmptyUnitList ();
				//UnitL = mapR.GetUnitList();
				//UnitL.ForEach((t) => {gameObject.GetComponent<UnitTemplate>().UnitMove(hit.point);});
			} else if (hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 8) {
				foreach (var selectHigh in FindObjectsOfType<UnitTemplate>()) {
					if (selectHigh.selectHighlight != null) {
						Destroy (selectHigh.selectHighlight.gameObject);
						selectHigh.selectHighlight = null;
					}
				}

			}
		}
		if (Input.GetMouseButtonDown (1)) {
			if (hit.collider.gameObject.layer == 10) {
				UnitL = mapR.GetUnitList();
				UnitL.ForEach((t) => {gameObject.GetComponent<UnitTemplate>().UnitMove(hit.point);});
			} else if (hit.collider.gameObject.layer == 9 || hit.collider.gameObject.layer == 8) {
				UnitL = mapR.GetUnitList();
				if(UnitL != null){
                    if(UnitL[0].UnitTeam != hit.collider.gameObject.GetComponent<UnitTemplate>().UnitTeam)
                        UnitL.ForEach((t) => { gameObject.GetComponent<UnitTemplate>().UnitShoot(); });
				}
			}
		}

		// Left mouse button release
		if (Input.GetMouseButtonUp (0)) {
			
			var selectedObjects = new List<UnitTemplate> ();
			hold = false;
			foreach (var selectableObject in FindObjectsOfType<UnitTemplate>()) {
				if (IsWithinSelectionBounds (selectableObject.gameObject)) {
					selectedObjects.Add (selectableObject);
				}
				
				/*if (hit.collider.gameObject != null && click)
                {
                    selectedObjects.Add(hit.collider.gameObject.GetComponent<UnitTemplate>());
                    click = false;
                }*/
			}
			if (click && hit.collider.gameObject != null) {
				hit.collider.gameObject.GetComponent<UnitTemplate> ().selectHighlight = Instantiate (selectionHighlightPrefab);
				hit.collider.gameObject.GetComponent<UnitTemplate> ().selectHighlight.transform.SetParent (hit.collider.gameObject.GetComponent<UnitTemplate> ().transform, false);
				hit.collider.gameObject.GetComponent<UnitTemplate> ().selectHighlight.transform.eulerAngles = new Vector3 (90, 0, 0);
			}
			
			var sb = new StringBuilder ();
			sb.AppendLine (string.Format ("Selecting [{0}] Units", selectedObjects.Count));
			foreach (var selectedObject in selectedObjects)
				sb.AppendLine ("-> " + selectedObject.gameObject.name);
			Debug.Log (sb.ToString ());
			
			
			
			
		}
		
		if (hold) {
			foreach (var selectableObject in FindObjectsOfType<UnitTemplate>()) {
				
				if (IsWithinSelectionBounds (selectableObject.gameObject)) {
					if (selectableObject.selectHighlight == null) {
						selectableObject.selectHighlight = Instantiate (selectionHighlightPrefab);
						selectableObject.selectHighlight.transform.SetParent (selectableObject.transform, false);
						selectableObject.selectHighlight.transform.eulerAngles = new Vector3 (90, 0, 0);
					}
				} else {
					if (selectableObject.selectHighlight != null) {
						Destroy (selectableObject.selectHighlight.gameObject);
						selectableObject.selectHighlight = null;
					}
				}
			}
		} 
	}
	
	
	



	public void OnGUI()
	{
			
		// draw the selection box
		if (hold)
		{
			// Create a rect from both mouse positions
			selectionBox = Utils.GetScreenRect( prevCursorPos, Input.mousePosition );
			Utils.DrawScreenRect( selectionBox, new Color( 0.8f, 0.8f, 0.95f, 0.25f ) );
			Utils.DrawScreenRectBorder( selectionBox, 2, new Color( 0.8f, 0.8f, 0.95f) );
		}
		
	}
	
	
	public bool IsWithinSelectionBounds( GameObject gameObject )
	{
		if( !hold )
			return false;
		
		Camera camera = Camera.main;
		Bounds viewportBounds =
			Utils.GetViewportBounds( camera, prevCursorPos, Input.mousePosition );
		
		return viewportBounds.Contains(
			camera.WorldToViewportPoint( gameObject.transform.position ) );
	}
	
};
