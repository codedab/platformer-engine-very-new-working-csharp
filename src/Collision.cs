using Microsoft.Xna.Framework;

namespace PlatformerEngine;

/// <summary>
/// AABB collision detection and resolution using MonoGame Rectangle.
/// Resolves along the axis of minimum penetration only, snapping the body
/// flush to the tile boundary and zeroing velocity on the resolved axis.
/// </summary>
public static class Collision
{
    /// <summary>
    /// Tests whether a PhysicsBody overlaps a Rectangle tile.
    /// </summary>
    public static bool Overlaps(PhysicsBody body, Rectangle tile)
    {
        return body.Position.X < tile.X + tile.Width
            && body.Right      > tile.X
            && body.Position.Y < tile.Y + tile.Height
            && body.Bottom     > tile.Y;
    }

    /// <summary>
    /// Resolves overlap between a PhysicsBody and a solid Rectangle tile.
    /// Resolves along the axis of minimum penetration only.
    /// </summary>
    public static void Resolve(PhysicsBody body, Rectangle tile)
    {
        if (!Overlaps(body, tile))
            return;

        float overlapLeft  = body.Right  - tile.X;
        float overlapRight = (tile.X + tile.Width)  - body.Position.X;
        float overlapTop   = body.Bottom - tile.Y;
        float overlapDown  = (tile.Y + tile.Height) - body.Position.Y;

        float minX = Math.Min(overlapLeft, overlapRight);
        float minY = Math.Min(overlapTop,  overlapDown);

        // Resolve along the axis with the smaller penetration depth only
        if (minX < minY)
        {
            if (overlapLeft < overlapRight)
            {
                body.Position.X -= overlapLeft;
            }
            else
            {
                body.Position.X += overlapRight;
            }
            body.Velocity.X = 0f;
        }
        else
        {
            if (overlapTop < overlapDown)
            {
                body.Position.Y -= overlapTop;
            }
            else
            {
                body.Position.Y += overlapDown;
            }
            body.Velocity.Y = 0f;
        }
    }
}
