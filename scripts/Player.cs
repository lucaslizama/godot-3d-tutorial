using Godot;

public partial class Player : CharacterBody3D
{
    [Export]
    public int Speed = 14;

    [Export]
    public int FallAcceleration = 75;

    [Export]
    public int JumpImpulse = 20;

    [Export]
    public int BounceImpulse = 16;

    private Vector3 _velocity = Vector3.Zero;
    [Signal]
    public delegate void HitEventHandler();

    private void Die()
    {
        EmitSignal(nameof(Hit));
        QueueFree();
    }

    public void OnMobDetectorBodyEntered(Node body)
    {
        Die();
    }

    public override void _PhysicsProcess(double delta)
    {
        var direction = Vector3.Zero;

        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1f;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction.Z += 1f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.Z -= 1f;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
            GetNode<Node3D>("Pivot").LookAt(Position + direction, Vector3.Up);
        }

        _velocity.X = direction.X * Speed;
        _velocity.Z = direction.Z * Speed;
        _velocity.Y -= FallAcceleration * (float)delta;
        _velocity.Y = Mathf.Clamp(_velocity.Y, -10f, float.MaxValue);

        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _velocity.Y = JumpImpulse;
        }

        for (int index = 0; index < GetSlideCollisionCount(); index++)
        {
            var collision = GetSlideCollision(index);

            if (collision.GetCollider() is Mob mob && mob.IsInGroup("mob"))
            {
                if (Vector3.Up.Dot(collision.GetNormal()) > 0.1)
                {
                    mob.Squash();
                    _velocity.Y = BounceImpulse;
                }
            }
        }

        Velocity = _velocity;

        MoveAndSlide();
    }
}
