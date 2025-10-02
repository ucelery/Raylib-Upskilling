using System.Numerics;
using Raylib_cs;

public class Drawable : Component
{
    private string texturePath;
    private Texture2D texture;
    private TextureFilter filter = TextureFilter.Bilinear;

    public Drawable(string texturePath)
    {
        this.texturePath = texturePath;
    }

    public Drawable(string texturePath, TextureFilter filter)
    {
        this.texturePath = texturePath;
        this.filter = filter;
    }

    public override void Initialize()
    {
        texture = Raylib.LoadTexture(this.texturePath);
        Raylib.SetTextureFilter(texture, this.filter);
    }

    public override void Update()
    {
        Vector2 offset = new Vector2(texture.Width / 2, texture.Height / 2);
        Raylib.DrawTextureV(texture, GameObject.position - offset, Color.White);
    }

    public override void Unload()
    {
        Raylib.UnloadTexture(texture);
    }
}