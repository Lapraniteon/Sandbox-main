using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Static reference to the instance
    public static GameManager Instance { get; private set; }

    public AttributeDictionary attributeBehaviourDictionary;

    private void Awake()
    {
        // If an instance already exists and it's not this one → destroy this
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign instance and make persistent
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize anything needed for the manager
        Initialize();
    }

    private void Initialize()
    {
        // Setup logic here
        Debug.Log("GameManager initialized!");
    }

    // Example method
    public void StartGame()
    {
        Debug.Log("Game Started");
    }
}