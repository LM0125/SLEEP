using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    // 拖入3种颜色的气泡预制体（数组长度自动变成3）
    public GameObject[] bubblePrefabs;
    public Transform spawnPoint;
    public Bubble currentControllableBubble;

    public float spawnRate;

    void Start()
    {
        // 检查是否拖入了3种预制体
        if (bubblePrefabs.Length < 3)
        {
            Debug.LogError("请拖入3种颜色的气泡预制体！");
            return;
        }
        SpawnNewBubble();
    }

    void SpawnNewBubble()
    {
        if (spawnPoint != null)
        {
            // 随机选择数组中的一个预制体（0=第一种，1=第二种，2=第三种）
            int randomIndex = Random.Range(0, bubblePrefabs.Length);
            GameObject selectedPrefab = bubblePrefabs[randomIndex];

            // 实例化选中的预制体
            GameObject newBubble = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
            currentControllableBubble = newBubble.GetComponent<Bubble>();
            if (currentControllableBubble != null)
            {
                currentControllableBubble.isLocked = false; // 解锁新气泡
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