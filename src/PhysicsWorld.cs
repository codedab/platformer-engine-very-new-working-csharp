using Microsoft.Xna.Framework;

namespace PlatformerEngine;

/// <summary>
/// Simulates a 2D physics world with constant gravity and AABB collision
/// against registered solid tiles. Uses MonoGame's Rectangle for tile data.
///
/// Gravity: 9.81 m/s² downward (positive Y). Applied once per Step() call.
/// Fixed timestep: 1/60 seconds per step.
/// </summary>
public class PhysicsWorld
{
    private const float Gravity = 9.81f;

    private readonly List<PhysicsBody> _bodies     = new();
    private readonly List<Rectangle>   _solidTiles = new();

    /// <summary>Registers a physics body to be simulated each step.</summary>
    public void AddBody(PhysicsBody body)
    {
        if (body is null) throw new ArgumentNullException(nameof(body));
        _bodies.Add(body);
    }

    /// <summary>Registers a MonoGame Rectangle as a solid collidable tile.</summary>
    public void AddSolidTile(Rectangle tile)
    {
        _solidTiles.Add(tile);
    }

    /// <summary>
    /// Advances the simulation by deltaTime seconds.
    /// Applies gravity exactly once, integrates position, then resolves collisions.
    /// </summary>
    public void Step(float deltaTime)
    {
        foreach (var body in _bodies)
        {
            // Apply gravity exactly once per step
            body.Velocity.Y += Gravity * deltaTime;

            // Integrate position
            body.Position.X += body.Velocity.X * deltaTime;
            body.Position.Y += body.Velocity.Y * deltaTime;
        }

        ResolveCollisions();
    }

    private void ResolveCollisions()
    {
        foreach (var body in _bodies)
            foreach (var tile in _solidTiles)
                Collision.Resolve(body, tile);
    }
}
