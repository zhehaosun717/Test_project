# Lab 1 Assessment

## 1. Project Description

In this Unity interactive prototype, I created a level where the player controls a cube that can move around a plane. As it moves, it leaves a dynamic, rainbow-colored trail of spheres. Each sphere in the trail disappears after five seconds. The player observes the action from a top-down perspective.

The core mechanic involves a QTE (Quick Time Event) system. During gameplay, clickable buttons will occasionally appear on the screen for a short duration. If the player successfully clicks a button in time (2 seconds), the cube grows slightly. After every two successful clicks, the cube levels up and changes its color, providing clear visual feedback on the player's progress.

![](C:\Users\A\Test_project\Assets\Assets\image-20250921143853008.png)

## 2. Features

-   **Player Control**: The cube can be controlled using the keyboard (WASD) or a gamepad to move around the scene.
-   **Dynamic Sphere Trail**: As the cube moves, it generates a trail of spheres behind it.
-   **Proportional Trail Scaling**: The size of the spheres in the trail scales proportionally with the cube's growth.
-   **QTE System**:
    -   A QTE button randomly appears in the **central area** of the screen **every 3 seconds**.
    -   The button itself is a **radially filled** circular progress bar that acts as a **2-second** countdown timer.
    -   The button disappears after timing out or being successfully clicked.
-   **Growth and Level-Up Mechanic**:
    -   Each successful QTE click increases the cube's size by 10%.
    -   The cube levels up after every **two** successful clicks.
    -   Leveling up changes the cube's color in the sequence of Red, Orange, Yellow, Green, Cyan, Blue, and Violet.
-   **Audio System**:
    -   Background music (BGM) plays on a loop throughout the game.
    -   Corresponding sound effects are played when the QTE button appears and when it is successfully clicked.



![](C:\Users\A\Test_project\Assets\Assets\image-20250921144110046.png)

## 3. How the Code Works

This project consists of five core scripts that work together to create the gameplay experience.

### `CS_PlayerController.cs` - Player Controller
This script is the brain of the player's cube. It handles movement based on player input, spawns the trail spheres, and manages the cube's state, including its size and color. It also features a public function, `OnQTESuccess()`, which allows the QTE Manager to tell the cube to grow and level up. As the cube grows, this script also ensures that newly spawned trail spheres are scaled up proportionally.

### `CS_TrailSphere.cs` - Trail Sphere Script
Attached to each sphere in the trail, this script has a simple but vital job: to manage the sphere's lifecycle. It ensures each sphere is destroyed after 5 seconds and, crucially, creates an independent material instance for each one. This prevents the color-changing effect of new spheres from affecting those already in the world.

### `CS_QTEManager.cs` - QTE Manager
This script acts as the director of the QTE system. Using a coroutine, it periodically spawns a new QTE button prefab at a random position within a safe, central screen area. It passes a reference of itself to the new button, allowing the button to report back upon a successful click. It also plays sound effects for the button's appearance and for a successful interaction.

### `CS_QTEButton.cs` - QTE Button Script
This script gives the QTE button its functionality. It drives the visual countdown timer by updating the `Image` component's fill amount each frame. It listens for player clicks and, when clicked, notifies the `CS_QTEManager`. If the timer runs out before a click, it destroys its parent prefab (including the background) to cleanly remove itself from the scene.

### `CS_BGMPlayer.cs` - BGM Player
A dedicated script for handling background music. It ensures that a specified audio clip is loaded and played on a loop as soon as the game starts.
