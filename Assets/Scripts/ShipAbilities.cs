using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Keeps track of the player's abilities, Ability energy, and animations relating to those abilities
/// </summary>

public class ShipAbilities : MonoBehaviour 
{
	//Public Particle Systems
	public ParticleSystem harvestRay;
	public ParticleSystem shieldBubble;
	public ParticleSystem teleportRay;
	public ParticleSystem teleportBubble;
	public ParticleSystem bulletAugmentLeft;
	public ParticleSystem bulletAugmentRight;

	//Public Icons
	public GameObject shieldIcon;
	public GameObject teleportIcon;
	public GameObject harvestIcon;
	public GameObject quickfireIcon;

	//Public energy bar sprite
	public GameObject energyBar;

	//Private Ability energy management
	private int radioactiveEnergy;
	private int totalEnergyPossible;

	//Private Ability activity states
	private bool isShieldUp;
	private bool isTeleporting;
	private bool bulletAugment;
	private bool isHarvesting;

	//Private Ability details
	private Vector3 teleportPosition;
	private float shrinkRate;
	private int animationTicks;

	//Property to get the current Ability energy
	public int RadioactiveEnergy
	{
		get
		{
			return radioactiveEnergy;
		}
	}

	//Property to get whether or not the shield is up
	public bool IsShieldUp
	{
		get
		{
			return isShieldUp;
		}
	}

	//Property to get whether or not the ship is teleporting
	public bool IsTeleporting
	{
		get
		{
			return isTeleporting;
		}
		
	}

	//Property to get whether or not the ship's quick-fire ability is active
	public bool BulletAugment
	{
		get
		{
			return bulletAugment;
		}
	}

	//Property to get whether or not the ship is harvesting
	public bool IsHarvesting
	{
		get
		{
			return isHarvesting;
		}
	}

