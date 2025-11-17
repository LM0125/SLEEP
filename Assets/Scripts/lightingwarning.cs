using UnityEngine;

public class LightningWarning : MonoBehaviour
{
    [Tooltip("预警图标渲染器")]
    public SpriteRenderer warningRenderer;
    [Tooltip("闪烁间隔时间（秒）")]
    public float flickerTime = 0.2f; // 每0.2秒闪烁一次
    [Tooltip("预警持续总时间（秒）")]
    public float totalWarningTime = 2f; // 和闪电预警时间保持一致

    private float flickerTimer;
    private float totalTimer;
    private bool isFlickering;

    // 确保初始状态正确
    void Awake()
    {
        if (warningRenderer == null)
            warningRenderer = GetComponent<SpriteRenderer>();

        if (warningRenderer != null)
            warningRenderer.enabled = false;

        isFlickering = false;
    }

    void Update()
    {
        if (!isFlickering) return;

        // 总计时器：超过预警时间后停止
        totalTimer += Time.deltaTime;
        if (totalTimer >= totalWarningTime)
        {
            StopFlicker();
            return;
        }

        // 闪烁计时器：控制显示/隐藏切换
        flickerTimer += Time.deltaTime;
        if (flickerTimer >= flickerTime)
        {
            warningRenderer.enabled = !warningRenderer.enabled; // 切换显示状态
            flickerTimer = 0; // 重置闪烁计时器
        }
    }

    // 开始闪烁预警
    public void StartFlicker()
    {
        if (warningRenderer == null) return;

        isFlickering = true;
        totalTimer = 0;
        flickerTimer = 0;
        warningRenderer.enabled = true;
        Debug.Log("预警开始闪烁");
        gameObject.SetActive(true);
    }

    // 停止闪烁并隐藏
    public void StopFlicker()
    {
        isFlickering = false;
        if (warningRenderer != null)
            warningRenderer.enabled = false;
        gameObject.SetActive(false);
    }
}