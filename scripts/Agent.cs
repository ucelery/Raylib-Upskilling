using System.Numerics;
using Raylib_cs;

public class Agent : Component
{
    public enum AgentType { Neutral, Ally, Enemy }
    public struct AgentProperties()
    {
        public int baseSpeed = 100;
        public int speedScale = 4;
        public float shootCd = 0.01f;
        public AgentType type = AgentType.Neutral;
    }

    protected AgentProperties props = new();

    protected Queue<Ball> balls = new();
    protected float shootCd;
    protected Vector2 direction = new Vector2(1, 0); // Initial direction to the right
    protected int speed;

    public override void Initialize()
    {
        speed = props.baseSpeed;
    }

    public Ball Shoot(Ball.BallConfig config)
    {
        Ball ball;
        if (balls.Count > 0)
        {
            ball = balls.Dequeue();
        }
        else
        {
            GameObject ballObj = new GameObject(GameObject.Scene);
            List<Texture2D> texture = AssetManager.Instance.Textures[config.spriteName];
            config.collisionSize = new Vector2(texture[0].Width, texture[0].Height);

            ball = new Ball(config);
            ballObj.AddComponent(ball);

            ball.OnDespawn += OnBallDespawn;
        }

        ball.GameObject.position = this.GameObject.position;
        ball.Reinitialize();

        ball.SetDirection(config.direction);

        return ball;
    }

    private void OnBallDespawn(Ball ball)
    {
        balls.Enqueue(ball);
    }
}