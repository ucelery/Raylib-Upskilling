using System.Numerics;
using Raylib_cs;

public class CollisionManager
{
    private static CollisionManager _instance;
    public static CollisionManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CollisionManager();

            return _instance;
        }
    }

    private Dictionary<Vector2, List<Collision>> collisionsCells = new();
    private List<Collision> newCollisions = new();
    private List<Collision> collisions = new();

    private Vector2 cellSize = new Vector2(50, 50);

    public void Update()
    {
        UpdateCollisionCells();
        HandleCollisions();

        DrawDebugGrid();
    }

    private void HandleCollisions()
    {
        HashSet<(Collision, Collision)> checked_pairs = new();

        foreach (List<Collision> col_list in collisionsCells.Values)
            for (int i = 0; i < col_list.Count; i++)
        {
            for (int j = i + 1; j < col_list.Count; j++)
            {
                Collision a = col_list[i];
                Collision b = col_list[j];

                var pair = a.GetHashCode() < b.GetHashCode() ? (a, b) : (b, a);

                if (checked_pairs.Contains(pair))
                    continue;

                checked_pairs.Add(pair);

                bool is_colliding = Raylib.CheckCollisionRecs(a.rectangle, b.rectangle);
                if (is_colliding)
                {
                    a.OnCollisionEnter?.Invoke(b);
                    b.OnCollisionEnter?.Invoke(a);
                }
            }
        }
    }

    private void UpdateCollisionCells()
    {
        foreach (var list in collisionsCells.Values)
            list.Clear();

        foreach (var col in collisions)
        {
            if (!col.enabled) continue;

            List<Vector2> cells = GetCell(col.rectangle);
            foreach (Vector2 cell in cells)
            {
                if (!collisionsCells.TryGetValue(cell, out var list))
                {
                    list = new List<Collision>();
                    collisionsCells[cell] = list;
                }

                list.Add(col);
            }
        }

        if (newCollisions.Count > 0)
        {
            collisions.AddRange(newCollisions);
            newCollisions.Clear();
        }
    }

    public void AddCollision(Collision collision)
    {
        if (!collisions.Contains(collision) && !newCollisions.Contains(collision))
        {
            newCollisions.Add(collision);
        }
    }

    public void RemoveCollision(Collision collision)
    {
        newCollisions.Remove(collision);
    }

    public void DrawDebugGrid()
    {
        float xCellSize = Raylib.GetScreenWidth() / cellSize.X;
        float yCellSize = Raylib.GetScreenHeight() / cellSize.Y;

        for (int x = 0; x < xCellSize; x++)
            for (int y = 0; y < yCellSize; y++)
            {
                Raylib.DrawRectangleLines(x * (int)cellSize.X, y * (int)cellSize.Y, (int)cellSize.X, (int)cellSize.Y, Color.White);
                Raylib.DrawText($"({x}, {y})", x * (int)cellSize.X, y * (int)cellSize.Y, 1, Color.Red);
            }

        // No. of Collisions
        Raylib.DrawText($"{collisions.Count}", 0, 25, 25, Color.Green);
    }
    
    public List<Vector2> GetCell(Rectangle rect)
    {

        List<Vector2> cells = new();

        int startX = (int)(rect.X / cellSize.X);
        int startY = (int)(rect.Y / cellSize.Y);
        int endX = (int)((rect.X + rect.Width) / cellSize.X);
        int endY = (int)((rect.Y + rect.Height) / cellSize.Y);

        for (int x = startX; x <= endX; x++)
            for (int y = startY; y <= endY; y++)
                cells.Add(new Vector2(x, y));

        return cells;
    }
}
