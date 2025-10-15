using System.Numerics;
using Raylib_cs;

public class Drawable : Component
{
    private TextureFilter filter = TextureFilter.Bilinear;
    public float scale = 1;
    private float rotation = 0;
    private Vector2 flip = Vector2.One;

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
        Vector2 offset = new Vector2(Texture.Width / 2 * scale, Texture.Height / 2 * scale);
        // Raylib.DrawTextureEx(Texture, GameObject.position - offset, 0, scale, Color.White);
        Rectangle source = new Rectangle(0, 0, Texture.Width * flip.X, Texture.Height * flip.Y);
        Rectangle dest = new Rectangle(GameObject.position - offset, Texture.Width * scale, Texture.Height * scale);
        
        Raylib.DrawTexturePro(
            Texture,
            source,
            dest,
            new Vector2(0, 0),
            0f,               
            Color.White
        );
    }

    public void SetScale(float scale)
    {
        this.scale = scale;
    }

    public void SetFlip(Vector2 flip)
    {
        this.flip = flip;
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