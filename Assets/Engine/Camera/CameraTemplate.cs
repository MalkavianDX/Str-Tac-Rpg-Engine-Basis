using UnityEngine;
using System.Collections;

abstract public class CameraTemplate : MonoBehaviour{

    // variables that need to be
	public float cameraSpeed = 15;
	public float cameraZoomSpeed = 10;

    //Abstract method to create a focuspoint for the camera
    public abstract GameObject CreateFocusPoint();

    //Abstract methods
    public abstract void CameraKeyPressed ();

    //Abstract methods that move the camera
	public abstract Vector3 CameraMovement (int k);
	public abstract Vector3 CameraRotation ();
	public abstract Vector3 CameraOrbit ();
	public abstract Vector3 CameraZoom (float scroll);
	
}


