using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    // ����3����ɫ������Ԥ���壨���鳤���Զ����3��
    public GameObject[] bubblePrefabs;
    public Transform spawnPoint;
    public Bubble currentControllableBubble;

    public float spawnRate;

    void Start()
    {
        // ����Ƿ�������3��Ԥ����
        if (bubblePrefabs.Length < 3)
        {
            Debug.LogError("������3����ɫ������Ԥ���壡");
            return;
        }
        SpawnNewBubble();
    }

    void SpawnNewBubble()
    {
        if (spawnPoint != null)
        {
            // ���ѡ�������е�һ��Ԥ���壨0=��һ�֣�1=�ڶ��֣�2=�����֣�
            int randomIndex = Random.Range(0, bubblePrefabs.Length);
            GameObject selectedPrefab = bubblePrefabs[randomIndex];

            // ʵ����ѡ�е�Ԥ����
            GameObject newBubble = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
            currentControllableBubble = newBubble.GetComponent<Bubble>();
            if (currentControllableBubble != null)
            {
                currentControllableBubble.isLocked = false; // ����������
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