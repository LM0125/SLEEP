using UnityEngine;
using System;

public class Bubble : MonoBehaviour
{
    public static event Action OnNeedNewBubble;
    public float moveSpeed = 0.5f;
    public float horizontalSpeed = 5f;
    // 气泡类型会影响控制效果
    public enum BubbleType { Default, Bubble_1, Bubble_2 }
    public BubbleType bubbleType = BubbleType.Default;
    [Range(0f, 2f)] public float controlRatio = 1f;
    public bool isLocked = false;
    private Rigidbody2D rb;
    private bool isCurrentControllable;
    public int maxControlCount = 3; // 最大控制次数
    private int currentControlCount = 0; // 当前已控制次数
    private bool hasReachedMaxControl = false; // 是否达到最大控制次数
    private bool isCountedThisControl = false;

    private bool isReversControl;

    public void ReverseControl()
    {
        isReversControl = true;
    }
    public void RecoverControl()
    {
        isReversControl = false;
    }
    public bool IsReversing() { return isReversControl; }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.bodyType = RigidbodyType2D.Dynamic;
        SetControlRatioByType();
    }

    private void SetControlRatioByType()
    {
        switch (bubbleType)
        {
            case BubbleType.Bubble_1:
                controlRatio = 0.8f; // 80%控制效果
                break;
            case BubbleType.Bubble_2:
                controlRatio = 0.2f; // 20%控制效果
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
            Vector2 windForce = FindObjectOfType<LevelController>().GetWindForce();

            if (isReversControl)
                h = -h;

            rb.velocity = new Vector2(h * actualHorizontalSpeed + windForce.x, moveSpeed);

            // 横向移动计数逻辑
            if (Mathf.Abs(h) > 0.1f)
            {
                if (!isCountedThisControl)
                {
                    currentControlCount++;
                    isCountedThisControl = true;

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
                isCountedThisControl = false;
            }
            
            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("exit"))
        {
            Destroy(gameObject);
            BubbleSpawner spawner = FindObjectOfType<BubbleSpawner>();
            if (spawner.currentControllableBubble == this)
            {
                spawner.currentControllableBubble = null;
                OnNeedNewBubble?.Invoke();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "topWall")
        {
            BubbleSpawner spawner = FindObjectOfType<BubbleSpawner>();
            spawner.currentControllableBubble = null;
            OnNeedNewBubble?.Invoke();
        }
    }

    public void LockThisBubble()
    {
        isLocked = true;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = new Color(0f, 0f, 0f); 
        }
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
        BubbleSpawner spawner = FindObjectOfType<BubbleSpawner>();
        if (spawner.currentControllableBubble == this)
        {
            spawner.currentControllableBubble = null;
            OnNeedNewBubble?.Invoke(); // 生成新气泡
        }
    }
}