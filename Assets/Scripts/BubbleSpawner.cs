using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject[] bubblePrefabs;
    public Transform spawnPoint;
    public Bubble currentControllableBubble;
    public float spawnRate;
    public bool isCurReversing;

    void Start()
    {
        if (bubblePrefabs.Length < 2)
        {
            Debug.LogError("需要至少2个气泡预制体!");
            return;
        }
        SpawnNewBubble();
    }

    // 反转当前气泡控制
    public void ReversCurBubbleControl()
    {
        if (currentControllableBubble != null)
        {
            currentControllableBubble.ReverseControl();
            isCurReversing = true;
        }
    }

    // 恢复当前气泡控制
    public void RecoverCurBubbleControl()
    {
        if (currentControllableBubble != null)
        {
            currentControllableBubble.RecoverControl();
            isCurReversing = false;
        }
    }

    public bool IsCurReversing()
    {
        return isCurReversing;
    }

    void SpawnNewBubble()
    {
        if (spawnPoint != null && bubblePrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, bubblePrefabs.Length);
            GameObject selectedPrefab = bubblePrefabs[randomIndex];

            GameObject newBubble = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
            currentControllableBubble = newBubble.GetComponent<Bubble>();

            // 如果正在反转状态，新生成的气泡也应该保持反转
            if (currentControllableBubble != null)
            {
                currentControllableBubble.isLocked = false;
                if (isCurReversing)
                {
                    currentControllableBubble.ReverseControl();
                }
            }
        }
    }

    void OnNeedNewBubble()
    {
        SpawnNewBubble();
    }

    void OnEnable()
    {
        Bubble.OnNeedNewBubble += OnNeedNewBubble;
    }

    void OnDisable()
    {
        Bubble.OnNeedNewBubble -= OnNeedNewBubble;
    }
}