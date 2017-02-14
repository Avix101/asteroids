using UnityEngine;
using System.Collections;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Handle the spawning of asteroids into the scene
/// </summary>

public class AsteroidSpawn : MonoBehaviour 
{
	public GameObject[] asteroidPrefabs;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Purpose: Generates a first level asteroid, places it in the scene, and passes critical information to
	/// the Asteroid Movement script attached to the new asteroid
	/// </summary>
	/// <returns>The asteroid</returns>
	public GameObject SpawnAsteroid()
	{
		//Set up variables for later assignment
		float x = 0;
		float y = 0;
		float angleOfMotion = 0;

		//Pick a spawn side for the asteroid (one of the four edges)
		int spawnSide = (int) Mathf.Floor (Random.Range (0, 4));

		//Based on the side picked, assign a new location and angle of motion for the asteroid
		switch (spawnSide)
		{
			//Left Side
			case 0:
				//X in this case is fixed (because the side is left) so that the asteroid is not visible upon spawn
				x = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - 1.5f;

				//Y in this case is a random value between the top and the bottom of the screen
				y = Random.Range(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y);
				
				//The angle of motion determines the asteroid's direction, so this angle must be random
				//but also constrained to a certain range so that it will most likely move across the screen (not away from the screen)
				angleOfMotion = Random.Range(220, 320);
				break;

			//Bottom Side
			case 1:
				//Similar x & y assignments, just switching based on the side of the screen
				//All points are converted from standard X and Y coordinates to coordinates usable in the game
				x = Random.Range(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x);
				y = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y - 1.5f;
				angleOfMotion = Random.Range(50, -50);
				break;
			//Right Side
			case 2:
				x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x + 1.5f;
				y = Random.Range(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y);
				angleOfMotion = Random.Range(40, 140);
				break;

			//Top Side
			case 3:
				x = Random.Range(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x);
				y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + 1.5f;
				angleOfMotion = Random.Range(130, 230);
				break;
		}
		//Create a position vector from the configured x and y values, and rotate the asteroid sprite a random amount
		Vector3 newPosition = new Vector3 (x, y, 1);
		Quaternion newAngle = Quaternion.Euler (0, 0, Random.Range(0,360));

		//Pick an asteroid prefab from the list, by picking an index from 0 to the list length -1
		int asteroidPrefab = (int)Mathf.Floor (Random.Range (0, asteroidPrefabs.Length));

		//Create the new asteroid and pass it the prefab, its new position, and its angle of rotation
		GameObject asteroid = (GameObject) Instantiate(asteroidPrefabs[asteroidPrefab], newPosition, newAngle);

		//Generate the asteroid's angle of motion based upon the previously calculated angle of motion 
		//(NOT the random angle that we rotated the sprite)
		Quaternion angleToRotate = Quaternion.Euler (0, 0, angleOfMotion);
		Vector3 newDirection = angleToRotate * Vector3.up;

		//Set the asteroid's direction of motion and assert that it is a first-level asteroid
		asteroid.GetComponent<AsteroidMovement> ().asteroidDirection = newDirection;
		asteroid.GetComponent<AsteroidMovement> ().secondLevel = false;

		//Determine which prefab was used to generate the asteroid
		switch (asteroidPrefab)
		{
			//Regular asteroid prefabs
			case 0:
				//Assign the radius of the asteroid based on tested values for each individual prefab
				asteroid.GetComponent<AsteroidMovement> ().radius = 1.4f;
				asteroid.GetComponent<AsteroidMovement> ().collectable = false;
				break;
			case 1:
				asteroid.GetComponent<AsteroidMovement> ().radius = 0.6f;
				asteroid.GetComponent<AsteroidMovement> ().collectable = false;
				break;
			case 2:
				asteroid.GetComponent<AsteroidMovement> ().radius = 1f;
				asteroid.GetComponent<AsteroidMovement> ().collectable = false;
				break;
			case 3:
				asteroid.GetComponent<AsteroidMovement> ().radius = 1.35f;
				asteroid.GetComponent<AsteroidMovement> ().collectable = false;
				break;
			//Radioactive asteroid prefab
			case 4:
				//Only the radioactive asteroids will become collectable
				asteroid.GetComponent<AsteroidMovement> ().radius = 0.4f;
				asteroid.GetComponent<AsteroidMovement> ().collectable = true;
				break;
		}
		//After everything in the new asteroid has been configured, pass the asteroid back to the caller
		return asteroid;
	}

	/// <summary>
	/// Purpose: Generates a second-level asteroid based on characteristics of the asteroid it is spawning from
	/// </summary>
	/// <returns>The second level asteroid</returns>
	/// <param name="oldAsteroid">Old asteroid (used as a reference for second-level asteroids)</param>
	public GameObject SpawnSecondLevelAsteroid(GameObject oldAsteroid)
	{
		//Generate a random direction for the asteroid that travels in about (plus or minus 30 degrees)
		//the same direction as the first-level asteroid
		Quaternion directionAdjustment = Quaternion.Euler (0, 0, Random.Range (-30, 30));
		Vector3 newDirection = directionAdjustment * oldAsteroid.GetComponent<AsteroidMovement>().asteroidDirection;

		//The new asteroid's sprite should still be rotated a random amount
		Quaternion angleToRotate = Quaternion.Euler (0, 0, Random.Range (0, 360));

		//Create the new second-level asteroid (using the same prefab) at the old asteroid's location, with the new rotation
		GameObject asteroid = (GameObject) Instantiate (oldAsteroid, oldAsteroid.transform.position, angleToRotate);

		//If the first-level asteroid was collectable
		if(oldAsteroid.GetComponent<AsteroidMovement>().collectable)
		{
			//The second-level asteroid will be collectable too
			asteroid.GetComponent<AsteroidMovement>().collectable = true;
		}

		//Randomly scale the second-level asteroid to be between 30-60% the size of the old asteroid
		float scaleAdjustment = Random.Range(0.3f,0.6f);

		//Apply the new scale to the second-level asteroid and ensure that the radius (used for collisions) is also adjusted
		asteroid.transform.localScale = asteroid.transform.localScale * scaleAdjustment;
		asteroid.GetComponent<AsteroidMovement> ().radius = oldAsteroid.GetComponent<AsteroidMovement> ().radius * scaleAdjustment;

		//Randomly adjust the speed of the new asteroid to be 10-90% the speed of the old asteroid
		asteroid.GetComponent<AsteroidMovement> ().speed = oldAsteroid.GetComponent<AsteroidMovement> ().speed - Random.Range(0.1f,0.9f);

		//Apply the new direction vector to the new asteroid and asser that it is a second-level asteroid
		asteroid.GetComponent<AsteroidMovement> ().asteroidDirection = newDirection;
		asteroid.GetComponent<AsteroidMovement> ().secondLevel = true;

		//After everything has been set, pass the asteroid back to the caller
		return asteroid;
	}
}
