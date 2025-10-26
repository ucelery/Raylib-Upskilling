using Raylib_cs;

public static class CollisionManager
{
    public static List<Collision> Collisions = new();
    public static List<Collision> NewCollisions = new();

    public static void Update()
    {
        for (int i = 0; i < Collisions.Count; i++)
        {
            Collision a_col = Collisions[i];
            if (!a_col.enabled) continue;

            for (int j = 0; j < Collisions.Count; j++)
            {
                Collision b_col = Collisions[j];
                if (!b_col.enabled) continue;
                if (a_col == b_col) continue;

                bool isColliding = Raylib.CheckCollisionRecs(a_col.rectangle, b_col.rectangle);
                if (isColliding)
                {
                    a_col.OnCollisionEnter?.Invoke(b_col);
                    b_col.OnCollisionEnter?.Invoke(a_col);
                }
            }
        }

        if (NewCollisions.Count > 0)
        {
            Collisions.AddRange(NewCollisions);
            NewCollisions.Clear();
        }
    }

    public static void AddCollision(Collision collision)
    {
        if (!Collisions.Contains(collision) && !Collisions.Contains(collision))
            NewCollisions.Add(collision);
    }

    public static void RemoveCollision(Collision collision)
    {
        NewCollisions.Add(collision);
    }
}
