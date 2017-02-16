using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class MouseExample : MouseTemplate {

    //Flags
    //Is left mousebutton Held
	bool flag_mousehold = false;
    //Are unit(s) to be moved
    bool flag_unitsmove = false;
    //Timer for 
    float timer_mouseclickorhold;

	Vector3 prevCursorPos;
	Rect selectionBox;

    public GameObject Controller;
    public GameObject selectHighlightPrefab;

    RaycastHit hit;

    List<UnitTemplate> ListObjects = new List<UnitTemplate>();
    List<UnitTemplate> selectedObjects = new List<UnitTemplate>();

    private Vector3 endPoint;


    // Use this for initialization
    void Start () {
        Controller = GameObject.FindGameObjectWithTag("GameController");
        selectHighlightPrefab = Resources.Load("selectHighlightPrefab") as GameObject;
    }

	// Update is called once per frame
	void Update () {

        

        if (Input.GetMouseButtonDown(0)) {
            
            ListObjects.Clear();
            selectedObjects.Clear();
            timer_mouseclickorhold = Time.time;
            prevCursorPos = Input.mousePosition;

            Debug.Log("Button Down");

            Physics.Raycast(Camera.main.ScreenPointToRay(prevCursorPos), out hit, Mathf.Infinity);
            
            flag_mousehold = true;

            foreach (var selectHigh in FindObjectsOfType<UnitTemplate>()) {
				if (selectHigh.selectHighlight != null) {
					Destroy (selectHigh.selectHighlight.gameObject);
					selectHigh.selectHighlight = null;
				}
			}
            /*if (hit.collider.gameObject != null)
            {
                Debug.Log("Logged hit: []");
                ListObjects.Add(hit.collider.gameObject.GetComponent<UnitTemplate>());
            }*/

		}
        else if (Input.GetMouseButtonDown(1))
        {
            
            prevCursorPos = Input.mousePosition;
            flag_mousehold = false;

            Physics.Raycast(Camera.main.ScreenPointToRay(prevCursorPos), out hit, Mathf.Infinity);

            //set a flag to indicate to move the gameobject
            flag_unitsmove = true;
            //save the click / tap position
            endPoint = hit.point;
            
            
            Debug.Log(endPoint);
        }

        // Was Left Mouse Button released as a click
        if (Input.GetMouseButtonUp(0) && ((Time.time - timer_mouseclickorhold) < 0.2))
        {
            flag_mousehold = false;
            //ListObjects.Clear();
            //selectedObjects.Clear();
            if (hit.collider.gameObject != null)
            {
                Debug.Log("That is a hit");
                ListObjects.Add(hit.collider.gameObject.GetComponent<UnitTemplate>());
                hit.collider.gameObject.GetComponent<UnitTemplate>().selectHighlight = Instantiate(selectHighlightPrefab) as GameObject;
                hit.collider.gameObject.GetComponent<UnitTemplate>().selectHighlight.transform.SetParent(hit.collider.gameObject.GetComponent<UnitTemplate>().transform, false);
                hit.collider.gameObject.GetComponent<UnitTemplate>().selectHighlight.transform.eulerAngles = new Vector3(90, 0, 0);
            }
            Debug.Log("ListObjects Count: " + ListObjects.Count);
            

        }

        if (flag_mousehold)
        {

            foreach (var selectableObject in FindObjectsOfType<UnitTemplate>())
            {

                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {

                    if (selectableObject.selectHighlight == null)
                    {

                        selectableObject.selectHighlight = Instantiate(selectHighlightPrefab) as GameObject;
                        selectableObject.selectHighlight.transform.SetParent(selectableObject.transform, false);
                        selectableObject.selectHighlight.transform.eulerAngles = new Vector3(90, 0, 0);
                        Debug.Log("Surround Selected");
                        selectedObjects.Add(selectableObject);
                        Debug.Log("ListObject added " + selectableObject.ToString());
                    }
                }
                else
                {
                    if (selectableObject.selectHighlight != null)
                    {
                        Destroy(selectableObject.selectHighlight.gameObject);
                        selectableObject.selectHighlight = null;
                        Debug.Log("How the fuck");
                    }
                }
            }
        }

        // Was Left mouse button released from hold
        if (Input.GetMouseButtonUp (0) && ((Time.time - timer_mouseclickorhold) > 0.2)) {
            Debug.Log("Button went Up");
            
            

            flag_mousehold = false;

            foreach (var selectableObject in selectedObjects) {
			    
                Debug.Log(" " + selectableObject.gameObject);
                ListObjects.Add (selectableObject);
                
                Debug.Log("List should fill");
               
			}
            Debug.Log("ListObjects Count: " + ListObjects.Count);
            Debug.Log("should hit dat booty");
			var sb = new StringBuilder ();
			sb.AppendLine (string.Format ("Selecting [{0}] Units", selectedObjects.Count));
			foreach (var selectedObject in selectedObjects)
				sb.AppendLine ("-> " + selectedObject.gameObject.name);
			Debug.Log (sb.ToString ());
            TurnUnitListToString(selectedObjects);



            selectedObjects.Clear();

        }

		

        //if (selectedObjects != null)
        //    Controller.GetComponent<SelectiveUI>().UpdateUI(selectedObjects);
        if (Input.GetMouseButtonUp(1))
        {
            
            Debug.Log("ListObjects Count: " + ListObjects.Count);
            if (ListObjects.Count != 0)
            {
                if (flag_unitsmove && !Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
                {
                    for (int i = 0; i < ListObjects.Count; i++)
                    {
                        ListObjects[i].UnitMove(endPoint);
                    }
                    
                }
                else if (flag_unitsmove && Mathf.Approximately(gameObject.transform.position.magnitude, endPoint.magnitude))
                {
                    flag_unitsmove = false;
                    Debug.Log("I am here");
                }
            }
        }

    }
    

	public void OnGUI()
	{

		// draw the selection box
		if (flag_mousehold)
		{
            // Create a rect from both mouse positions
            Debug.Log("Making the Box");
            selectionBox = Utils.GetScreenRect( prevCursorPos, Input.mousePosition );
			Utils.DrawScreenRect( selectionBox, new Color( 0.8f, 0.8f, 0.95f, 0.25f ) );
			Utils.DrawScreenRectBorder( selectionBox, 2, new Color( 0.8f, 0.8f, 0.95f) );
		}
       
	}


	public bool IsWithinSelectionBounds( GameObject gameObject )
	{
		if( !flag_mousehold )
			return false;
		
		Camera camera = Camera.main;
		Bounds viewportBounds =
			Utils.GetViewportBounds( camera, prevCursorPos, Input.mousePosition );

        Debug.Log("Oh viewport check: " + viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position)).ToString());

		return viewportBounds.Contains(
			camera.WorldToViewportPoint( gameObject.transform.position ) );
	}

    public void TurnUnitListToString(List<UnitTemplate> l)
    {
        var sb = new StringBuilder();
        Debug.Log("Getting Visited");
        if (l.Count != 0)
        {
            l.ForEach((UnitTemplate t)  =>
            {
                if (t.UnitTeam == 0)
                    Debug.Log("There is Team");                
                sb.AppendLine("W" + t.ToString() + ". ");

                Debug.Log("Oh fuck why no list");
                Debug.Log(sb.ToString());
            });
        }
        else
        {
            Debug.Log("Empty List");
        }
    }
};
