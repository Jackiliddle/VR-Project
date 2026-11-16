# Bin the buNS

A Unity VR garden game prototype presented in the main scene as **shoo, bunnies!** Protect your vegetables from rabbits, catch them to earn points, and keep the garden alive for as long as possible.

## Gameplay

Choose Easy, Medium, or Hard to begin a countdown. Vegetables grow as the game starts, and rabbits emerge from burrows. Rabbits seek vegetables, wander around them, and eat them after completing a configured number of laps. They also flee when the player gets close.

The main scene's instructions describe three rabbit types:

| Rabbit | Behaviour described in the scene | Points |
| --- | --- | --- |
| White | Slow movement, fast eating | 5 |
| Brown | Faster movement, faster eating | 10 |
| Black | Very fast movement, slow eating | 15 |

Higher difficulty increases the number of rabbits per wave and reduces the intervals between rabbits and new burrows. The game ends when no vegetables remain. The game manager tracks the current score and saves the high score locally using Unity `PlayerPrefs`.

## Requirements

* Unity **6000.3.0f1**, as recorded in `ProjectSettings/ProjectVersion.txt`.
* Git installed and available to Unity for the Git package dependency.
* Network access to restore Unity packages and the ExtralityLab package hosted at `gitea.dsv.su.se`.
* For headset testing, an appropriate OpenXR runtime and compatible XR hardware. A verified list of supported headsets is not supplied in this repository.
* For Android builds, the Android build support tools for this Unity editor version.

## Open the project

1. Clone the repository:

   ```sh
   git clone https://github.com/Jackiliddle/VR-Project.git
   ```

2. In Unity Hub, add the cloned `VR-Project` folder, which contains `Assets`, `Packages`, and `ProjectSettings`.
3. Open it with Unity **6000.3.0f1** and allow package restoration and asset importing to finish.
4. Open `Assets/Scenes/Main Scene.unity`.
5. Check the Console for missing scripts or import errors before entering Play mode.
6. Enter Play mode and select a difficulty from the title screen.

`Main Scene.unity` is the only enabled scene in the checked-in `EditorBuildSettings.asset`. Other scenes and imported demonstrations are also included.

If package restoration fails, check access to the ExtralityLab Git dependency declared in `Packages/manifest.json`:

```text
https://gitea.dsv.su.se/ExtralityLab/se.su.dsv.extralitylab.unity.git
```

## Controls and XR interaction

The main scene includes these desktop instructions, supported by the custom gameplay scripts:

| Input | Action |
| --- | --- |
| WASD or arrow keys | Move using the Horizontal and Vertical input axes |
| Spacebar | Catch the rabbit currently inside the catch trigger |
| Escape | Toggle pause while the game is active |

The project also contains XR Interaction Toolkit grab components and custom rabbit interaction scripts. `BunnyFlyToHand` looks for a child named `HandAttach` on the selecting interactor. `RabbitGrabPause` can suspend assigned movement components and adjust rigidbody physics while a rabbit is held, then restore them on release.

The XR Interaction Simulator is configured to instantiate automatically in the editor. Use its input feedback UI for simulator controls. Exact headset button bindings and a complete VR catching/scoring flow have not been verified; the keyboard catch handler explicitly awards points and destroys the target rabbit.

## Main dependencies

Versions below come from `Packages/manifest.json`.

| Package | Version |
| --- | --- |
| Universal Render Pipeline | 17.3.0 |
| Input System | 1.16.0 |
| XR Interaction Toolkit | 3.3.1 |
| OpenXR Plugin | 1.16.1 |
| XR Plug-in Management | 4.5.4 |
| XR Hands | 1.7.3 |
| Android XR OpenXR | 1.1.0 |
| AR Foundation | 6.3.2 |
| Cinemachine | 3.1.5 |
| Character Controller | 1.4.2 |
| ExtralityLab | Git URL in the package manifest |

Imported sample folders can contain older package version numbers than the versions currently declared in the manifest.

## Project structure

```text
Assets/
  Scenes/             Main game scene and additional scenes
  Scripts/            Custom gameplay and interaction scripts
  Prefabs/Pests/      White, brown, and black rabbit prefabs
  Prefabs/Veggies/     Carrot, eggplant, pumpkin, and turnip prefabs
  Imported Assets/    Imported art, controllers, UI, and other assets
  BOXOPHOBIC/         Skybox assets and utilities
  Samples/            Imported Unity and ExtralityLab examples
  VRTemplateAssets/   VR template content
  XR/                 XR loaders and configuration
  XRI/                XR interaction configuration
Packages/             Package manifest and lock file
ProjectSettings/      Unity editor, player, input, and build settings
```

Key custom scripts:

* `GameManager.cs`: menus, countdown, scoring, high score persistence, pause, restart, and game over.
* `Difficulty.cs`: starts the game with the button's configured difficulty.
* `PestSpawner.cs`: burrow appearance, rabbit waves, and difficulty scaling.
* `RabbitMover.cs`: movement, fleeing, vegetable targeting, and eating.
* `DetectCollisions.cs`: keyboard catching, points, sound, and particle effects.
* `CropGrower.cs` and `RootCropGrower.cs`: crop growth animations.
* `Veggie.cs`: requests a game over check when a vegetable is destroyed.
* `BunnyFlyToHand.cs` and `RabbitGrabPause.cs`: rabbit grab behaviour.
* `MusicToggle.cs`: music playback toggle.

## Build and validation status

Open **File > Build Profiles**, select the intended target platform, and confirm that `Assets/Scenes/Main Scene.unity` is included. Review XR Plug-in Management and OpenXR settings for that target before building.

The checked-in Android player settings specify ARM64 and a minimum Android API level of 32. These settings alone do not establish device compatibility or a successful build.

This documentation is based on static file inspection. The project has not been imported, compiled, run in Unity, or tested on a headset as part of preparing this README.

Some serialized component identifiers differ from the current script class names. For example, the main scene contains `Assembly-CSharp::DifficultyButton`, while `Difficulty.cs` defines `Difficulty`; the black rabbit prefab also contains identifiers such as `PestBehaviour` and `NewMonoBehaviourScript`. Check their script references in the Unity Inspector and resolve any missing components before treating the project as ready to play or distribute.

## Assets and licensing

The repository includes third-party assets and package samples, including BOXOPHOBIC content, Starter Assets, rabbits, animal assets, and farm UI assets. Their inclusion does not establish permission to redistribute them independently; consult the applicable asset and package licences.

No repository-level licence file was found in the inspected revision.

## Documentation reference

Prepared from repository revision [`b39c72b`](https://github.com/Jackiliddle/VR-Project/tree/b39c72b58a1dd7b68fda6d5205600cc9f1561a52), including the main scene, custom scripts, black rabbit prefab, package manifest, player settings, build settings, and XR simulator settings.