	// Use this for initialization
	void Start () 
	{
		//Start the ship with a full gauge of Ability energy (AKA radioactive energy)
		radioactiveEnergy = 1000;
		totalEnergyPossible = 1000;

		//Set the animation shrink rate
		shrinkRate = 0.85f;

		//All activity states are false to start
		isShieldUp = false;
		isTeleporting = false;
		bulletAugment = false;
		isHarvesting = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Update the width of the power bar and the position of the teleport cursor
		UpdatePowerBar ();
		UpdateTeleportCursor ();

		//Reset the color of all icons if the player has enough ability points to activate that ability
		if(radioactiveEnergy > 200 && !isTeleporting)
		{
			teleportIcon.GetComponent<SpriteRenderer>().color = Color.white;
		}

		if(radioactiveEnergy > 19 && !bulletAugment)
		{
			quickfireIcon.GetComponent<SpriteRenderer>().color = Color.white;
		}

		if(radioactiveEnergy > 1 && !isShieldUp)
		{
			shieldIcon.GetComponent<SpriteRenderer>().color = Color.white;
		}

		if(radioactiveEnergy > 0 && !isHarvesting)
		{
			harvestIcon.GetComponent<SpriteRenderer>().color = Color.white;
		}

		//If the player presses the '3' key and they have greater than 0 ability points
		if(Input.GetKey(KeyCode.Alpha3) && radioactiveEnergy > 0)
		{
			//If the ship is not currently playing the harvest animation
			if(!isHarvesting)
			{
				//Play the animation
				harvestRay.Play();
			}

			//Update the ability state variable, color the icon and decrement the ability points
			isHarvesting = true;
			harvestIcon.GetComponent<SpriteRenderer>().color = Color.yellow;
			radioactiveEnergy--;
		}

		//If the player stops pressing '3' or if the number of ability points depletes
		if(Input.GetKeyUp(KeyCode.Alpha3) || radioactiveEnergy <= 0)
		{
			//Stop the animation, update the state, and recolor the icon
			harvestRay.Stop();
			isHarvesting = false;
			harvestIcon.GetComponent<SpriteRenderer>().color = Color.white;
		}

		//If the player doesn't have enough energy to activate the ability, color it gray
		if(radioactiveEnergy <= 0)
		{
			harvestIcon.GetComponent<SpriteRenderer>().color = Color.gray;
		}

		//If the '1' key is pressed and the player has at least 2 energy
		if(Input.GetKey(KeyCode.Alpha1) && radioactiveEnergy > 1)
		{
			//Same as the harvest ability
			if(!isShieldUp)
			{
				shieldBubble.Play();
			}
			isShieldUp = true;
			shieldIcon.GetComponent<SpriteRenderer>().color = Color.yellow;
			
			radioactiveEnergy--;
		}

		//Same as the harvest ability (only difference is energy cost)
		if(Input.GetKeyUp(KeyCode.Alpha1) || radioactiveEnergy <= 1)
		{
			shieldBubble.Stop();
			isShieldUp = false;
			shieldIcon.GetComponent<SpriteRenderer>().color = Color.white;
		}

		//Same as the harvest ability
		if(radioactiveEnergy <= 1)
		{
			shieldIcon.GetComponent<SpriteRenderer>().color = Color.gray;
		}

		//If the player presses '2' and they have enough energy
		if(Input.GetKey(KeyCode.Alpha2) && radioactiveEnergy > 200)
		{
			//Play the animation if it isn't already
			if(!teleportRay.isPlaying)
			{
				teleportRay.Play();
			}
		}

		//If the user clicks the right mouse button
		if(Input.GetMouseButtonDown(1))
		{
			//Stop the animation
			teleportRay.Stop();
		}

		//Same as the above abilites, when the energy gets too low to use an ability it stops the animation and recolors the icon to be gray
		if(radioactiveEnergy <= 199)
		{
			teleportRay.Stop();
			teleportIcon.GetComponent<SpriteRenderer>().color = Color.gray;
		}

		//If the teleport ray animation is playing, the player clicks the left mouse button, and they have enough energy
		if(teleportRay.isPlaying && Input.GetMouseButtonDown(0) && radioactiveEnergy > 200)
		{
			//Stop the teleport ray animation and start the teleport bubble animation (actual teleport)
			teleportRay.Stop();
			teleportBubble.Play();

			//Record the current position of the mouse
			teleportPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			teleportPosition.z = 0;

			//Update the state variable
			isTeleporting = true;

			//Subtract the energy cost
			radioactiveEnergy -= 200;
		}

		//If the player is currently teleporting, recolor the icon
		if(isTeleporting)
		{
			teleportIcon.GetComponent<SpriteRenderer>().color = Color.yellow;
		}

		//If the teleport animation is playing and has been playing for a certain amount of time
		if(teleportBubble.isPlaying && teleportBubble.time > 1.3f && isTeleporting)
		{
			//Scale the ship down to almost nothing
			gameObject.transform.localScale = gameObject.transform.localScale * shrinkRate;
		}

		//If the teleport animation has completed the first phase
		if(isTeleporting && !teleportBubble.isPlaying)
		{
			//Move the ship to the new location, and give it a base scale
			gameObject.transform.position = teleportPosition;
			gameObject.GetComponent<ShipMovement>().ShipPosition = teleportPosition;
			gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

			//Play the animation once again, update the state variable and icon
			teleportBubble.Play();
			isTeleporting = false;
			teleportIcon.GetComponent<SpriteRenderer>().color = Color.white;

			//Set the number of animation ticks for the next phase of the animation
			animationTicks = 100;
		}

		//This is the final stage of the teleportation animation
		if(!isTeleporting && teleportBubble.isPlaying)
		{
			//If there are animation ticks left, decrement the ticks
			if(animationTicks > 0)
			{
				animationTicks--;
			}
			else
			{
				//If there are no more animation ticks scale the ship up to 1, 1, 1
				gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

				//Clamp the scale to 1, 1, 1
				if (gameObject.transform.localScale.x > 1)
				{
					gameObject.transform.localScale = new Vector3(1, 1, 1);
				}
			}
		}

		//If '4' is pressed and the player has at least 20 energy
		if(Input.GetKeyDown(KeyCode.Alpha4) && radioactiveEnergy > 19)
		{
			//If the ability is currently not active
			if(!bulletAugment)
			{
				//Play the animations, update the state variable and color the icon
				bulletAugmentLeft.Play();
				bulletAugmentRight.Play();
				bulletAugment = true;
				quickfireIcon.GetComponent<SpriteRenderer>().color = Color.yellow;
			}
			//If the ability is currently active, it should toggle off
			else
			{
				//Stop the two animations, update the state variable, and recolor the icon
				bulletAugmentLeft.Stop ();
				bulletAugmentRight.Stop ();
				bulletAugment = false;

				quickfireIcon.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}

		//Same as the other abilities, if the player doesn't have enough energy, recolor the icon and turn off the ability
		if(radioactiveEnergy <= 19)
		{
			bulletAugmentLeft.Stop ();
			bulletAugmentRight.Stop ();
			bulletAugment = false;

			quickfireIcon.GetComponent<SpriteRenderer>().color = Color.gray;
		}

		//The number of current Ability points / radioactive energy cannot go below 0
		if(radioactiveEnergy < 0)
		{
			radioactiveEnergy = 0;
		}
	}

	/// <summary>
	/// Purpose: Updates the power bar's width
	/// </summary>
	void UpdatePowerBar()
	{
		//Set the X component of the scale equal to the current number of ability points divided by the total possible
		energyBar.GetComponent<SpriteRenderer> ().transform.localScale = new Vector3((float) radioactiveEnergy / totalEnergyPossible, 1, 1);
	}

	/// <summary>
	/// Purpose: Augments the radioactive energy (Ability points)
	/// </summary>
	/// <param name="amount">The amount to add to the total energy (Ability points)</param>
	public void AugmentRadioactiveEnergy(int amount)
	{
		//If the current amount plus the addition is less than the total possible
		if(radioactiveEnergy + amount < totalEnergyPossible)
		{
			//Add the new amount to the current
			radioactiveEnergy += amount;
		}
		else
		{
			//Cap the amount to the total possible
			radioactiveEnergy = totalEnergyPossible;
		}
	}

	/// <summary>
	/// Purpose: Alerts this class that the quick-fire ability has been used, and that 20 ability points should be taken away.
	/// </summary>
	public void QuickfireUse()
	{
		//Subtract 20 ability points
		radioactiveEnergy -= 20;
	}

	/// <summary>
	/// Purpose: Updates the teleport cursor's position
	/// </summary>
	void UpdateTeleportCursor()
	{
		//Get the position of the mouse in game coordinates
		Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		//Set the z component of the position to 0
		newPosition.z = 0;

		//Apply the position transformation
		teleportRay.transform.position = newPosition;
	}

	/// <summary>
	/// Raises the GUI event.
	/// Purpose: Display a specific measure of how many ability points the player has left
	/// </summary>
	void OnGUI()
	{
		//If the player has less than or equal to half a gauge of ability points
		if(radioactiveEnergy <= 500)
		{
			//Color the text yellow
			GUI.color = Color.yellow;
		}

		//If the player has no ability points left
		if(radioactiveEnergy <= 0)
		{
			//Color the text magenta (red hurts the eyes)
			GUI.color = Color.magenta;
		}

		//Set the GUI background to be clear, the text aligned in the center, and the text size to be normal
		GUI.backgroundColor = Color.clear;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.skin.label.fontSize = 14;

		//Draw the text on the screen roughly in the location of the ability points bar
		GUI.Label (new Rect (20, 120, 160, 30), radioactiveEnergy + "/" + totalEnergyPossible + " Ability Energy");
	}
}
