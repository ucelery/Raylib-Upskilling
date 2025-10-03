using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;
using Raylib_cs;

public class Player : Component
{
    // Movement options
    private int speed;
    private int baseSpeed = 100;
    private int speedScale = 4;
    private Vector2 direction = Vector2.Zero;

    // Shoot options
    private float cooldown = 0.001f;
    private float currentCd;
    private Queue<Ball> balls = new();

    public override void Initialize()
    {
        speed = baseSpeed;
    }

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

        ball.SetDirection(RandomDirection());
    }

    private Vector2 RandomDirection()
    {
        Random rnd = new Random();

        float min = -1;
        float max = 1;
        float randX = (float)(rnd.NextDouble() * (max - min) + min);
        float randY = (float)(rnd.NextDouble() * (max - min) + min);

        return new Vector2(randX, randY);
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

        // Sneak-like movement
        if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
            speed = (baseSpeed * speedScale) / 2;
        else speed = (baseSpeed * speedScale);

        if (this.direction != Vector2.Zero)
            GameObject.position += Vector2.Normalize(this.direction) * speed * Raylib.GetFrameTime();
    }
}