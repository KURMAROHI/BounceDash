# BounceDash
A 2D Unity platformer where players bounce on platforms, dodge obstacles, and collect coins and gems to achieve the highest score. 
Features endless procedural levels, smooth asynchronous scene loading, and a vibrant doodle-style aesthetic.

## Features
1. Endless vertical platforming with procedurally generated levels
2. Responsive player movement using touch swipes or keyboard input
3. Varied platforms: normal, breakable, and springs for high jumps
4. Obstacles with animations and dynamic collision handling
5. Collect coins and gems for score boosts with audio feedback
6. Persistent scoring system with bonuses
7. Asynchronous scene transitions with a progress bar
8. Lightweight, mobile-friendly design with TextMeshPro UI
9. Audio effects for jumps, collectibles, and collisions


## Getting Started
1. Clone the repo
2. Open in Unity 6000.0+
3. Configure assets (audio clips, sprites, fonts) as described below.
4. Play and start bouncing!

# Requirements
1. Unity Version: 6000.0+ or later
2. Platforms: Windows and Android
- Dependencies:
3. DOTween (add Package)
4. Audio clips for jumps, collectibles, and obstacles (user-provided)
5. Doodle-style sprites for player, platforms, obstacles, and UI (user-provided)

# How to Play
1. HomeScreen: Click "Play" to load the game with a progress bar.
- Controls:
2. Desktop: A/Left or D/Right to move horizontally.
3. Mobile: Swipe left/right to move.
- Gameplay:
4. Bounce on platforms to climb higher and increase your score.
5. Collect coins and gems for extra points.
6. Avoid obstacles; side/top collisions end the game with an animation.
7. Use springs for high jumps; breakable platforms collapse on contact.
8. Game Over: Restart via the popup to try again.

# Project Structure
- Scripts:
1. Player.cs: Player movement, platform collisions, audio control
2. SceneTransitionManager.cs: Async scene loading with progress bar
3. LevelGenerator.cs: Procedural platform and obstacle generation
4. Obstacle.cs & IObstacle.cs: Obstacle collision and animation logic
5. FlyEnemy.cs: Horizontal obstacle movement
6. ScoreManager.cs: Score tracking and bonuses
7. UIManager.cs: UI management for score and game over
8. AudioManager.cs & PlayerAudioControl.cs: Audio playback
9. CoinCollect.cs & GemsCollect.cs & ICollect.cs: Collectible logic
10. Scenes: HomeScreen,GameplayScreen
11. Resources: Popups/GameOverUI, user-provided audio/sprites




## Short Note on Game Feel and Optimization

1. Game Feel:
    Controls: Emphasizes Player.cs’s smooth movement with _touchSensitivity for mobile swipes and 
    velocity clamping for desktop, ensuring responsive input (_horizontalSpeed, _maxHorizontalSpeed).

2. Feedback: 
    Highlights audio cues for jumps (_jumpClip, _HightjumpClip in PlayerAudioControl.cs),
    collectibles (_coinCollectClip, _gemCollectClip in CoinCollect.cs, GemsCollect.cs),
    and collisions (_hurtClip), managed by AudioManager.cs.

3. Visuals: 
    Mentions text UI with Bangers/Comic Neue (UIManager.cs) and platform 
    animations (BreakPlatForm, Spring tags in Player.cs) for a cohesive, lively aesthetic.

## Optimization:

1. Async Loading: SceneTransitionManager.cs uses SceneManager.LoadSceneAsync 
with a progress bar (_progressBar, _progressText) to ensure smooth 
transitions, critical for mobile.

2. Physics: Uses lightweight 2D physics with trigger-based collisions 
(Obstacle.cs, CoinCollect.cs, GemsCollect.cs) to reduce performance 
overhead, as seen in OnTriggerEnter2D.

3. Efficiency: Singletons (ScoreManager.cs, AudioManager.cs, UIManager.cs) with
DontDestroyOnLoad minimize reinstantiation. LevelGenerator.cs uses ScriptableObjects 
(_platformData, _itemData) for decoupling and overlap checks for efficient procedural generation.











