using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerEngine;

/// <summary>
/// Manages sprite animation state for a platformer entity.
/// Tracks the current animation clip, frame index, and elapsed time.
/// </summary>
public class AnimationState
{
    private readonly Dictionary<string, (int StartFrame, int FrameCount, float FrameDuration)> _clips = new();

    public string CurrentClip    { get; private set; } = string.Empty;
    public int    CurrentFrame   { get; private set; }
    public float  ElapsedTime    { get; private set; }
    public bool   IsPlaying      { get; private set; }

    /// <summary>
    /// Registers an animation clip by name.
    /// </summary>
    public void RegisterClip(string name, int startFrame, int frameCount, float frameDuration)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Clip name cannot be empty.", nameof(name));
        if (frameCount <= 0)                 throw new ArgumentOutOfRangeException(nameof(frameCount));
        if (frameDuration <= 0f)             throw new ArgumentOutOfRangeException(nameof(frameDuration));

        _clips[name] = (startFrame, frameCount, frameDuration);
    }

    /// <summary>
    /// Switches to the named clip, resetting frame and time if the clip changed.
    /// </summary>
    public void Play(string clipName)
    {
        if (!_clips.ContainsKey(clipName))
            throw new KeyNotFoundException($"Animation clip '{clipName}' is not registered.");

        if (CurrentClip == clipName) return;

        CurrentClip  = clipName;
        CurrentFrame = 0;
        ElapsedTime  = 0f;
        IsPlaying    = true;
    }

    /// <summary>
    /// Advances the animation by deltaTime seconds.
    /// </summary>
    public void Update(float deltaTime)
    {
        if (!IsPlaying || string.IsNullOrEmpty(CurrentClip)) return;
        if (!_clips.TryGetValue(CurrentClip, out var clip))  return;

        ElapsedTime += deltaTime;

        if (ElapsedTime >= clip.FrameDuration)
        {
            ElapsedTime  -= clip.FrameDuration;
            CurrentFrame  = (CurrentFrame + 1) % clip.FrameCount;
        }
    }

    /// <summary>
    /// Returns the source rectangle for the current frame on a sprite sheet.
    /// </summary>
    public Rectangle GetSourceRectangle(int frameWidth, int frameHeight)
    {
        if (!_clips.TryGetValue(CurrentClip, out var clip))
            return new Rectangle(0, 0, 0, 0);

        int absoluteFrame = clip.StartFrame + CurrentFrame;
        return new Rectangle(absoluteFrame * frameWidth, 0, frameWidth, frameHeight);
    }

    /// <summary>Pauses animation advancement.</summary>
    public void Stop() => IsPlaying = false;

    /// <summary>Resumes animation advancement.</summary>
    public void Resume() => IsPlaying = true;
}
