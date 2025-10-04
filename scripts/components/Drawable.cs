using System.Numerics;
using Raylib_cs;

public class Drawable : Component
{
    private string texturePath = null!;
    private TextureFilter filter = TextureFilter.Bilinear;
    private float scale = 1;
    private float rotation = 0;

    public Texture2D Texture { get; private set; }

    public Drawable() {}

    public Drawable(string texturePath)
    {
        this.texturePath = texturePath;
    }

    public Drawable(string texturePath, TextureFilter filter)
    {
        this.texturePath = texturePath;
        this.filter = filter;
    }

    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
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
        Raylib.DrawTextureEx(Texture, GameObject.position - offset, 0, scale, Color.White);
    }

    public override void Unload()
    {
        Raylib.UnloadTexture(Texture);
    }

    public void SetScale(float scale)
    {
        this.scale = scale;
    }

    public void SetRotation(float rotation)
    {
        this.rotation = rotation;
    }
}