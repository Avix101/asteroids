using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Manages the movement of the ship, including rotating, moving forward / back, and updating thrusters
/// </summary>

public class ShipMovement : MonoBehaviour 
{
	//Public Thruster sets
	public GameObject[] mainThrusters;
	public GameObject[] rotateClockwiseThrusters;
	public GameObject[] rotateCounterThrusters;
	public GameObject[] reverseThrusters;
	public GameObject[] sideRecoveryThrusters;

	//Private ship motion details
	private Vector3 shipPosition;
	private Vector3 shipVelocity;
	private Vector3 shipDirection;
	private Vector3 shipAcceleration;
	private Quaternion totalRotation;

	//Is the ship currently functioning
	public bool alive;

	//A property to get and set the ship's position
	public Vector3 ShipPosition
	{
		get
		{
			return shipPosition;
		}

		set
		{
			shipPosition = value;
		}
	}

	//A property to get the ship's direction
	public Vector3 ShipDirection
	{
		get
		{
			return shipDirection;
		}
	}

	//A property to get the ship's velocity
	public Vector3 ShipVelocity
	{
		get
		{
			return shipVelocity;
		}
	}

	//A property to get the ship's total rotation
	public Quaternion TotalRotation
	{
		get
		{
			return totalRotation;
		}
	}

	//Variable to manage a buffer when wrapping the ship around the screen
	private float wrapBuffer;

	//Public speed related variables
	public float currentSpeed;
	public float accelerationRate;
	public float decelerationRate;
	public float maxSpeed;
	public float rotateSpeed;

	// Use this for initialization
	void Start () 
	{
		//Set all vectors to their initial states
		shipPosition = Vector3.zero;
		shipVelocity = Vector3.zero;
		shipDirection = Vector3.up;
		shipAcceleration = Vector3.zero;

		//Set all speed and wrap related variables
		currentSpeed = 0.1f;
		accelerationRate = 0.03f;
		decelerationRate = 0.95f;
		maxSpeed = 0.06f;
		rotateSpeed = 1.5f;
		wrapBuffer = 0.01f;

		//Turn off all thrusters initially
		SetThrusters (mainThrusters, false);
		SetThrusters (rotateClockwiseThrusters, false);
		SetThrusters (rotateCounterThrusters, false);
		SetThrusters (reverseThrusters, false);
		SetThrusters (sideRecoveryThrusters, false);

		//The ship is currently functioning
		alive = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//First rotate, then move, then wrap if necessary, and finally apply the transformations
		ShipRotation ();
		ShipMove ();
		ShipWrap ();
		ApplyTransformation ();
	}

	/// <summary>
	/// Purpose: Sets the group of thrusters to be active or inactive
	/// </summary>
	/// <param name="thrusters">A group of Thrusters</param>
	/// <param name="state">The activity state to set the thrusters to</param>
	void SetThrusters(GameObject[] thrusters, bool state)
	{
		//For each thruster in the group
		foreach(GameObject thruster in thrusters)
		{
			//Set the thruster's renderer to the defined state
			thruster.GetComponent<SpriteRenderer>().enabled = state;
		}
	}

	/// <summary>
	/// Purpose: Rotates the ship based on user input
	/// </summary>
	void ShipRotation()
	{
		//If the left arrow key is pressed and the ship is alive (rotate left)
		if(Input.GetKey(KeyCode.LeftArrow) && alive)
		{
			//Generate a quaternion, apply it to the ship's current direction, and record the total rotation of the sprite
			Quaternion angleToRotate = Quaternion.Euler (0, 0, rotateSpeed);
			shipDirection = angleToRotate * shipDirection;
			totalRotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z + rotateSpeed);

			//Set the appropriate group of thrusters to be active
			SetThrusters(rotateCounterThrusters, true);
		}		
		//If the right arrow key is pressed and the ship is alive (rotate right)
		else if(Input.GetKey(KeyCode.RightArrow) && alive)
		{
			//Same process as above, just rotating in the opposite direction
			Quaternion angleToRotate = Quaternion.Euler (0, 0, -rotateSpeed);
			shipDirection = angleToRotate * shipDirection;
			totalRotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z - rotateSpeed);

