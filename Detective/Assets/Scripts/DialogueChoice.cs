using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Class for handling Dialogue choices by the player
// what the button looks like, what item/move/clue they unlock and what muscle options they work with or make available
// as well as how long the text sequence is
public class DialogueChoice {

	string buttonText;

	string eventTextDisplay;

	// a dialogue choice holds onto its potential direct follow ups
	List<DialogueChoice> followUpDialogues = new List<DialogueChoice>();
	List<string> unlockItems = new List<string>();
	List<string> unlockMoves = new List<string>();
	List<string> unlockClues = new List<string>();
	List<string> muscleKeywords = new List<string>();
	List<string> eventChainList = new List<string>();

	int choiceNo;

	bool canFollowUp;

	public DialogueChoice(string npc, string keyword, string filePath) {

		string baseFilePath = filePath + npc + " " + keyword;

		TextAsset buttonTextFile = Resources.Load(baseFilePath + " Button") as TextAsset;
		TextAsset eventTextFile = Resources.Load(baseFilePath + " Event") as TextAsset;
		TextAsset choiceFile = Resources.Load (baseFilePath + " Choices") as TextAsset;
		TextAsset itemFile = Resources.Load (baseFilePath + " Items") as TextAsset;
		TextAsset moveFile = Resources.Load (baseFilePath + " Moves") as TextAsset;
		TextAsset clueFile = Resources.Load (baseFilePath + " Clue") as TextAsset;
		TextAsset muscleFile = Resources.Load (baseFilePath + " Muscle") as TextAsset;

		buttonText = buttonTextFile.text;
		eventTextDisplay = eventTextFile.text;

		choiceNo = int.Parse(choiceFile.text);

		// choiceNo decides if there are any follow up dialogue to select
		if (choiceNo > 0)
		{
			canFollowUp = true;
		}
		else
		{
			canFollowUp = false;
		}
		// If there is follow up, find the keyword for the NPC
		// Which has its own choice file that may decide any more follow ups
		// Button text for its button appearance
		// An event which is what is displayed when the button is pushed, often the NPCs reaction and/or speech
		// And any item/move/clue/muscle unlocks as appropriate
		if (canFollowUp)
		{
			for (int i = 1; i <= choiceNo; ++i)
			{

				TextAsset keywordFile = Resources.Load (baseFilePath + " Keyword " + i) as TextAsset;

				string followUpKeyword = keywordFile.text;

				followUpDialogues.Add (new DialogueChoice(npc, followUpKeyword, filePath));

			}
		}

		if (itemFile)
		{
			string[] unlockArray = itemFile.text.Split('\n');

			for (int i = 0; i < unlockArray.Length; ++i)
			{
				unlockItems.Add (unlockArray[i]);
			}
		}

		if (moveFile)
		{
			string[] moveArray = moveFile.text.Split('\n');
			
			for (int i = 0; i < moveArray.Length; ++i)
			{
				unlockMoves.Add (moveArray[i]);
			}
		}

		if (clueFile)
		{
			string[] clueArray = clueFile.text.Split('\n');
			
			for (int i = 0; i < clueArray.Length; ++i)
			{
				unlockClues.Add (clueArray[i]);
			}
		}

		if (muscleFile)
		{
			string[] muscleArray = muscleFile.text.Split('\n');
			
			for (int i = 0; i < muscleArray.Length; ++i)
			{
				muscleKeywords.Add (muscleArray[i]);
			}
		}

		int chainCount = 0;
		TextAsset eventChainTextFile = Resources.Load(baseFilePath + " Event Chain " + chainCount) as TextAsset;

		while (eventChainTextFile)
		{
			eventChainList.Add (eventChainTextFile.text);
			++chainCount;

			eventChainTextFile = Resources.Load(baseFilePath + " Event Chain " + chainCount) as TextAsset;
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetButtonText()
	{
		return buttonText;
	}

	public string GetEventText()
	{
		return eventTextDisplay;
	}

	public List<string> GetClueList()
	{
		return unlockClues;
	}

	public List<string> GetMuscleKeywords()
	{
		return muscleKeywords;
	}

	public List<string> GetChainList()
	{
		return eventChainList;
	}


	public int GetDialogueCount()
	{
		return followUpDialogues.Count;
	}

	public DialogueChoice GetNextDialogue(int index)
	{
		return followUpDialogues[index];
	}

	public bool HasItemUnlocks()
	{
		if (unlockItems.Count > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool HasMoveUnlocks()
	{
		if (unlockMoves.Count > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool HasClueUnlocks()
	{
		if (unlockClues.Count > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool IsItemInUnlock(string item)
	{
		for (int i = 0; i < unlockItems.Count; ++i)
		{
			if (unlockItems[i] == item)
			{
				return true;
			}
		}

		return false;
	}

	public bool IsMoveInUnlock(string move)
	{
		for (int i = 0; i < unlockMoves.Count; ++i)
		{
			if (unlockMoves[i] == move)
			{
				return true;
			}
		}
		
		return false;
	}

	public bool HasMuscle()
	{
		return (muscleKeywords.Count != 0);
	}

	public bool HasChain()
	{
		return (eventChainList.Count > 0);
	}
}
