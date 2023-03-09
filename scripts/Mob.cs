using Godot;

public partial class Mob : CharacterBody3D
{
    [Export]
    public int MinSpeed = 10;
    [Export]
    public int MaxSpeed = 18;
    [Signal]
    public delegate void SquashedEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);

        RotateY((float)GD.RandRange(-Mathf.Pi / 4.0, Mathf.Pi / 4.0));

        float randomSpeed = (float)GD.RandRange(MinSpeed, MaxSpeed);

        Velocity = Vector3.Forward * randomSpeed;
        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);
    }
    private void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }

    public void Squash()
    {
        EmitSignal(nameof(Squashed));
        QueueFree();
    }
}


