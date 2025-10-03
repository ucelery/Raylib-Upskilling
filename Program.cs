using System.Numerics;
using System.Reflection;
using Raylib_cs;

public class Program
{
    static void Main(string[] args)
    {
        Vector2 initPos = new Vector2(500, 500);
        Scene gameScene = new Scene();

        GameObject playerObject = new GameObject();
        playerObject.AddComponent(new Drawable("resources/agents/player/Player.png"));
        playerObject.AddComponent(new Player());

        gameScene.CreateObject(playerObject);

        Raylib.InitWindow(1280, 720, "Test");
        Raylib.SetTargetFPS(144);

        Texture2D background = Raylib.LoadTexture("resources/background/Background_ingame.png");

        gameScene.Initialize();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.DrawTexture(background, 0, 0, Color.White);

            gameScene.Update();

            Raylib.DrawFPS(0, 0);
            Raylib.EndDrawing();
        }

        playerObject.Destroy();
        Raylib.UnloadTexture(background);
        Raylib.CloseWindow();
    }
}