using System.Numerics;
using Raylib_cs;

public class Player
{
    private int speed;
    private Texture2D playerSprite;

    public Vector2 playerPos = new Vector2();

    public Player(int speed, Vector2 initPos)
    {
        this.speed = speed;
        this.playerPos = initPos;
    }

    public void Initialize()
    {
        playerSprite = Raylib.LoadTexture("DrunkenStupor.png");
        Raylib.SetTextureFilter(playerSprite, TextureFilter.Point);
    }

    public void Movement()
    {
        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            playerPos.Y -= speed * Raylib.GetFrameTime();
        }
        if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            playerPos.Y += speed * Raylib.GetFrameTime();
        }
        if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            playerPos.X -= speed * Raylib.GetFrameTime();
        }
        if (Raylib.IsKeyDown(KeyboardKey.D))
        {
            playerPos.X += speed * Raylib.GetFrameTime();
        }
    }

    public void Draw()
    {
        Vector2 offset = new Vector2(playerSprite.Width / 2, playerSprite.Height / 2);
        Raylib.DrawTextureV(playerSprite, playerPos - offset, Color.White);
    }

    public void Unload()
    {
        Raylib.UnloadTexture(playerSprite);
    }
}