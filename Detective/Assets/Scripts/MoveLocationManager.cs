using UnityEngine;
using System.Collections;

// Class for handling Buttons that are selecxted by the player to move to another location
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

	// function for button press to execute scsne transition to new location
	public void LoadAssignedLocation()
	{
		//Application.LoadLevel(assignedLocation);
		sceneContent.SetCurrentLocation(assignedLocation);
		managerRef.ExecuteMoveTransition();
	}
}
