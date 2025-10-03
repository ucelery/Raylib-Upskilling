using System.Numerics;
using System.Security.Cryptography;
using Raylib_cs;

public class Ball : Component
{
    private int speed = 100;
    private int speedScale = 4;
    private float despawnTimeLeft;
    private float despawnTimer = 5;
    private Vector2 direction = Vector2.Zero;

    public delegate void BallEvent(Ball ball);
    public event BallEvent? OnDespawn;

    public void Reinitialize()
    {
        GameObject.enabled = true;
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

        HandleBounce();
    }

    private void HandleBounce()
    {
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
            direction.X = -direction.X;
        }

        if (outsideYBounds)
        {
            direction.Y = -direction.Y;
        }
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

        // !! Comment this down when not debugging
        // DrawHitboxes();
    }

    private void DrawHitboxes()
    {
        Drawable ballDrawable = GameObject.GetComponent<Drawable>();

        int width = ballDrawable.Texture.Width;
        int height = ballDrawable.Texture.Height;

        Vector2 offset = new Vector2(ballDrawable.Texture.Width / 2, ballDrawable.Texture.Height / 2);
        Vector2 pos = GameObject.position;

        // NOTE: The positioning for DrawRectangleLines uses int as its position so it can be inaccurate
        Raylib.DrawRectangleLines((int)(pos.X - offset.X), (int)(pos.Y - offset.Y), width, height, Color.Red);
        Raylib.DrawText($"{GameObject.position}", (int)pos.X, (int)pos.Y + 10, 16, Color.Red);
    }
}