Project Title:

Asteroids


Project Description:

This project recreates the classic Atari videogame, “Asteroids”.  The player (ship) must avoid colliding into enemies (asteroids) that fly around the screen, while shooting them to break them apart and gain points. The game is over if the player gets hit three times. The reference for this project (another version of “Asteroids”) can be found here: http://www.freeasteroids.org/


User Responsibilities:

-Please play the game using the default screen size (1024, 768) as some things may look bizarre if you make the screen bigger or smaller. Also the ability points dialogue may look out of place, it ended up being placed in a different location on export.

-Press the arrow keys to move the ship. The ‘Left’ and ‘Right’ keys will rotate the ship to the left and right respectively. The ‘Up’ key will move the ship forward in its current direction. The ‘Down’ key will move the ship backwards (opposite its current direction).

-Press the ‘Space Bar’ to shoot bullets from the ship’s two main laser cannons. Shots fired will alternate between being shot from the left cannon and the right cannon.

-Press the ‘1’ key (not on the numpad) to activate your shield ability. You must hold this key for as long as you would like the shield to be active. You cannot use an ability (shield, teleport, harvest, quick-fire) if you do not have enough ability energy to activate that specific ability. Your ability energy can be monitored in the top left of the screen by the large green bar. This ability costs 2 energy points per frame of use.

-Press the ‘2’ key (not on the numpad) to activate your teleport ability. Once you have activated this ability you will see purple rings surrounding the mouse when it is in the game window. To select a teleport location, hover over the desired spot with your mouse and click the ‘Left Mouse Button’. If you wish to cancel the teleport ability before use, you may do so by clicking the ‘Right Mouse Button’. You may move while in the first phase of teleporting (after you have selected a location and have begun teleporting), but you may not shoot. You may move and shoot while in the second phase of teleporting and after you have finished teleporting. This ability costs 200 energy points per use.

-Press the ‘3’ key (not on the numpad) to activate your harvest ability. While you are using this ability any second-level radioactive asteroids (green asteroids that have been shot once) will accelerate towards you so that you may have an easier time collecting them. If you tap this ability, qualifying asteroids will set a new course for your current location (at the time of activation only). If you hold this ability, qualifying asteroids will actively seek your ship and accelerate towards it. This ability costs 1 energy point per frame of use.

-Press the ‘4’ key (not on the numpad) to toggle your quick-fire ability. While activated, the time between your bullets firing is significantly reduced. You can turn this ability on and off at will, without penalty. Each quick-fire bullet costs 20 energy points.

-Navigate the asteroid field without getting hit. If you collide with an asteroid 3 times, your ship will explode and the game will be over. After a collision you will be given a couple of seconds of invincibility frames. Shoot asteroids to gain points. You cannot shoot second level radioactive asteroids, but you can collect them by running into them. Collecting / harvesting second level radioactive asteroids will restore some of your ability energy. Use your abilities to your advantage! They will aid you greatly in sticky situations as well as maximize your mobility and efficiency if used correctly.


Above and Beyond:

-I created all of the assets that I used for this project. The artwork isn’t that great… but I put a lot of time into trying to make each sprite have a good level of quality. I also spent a good bit of time researching how to effectively make decent looking asteroids using GIMP.

-When the ship dies, it will explode and the two main engines will detach from the main capsule. The capsule will endlessly drift in its current direction and the two engines will rotate at randomly set speeds.

-I created a radioactive asteroid which when broken into smaller pieces can be collected (not shot, because that would be too easy). The player simply needs to collide with these smaller green asteroids in order to regain ability energy, which they can use to activate their 4 abilities.

-I created 4 abilities along with animations (through particle systems) for each. The shield ability will allow the player to keep any lives that would have been lost. The teleport ability gives the player temporary invincibility (before teleport) at the cost of not being able to attack, and moves the player to the specified new location. The harvest ability creates a magnet effect (only while active) that attracts collectable radioactive asteroids to the ship. The quick-fire ability allows the player to significantly lower their bullet cooldown. Ability energy can be monitored via the green bar in the top-left corner of the screen, under the life counter display.

-The number of asteroids in the game (counting first-level spawns only) is initially capped at 5, but for every 500 points that the player gets, the cap will be increased by one. This effectively makes the game harder and faster paced as the player gets a higher score.

-The thrusters on the ship respond fairly accurately to controls used. By this, I mean if you move forward, the correct thrusters on the ship will activate, etc. Because the ship slows to a standstill when input from the user stops, the appropriate thrusters will activate to recover the ship and bring it to a halt (Try moving forward / backward and letting the ship stop itself).

-The speed of bullets fired from the ship is respective to the speed of the ship so you can’t do something ridiculous like catch up to your own bullets.


Known Issues:

-Sometimes the asteroids overlap each other and trade places in the layer hierarchy. It looks odd when asteroids randomly pop over each other, but they all share the same z position, so I’m not sure why they would randomly change.

-Occasionally asteroids will pick a direction and starting location that causes them to just barely appear in a corner of the screen before leaving. This isn’t necessarily a concern, as it will de-spawn and a new asteroid will take its place, but early on in the game there are times when the screen will be mostly empty. 

-The thrusters on the ship don’t always behave properly, especially if you try a complicated sequence of motions like repeated rotations and forward / backward movement. Sometimes they will be set to recover the ship when the ship doesn’t need to be recovered. However, this odd behavior is an exception and most of the time it’s not noticeable.

-Sometimes the animations associated with the abilities make the duration of the ability confusing. For instance, as soon as the player lets go of the ‘1’ key the shield will stop being effective, but the animation (although shutting down) appears to be active for longer than it is.

-Some of the ability animations persist upon death, but I actually liked this effect so I kept it.


Notes:

-Just to reiterate, all assets were created by me, but I did use this tutorial to learn how to create asteroids in GIMP: https://www.youtube.com/watch?v=HDahVd2Q49k

