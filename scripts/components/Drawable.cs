using System.Numerics;
using Raylib_cs;

public class Drawable : Component
{
    private TextureFilter filter = TextureFilter.Bilinear;
    private float scale = 1;
    private float rotation = 0;

    public Texture2D Texture { get; private set; }

    public Drawable() { }

    public Drawable(Texture2D texture)
    {
        this.Texture = texture;
    }

    public Drawable(Texture2D texture, TextureFilter filter)
    {
        this.Texture = texture;
        this.filter = filter;
    }

    public void SetTexture(Texture2D texture)
    {
        Texture = texture;
    }

    public override void Initialize() { }

    public override void Update()
    {
        Vector2 offset = new Vector2(Texture.Width / 2, Texture.Height / 2);
        Raylib.DrawTextureV(Texture, GameObject.position - offset, Color.White);
        Raylib.DrawTextureEx(Texture, GameObject.position - offset, 0, scale, Color.White);
    }

    public void SetScale(float scale)
    {
        this.scale = scale;
    }

    public void SetRotation(float rotation)
    {
        this.rotation = rotation;
    }

    public void SetFilter(TextureFilter newFilter)
    {
        this.filter = newFilter;
        Raylib.SetTextureFilter(Texture, newFilter);
    }
}