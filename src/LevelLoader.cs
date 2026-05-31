using Microsoft.Xna.Framework;

namespace PlatformerEngine;

/// <summary>
/// Loads level tile data from plain-text level files into MonoGame Rectangle objects.
///
/// File format:
///   - Lines starting with '#' are comments and are skipped.
///   - Empty/whitespace lines are skipped.
///   - Each valid line: x,y,width,height  (integers or floats)
/// </summary>
public class LevelLoader
{
    /// <summary>
    /// Reads a level file and returns a list of solid tile rectangles.
    /// </summary>
    /// <param name="filePath">Path to the plain-text level file.</param>
    /// <returns>One Rectangle per valid (non-comment, non-empty) tile line.</returns>
    /// <exception cref="FileNotFoundException">Level file does not exist.</exception>
    /// <exception cref="FormatException">A tile line cannot be parsed as x,y,width,height.</exception>
    public static List<Rectangle> LoadTiles(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Level file not found: {filePath}");

        var tiles = new List<Rectangle>();

        foreach (var rawLine in File.ReadAllLines(filePath))
        {
            var line = rawLine.Trim();

            // Skip blank lines
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Skip comment lines
            if (line.StartsWith("#"))
                continue;

            var parts = line.Split(',');
            if (parts.Length < 4)
                throw new FormatException(
                    $"Invalid tile line (expected x,y,width,height): '{line}'");

            int x      = int.Parse(parts[0].Trim());
            int y      = int.Parse(parts[1].Trim());
            int width  = int.Parse(parts[2].Trim());
            int height = int.Parse(parts[3].Trim());

            tiles.Add(new Rectangle(x, y, width, height));
        }

        return tiles;
    }
}
