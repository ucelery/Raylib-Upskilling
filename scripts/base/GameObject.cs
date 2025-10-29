using System.Numerics;
using Raylib_cs;

public class GameObject
{
    public string name = "New GameObject";
    public Scene Scene = null!;
    public bool enabled = true;
    public Vector2 position = Vector2.Zero;
    public List<Component> components = new List<Component>();
    public string Tag = "";

    public T GetComponent<T>() where T : Component
    {
        foreach (var component in components)
        {
            if (component is T tComponent) return tComponent;
        }

        return null!;
    }

    public GameObject(Scene scene)
    {
        this.Scene = scene;
        scene.AddObject(this);
    }

    public void SetActive(bool flag)
    {
        enabled = flag;
        foreach (Component c in components)
        {
            c.SetActive(flag);
        }

        if (enabled) OnEnable();
        else OnDisable();
    }

    public void AddComponent(Component component)
    {
        component.Attach(this);
        components.Add(component);

        // For when adding a component during run time
        if (Scene != null && Scene.IsSceneReady)
        {
            component.Initialize();
            component.Start();
        }
            
    }

    public virtual void Initialize()
    {
        List<Component> copy = components.ToList();
        foreach (Component component in copy)
        {
            component.Initialize();
        }
    }

    public virtual void Start()
    {
        List<Component> copy = components.ToList();
        foreach (Component component in copy)
        {
            component.Start();
        }
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