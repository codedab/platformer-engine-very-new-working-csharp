using Microsoft.Xna.Framework;

namespace PlatformerEngine;

/// <summary>
/// Represents a physics-simulated body in the 2D world.
/// Uses MonoGame's Vector2 and Rectangle for framework compatibility.
/// </summary>
public class PhysicsBody
{
    public Vector2 Position;
    public Vector2 Velocity;
    public float Width;
    public float Height;

    public PhysicsBody(float x, float y, float width, float height)
    {
        Position = new Vector2(x, y);
        Velocity = Vector2.Zero;
        Width    = width;
        Height   = height;
    }

    /// <summary>Returns the right edge X coordinate of this body.</summary>
    public float Right  => Position.X + Width;

    /// <summary>Returns the bottom edge Y coordinate of this body.</summary>
    public float Bottom => Position.Y + Height;

    /// <summary>Returns the bounding rectangle of this body.</summary>
    public Rectangle Bounds => new Rectangle(
        (int)Position.X, (int)Position.Y,
        (int)Width,      (int)Height);
}
