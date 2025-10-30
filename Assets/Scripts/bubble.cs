using UnityEngine;
using System;

public class Bubble : MonoBehaviour
{
    public static event Action OnNeedNewBubble;
    public float moveSpeed = 0.5f;
    public float horizontalSpeed = 5f;
    // �ù�������ǿ���������ⲿҲ�ܿ���״̬
    public bool isLocked = false;
    private Rigidbody2D rb;
    // ��¼�Լ��ǲ��ǵ�ǰ�����Ƶ�����
    private bool isCurrentControllable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void Update()
    {
        // ���ж��Լ��ǲ��ǵ�ǰ�����Ƶ�����
        isCurrentControllable = FindObjectOfType<BubbleSpawner>().currentControllableBubble == this;

        // ֻ�У�û������ + �ǵ�ǰ���Ƶ����� �� ���ܶ�
        if (!isLocked && isCurrentControllable)
        {
            float h = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(h * horizontalSpeed, moveSpeed);
        }
        // �����������ȫ���������������ƶ�
    }

    // ����������
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "TopTrigger")
        {
            LockThisBubble();
        }
    }

    // ���κ����ݾ����������ܱ�ǩ��������⣩
    void OnCollisionEnter2D(Collision2D other)
    {
        // ֻҪ��ײ�������ݣ�ͨ���Ƿ���Bubble����жϣ����ñ�ǩ��
        if (other.gameObject.GetComponent<Bubble>() != null)
        {
            LockThisBubble();
        }
    }

    // ����������ǰ����
    void LockThisBubble()
    {
        isLocked = true; // ǿ������
        // �������������л����ƶ���
        BubbleSpawner spawner = FindObjectOfType<BubbleSpawner>();
        if (spawner.currentControllableBubble == this)
        {
            spawner.currentControllableBubble = null;
            OnNeedNewBubble?.Invoke(); // �����µ�
        }
    }
}