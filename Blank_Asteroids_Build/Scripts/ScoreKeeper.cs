using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Keeps track of the player's score and determines whether or not the player has reached a score milestone
/// </summary>

public class ScoreKeeper : MonoBehaviour 
{
	//All score information is private
	private int score;
	private int milestone;

	// Use this for initialization
	void Start () 
	{
		//The starting score is 0 and the interval for a score milestone is 500
		score = 0;
		milestone = 500;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Purpose: Augments the score, and determines if the player has reached a score milestone
	/// </summary>
	/// <returns><c>true</c>If the player reached a score milestone<c>false</c>If the player did not reach a score milestone</returns>
	/// <param name="addition">The amount to add to the score</param>
	public bool AugmentScore(int addition)
	{
		//Add the given amount to the score
		score += addition;

		//If the score is greater than the milestone
		if(score > milestone)
		{
			//Set a new milestone
			milestone += 500;

			//Alert the caller that a milestone was set
			return true;
		}

		//Alert the caller that a milestone was not set
		return false;
	}

	/// <summary>
	/// Raises the GUI event.
	/// Purpose: Draw the score onto the screen for the player to see
	/// </summary>
	void OnGUI()
	{
		//Set the background to be clear, the text to be white, the font size to be large, and the alignment to be in the center
		GUI.backgroundColor = Color.clear;
		GUI.contentColor = Color.white;
		GUI.skin.label.fontSize = 36;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		//Draw the score onto the screen at the specified location
		GUI.Label (new Rect (Screen.width / 2 - 100, 0, 200, 100), score.ToString());
	}
}
