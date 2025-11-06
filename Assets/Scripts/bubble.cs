using UnityEngine;
using System;

public class Bubble : MonoBehaviour
{
    public static event Action OnNeedNewBubble;
    public float moveSpeed = 0.5f;
    public float horizontalSpeed = 5f;
    // 用公开变量强制锁死，外部也能看到状态
    // 泡泡类型枚举
    public enum BubbleType { Default, Bubble_1, Bubble_2 }
    // 当前泡泡类型（在Inspector面板可选择）
    public BubbleType bubbleType = BubbleType.Default;
    // 控制比例（根据类型设置）
    [Range(0f, 2f)] public float controlRatio = 1f;
    public bool isLocked = false;
    private Rigidbody2D rb;
    // 记录自己是不是当前被控制的气泡
    private bool isCurrentControllable;
    // 新增：控制次数相关
    public int maxControlCount = 3; // 最大可控制次数
    private int currentControlCount = 0; // 当前已控制次数
    private bool hasReachedMaxControl = false; // 是否已达最大控制次数
                                               // 新增：用于判断是否已在本次控制中计数
    private bool isCountedThisControl = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Dynamic;
        // 设置控制比例
        SetControlRatioByType();
    }
    // 根据泡泡类型设置控制比例
    private void SetControlRatioByType()
    {
        switch (bubbleType)
        {
            case BubbleType.Bubble_1:
                controlRatio = 0.8f; // 80%控制灵敏度
                break;
            case BubbleType.Bubble_2:
                controlRatio = 0.3f; // 30%控制灵敏度
                break;
            default:
                controlRatio = 1f; // 默认100%
                break;
        }
    }
    void Update()
    {
        isCurrentControllable = FindObjectOfType<BubbleSpawner>().currentControllableBubble == this;

        if (!isLocked && isCurrentControllable && !hasReachedMaxControl)
        {
            float h = Input.GetAxis("Horizontal");
            float actualHorizontalSpeed = horizontalSpeed * controlRatio;
            rb.velocity = new Vector2(h * actualHorizontalSpeed, moveSpeed);

            // 优化计数逻辑：只有输入从无到有时才计数一次
            if (Mathf.Abs(h) > 0.1f)
            {
                if (!isCountedThisControl)
                {
                    currentControlCount++;
                    isCountedThisControl = true; // 标记本次控制已计数

                    if (currentControlCount >= maxControlCount)
                    {
                        hasReachedMaxControl = true;
                        LockThisBubble();
                        return;
                    }
                }
            }
            else
            {
                // 输入为0时重置计数标记
                isCountedThisControl = false;
            }
        }
    }

    // 在OnCollisionEnter2D方法下方添加以下代码
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("exit"))
        {
            // 销毁当前泡泡
            Destroy(gameObject);
            // 触发生成新泡泡（与锁定时逻辑一致）
            BubbleSpawner spawner = FindObjectOfType<BubbleSpawner>();
            if (spawner.currentControllableBubble == this)
            {
                spawner.currentControllableBubble = null;
                OnNeedNewBubble?.Invoke();
            }
        }
    }

    // 保留原有的碰撞墙壁逻辑
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "topWall" || other.transform.tag == "Bubble")
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