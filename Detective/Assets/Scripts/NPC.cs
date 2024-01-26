using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Skeleton for definine an NPC
// From their name, to what displays when you talk to them, any follow ups to that initial dialogue and any physical options for muscle mode
public class NPC {

	string npcName;

	string npcFilePath;

	string talkIntro;

	List<string> muscleKeywords = new List<string>();

	List<DialogueChoice> initFollowUpDialogues = new List<DialogueChoice>();

	public NPC(string na, string npcSub)
	{
		npcName = na;

		npcFilePath = npcSub + npcName + "/";

		string baseFilePath = npcFilePath + npcName;

		TextAsset initialTalkFile = Resources.Load(baseFilePath + " Event") as TextAsset;
		TextAsset choiceFile = Resources.Load (baseFilePath + " Choices") as TextAsset;
		TextAsset muscleFile = Resources.Load (baseFilePath + " Muscle") as TextAsset;

		talkIntro = initialTalkFile.text;

		int choices = int.Parse(choiceFile.text);

		for (int i = 1; i <= choices; ++i)
		{

			TextAsset keywordFile = Resources.Load (baseFilePath + " Keyword " + i) as TextAsset;
			
			string followUpKeyword = keywordFile.text;
			
			initFollowUpDialogues.Add (new DialogueChoice(npcName, followUpKeyword, npcFilePath));


			
		}

		if (muscleFile)
		{
			string[] muscleArray = muscleFile.text.Split('\n');

			for (int i = 0; i < muscleArray.Length; ++i)
			{
				muscleKeywords.Add (muscleArray[i]);
			}
		}




	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string GetName()
	{
		return npcName;
	}

	public string GetTalkIntro()
	{
		return talkIntro;
	}

	public List<string> GetMuscleKeywords()
	{
		return muscleKeywords;
	}


	public int GetDialogueCount()
	{
		return initFollowUpDialogues.Count;
	}

	public bool HasMuscle()
	{
		return (muscleKeywords.Count != 0);
	}

	public DialogueChoice GetNextDialogue(int index)
	{
		return initFollowUpDialogues[index];
	}
}
