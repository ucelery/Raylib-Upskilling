using System.Numerics;
using System.Reflection;
using Raylib_cs;

public class Program
{
    static void Main(string[] args)
    {
        int windowWidth = 1280;
        int windowHeight = 720;

        Scene gameScene = new Scene();

        GameObject playerObject = new GameObject();
        playerObject.AddComponent(new Drawable("resources/agents/player/Player.png"));
        playerObject.AddComponent(new Player());

        gameScene.CreateObject(playerObject);

        Raylib.InitWindow(windowWidth, windowHeight, "Test");
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