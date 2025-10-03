using System.Numerics;

public class GameObject
{
    public string name = "New GameObject";
    public Scene Scene = null!;
    public bool enabled = true;
    public Vector2 position = Vector2.Zero;

    public List<Component> components = new List<Component>();

    public GameObject()
    {
        Initialize();
    }

    public T GetComponent<T>() where T : Component
    {
        foreach (var component in components)
        {
            if (component is T tComponent) return tComponent;
        }

        return null;
    }

    public void Attach(Scene scene)
    {
        this.Scene = scene;
    }

    public void SetActive(bool flag)
    {
        enabled = flag;

        if (enabled) OnEnable();
        else OnDisable();
    }

    public void AddComponent(Component component)
    {
        component.Attach(this);
        components.Add(component);
    }

    public virtual void Initialize()
    {
        List<Component> copy = components.ToList();
        foreach (Component component in copy)
        {
            component.Initialize();
        }

        Console.WriteLine($"GameObject {name} Initialized");
    }

    public virtual void Update()
    {
        if (!enabled) return;

        foreach (Component component in components)
        {
            if (!component.enabled) continue;

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

    protected virtual void OnDisable() { }

    protected virtual void OnEnable() { }
}