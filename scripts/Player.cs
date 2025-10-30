using System.Numerics;
using Raylib_cs;

public class Player : Agent
{
    private Animator anim = null!;
    private Drawable dr = null!;

    public override void Initialize()
    {
        GameObject.AddComponent(new Drawable(AssetManager.Instance.Textures["player_idle"][0]));
        GameObject.AddComponent(new Animator());

        anim = GameObject.GetComponent<Animator>();
        dr = GameObject.GetComponent<Drawable>();

        // Vector2 size = new(dr.Texture.Width, dr.Texture.Height);
        // GameObject.AddComponent(new Collision(size, new Vector2(0, size.Y / -2)));

        dr.SetScale(3);

        props.type = AgentType.Ally;
        props.shootCd = 0.0000001f;
        GameObject.Tag = "player";
        GameObject.name = "Player Object";
    }

    public override void Update()
    {
        HandleMovement();
        HandleShoot();
    }

    public void HandleShoot()
    {
        if (shootCd > 0) shootCd -= Raylib.GetFrameTime();
        if (!Raylib.IsMouseButtonDown(0) || shootCd > 0) return;

        Ball.BallConfig config = new();
        // config.canBounce = true;
        config.origin = this;
        config.targets.Add("enemy");
        config.spriteName = "player_bullet";
        config.direction = BulletSpread();

        Shoot(config);

        shootCd = props.shootCd;
    }

    public Vector2 BulletSpread()
    {
        Vector2 mousePos = Raylib.GetMousePosition();
        Random rand = new Random();
        float randomValue = rand.NextSingle() * (10 - 0) + 0;

        return AngleOffset(mousePos, randomValue);
    }
    
    public Vector2 AngleOffset(Vector2 currentDir, float angle)
    {
        float angleOffset = Raylib.DEG2RAD * angle;
        Vector2 dir = Vector2.Normalize(currentDir - GameObject.position);

        // Rotation formula
        float cos = MathF.Cos(angleOffset);
        float sin = MathF.Sin(angleOffset);

        return new Vector2(
            dir.X * cos - dir.Y * sin,
            dir.X * sin + dir.Y * cos
        );
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
            speed = (props.baseSpeed * props.speedScale) / 2;
        else speed = (props.baseSpeed * props.speedScale);

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