using System.Numerics;
using System.Reflection;
using Raylib_cs;

public class Program
{
    static void Main(string[] args)
    {
        int windowWidth = 1280;
        int windowHeight = 720;

        Raylib.InitWindow(windowWidth, windowHeight, "Test");
        Raylib.SetTargetFPS(144);

        AssetManager.Instance.Initialize();

        Scene gameScene = new Scene();

        GameObject playerObject = new GameObject();
        playerObject.AddComponent(new Drawable(AssetManager.Instance.Textures["Player"][0]));
        playerObject.AddComponent(new Player());

        gameScene.AddObject(playerObject);

        GameObject sampleSprite = new GameObject();
        Drawable dr = new Drawable();
        dr.SetScale(4);
        sampleSprite.AddComponent(dr);
        sampleSprite.AddComponent(new Animator(AssetManager.Instance.Textures["eye"], 0.5f, false));

        gameScene.Initialize();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.DrawTexture(AssetManager.Instance.Textures["Background_ingame"][0], 0, 0, Color.White);

            gameScene.Update();

            Raylib.DrawFPS(0, 0);
            Raylib.EndDrawing();
        }

        // playerObject.Destroy();
        AssetManager.Instance.Unload();
        Raylib.CloseWindow();
    }
}