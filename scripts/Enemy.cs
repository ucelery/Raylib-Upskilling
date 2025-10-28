using System.Numerics;
using Raylib_cs;

public class Enemy : Agent
{
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
        base.Initialize();

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

        props.type = AgentType.Enemy;
        GameObject.Tags.Add("enemy");
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
        if (shootCd > 0)
        {
            shootCd -= Raylib.GetFrameTime();
            return;
        }

        Ball.BallConfig config = new();
        config.origin = this;
        config.targets.Add("player");

        Shoot(config);

        shootCd = props.shootCd;
    }

    public void HandleMovement()
    {
        GameObject.position += Vector2.Normalize(this.direction) * speed * Raylib.GetFrameTime();
    }
}