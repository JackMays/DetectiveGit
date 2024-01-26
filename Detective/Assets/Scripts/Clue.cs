using UnityEngine;
using System.Collections;

// The Skeleton of a clue and how they are stored
// for combining into deductions and proceeding to the finale of cases
public class Clue {

	string clueID;

	string selectText;
	string buttonText;


	public Clue (string id, string select, string button)
	{
		clueID = id;
		selectText = select;
		buttonText = button;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetClueID() 
	{
		
		return clueID;
		
	}

	public string GetButtonText() 
	{

		return buttonText;
		
	}

	public string GetSelectText() 
	{
		return selectText;
		
	}
}
