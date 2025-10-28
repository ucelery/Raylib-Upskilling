using System.Numerics;

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
    
    public void Shoot(Ball.BallConfig config)
    {
        Ball ball;
        if (balls.Count > 0)
        {
            ball = balls.Dequeue();
        }
        else
        {
            GameObject ballObj = new GameObject();
            config.collisionSize = new Vector2(AssetManager.Instance.Textures["player_bullet"][0].Width, AssetManager.Instance.Textures["player_bullet"][0].Height);

            ball = new Ball();
            ballObj.AddComponent(ball);
            ballObj.AddComponent(new Drawable(AssetManager.Instance.Textures["player_bullet"][0]));
            ballObj.AddComponent(new Animator(AssetManager.Instance.Textures["player_bullet"], 0.1f, true));
            ballObj.AddComponent(new Collision(config.collisionSize));

            GameObject.Scene.AddObject(ballObj);

            ball.SetConfig(config);
            ball.OnDespawn += OnBallDespawn;

            ball.GameObject.position = this.GameObject.position;
            ball.Reinitialize();

            ball.SetDirection(RandomDirection());
        }
    }

    private void OnBallDespawn(Ball ball)
    {
        balls.Enqueue(ball);
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
}