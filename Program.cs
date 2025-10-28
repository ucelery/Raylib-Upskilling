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
        playerObject.AddComponent(new Drawable());
        playerObject.AddComponent(new Player());
        playerObject.AddComponent(new Animator());
        playerObject.name = "Player Object";
        gameScene.AddObject(playerObject);

        GameObject enemyObject = new GameObject();
        enemyObject.AddComponent(new Drawable(AssetManager.Instance.Textures["Alien02"][0]));
        enemyObject.AddComponent(new Enemy());
        enemyObject.position = new Vector2(windowWidth / 2, windowHeight / 2);
        gameScene.AddObject(enemyObject);

        gameScene.Initialize();
        gameScene.Start();

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.DrawTexture(AssetManager.Instance.Textures["Background_ingame"][0], 0, 0, Color.White);

            gameScene.Update();
            CollisionManager.Instance.Update();

            Raylib.DrawFPS(0, 0);
            Raylib.EndDrawing();
        }

        AssetManager.Instance.Unload();
        Raylib.CloseWindow();
    }
}