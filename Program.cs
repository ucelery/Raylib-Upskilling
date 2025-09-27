using System.Numerics;
using System.Reflection;
using Raylib_cs;

public class Program
{
    static void Main(string[] args)
    {
        Vector2 initPos = new Vector2(500, 500);
        Player player = new Player(50, initPos);

        Raylib.InitWindow(1280, 720, "Test");
        Raylib.SetTargetFPS(144);

        Texture2D background = Raylib.LoadTexture("OCC_BG.jpg");

        player.Initialize();

        while (!Raylib.WindowShouldClose())
        {
            player.Movement();

            Raylib.BeginDrawing();
            Raylib.DrawTexture(background, 0, 0, Color.White);
            player.Draw();

            Raylib.DrawFPS(0, 0);
            Raylib.EndDrawing();
        }

        player.Unload();
        Raylib.UnloadTexture(background);
        Raylib.CloseWindow();
    }
}