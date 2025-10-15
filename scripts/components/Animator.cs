using System.Numerics;
using Raylib_cs;

public class Animator : Component
{
    private List<Texture2D> sprites;
    private int index = 0;
    private float timer = 0;
    public float duration = 0.5f;

    private bool loop = false;

    public Animator(List<Texture2D> sprites)
    {
        this.sprites = sprites;
    }


    public Animator(List<Texture2D> sprites, float duration, bool loop)
    {
        this.sprites = sprites;
        this.duration = duration;
        this.loop = loop;
    }

    public override void Update()
    {
        HandleTimer();

        Drawable dr = GameObject.GetComponent<Drawable>();
        dr.SetTexture(sprites[index]);
    }

    private void HandleTimer()
    {
        if (!loop && index > sprites.Count - 1) return;

        timer += Raylib.GetFrameTime();
        if (timer > duration)
        {
            index++;

            if (index > sprites.Count - 1)
            {
                if (loop)
                {

                    index = 0;
                }
                else
                {
                    index = sprites.Count - 1;
                }   
            }
            
            timer = 0;
        }
    }

    public void Play()
    {
        timer = 0;
        index = 0;
    }
}