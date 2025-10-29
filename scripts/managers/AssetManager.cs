using System.Numerics;
using Raylib_cs;

public class AssetManager
{
    private static AssetManager _instance;
    public static AssetManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new AssetManager();

            return _instance;
        }
    }

    private AssetManager() { }

    public Dictionary<string, List<Texture2D>> Textures { get; private set; } = new();

    public void Initialize()
    {
        /// INFO: Texture keys for sprites will be their exact file name
        /// INFO: Texture keys for animations will be their directory name
        
        // Initialize Sprites
        string[] spriteFiles = Directory.GetFiles("resources/sprites", "*", SearchOption.AllDirectories);
        foreach (string file in spriteFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            AddTextures(fileName, file);
        }

        /// TODO: Make it so that its possible to make nesting folders within the animations directory

        // Initialize Animations
        string[] animDirectories = Directory.GetDirectories("resources/animations");
        foreach (string dir in animDirectories)
        {
            List<string> filePaths = [.. Directory.GetFiles(dir)];
            string dirName = Path.GetFileName(dir);
            AddTextures(dirName, filePaths);
        }
    }

    public void AddTextures(string name, List<string> paths)
    {
        if (Textures.ContainsKey(name))
            throw new Exception("Sprite key already exists");

        List<Texture2D> newSprites = new List<Texture2D>();
        paths.ForEach(path =>
        {
            newSprites.Add(Raylib.LoadTexture(path));
        });

        Textures.Add(name, newSprites);
    }

    public void AddTextures(string name, string path)
    {
        if (Textures.ContainsKey(name))
            throw new Exception("Sprite key already exists");

        Textures.Add(name, new List<Texture2D>() { Raylib.LoadTexture(path) });
    }

    public void Unload()
    {
        foreach (var kvp in Textures)
            foreach (Texture2D t in kvp.Value)
                Raylib.UnloadTexture(t);

    }
}