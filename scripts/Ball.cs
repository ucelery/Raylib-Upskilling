using System.Numerics;
using Raylib_cs;

public class Ball : Component
{
    public struct BallConfig() {
        public int speed = 100;
        public int speedScale = 4;
        public float despawnTimer = 5;
        public Vector2 direction = Vector2.Zero;
        public bool canBounce = false;
        public Vector2 collisionSize = Vector2.Zero;
        public Agent origin = null!;
        public List<string> targets = new();
        public string spriteName = "";
    }

    private BallConfig config = new();

    public float despawnTimeLeft;
    public delegate void BallEvent(Ball ball);
    public event BallEvent? OnDespawn;

    public Ball(BallConfig config)
    {
        this.config = config;
    }

    public override void Initialize()
    {
        GameObject.name = "Ball";
    }

    public override void Start()
    {
        GameObject.AddComponent(new Collision(config.collisionSize));
        GameObject.AddComponent(new Drawable());
        GameObject.AddComponent(new Animator());

        Collision col = GameObject.GetComponent<Collision>();
        col.OnCollisionEnter += HandleHit;
    }

    public override void Update()
    {
        HandleMovement();
        HandleDespawn();
    }

    public void Reinitialize()
    {
        GameObject.SetActive(true);
        despawnTimeLeft = config.despawnTimer;
    }

    public void SetDirection(Vector2 direction)
    {
        config.direction = direction;
    }

    public void HandleMovement()
    {
        if (config.direction != Vector2.Zero)
            GameObject.position += Vector2.Normalize(config.direction) * config.speed * config.speedScale * Raylib.GetFrameTime();

        HandleBounce();
    }

    private void HandleHit(Collision other)
    {
        bool isBall = other.GameObject.GetComponent<Ball>() != null;
        bool isSameOrigin = config.targets.Contains(other.GameObject.Tag);

        Console.WriteLine($"{isBall} || {isSameOrigin} : {config.targets.Contains(other.GameObject.Tag)}");

        // Ignore Ball and Self Collision
        if (isBall || isSameOrigin) return;

        // GameObject.SetActive(false);
        OnDespawn?.Invoke(this);
    }

    private void HandleBounce()
    {
        if (!config.canBounce) return;
        
        // Bounce
        float centerY = Raylib.GetScreenHeight() / 2;
        float centerX = Raylib.GetScreenWidth() / 2;
        Rectangle screenRec = new Rectangle(centerX, centerY, new Vector2(Raylib.GetScreenWidth(), Raylib.GetScreenHeight()));

        Drawable ballDrawable = GameObject.GetComponent<Drawable>();
        Vector2 offset = new Vector2(ballDrawable.Texture.Width / 2, ballDrawable.Texture.Height / 2);
        Vector2 negatives = GameObject.position - offset;
        Vector2 positives = GameObject.position + offset;

        bool outsideXBounds = negatives.X < 0 || positives.X > Raylib.GetScreenWidth();
        bool outsideYBounds = negatives.Y < 0 || positives.Y > Raylib.GetScreenHeight();

        if (outsideXBounds)
        {
            config.direction.X = -config.direction.X;
        }

        if (outsideYBounds)
        {
            config.direction.Y = -config.direction.Y;
        }
    }

    public void HandleDespawn()
    {
        despawnTimeLeft -= Raylib.GetFrameTime();

        if (despawnTimeLeft <= 0)
        {
            GameObject.SetActive(false);
            OnDespawn?.Invoke(this);
        }
    }
}