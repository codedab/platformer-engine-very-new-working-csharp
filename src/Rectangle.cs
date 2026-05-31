namespace PlatformerEngine;

/// <summary>
/// Axis-aligned rectangle used for solid tiles and bounding boxes.
/// </summary>
public struct Rectangle
{
    public float X;
    public float Y;
    public float Width;
    public float Height;

    public Rectangle(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public float Right  => X + Width;
    public float Bottom => Y + Height;

    public override string ToString() => $"Rect({X}, {Y}, {Width}, {Height})";
}
