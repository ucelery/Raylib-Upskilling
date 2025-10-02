using System.Numerics;

public class GameObject
{
    public Vector2 position = Vector2.Zero;

    public List<Component> components = new List<Component>();

    public void AddComponent(Component component)
    {
        component.Attach(this);
        components.Add(component);
    }

    public virtual void Initialize()
    {
        foreach (Component component in components)
        {
            component.Initialize();
        }
    }

    public virtual void Update()
    {
        foreach (Component component in components)
        {
            component.Update();
        }
    }

    public virtual void Destroy()
    {
        foreach (Component component in components)
        {
            component.Unload();
        }
    }
}