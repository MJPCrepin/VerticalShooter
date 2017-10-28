VertShooterDemo
====
Manuel Crepin (manuel@crep.in), Oct'17

An infinite vertical shooter game:
Escape the hostile planet
Kill enemies for a power boost
Press any button to begin

## Mechanics

* Camera follows the player's max height, which acts as score
* Enemies spawn infinitely and aim around (at random) the player
* Player auto-aims to closest enemy (in 3d space)
* Enemies/player die when out of HP/power
* All gameObjects cleaned up when out of bounds

## Potential improvements
* Create a parent bullet object player/enemies inherit
* Better responsive UI (scale to screen)
* Pause functionality
* Custom sprites for explosions
* Audio

## Tools used
Unity 5.6.1f1, coded in Visual Studio 2017
ProCore Probuilder2 Advanced v2.9.4f1 (used exclusively for model creation)