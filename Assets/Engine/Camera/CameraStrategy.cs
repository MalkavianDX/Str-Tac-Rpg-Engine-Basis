using UnityEngine;
using System.Collections;

public class CameraStrategy : CameraTemplate {
    // Call variable for the camera component
    private Camera cam;
    // Camera Position
	public Vector3 camPos;
    // Call variable for focus point object
	public GameObject FocusPoint;

    // Maximum movement speed, Velocity & Zoom speed
	public float camMaxSpeed = 2.0f;
	public float cameraVelocity = 0.5f;
	public int zoomSpeed = 5;

    //Mouse boundary for mouse movement
    public int boundary = 15;

    //Variables for screen width & length
    public int width;
    public int height;

    //Boundaries for camera
	public float Min_Z = -500f;
	public float Min_X = -500f;
	public float Min_Y = -500f;
	public float Max_Z = 500f;
	public float Max_X = 500f;
	public float Max_Y = 500f;

    //Variable for counted movement vector
	Vector3 finalMove = new Vector3(0,0,0);
	
	// Use this for initialization
	void Start () {

        width = Screen.width;
        height = Screen.height;

		FocusPoint = CreateFocusPoint ();

		cam = gameObject.GetComponent<Camera> ();
		cam.orthographic = true;

		cam.backgroundColor = Color.green;
		cam.enabled = true;
		FocusPoint.transform.parent = cam.transform;

		if (cam == null)
			Debug.Log("Efff");



	}
	
	// Update is called once per frame
	void Update () {

        
    }

    // Last Update of the frame
	void LateUpdate () {
		// Get camera position
		camPos = cam.transform.position;

        //Mouse boundary movement
        finalMove += CameraMovementMouseBounds();

        //Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0)
			CameraZoom (scroll);

        //Keys to move camera Arrow keys or WASD keys, Call CameraMovement() to create movement vector
        //Add created vector to the Final movement vector
		if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0 ){
			finalMove += CameraMovement (0);
		}
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) {
			finalMove += CameraMovement (1);
		}
		
		//Rotation
		if (Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.E)) {
			CameraRotation();
		}

        //Orbit
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.C))
        {
            CameraOrbit();
        }
        
        //Exec Final Move
        Vector3 MoveIt = Vector3.zero;

        //Assign movement speed boundaries for the camera
		MoveIt = new Vector3 (
			Mathf.Clamp (finalMove.x, -camMaxSpeed, camMaxSpeed),
            Mathf.Clamp (finalMove.y, -camMaxSpeed, camMaxSpeed), 
			Mathf.Clamp (finalMove.z, -camMaxSpeed, camMaxSpeed));

        //Debug for Vector MoveIt, Not used due to restraining order
		//Debug.Log ("1: " + finalMove + ", 2: " + MoveIt);

        //Remove possible unintended z-axis movement
        transform.Translate(new Vector3(MoveIt.x, MoveIt.y,0));

        //The actual movement and clamping camera to boundary
        cam.transform.position = new Vector3(
            Mathf.Clamp(cam.transform.position.x, Min_X, Max_X),
            Mathf.Clamp(cam.transform.position.y, Min_Y, Max_Y),
            Mathf.Clamp(cam.transform.position.z, Min_Z, Max_Z));

        //Zero the movement vector
		finalMove = Vector3.zero;
	}

    //For saved camera locations, unused
	public override void CameraKeyPressed(){}

    // Check wasd or arrow keys forinput and move as such
	public override Vector3 CameraMovement(int k){

        //Arrow keys axis variables
        float mHorizontal;
        float mVertical;

        Vector3 move = new Vector3 (0, 0, 0);
        //Arrow Keys
		if (k == 0) {
            //Get axis input from arrow keys
			mHorizontal = Input.GetAxis ("Horizontal");
			mVertical = Input.GetAxis ("Vertical");

            //Add to return Vector
			move = new Vector3 (mHorizontal, mVertical, 0.0f);

		} //WASD keys
        else {
			if (Input.GetKey (KeyCode.W))
				move += (transform.forward * cameraVelocity * Time.deltaTime);
			if (Input.GetKey (KeyCode.A))
				move += (transform.right * cameraVelocity * Time.deltaTime);
			if (Input.GetKey (KeyCode.D))
				move += (-transform.right * cameraVelocity * Time.deltaTime);
			if (Input.GetKey (KeyCode.S))
				move += (-transform.forward * cameraVelocity * Time.deltaTime);
		}


        //return final vector
		return move;
	}

    // Mouse boundaries for camera movement
    public Vector3 CameraMovementMouseBounds()
    {
        // Zero the movement vector
        Vector3 movement = new Vector3(0,0,0);

        // Check boundaries, right side
        if (Input.mousePosition.x > width - boundary)
        {
            movement += transform.right * Time.deltaTime * (25 * cameraVelocity);
        }

        // Check boundaries, left side
        if (Input.mousePosition.x < boundary)
        {
            movement += -transform.right * Time.deltaTime * (25 * cameraVelocity);
        }

        // Check boundaries, up
        if (Input.mousePosition.y > height - boundary)
        {
            movement += -transform.forward * Time.deltaTime * (25 * cameraVelocity);
        }

        // Check boundaries, below
        if (Input.mousePosition.y < boundary)
        {
            movement += transform.forward * Time.deltaTime * (25 * cameraVelocity);
        }
        // return final movement vector
        return movement;
    }

    //Camera Rotation Method, returns movement vector only activated if q key or e key was pressed
	public override Vector3 CameraRotation(){
        //Temp move vector
        Vector3 move = new Vector3 (0, 0, 0);
        //Check if key pressed was Q otherwise it was E, Add movement to vector
		if (Input.GetKey (KeyCode.Q))
			cam.transform.RotateAround (FocusPoint.transform.position, Vector3.down, 200 * cameraVelocity * Time.deltaTime);
		else
			cam.transform.RotateAround (FocusPoint.transform.position, Vector3.up, 200 * cameraVelocity * Time.deltaTime);
        // Return movement vector
		return move;
	}

    // Camera orbit, turns camera around its own axle
	public override Vector3 CameraOrbit(){
        //Temp move vector
        Vector3 move = new Vector3 (0, 0, 0);
        //Check if key pressed was Z otherwise it was C, Add movement to vector
        if (Input.GetKey(KeyCode.Z))
            cam.transform.RotateAround(cam.transform.position, Vector3.down, 200 * cameraVelocity * Time.deltaTime);
        else
            cam.transform.RotateAround(cam.transform.position, Vector3.up, 200 * cameraVelocity * Time.deltaTime);
        // Return movement vector
        return move;
	}

    // Camera Zoom
	public override Vector3 CameraZoom(float scroll){

        // Check zcroll and zoom as instructed
        cam.orthographicSize += (scroll * 5);

        // Returns nothing, Remains for possible future use
		return new Vector3 (0, 0, 0);
	}

    // Creates an empty object as a camera focal point
	public override GameObject CreateFocusPoint ()
	{
		GameObject Focus = new GameObject ();
		Focus.transform.position = Vector3.zero;
		return Focus;
	}


}
