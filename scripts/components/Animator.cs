using System.Numerics;
using Raylib_cs;

public class Animator : Component
{
    private List<Texture2D> sprites;
    private int index = 0;
    private float timer = 0;
    public float duration = 0.5f;

    public Animator(List<Texture2D> sprites)
    {
        this.sprites = sprites;
    }

    public override void Update()
    {
        HandleTimer();

        Drawable dr = GameObject.GetComponent<Drawable>();
        dr.SetTexture(sprites[index]);
    }

    private void HandleTimer()
    {
        timer += Raylib.GetFrameTime();
        if (timer > duration)
        {
            index++;

            if (index > sprites.Count - 1)
                index = 0;

            timer = 0;
        }
    }
}