using System.Numerics;
using Raylib_cs;

public class Ball : Component
{
    private int speed = 100;
    private int speedScale = 4;
    private float despawnTimeLeft;
    private float despawnTimer = 5;
    private Vector2 direction = Vector2.Zero;

    public delegate void BallEvent(Ball ball);
    public event BallEvent OnDespawn;

    public void Reinitialize() {
        despawnTimeLeft = despawnTimer;
    }

    public void SetDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void HandleMovement()
    {
        if (this.direction != Vector2.Zero)
            GameObject.position += Vector2.Normalize(this.direction) * speed * speedScale * Raylib.GetFrameTime();
    }

    public void HandleDespawn()
    {
        despawnTimeLeft -= Raylib.GetFrameTime();

        if (despawnTimeLeft <= 0)
        {
            GameObject.enabled = false;
            OnDespawn?.Invoke(this);
        }
    }

    public override void Initialize()
    {
        GameObject.name = "Ball";
    }

    public override void Update()
    {
        HandleMovement();
        HandleDespawn();
    }
}