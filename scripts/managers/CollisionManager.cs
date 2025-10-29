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
        foreach (List<Collision> col_list in collisionsCells.Values)
            foreach (Collision a_col in col_list)
                foreach (Collision b_col in col_list)
                {
                    if (a_col == b_col) continue;

                    bool isColliding = Raylib.CheckCollisionRecs(a_col.rectangle, b_col.rectangle);

                    if (isColliding)
                    {
                        a_col.OnCollisionEnter?.Invoke(b_col);
                        b_col.OnCollisionEnter?.Invoke(a_col);
                    }
                }
    }

    private void UpdateCollisionCells()
    {
        Dictionary<Vector2, List<Collision>> temp = new();
        foreach (var col in collisions)
        {
            if (!col.enabled) continue;

            Vector2 cell = GetCell(col.GameObject.position);

            if (!temp.ContainsKey(cell))
                temp.Add(cell, new List<Collision>());

            temp[cell].Add(col);
        }

        collisionsCells = temp;

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
                Raylib.DrawText($"({x}, {y})", x * (int)cellSize.X, y * (int)cellSize.Y, 1, Color.White);
            }
    }
    
    public Vector2 GetCell(Vector2 position)
    {
        int cellX = (int)(position.X / cellSize.X);
        int cellY = (int)(position.Y / cellSize.Y);
        return new Vector2(cellX, cellY);
    }
}
