using System.Numerics;
using Raylib_cs;

public class Collision : Component
{
    public Vector2 size = new();
    public delegate void CollisionEvent(Collision other);
    public CollisionEvent? OnCollisionEnter;

    public Rectangle rectangle;

    public Collision(Vector2 size)
    {
        this.size = size;
    }
    
    public override void Start()
    {
        CollisionManager.AddCollision(this);
    }

    public override void Unload()
    {
        CollisionManager.RemoveCollision(this);
    }
}