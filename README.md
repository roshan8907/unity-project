# 🏰 Dungeon Escape (RogueLike Game)

**Student:** Roshan Dhakal
**Course:** IA623002 – Introductory Game Development
**Engine:** Unity
**Genre:** 2D RogueLike Action Game

---

## 📖 Game Overview

Dungeon Escape is a 2D RogueLike action game developed in Unity. Players explore dangerous dungeon levels, battle enemies, collect health pickups, earn points, and progress through increasingly challenging stages. The ultimate objective is to survive all levels and defeat the final boss to escape the dungeon.

The project demonstrates the use of Unity game development principles, object-oriented programming, enemy AI, collision systems, scene management, sound effects, scoring systems, and level progression mechanics.

---

## 🎮 Play the Game

### Web Version (Image Background)

https://roshanio.itch.io/dunegon-escape-devil-edition-by-roshan

### Downloadable Version (Video Background)

https://roshanio.itch.io/dunegon-escape-devil-edition-by-roshan

**Note:**
The downloadable version contains the original animated video background. The web version uses image backgrounds because Unity WebGL has limitations with the video background implementation used in the project.

---

## 🎯 Game Features

### Main Menu

* Start Game
* High Scores
* Exit Game

### Player System

* Smooth player movement
* Unity Input System
* Health management

### Enemy System

* Enemy AI follows the player
* Enemy attack mechanics
* Dynamic enemy spawning

### Combat System

* Player attacks enemies
* Enemies damage player
* Boss battle mechanics

### Health System

* Player health display
* Enemy health display
* Boss health display
* Health pickup items

### Score System

* Score increases when enemies are defeated
* Real-time score display
* High score tracking

### Audio System

* Background music
* Boss battle music
* Sword attack sound effects
* Enemy attack sounds
* Health pickup sounds
* Teleport sounds
* Win and Game Over sounds

### Pause System

* Press **P** to pause the game
* Press **P** again to resume

### Scene Management

* Main Menu
* Multiple Levels
* High Score Scene
* Game Over Scene
* Win Scene

---

## 🗺️ Levels

### Level 1

Introduction to dungeon exploration and combat.

### Level 2

Increased enemy difficulty and challenges.

### Level 3

Advanced combat and survival mechanics.

### Boss Battle

Final battle against the dungeon boss.

---

## ⭐ Tier 2 Mechanics

* Multiple levels
* Background music
* Boss music
* Random dungeon generation
* DungeonGenerator system

---

## ⭐ Tier 3 Mechanics

### Boss Battle

* Dedicated boss enemy
* Boss health system
* Victory condition

### Teleportation System

* Teleport pads
* Level progression
* Area transitions

---

## 🔧 Custom Mechanics

### Health Pickup System

Collect health items to restore player health.

### Heart Pulse Effect

Animated visual effect for player health feedback.

### Enemy Spawn System

Dynamic enemy spawning throughout gameplay.

### Level Exit System

Transition between levels using teleports.

---

## 📊 Data Structures Used

### Integer Variables

Used for:

* Health
* Score
* Damage
* Enemy Health

Example:

```csharp
public int health = 100;
public static int score = 0;
```

### Arrays and Lists

Used for:

* Enemy collections
* Spawn locations
* Scene object management

### Classes

Examples:

* PlayerHealth
* EnemyHealth
* ScoreManager
* EnemySpawner
* DungeonGenerator

### GameObjects

Used for:

* Player
* Enemy
* Boss
* Health Pickup
* Teleport Pad

---

## 🧠 Algorithms Used

### Enemy Follow Algorithm

Enemies calculate the direction to the player and move toward the target.

### Collision Detection

Unity Collider2D system handles:

* Combat interactions
* Pickups
* Teleport triggers

### Health Reduction Algorithm

Damage values reduce health points.

### Scene Transition Algorithm

Loads the next level when progression conditions are met.

### Score Tracking Algorithm

Updates score when enemies are defeated.

---

## 💻 Code Quality

* Organized Scripts folder structure
* Meaningful class names
* Meaningful variable names
* Consistent coding style
* XML comments
* Minimal dead code
* Efficient Update() usage
* Object-oriented design principles

---

## 📸 Screenshots

### Main Menu

* Start Game
* High Scores
* Exit Game

### Level 1 Gameplay

### Level 2 Gameplay

### Level 3 Gameplay

### Boss Battle

### High Score Screen

### Game Over Screen

### Win Screen

*(Insert screenshots here if desired.)*

---

## 🏆 Learning Outcomes

This project demonstrates:

* Unity 2D Game Development
* Object-Oriented Programming (OOP)
* Scene Management
* Enemy AI
* Collision Detection
* Health and Scoring Systems
* Audio Integration
* User Interface Design
* Level Progression
* Boss Battle Design

---

## 📄 Conclusion

Dungeon Escape is a fully playable RogueLike game developed in Unity. The game includes combat mechanics, enemy AI, pickups, multiple levels, a boss battle, scoring systems, menus, sound effects, pause functionality, high score tracking, and win/game-over scenes.

This project successfully demonstrates the core concepts and practical skills required for the IA623002 Introductory Game Development assessment while providing an engaging gameplay experience.
