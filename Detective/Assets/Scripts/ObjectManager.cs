using UnityEngine;
using System.Collections;

// Parent Class for all manager clases assigned to buttons
// Each have the ID of the object they have (such as an NPCs ID or items ID) and a reference to Gsme Manager to communicate back to
public class ObjectManager : MonoBehaviour {

	protected int assignedObjectID;
	
	protected GameManager managerRef;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AssignObject(int id, GameManager manager)
	{
		assignedObjectID = id;
		managerRef = manager;
	}
}
