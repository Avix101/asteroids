using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Name: Stasha Blank
/// Date: 10/19/16
/// Project: Asteroids
/// Purpose of Class: Manages top-level operations within the scene, such as collision detection and interaction between the ship's
/// abilities and environmental factors
/// </summary>

public class SceneManager : MonoBehaviour 
{
	//Public scene information
	public float shipRadius;
	public float asteroidRadius;
	public GameObject ship;
	public List<GameObject> asteroids;

	//Private scene information
	private int numAsteroids;
	private CollisionDetection collisionScript;
	private AsteroidSpawn asteroidSpawnScript;
	private ShipLifeManager lifeManager;
	private ScoreKeeper scoreKeeper;
	private bool alive;
	private float invincibilityFrames;

	// Use this for initialization
	void Start () 
	{
		//Create easy to use references of the scripts attached to the Scene Manager
		collisionScript = GetComponent<CollisionDetection> ();
		asteroidSpawnScript = GetComponent<AsteroidSpawn> ();
		lifeManager = GetComponent<ShipLifeManager> ();
		scoreKeeper = GetComponent<ScoreKeeper> ();

		//Create a new asteroid list
		asteroids = new List<GameObject> ();

		//The starting cap for first-level asteroids is 5, the ship is alive to begin, and there are no invincibility frames to start
		numAsteroids = 5;
		alive = true;
		invincibilityFrames = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If there aren't enough first-level asteroids in the scene
		if(asteroids.Count < numAsteroids)
		{
			//Spawn a new first-level asteroid and add it to the master asteroid list
			asteroids.Add(asteroidSpawnScript.SpawnAsteroid());
		}

		//If the ship is not alive
		if(!alive)
		{
			//No further updates are necessary
			return;
		}

		//If the ship currently has invincibility frames
		if (invincibilityFrames > 0)
		{
			//Decrement the invincibility frames and color the ship red
			invincibilityFrames -= Time.deltaTime;
			ship.GetComponent<SpriteRenderer>().color = Color.red;
		}
		else
		{
			//Color the ship normally and reset invincibility frames to 0
			ship.GetComponent<SpriteRenderer>().color = Color.white;
			invincibilityFrames = 0;
		}

		//Start by looking at the first asteroid
		int asteroidIndex = 0;

		//While there are asteroids to check (for collisions)
		while(asteroidIndex < asteroids.Count)
		{
			//Grab the current asteroid from the master list and record its radius
			GameObject asteroid = asteroids[asteroidIndex];
			asteroidRadius = asteroid.GetComponent<AsteroidMovement>().radius;

			//Check to see if the ship has its shield deployed or if it is teleporting
			bool shield = ship.GetComponent<ShipAbilities>().IsShieldUp;
			bool teleport = ship.GetComponent<ShipAbilities>().IsTeleporting;

			//Check to see if the ship and the current asteroid are colliding using the Circle Collision method
			//Ignore the collision check if the ship's shields are up or if the ship is teleporting
			if(collisionScript.CircleCollision(ship, shipRadius, asteroid, asteroidRadius) && !shield && !teleport)
			{
				//If the asteroid is radioactive and second-level (the ship can collect asteroids of this type)
				if(asteroid.GetComponent<AsteroidMovement>().collectable && asteroid.GetComponent<AsteroidMovement>().secondLevel)
				{
					//Remove the asteroid from the master list, destroy it, and lower the asteroid index to make sure we
					//check the next asteroid (the index always increments at the end of the this update method, but
					//because we removed the current asteroid we need to check the current index, hence -- and ++
					asteroids.Remove(asteroid);
					Destroy(asteroid);
					asteroidIndex--;

					//Give the player a random amount of ability energy
					int energy = (int) Random.Range(50,100);
					ship.GetComponent<ShipAbilities>().AugmentRadioactiveEnergy(energy);

					//Give the player 100 points, and if they've reached a miletone, increment the number of asteroids to spawn
					if(scoreKeeper.AugmentScore(100))
					{
						numAsteroids++;
					}
				}
				//If the ship does not currently have invincibility frames, lose a life
				else if(invincibilityFrames == 0 && lifeManager.LoseLife())
				{
					//If the ship has lost all of its lives, explode it, set alive to false, and alert the Ship Movement component that
					//the ship is no longer functioning
					ship.GetComponent<ShipExplosion>().Explode();
					alive = false;
					ship.GetComponent<ShipMovement>().alive = false;
				}
				//If the ship didn't die from the hit and it currently doesn't have any invincibility frames
				else if(invincibilityFrames == 0)
				{
					//Give the ship some invincibility frames
					invincibilityFrames += 2;
				}
			}

			//Control variables for running through the list of bullets
			int numBullets = ship.GetComponent<BulletSpawn> ().bulletList.Count;
			int bulletIndex = 0;

			//If the asteroid is radioactive and second-level
			if(asteroid.GetComponent<AsteroidMovement>().collectable && asteroid.GetComponent<AsteroidMovement>().secondLevel)
			{
				//Don't bother checking for collisions against bullets, as bullets cannot destroy these asteroids
				bulletIndex = numBullets;
			}

			//While there are bullets to check
			while(bulletIndex < numBullets)
			{
				//Grab the current bullet from the master bullet list
				GameObject bullet = ship.GetComponent<BulletSpawn> ().bulletList[bulletIndex];

				//If the bullet is colliding with the asteroid
				if(collisionScript.BulletCollision(asteroid, asteroidRadius, bullet))
				{
					//Remove the bullet from the master list and destroy it
					ship.GetComponent<BulletSpawn> ().bulletList.Remove(bullet);
					Destroy(bullet);

					//Stop the bullet collision checks for this asteroid, as they won't affect anything
					bulletIndex = numBullets;

					//If the asteroid is first-level
					if(!asteroid.GetComponent<AsteroidMovement>().secondLevel)
					{
						//Pick a number of smaller asteroids to spawn
						int numOfSmallerAsteroids = (int) Mathf.Floor(Random.Range (2, 5));

						//Spawn each second-level asteroid and add it to the master list
						for (int i = 0; i < numOfSmallerAsteroids; i++)
						{
							asteroids.Add(asteroidSpawnScript.SpawnSecondLevelAsteroid(asteroid));
						}

						//Give the player 20 points, and if they hit a score milestone increase the number of spawned asteroids
						if(scoreKeeper.AugmentScore(20))
						{
							numAsteroids++;
						}
					}
					//If the asteroid is second-level
					else
					{
						//Give the player 50 points, and if they hit a score milestone increase the number of spawned asteroids
						if(scoreKeeper.AugmentScore(50))
						{
							numAsteroids++;
						}
					}

					//Remove the asteroid from the master list and destroy it
					asteroids.Remove(asteroid);
					Destroy(asteroid);
					asteroidIndex--;
				}
				else
				{
					//If the bullet didn't collide with the asteroid, check the next bullet
					bulletIndex++;
				}
			}
			//Check the next asteroid
			asteroidIndex++;
		}
	}
}
