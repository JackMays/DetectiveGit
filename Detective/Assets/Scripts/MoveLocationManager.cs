using UnityEngine;
using System.Collections;

public class MoveLocationManager : MonoBehaviour {

	int assignedLocation = 0;
	SceneContent sceneContent;
	GameManager managerRef;

	// Use this for initialization
	void Awake () 
	{
		sceneContent = GameObject.FindGameObjectWithTag("SceneContent").GetComponent<SceneContent>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignMoveLocation(int loc, GameManager gRef)
	{
		assignedLocation = loc;
		managerRef = gRef;
	}

	public void LoadAssignedLocation()
	{
		//Application.LoadLevel(assignedLocation);
		sceneContent.SetCurrentLocation(assignedLocation);
		managerRef.ExecuteMoveTransition();
	}
}
