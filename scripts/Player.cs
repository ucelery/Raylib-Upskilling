using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

public class Player : Component
{
    // Movement options
    private int speed = 100;
    private int speedScale = 4;
    private Vector2 direction = Vector2.Zero;

    // Shoot options
    private float cooldown = 0.2f;
    private float currentCd;
    private Queue<Ball> balls = new();

    public override void Update()
    {
        HandleMovement();
        HandleShoot();
    }

    public void HandleShoot()
    {
        if (currentCd > 0) currentCd -= Raylib.GetFrameTime();
        if (!Raylib.IsKeyDown(KeyboardKey.Space) || currentCd > 0) return;

        currentCd = cooldown;

        Ball ball;
        if (balls.Count > 0)
        {
            ball = balls.Dequeue();
            Console.WriteLine("Reuse Ball");
        }
        else
        {
            GameObject ballObj = new GameObject();
            ballObj.AddComponent(new Ball());

            // TODO: Try to optimize; i feel like we can reuse the same texture that was loaded
            ballObj.AddComponent(new Drawable("resources/bullets/Bullet_player.png")); 

            ball = GameObject.Scene.CreateObject(ballObj).GetComponent<Ball>();
            ball.OnDespawn += OnBallDespawn;
        }

        ball.GameObject.position = this.GameObject.position;
        ball.Reinitialize();
        ball.SetDirection(new Vector2(0, -1));
    }

    private void OnBallDespawn(Ball ball)
    {
        balls.Enqueue(ball);

        Console.WriteLine("Ball Despawn");
    }

    public void HandleMovement()
    {
        this.direction.Y = (Raylib.IsKeyDown(KeyboardKey.W) ? -1 : 0) + (Raylib.IsKeyDown(KeyboardKey.S) ? 1 : 0);
        this.direction.X = (Raylib.IsKeyDown(KeyboardKey.A) ? -1 : 0) + (Raylib.IsKeyDown(KeyboardKey.D) ? 1 : 0);

        if (this.direction != Vector2.Zero)
            GameObject.position += Vector2.Normalize(this.direction) * speed * speedScale * Raylib.GetFrameTime();
    }
}