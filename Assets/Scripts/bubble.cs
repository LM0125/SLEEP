using UnityEngine;
using System;

public class Bubble : MonoBehaviour
{
    public static event Action OnNeedNewBubble;
    public float moveSpeed = 0.5f;
    public float horizontalSpeed = 5f;
    // 用公开变量强制锁死，外部也能看到状态
    public bool isLocked = false;
    private Rigidbody2D rb;
    // 记录自己是不是当前被控制的气泡
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
        // 先判断自己是不是当前被控制的气泡
        isCurrentControllable = FindObjectOfType<BubbleSpawner>().currentControllableBubble == this;

        // 只有：没被锁定 + 是当前控制的气泡 → 才能动
        if (!isLocked && isCurrentControllable)
        {
            float h = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(h * horizontalSpeed, moveSpeed);
        }
        // 其他情况：完全不处理，保留物理移动
    }

    // 触顶就锁死
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "TopTrigger")
        {
            LockThisBubble();
        }
    }

    // 碰任何气泡就锁死（不管标签，暴力检测）
    void OnCollisionEnter2D(Collision2D other)
    {
        // 只要碰撞的是气泡（通过是否有Bubble组件判断，不用标签）
        if (other.gameObject.GetComponent<Bubble>() != null)
        {
            LockThisBubble();
        }
    }

    // 暴力锁死当前气泡
    void LockThisBubble()
    {
        isLocked = true; // 强制锁死
        // 立即让生成器切换控制对象
        BubbleSpawner spawner = FindObjectOfType<BubbleSpawner>();
        if (spawner.currentControllableBubble == this)
        {
            spawner.currentControllableBubble = null;
            OnNeedNewBubble?.Invoke(); // 生成新的
        }
    }
}