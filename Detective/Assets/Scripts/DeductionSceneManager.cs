using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DeductionSceneManager : MonoBehaviour {

	public Button clueButton;
	public Button itemButton;
	public Button deductionButton;
	public Button suspectButton;
	public Button conclusionButton;
	public Button reviewButton;

	Button continueButton;

	List<Deduction> deductionList = new List<Deduction>();
	List<string> deducedList = new List<string>();
	
	GameManager gameManagerRef;

	string firstObject = "";
	string secondObject = "";
	string folderPath = "Text/Deductions/";

	bool isChainHideOnce = false;



	// Use this for initialization
	void Start () 
	{
		deductionButton.gameObject.SetActive(false);
		suspectButton.gameObject.SetActive(false);
		conclusionButton.gameObject.SetActive(false);
		reviewButton.gameObject.SetActive(false);

		//continueButton = GameObject.FindGameObjectWithTag("Continue").GetComponent<Button>();

		gameManagerRef = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

		//gameManagerRef.DeductionHide();

		gameManagerRef.TypeEventTextWithWipe("Review the Evidence");

		int i = 0;

		string fullFilePath = folderPath + SceneManager.GetActiveScene().name + "/" + SceneManager.GetActiveScene().name + " ";

		TextAsset deductionFile = Resources.Load (fullFilePath + i.ToString()) as TextAsset;

		while (deductionFile)
		{
			string[] deductionArray = deductionFile.text.Split('\n');

			Debug.Log ("while: " + i);

			deductionList.Add (new Deduction(deductionArray[0], deductionArray[1], deductionArray[2], deductionArray[3]));

			++i;

			deductionFile = Resources.Load (fullFilePath + i.ToString()) as TextAsset;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (gameManagerRef.HasConclusionChainActive() && !isChainHideOnce)
		{
			ChainHide();
			isChainHideOnce = true;
		}
		else if (!gameManagerRef.HasConclusionChainActive() && isChainHideOnce)
		{
			// If theres no chain but the Ui has been prepped for a chain before
			// One has ended and its time for the finale sequence
			StartFinalMuscle();
			isChainHideOnce = false;
		}
	}

	void resetObjects()
	{
		firstObject = "";
		secondObject = "";
	}

	void PassDeduction()
	{
		foreach (Deduction deduction in deductionList)
		{
			string text = deduction.CheckDeduction(firstObject, secondObject);
			
			if (text != "")
			{
				bool isDeduced = false;

				for (int i = 0; i < deducedList.Count; ++i)
				{
					if (deduction.GetID() == deducedList[i])
					{
						isDeduced = true;
					}
				}

				if (!isDeduced)
				{
					gameManagerRef.WipePanelButtons();
					gameManagerRef.ExecuteDeductionDisplayAction(text);

					deducedList.Add (deduction.GetID());

					if (deducedList.Count == deductionList.Count)
					{
						deductionButton.gameObject.SetActive(true);
						suspectButton.gameObject.SetActive(true);
						conclusionButton.gameObject.SetActive(true);
						itemButton.gameObject.SetActive(false);
						clueButton.gameObject.SetActive(false);
					}

					resetObjects();
					return;
				}
				else
				{
					gameManagerRef.ExecuteDeductionDisplayAction("Already Matched.");
					resetObjects();
					return;
				}
			}
		}
		
		gameManagerRef.ExecuteDeductionDisplayAction("Those do not go together.");
		resetObjects();
	}

	void ChainHide()
	{
		itemButton.gameObject.SetActive(false);
		clueButton.gameObject.SetActive(false);
		deductionButton.gameObject.SetActive(false);
		suspectButton.gameObject.SetActive(false);
		conclusionButton.gameObject.SetActive(false);
		reviewButton.gameObject.SetActive(false);
	}

	public void SaveObjectSelection(string selection, string examine)
	{
		if (firstObject == "")
		{
			firstObject = selection;
			gameManagerRef.ExecuteDeductionDisplayAction(examine);
		}
		else
		{
			secondObject = selection;
			PassDeduction();

		}

	}

	public void DisplayClues()
	{
		gameManagerRef.LoadSubButtons(13);
	}
	
	public void DisplayItems()
	{
		gameManagerRef.LoadSubButtons(14);
	}
	
	public void DisplaySuspects()
	{
		gameManagerRef.LoadSubButtons(15);
	}
	
	public void DisplayDeductions()
	{
		gameManagerRef.LoadSubButtons(deductionList);
	}

	public void DisplayDeducedText(string text)
	{
		gameManagerRef.TypeEventTextWithWipe(text);
	}

	public void StartConclusionPhase()
	{
		deductionButton.gameObject.SetActive(false);
		suspectButton.gameObject.SetActive(false);
		conclusionButton.gameObject.SetActive(false);
		reviewButton.gameObject.SetActive(true);

		gameManagerRef.LoadSubButtons(16);


	}

	public void ReturnToReviewPhase()
	{
		deductionButton.gameObject.SetActive(true);
		suspectButton.gameObject.SetActive(true);
		conclusionButton.gameObject.SetActive(true);
		reviewButton.gameObject.SetActive(false);

		gameManagerRef.WipePanelButtons();
		gameManagerRef.TypeEventTextWithWipe("Review the Evidence");
	}

	public void StartFinalMuscle()
	{
		reviewButton.gameObject.SetActive(false);
		gameManagerRef.ToggleMuscleButton(true);
		gameManagerRef.ToggleContinueButton(false);
		gameManagerRef.WipePanelButtons();
	}
}
