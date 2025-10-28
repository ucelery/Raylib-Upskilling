using System.Numerics;
using Raylib_cs;

public class Player : Component
{
    private Animator anim = null!;
    private Drawable dr = null!;

    // Movement options
    private int speed;
    private int baseSpeed = 100;
    private int speedScale = 4;
    private Vector2 direction = Vector2.Zero;

    // Shoot options
    private float cooldown = 0.1f;
    private float currentCd;
    private Queue<Ball> balls = new();

    public override void Initialize()
    {
        speed = baseSpeed;
        anim = GameObject.GetComponent<Animator>();
        dr = GameObject.GetComponent<Drawable>();

        dr.SetScale(5);
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
        }
        else
        {
            GameObject ballObj = new GameObject();
            Ball.BallConfig config = new();
            config.canBounce = true;
            config.collisionSize = new Vector2(AssetManager.Instance.Textures["player_bullet"][0].Width, AssetManager.Instance.Textures["player_bullet"][0].Height);

            ball = new Ball();
            ballObj.AddComponent(ball);
            ballObj.AddComponent(new Drawable(AssetManager.Instance.Textures["player_bullet"][0]));
            ballObj.AddComponent(new Animator(AssetManager.Instance.Textures["player_bullet"], 0.1f, true));
            ballObj.AddComponent(new Collision(config.collisionSize));
    
            GameObject.Scene.AddObject(ballObj);

            ball.SetConfig(config);
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
    }

    public void HandleMovement()
    {
        this.direction.Y = (Raylib.IsKeyDown(KeyboardKey.W) ? -1 : 0) + (Raylib.IsKeyDown(KeyboardKey.S) ? 1 : 0);
        this.direction.X = (Raylib.IsKeyDown(KeyboardKey.A) ? -1 : 0) + (Raylib.IsKeyDown(KeyboardKey.D) ? 1 : 0);

        if (direction.X > 0)
            dr.SetFlip(new Vector2(1, 1));
        else if (direction.X < 0)
            dr.SetFlip(new Vector2(-1, 1));

        // Sneak-like movement
        if (Raylib.IsKeyDown(KeyboardKey.LeftShift))
            speed = (baseSpeed * speedScale) / 2;
        else speed = (baseSpeed * speedScale);

        if (this.direction != Vector2.Zero)
        {
            GameObject.position += Vector2.Normalize(this.direction) * speed * Raylib.GetFrameTime();
            anim.SetAnimation(AssetManager.Instance.Textures["player_move"], 0.25f, true);
        }
        else
        {
            anim.SetAnimation(AssetManager.Instance.Textures["player_idle"], 0.25f, true);
        }
    }
}