using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Static reference to the instance
    public static GameManager Instance { get; private set; }

    public AttributeDictionary attributeBehaviourDictionary;

    public Welder welder;

    public bool SpawnMode;
    [SerializeField] private GameObject spawnModeUI;

    private int chaosScore;
    public int ChaosScore => chaosScore;
    private int displayedChaosScore;
    private int interactionAmount;
    public int InteractionAmount => interactionAmount;
    private int displayedInteractionAmount;

    [SerializeField] private RectTransform chaosUIPanel;
    [SerializeField] private TextMeshProUGUI chaosScoreText;
    [SerializeField] private TextMeshProUGUI interactionAmountText;

    [Header("Timer")] 
    [SerializeField] private float gameTimer;
    [SerializeField] private TextMeshProUGUI timerText;

    private bool runTimer;

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

    private void Start()
    {
        chaosUIPanel.DOScale(1.2f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        
        SetTimerState(true);
    }

    private void Update()
    {
        gameTimer += Time.deltaTime;
        PresentTimer(gameTimer);
        
        if (Input.GetKeyDown(KeyCode.Delete))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleSpawnMode()
    {
        SpawnMode = !SpawnMode;
        spawnModeUI.SetActive(SpawnMode);
    }

    public void RegisterInteraction(int score, bool award)
    {
        if (gameTimer < 2f || !award)
            return;
        
        interactionAmount++;
        chaosScore += score;
        interactionAmountText.text = $"{interactionAmount} interactions";
        
        DOTween.To(
            () => displayedChaosScore,
            x =>
            {
                displayedChaosScore = x;
                chaosScoreText.text = displayedChaosScore.ToString();
            },
            chaosScore,
            0.5f
        );
    }
    
    public void SetTimerState(bool state) => runTimer = state;

    public void PresentTimer(float time)
    {
        timerText.text = $"{time.ToString("0.00")}";
    }
    
}