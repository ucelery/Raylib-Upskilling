using System.Numerics;
using Raylib_cs;

public class Player : Component
{
    private int speed = 100;
    private int speedScale = 4;
    private Vector2 direction = Vector2.Zero;

    public void HandleMovement()
    {
        this.direction.Y = (Raylib.IsKeyDown(KeyboardKey.W) ? -1 : 0) + (Raylib.IsKeyDown(KeyboardKey.S) ? 1 : 0);
        this.direction.X = (Raylib.IsKeyDown(KeyboardKey.A) ? -1 : 0) + (Raylib.IsKeyDown(KeyboardKey.D) ? 1 : 0);

        if (this.direction != Vector2.Zero)
            GameObject.position += Vector2.Normalize(this.direction) * speed * speedScale * Raylib.GetFrameTime();
    }

    public override void Update()
    {
        HandleMovement();
    }
}