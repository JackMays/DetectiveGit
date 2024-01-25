using UnityEngine;
using System.Collections;

public class AccusationManager : MonoBehaviour {

	GameManager gameManagerRef;
	DeductionSceneManager deductionManagerRef;

	int assignedSuspect;

	public void AssignManager(int suspID, GameManager manager)
	{
		assignedSuspect = suspID;

		gameManagerRef = manager;
	}

	public void CheckAccusation()
	{
		bool isRight = gameManagerRef.ExecuteAccusation(assignedSuspect);

		if (isRight && !gameManagerRef.HasConclusionChainActive())
		{
			deductionManagerRef.StartFinalMuscle();
		}
	}

	// Use this for initialization
	void Start () 
	{
		deductionManagerRef = GameObject.FindGameObjectWithTag("DeductionSceneManager").GetComponent<DeductionSceneManager>();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
