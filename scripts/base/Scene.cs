public class Scene
{
    private List<GameObject> children = new List<GameObject>();
    private List<GameObject> toAdd = new List<GameObject>();

    public bool IsSceneReady = false;

    public void Initialize()
    {
        IsSceneReady = true;
        foreach (GameObject obj in toAdd)
        {
            obj.Initialize();
        }
    }

    public void Start()
    {
        foreach (GameObject obj in children)
        {
            obj.Start();
        }
    }

    public void Update()
    {
        foreach (GameObject obj in children)
        {
            obj.Update();
        }

        // C# throws an error when modifying lists, so we have to add the object on a temporary list
        // and then add them back in to be called on the next frame
        if (toAdd.Count > 0)
        {
            children.AddRange(toAdd);
            toAdd.Clear();
        }
    }

    public void Unload()
    {
        foreach (GameObject obj in children)
        {
            obj.Destroy();
        }
    }

    public GameObject AddObject(GameObject gameObject)
    {
        toAdd.Add(gameObject);

        return gameObject;
    }
}