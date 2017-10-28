VertShooterDemo
====
Manuel Crepin (manuel@crep.in), Oct'17

An infinite vertical shooter game:

Escape the hostile planet

Kill enemies for a power boost

Press any button to begin



To launch run release-1.0.exe or import to Unity and run from editor. A 16:9 aspect ratio is recommended (or health/power bar may be off screen).

## Mechanics

* Camera follows the player's max height, which acts as score
* Enemies spawn infinitely and aim around (at random) the player
* Player auto-aims to closest enemy (in 3d space)
* Enemies/player die when out of HP/power
* All gameObjects cleaned up when out of bounds

## Potential improvements
* Create a parent bullet object player/enemies inherit
* Different enemy types (different weapons/health?)
* Better enemy horizontal alignment (randomise?)
* Better responsive UI (made to fit 16:9 aspect ratio)
* Pause functionality
* Audio/skybox (no 3rd party asset used)

## Tools used
Unity 5.6.1f1, coded in Visual Studio 2017

ProCore Probuilder2 Advanced v2.9.4f1 (used exclusively for model creation)