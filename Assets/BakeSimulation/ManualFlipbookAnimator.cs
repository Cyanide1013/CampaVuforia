using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ManualFlipBookAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Animation frames (baked meshes)")]
    public Mesh[] meshes;
    
    [Tooltip("Total duration of the animation in seconds")]
    public float duration = 5f;
    
    [Tooltip("Playback speed (1 = normal speed, 2 = double speed)")]
    public float speed = 1f;
    
    [Tooltip("Whether to loop the animation")]
    public bool loop = true;

    [Header("Runtime Info")]
    [SerializeField] private float currentTime;
    [SerializeField] private int currentIndex;

    private MeshFilter meshFilter;
    private int lastIndex = -1;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        ValidateComponents();
    }

    void Update()
    {
        if (meshes.Length == 0) return;
        
        UpdateTime();
        UpdateAnimation();
    }

    void UpdateTime()
    {
        currentTime += speed * Time.deltaTime;
        
        if (loop)
        {
            currentTime = Mathf.Repeat(currentTime, duration);
        }
        else
        {
            currentTime = Mathf.Clamp(currentTime, 0f, duration);
        }
    }

    void UpdateAnimation()
    {
        currentIndex = Mathf.RoundToInt((currentTime / duration) * (meshes.Length - 1));
        currentIndex = Mathf.Clamp(currentIndex, 0, meshes.Length - 1);

        if (currentIndex != lastIndex)
        {
            meshFilter.sharedMesh = meshes[currentIndex];
            lastIndex = currentIndex;
        }
    }

    void ValidateComponents()
    {
        if (meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();
        
        if (meshes == null || meshes.Length == 0)
            Debug.LogError("No meshes assigned!", this);
    }

    // Public controls
    public void SetNormalizedTime(float t) => currentTime = Mathf.Clamp01(t) * duration;
    public void Play() => speed = Mathf.Abs(speed);
    public void Reverse() => speed = -Mathf.Abs(speed);
    public void Pause() => speed = 0f;
    public void ResetAnimation() => currentTime = 0f;
}