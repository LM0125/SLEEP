using UnityEngine;
using TMPro;

public class QuestionBubble : MonoBehaviour
{
    public Transform player;            // 玩家摄像机
    public Transform exitPoint;         // 出口目标
    public float showDistance = 3f;     // 显示文字的距离
    public float flySpeed = 2f;         // 飞向出口速度
    public float fadeSpeed = 1f;        // 消失速度

    private TextMeshPro textMesh;
    private Renderer bubbleRenderer;
    private Material bubbleMat;
    private bool isClicked = false;

    void Start()
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
        bubbleRenderer = GetComponent<Renderer>();
        bubbleMat = bubbleRenderer.material;

        // 文字一开始透明
        Color ct = textMesh.color;
        ct.a = 0;
        textMesh.color = ct;
    }

    void Update()
    {
        FacePlayer();
        HandleTextDisplay();

        if (isClicked)
        {
            FlyAndFade();
        }
    }

    // 让文字永远面向玩家
    void FacePlayer()
    {
        if (textMesh == null) return;
        textMesh.transform.LookAt(player);
        textMesh.transform.Rotate(0, 180, 0);  // 反转
    }

    // 接近气泡 → 显示文字
    void HandleTextDisplay()
    {
        float dist = Vector3.Distance(transform.position, player.position);

        Color ct = textMesh.color;

        if (dist <= showDistance)
        {
            ct.a = Mathf.Lerp(ct.a, 1, Time.deltaTime * 3f);   // 淡入
        }
        else
        {
            ct.a = Mathf.Lerp(ct.a, 0, Time.deltaTime * 3f);   // 淡出
        }

        textMesh.color = ct;
    }

    // 点击触发飞向出口
    void OnMouseDown()
    {
        if (!isClicked)
            isClicked = true;
    }

    // 飞向出口 & 淡出
    void FlyAndFade()
    {
        // 移动
        transform.position = Vector3.MoveTowards(
            transform.position,
            exitPoint.position,
            flySpeed * Time.deltaTime);

        // 当靠近出口 → 开始淡出
        if (Vector3.Distance(transform.position, exitPoint.position) < 0.5f)
        {
            // 气泡淡出
            Color c = bubbleMat.color;
            c.a -= fadeSpeed * Time.deltaTime;
            bubbleMat.color = c;

            // 文字淡出
            Color ct = textMesh.color;
            ct.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = ct;

            if (c.a <= 0)
                Destroy(gameObject);
        }
    }
}
