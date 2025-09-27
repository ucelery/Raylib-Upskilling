using System.Numerics;
using Raylib_cs;

public class Ball
{
    private int speed;
    private Texture2D sprite;

    public Vector2 position = new Vector2();

    public Ball(int speed, Vector2 initPos)
    {
        this.speed = speed;
        this.position = initPos;
    }

    public void Initialize()
    {
        // sprite = Raylib.LoadTexture("DrunkenStupor.png");
        Raylib.SetTextureFilter(sprite, TextureFilter.Point);
    }

    public void Movement()
    {
        
    }

    public void Draw()
    {
        Vector2 offset = new Vector2(sprite.Width / 2, sprite.Height / 2);
        Raylib.DrawTextureV(sprite, position - offset, Color.White);
    }

    public void Unload()
    {
        Raylib.UnloadTexture(sprite);
    }
}