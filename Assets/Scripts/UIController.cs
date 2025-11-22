using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Tooltip("关联的关卡控制器")]
    public LevelController levelController;

    [Tooltip("气泡计数文本")]
    public Text UIBubbleCounterText;

    [Tooltip("计时器填充图片")]
    public Image timer;

    [Tooltip("最大关卡时间（秒）")]
    public float maxLevelTime = 60f;

    void Start()
    {
        // 自动查找LevelController（如果未手动指定）
        if (levelController == null)
        {
            levelController = FindObjectOfType<LevelController>();
        }

        // 初始化UI显示
        UpdateBubbleCounter();
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (levelController != null)
        {
            UpdateBubbleCounter();
            UpdateTimerDisplay();
        }
    }

    /// <summary>
    /// 更新气泡计数器显示
    /// </summary>
    void UpdateBubbleCounter()
    {
        if (UIBubbleCounterText != null)
        {
            UIBubbleCounterText.text = $"气泡: {levelController.bubbleCounter}";
        }
    }

    /// <summary>
    /// 更新计时器显示
    /// </summary>
    void UpdateTimerDisplay()
    {
        if (timer != null && maxLevelTime > 0)
        {
            // 计算剩余时间比例
            float timeRatio = Mathf.Clamp01(1 - (levelController.curPassingTime / maxLevelTime));
            timer.fillAmount = timeRatio;
        }
    }
}