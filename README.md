# BounceDash
A 2D Unity platformer where players bounce on platforms, dodge obstacles, and collect coins and gems to achieve the highest score. 
Features endless procedural levels, smooth asynchronous scene loading, and a vibrant doodle-style aesthetic.

# Features
1)Endless vertical platforming with procedurally generated levels
2)Responsive player movement using touch swipes or keyboard input
3)Varied platforms: normal, breakable, and springs for high jumps
4)Obstacles with animations and dynamic collision handling
5)Collect coins and gems for score boosts with audio feedback
6)Persistent scoring system with bonuses
7)Asynchronous scene transitions with a progress bar
8)Lightweight, mobile-friendly design with TextMeshPro UI
9)Audio effects for jumps, collectibles, and collisions

# Getting Started
1)Clone the repo
2)Open in Unity 6000.0+
3)Configure assets (audio clips, sprites, fonts) as described below.
4)Play and start bouncing!

# Requirements
Unity Version: 6000.0+ or later
Platforms: Windows and Android
Dependencies:
DOTween (add Package)
Audio clips for jumps, collectibles, and obstacles (user-provided)
Doodle-style sprites for player, platforms, obstacles, and UI (user-provided)

# How to Play
HomeScreen: Click "Play" to load the game with a progress bar.
Controls:
Desktop: A/Left or D/Right to move horizontally.
Mobile: Swipe left/right to move.
Gameplay:
Bounce on platforms to climb higher and increase your score.
Collect coins and gems for extra points.
Avoid obstacles; side/top collisions end the game with an animation.
Use springs for high jumps; breakable platforms collapse on contact.
Game Over: Restart via the popup to try again.

# Project Structure
Scripts:
Player.cs: Player movement, platform collisions, audio control
SceneTransitionManager.cs: Async scene loading with progress bar
LevelGenerator.cs: Procedural platform and obstacle generation
Obstacle.cs & IObstacle.cs: Obstacle collision and animation logic
FlyEnemy.cs: Horizontal obstacle movement
ScoreManager.cs: Score tracking and bonuses
UIManager.cs: UI management for score and game over
AudioManager.cs & PlayerAudioControl.cs: Audio playback
CoinCollect.cs & GemsCollect.cs & ICollect.cs: Collectible logic
Scenes: HomeScreen, Loading, GameplayScreen
Resources: Popups/GameOverUI, user-provided audio/sprites
















