using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject[] bubblePrefabs;
    public Transform spawnPoint;
    public Bubble currentControllableBubble;
    public float spawnRate;

    void Start()
    {
        if (bubblePrefabs.Length < 2)
        {
            Debug.LogError("请至少添加2个气泡预制体!");
            return;
        }
        SpawnNewBubble();
    }

    void SpawnNewBubble()
    {
        if (spawnPoint != null)
        {
            int randomIndex = Random.Range(0, bubblePrefabs.Length);
            GameObject selectedPrefab = bubblePrefabs[randomIndex];

            GameObject newBubble = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
            currentControllableBubble = newBubble.GetComponent<Bubble>();
            if (currentControllableBubble != null)
            {
                currentControllableBubble.isLocked = false;
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