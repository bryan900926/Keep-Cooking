using UnityEngine;

public class FindAllManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SceneController scenemanager;
    public AudioManager audioManager;
    public static FindAllManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
       scenemanager = FindFirstObjectByType<SceneController>();
       audioManager = FindFirstObjectByType<AudioManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
