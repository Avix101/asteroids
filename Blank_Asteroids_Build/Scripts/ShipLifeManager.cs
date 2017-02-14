using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Manages the ship's lives
/// </summary>

public class ShipLifeManager : MonoBehaviour 
{
	//Private lives number
	private int lives;

	//Public UI sprites
	public GameObject shipLife1;
	public GameObject shipLife2;
	public GameObject shipLife3;

	public GameObject shipNoLife1;
	public GameObject shipNoLife2;
	public GameObject shipNoLife3;

	// Use this for initialization
	void Start () 
	{
		//Give the player 3 lives to start
		lives = 3;

		//Enable the correct graphics and disable the others
		shipLife1.GetComponent<SpriteRenderer>().enabled = true;
		shipNoLife1.GetComponent<SpriteRenderer>().enabled = false;
		shipLife2.GetComponent<SpriteRenderer>().enabled = true;
		shipNoLife2.GetComponent<SpriteRenderer>().enabled = false;
		shipLife3.GetComponent<SpriteRenderer>().enabled = true;
		shipNoLife3.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	/// <summary>
	/// Purpose: Takes a life away from the player
	/// </summary>
	/// <returns><c>true</c>If the player has lost all lives<c>false</c>If the player still has lives left.</returns>
	public bool LoseLife()
	{
		//Evaluate the number of lives currently available
		switch(lives)
		{
			//Update the UI (Ship lives display), decrement the lives and return false 
			case 3:
				shipLife3.GetComponent<SpriteRenderer>().enabled = false;
				shipNoLife3.GetComponent<SpriteRenderer>().enabled = true;
				lives--;
				return false;

			//Same as above
			case 2:
				shipLife2.GetComponent<SpriteRenderer>().enabled = false;
				shipNoLife2.GetComponent<SpriteRenderer>().enabled = true;
				lives--;
				return false;

			//Same as above, but return true, for the player has lost all lives
			case 1:
				shipLife1.GetComponent<SpriteRenderer>().enabled = false;
				shipNoLife1.GetComponent<SpriteRenderer>().enabled = true;
				lives--;
				return true;
		}

		//If we somehow reach this point without a return, return false
		return false;
	}
}
