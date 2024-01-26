using UnityEngine;
using System.Collections;

// Class for handling buttons when selecting NPCs to talk to
public class NPCManager : ObjectManager {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// function for button press to pass NPC selected
	public void PassTalkTarget()
	{
		managerRef.ExecuteTalkTarget(assignedObjectID);
	}
}
