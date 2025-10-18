using System.Numerics;
using System.Reflection.Metadata;
using Raylib_cs;

public class Enemy : Component
{
    // Movement options
    private int speed;
    private int baseSpeed = 100;
    private int speedScale = 4;
    private Vector2 direction = new Vector2(1, 0); // Initial direction to the right

    // Shoot options
    private float cooldown = 0.01f;
    private float currentCd;
    private Queue<Ball> balls = new();

    // Behaviour
    public enum EnemyState { Idle, Move , Shoot }
    public struct EnemyBehaviour() {
        public EnemyState state = EnemyState.Idle;
        public float duration = 0;
    }

    private List<EnemyBehaviour> behaviours = new();
    private int currentBehaviour = 0;
    private float actionTimer;


    public override void Initialize()
    {
        speed = baseSpeed;

        // Behaviour Pattern = Idle(0.75) -> Shoot(1) -> Idle(0.5) -> Move(1.25) -> Repeat
        EnemyBehaviour idleBeh = new() { state = EnemyState.Idle, duration = 0.75f };
        EnemyBehaviour idleBeh2 = new() { state = EnemyState.Idle, duration = 0.5f };
        EnemyBehaviour shootBeh = new() { state = EnemyState.Shoot, duration = 1f };
        EnemyBehaviour moveBeh = new() { state = EnemyState.Move, duration = 1.25f };

        behaviours.Add(idleBeh);
        behaviours.Add(idleBeh2);
        behaviours.Add(shootBeh);
        behaviours.Add(moveBeh);

        actionTimer = behaviours[currentBehaviour].duration;
    }

    public override void Update()
    {
        actionTimer -= Raylib.GetFrameTime();
        if (actionTimer <= 0)
        {
            currentBehaviour += 1;
            if (currentBehaviour >= behaviours.Count)
                currentBehaviour = 0;

            EnemyBehaviour curBeh = behaviours[currentBehaviour];

            // Switch direction
            if (curBeh.state == EnemyState.Move)
                direction = new Vector2(direction.X * -1, direction.Y);

            actionTimer = curBeh.duration;
        }

        switch (behaviours[currentBehaviour].state)
        {
            case EnemyState.Idle:
                break;
            case EnemyState.Move:
                HandleMovement();
                break;
            case EnemyState.Shoot:
                HandleShoot();
                break;
        }
    }

    public void HandleShoot()
    {
        if (currentCd > 0)
        {
            currentCd -= Raylib.GetFrameTime();
            return;
        }

        currentCd = cooldown;

        Ball ball;
        if (balls.Count > 0)
        {
            ball = balls.Dequeue();
        }
        else
        {
            GameObject ballObj = new GameObject();
            ballObj.AddComponent(new Ball());

            // TODO: Try to optimize; i feel like we can reuse the same texture that was loaded
            // TODO: Make asset manager system
            ballObj.AddComponent(new Drawable(AssetManager.Instance.Textures["enemy_bullet"][0]));
            ballObj.AddComponent(new Animator(AssetManager.Instance.Textures["enemy_bullet"], 0.1f, true));

            ball = GameObject.Scene.AddObject(ballObj).GetComponent<Ball>();

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
        GameObject.position += Vector2.Normalize(this.direction) * speed * Raylib.GetFrameTime();
    }
}