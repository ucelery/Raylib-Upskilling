using System.Numerics;
using Raylib_cs;

public class Drawable : Component
{
    private string texturePath;
    private TextureFilter filter = TextureFilter.Bilinear;

    public Texture2D Texture { get; private set; }

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
        Texture = Raylib.LoadTexture(this.texturePath);
        Raylib.SetTextureFilter(Texture, this.filter);
    }

    public override void Update()
    {
        Vector2 offset = new Vector2(Texture.Width / 2, Texture.Height / 2);
        Raylib.DrawTextureV(Texture, GameObject.position - offset, Color.White);
    }

    public override void Unload()
    {
        Raylib.UnloadTexture(Texture);
    }
}