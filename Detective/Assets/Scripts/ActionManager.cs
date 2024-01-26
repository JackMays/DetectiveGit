using UnityEngine;
using System.Collections;

// Parent class for any manager that handles actions
// where the player presses a button and triggers an event such as Interacting with the scene
// IDs for actions of different contexts are communicated to game manager through the reference
public class ActionManager : MonoBehaviour {

	protected int actionID = 0;
	protected GameManager managerRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignManager (int action, GameManager manager)
	{
		actionID = action;
		managerRef = manager;
	}
}
