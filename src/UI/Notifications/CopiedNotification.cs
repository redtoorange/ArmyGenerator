using Godot;

public partial class CopiedNotification : Panel
{
    [Export] private float deltaYOffset = 30;
    [Export] private float tweenTime = 1;
    [Export] private float hideTime = 5;

    public void DisplayNotification()
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this, "position", Position + new Vector2(0, -deltaYOffset), tweenTime)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Quad);
        tween.Finished += HandleTweenFinished;
    }

    private void HandleTweenFinished()
    {
        GetTree().CreateTimer(hideTime).Timeout += HandleTimeout;
    }

    private void HandleTimeout()
    {
        Tween tween = CreateTween();
        tween.TweenProperty(this, "position", Position + new Vector2(0, deltaYOffset), tweenTime)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Quad);
    }
}