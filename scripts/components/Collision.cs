using System.Numerics;
using Raylib_cs;

public class Collision : Component
{
    public Vector2 size = new();
    public delegate void CollisionEvent(Collision other);
    public CollisionEvent? OnCollisionEnter;

    public Rectangle rectangle;

    private Vector2 offset;

    public Collision(Vector2 size)
    {
        this.size = size;
    }

    public override void Start()
    {
        CollisionManager.Instance.AddCollision(this);
        offset = size / 2;
    }

    public override void Update()
    {
        rectangle = new(GameObject.position - offset, size);

        DrawHitboxes();
    }

    public override void Unload()
    {
        CollisionManager.Instance.RemoveCollision(this);
    }

    private void DrawHitboxes()
    {
        Vector2 cel_pos = CollisionManager.Instance.GetCell(GameObject.position);

        Raylib.DrawText($"({cel_pos.X}, {cel_pos.Y})", (int)GameObject.position.X, (int)GameObject.position.Y - 10, 5, Color.White);
        Raylib.DrawRectangleLines((int)GameObject.position.X - (int)offset.X, (int)GameObject.position.Y - (int)offset.Y, (int)rectangle.Width, (int)rectangle.Height, Color.Green);
    }
}