			//Set the appropriate group of thrusters to be active
			SetThrusters(rotateClockwiseThrusters, true);
		}

		//If the input keys are released, reset the thruster groups so that they are off
		if(Input.GetKeyUp(KeyCode.LeftArrow))
		{
			SetThrusters(rotateCounterThrusters, false);
		}

		if(Input.GetKeyUp(KeyCode.RightArrow))
		{
			SetThrusters(rotateClockwiseThrusters, false);
		}
	}

	/// <summary>
	/// Purpose: Moves the ship forward or back depending on user input
	/// Limits: Sometimes the thrusters don't activate in the way that they should. This was noted in the documentation
	/// </summary>
	void ShipMove()
	{
		//If the up arrow key is pressed and the player is alive (move forward along the direction vector)
		if(Input.GetKey(KeyCode.UpArrow) && alive)
		{
			//Calculate acceleration based on the rate, current direction, and delta time
			shipAcceleration = accelerationRate * shipDirection * Time.deltaTime;

			//Apply the acceleration to the velocity
			shipVelocity += shipAcceleration;

			//Clamp the velocity to the max speed so that it can't get too fast
			shipVelocity = Vector3.ClampMagnitude(shipVelocity, maxSpeed);

			SetThrusters(mainThrusters, true);
		}
		//Same as above, except check for down arrow to move backwards (along the direction vector)
		else if(Input.GetKey(KeyCode.DownArrow) && alive)
		{
			shipAcceleration = accelerationRate * -shipDirection * Time.deltaTime;
			shipVelocity += shipAcceleration;
			shipVelocity = Vector3.ClampMagnitude(shipVelocity, maxSpeed);

			SetThrusters(reverseThrusters, true);
		}
		//If there is no forward / back player input
		else
		{
			//if the ship is alive
			if(alive)
			{
				//Decelerate the ship by multiplying the deceleration rate times the current velocity
				shipVelocity *= decelerationRate;
			}
			//The ship isn't alive
			else
			{
				//Set the ship's speed to a low value
				shipVelocity = Vector3.ClampMagnitude(shipVelocity, 0.002f);

				//turn off all thrusters
				SetThrusters (mainThrusters, false);
				SetThrusters (rotateClockwiseThrusters, false);
				SetThrusters (rotateCounterThrusters, false);
				SetThrusters (reverseThrusters, false);
				SetThrusters (sideRecoveryThrusters, false);
			}

			//If the ship's velocity gets below a certain point and it is alive
			if (shipVelocity.magnitude < 0.001 && shipVelocity.magnitude > 0 && alive)
			{
				//Stop the ship completely
				shipVelocity = Vector3.zero;
			
				//Turn off recovery thrusters
				SetThrusters(mainThrusters, false);
				SetThrusters(reverseThrusters, false);
				SetThrusters(sideRecoveryThrusters, false);

			}
			//If the ship is not receiving player input and is alive
			else if(shipVelocity.magnitude >= 0.001 && alive) 
			{	
				//If the ship is traveling forward (within a certain angle of motion)
				if(Mathf.Abs(Vector3.Angle(shipVelocity, shipDirection)) < 90 && alive)
				{
					//Use the backwards thrusters to recover the ship and slow it down
					SetThrusters(reverseThrusters, true);
				}
				else
				{
					//Use the main thrusters to recover the ship and slow it down
					SetThrusters(mainThrusters, true);
				}
			}
		}

		//Apply the ship's velocity to its position
		shipPosition += shipVelocity;

		//If the ship is not receiving forwards or backwards movement, reset the thrusters
		if(Input.GetKeyUp(KeyCode.UpArrow))
		{
			SetThrusters(mainThrusters, false);
		}

		if(Input.GetKeyUp(KeyCode.DownArrow))
		{
			SetThrusters(reverseThrusters, false);
		}

	}

	/// <summary>
	/// Purpose: Wraps the ship around the screen, so that it can never be offscreen
	/// </summary>
	void ShipWrap()
	{
		//If the ship goes too far in either the y or x on either end, flip the offending coordinate so that the ship pops out at the
		//other side moving in a direction that brings the ship closer to the origin
		if(shipPosition.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x)
		{
			shipPosition = new Vector3(-shipPosition.x + wrapBuffer, shipPosition.y, shipPosition.z);
		}
		else if(shipPosition.x < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x)
		{
			shipPosition = new Vector3(-shipPosition.x - wrapBuffer, shipPosition.y, shipPosition.z);
		}

		if(shipPosition.y > Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y)
		{
			shipPosition = new Vector3(shipPosition.x, -shipPosition.y + wrapBuffer, shipPosition.z);
		}
		else if(shipPosition.y < Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y)
		{
			shipPosition = new Vector3(shipPosition.x, -shipPosition.y - wrapBuffer, shipPosition.z);
		}
	}

	/// <summary>
	/// Purpose: Apply the position and rotation transformations to the ship
	/// </summary>
	void ApplyTransformation()
	{
		//Apply both the new position and new rotation transformations to the ship
		transform.position = shipPosition;
		transform.rotation = totalRotation;
	}
}
