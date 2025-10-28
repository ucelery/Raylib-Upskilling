using System.Numerics;
using Raylib_cs;

public class Player : Agent
{
    private Animator anim = null!;
    private Drawable dr = null!;

    public override void Initialize()
    {
        GameObject.AddComponent(new Drawable());
        GameObject.AddComponent(new Animator());

        anim = GameObject.GetComponent<Animator>();
        dr = GameObject.GetComponent<Drawable>();

        Vector2 size = new(dr.Texture.Width, dr.Texture.Height);
        GameObject.AddComponent(new Collision(size));

        dr.SetScale(5);

        props.type = AgentType.Ally;
        GameObject.Tags.Add("player");
    }

    public override void Update()
    {
        HandleMovement();
        HandleShoot();
    }

    public void HandleShoot()
    {
        if (shootCd > 0) shootCd -= Raylib.GetFrameTime();
        if (!Raylib.IsKeyDown(KeyboardKey.Space) || shootCd > 0) return;

        Ball.BallConfig config = new();
        config.canBounce = true;
        config.origin = this;
        config.targets.Add("enemy");
        
        Shoot(config);

        shootCd = props.shootCd;
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