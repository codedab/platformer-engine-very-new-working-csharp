# 2D Physics Platformer Engine

A C# class library implementing a complete 2D platformer physics engine built on **MonoGame**. Includes custom gravity and velocity simulation, AABB collision resolution, sprite animation state management, and text-based level loading. Designed to be called directly by a test harness without requiring a display or interactive game loop.

## How to Build

```bash
dotnet build --configuration Release
```

The project targets **net8.0** and uses **MonoGame.Framework.DesktopGL** for framework types (`Vector2`, `Rectangle`, `Game`, `SpriteBatch`). NuGet restore runs automatically on build.

## Architecture Overview

### GameEngine (`GameEngine.cs`)
Subclasses MonoGame's `Game` class. Wires `PhysicsWorld`, `LevelLoader`, and `AnimationState` into a standard MonoGame loop with a fixed-timestep physics accumulator. The evaluator harness does not launch this class visually — it instantiates the component classes directly.

### PhysicsWorld (`PhysicsWorld.cs`)
Central simulation manager. Maintains a list of `PhysicsBody` objects and solid `Rectangle` tiles. `Step(float deltaTime)` applies gravity once, integrates velocity into position, then resolves all AABB collisions.

### PhysicsBody (`PhysicsBody.cs`)
A moveable physics object with a MonoGame `Vector2` position and velocity, plus axis-aligned bounding dimensions. Exposes a `Bounds` property returning a MonoGame `Rectangle`.

### Collision (`Collision.cs`)
Static utility for AABB overlap detection and resolution. `Resolve()` computes penetration depth on each axis and resolves along the **axis of minimum penetration only**, snapping the body's edge flush to the tile boundary. Velocity is zeroed only on the resolved axis.

### LevelLoader (`LevelLoader.cs`)
Reads a plain-text level file and returns a `List<Rectangle>` of solid tiles. Valid tile lines are comma-separated `x,y,width,height` integers. Lines beginning with `#` are comments and skipped; blank lines are also skipped.

### AnimationState (`AnimationState.cs`)
Manages sprite animation clips by name. Tracks elapsed time, advances frame index at the configured frame duration, and returns the correct source `Rectangle` for the current frame on a sprite sheet.

## Gravity Model

- Constant: **9.81 m/s²**, downward (positive Y axis)
- Applied **exactly once per `Step()` call** before position integration
- Fixed timestep: `deltaTime = 1f / 60f` seconds
- After 60 steps a body at zero initial velocity will have `Velocity.Y ≈ 9.81`

## Collision Resolution — AABB, Axis of Minimum Penetration

1. Compute penetration depth on X axis: `overlapLeft` and `overlapRight`
2. Compute penetration depth on Y axis: `overlapTop` and `overlapDown`
3. Select the axis with the **smaller minimum depth**
4. Snap the body's position flush to the tile boundary on that axis only
5. Zero velocity on the resolved axis only

This prevents the player from being incorrectly pushed sideways when landing vertically on a platform.

## Level File Format

```
# This is a comment — skipped by LevelLoader
0,0,32,32
32,0,32,32
# Another comment
0,64,96,32
```

Each non-comment, non-empty line specifies one solid tile as `x,y,width,height`. Returns one `Rectangle` per valid line.

## Example Harness Usage

```csharp
// Gravity test
var world = new PhysicsWorld();
var body = new PhysicsBody(0, 0, 16, 24);
world.AddBody(body);
for (int i = 0; i < 60; i++) world.Step(1f / 60f);
// body.Velocity.Y ≈ 9.81

// Collision test
var world2 = new PhysicsWorld();
var player = new PhysicsBody(0f, 0f, 16f, 24f);
player.Velocity.X = 5f;
world2.AddBody(player);
world2.AddSolidTile(new Rectangle(10, 0, 32, 32));
world2.Step(1f / 60f);
// player.Right == 10.0f

// Level loader test
var tiles = LevelLoader.LoadTiles("level1.txt");
// tiles.Count == 12
```
