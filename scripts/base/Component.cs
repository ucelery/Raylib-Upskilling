public abstract class Component
{
    public bool enabled = true;
    public GameObject GameObject { get; private set; } = null!;

    internal void Attach(GameObject gameObject)
    {
        GameObject = gameObject;
    }

    public virtual void Initialize() { }
    public virtual void Update() { }
    public virtual void Unload() { }
}
