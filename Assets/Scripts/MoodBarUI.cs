using UnityEngine;
using UnityEngine.UI;
using TMPro; // 引入 TextMeshPro 命名空间

public class MoodBarUI : MonoBehaviour
{
    [Header("UI 组件")]
    public Slider moodSlider; // 关联场景中的情绪进度条 Slider
    public TextMeshProUGUI moodValueText; // 关联我们刚刚创建的文本对象

    [Header("情绪值设置")]
    public int maxMood = 10;      // 最大情绪值
    public int minMood = 0;       // 最小情绪值
    public int startingMood = 5;  // 初始情绪值

    private int currentMood;

    void Start()
    {
        // 初始化情绪值
        if (moodSlider != null)
        {
            moodSlider.maxValue = maxMood;
            moodSlider.minValue = minMood;
            currentMood = startingMood;
            moodSlider.value = currentMood;

            // 更新UI（包括进度条和文本）
            UpdateUI();
        }
        else
        {
            Debug.LogError("MoodBarUI: 未为 Mood Slider 字段赋值");
        }

        // 检查文本组件是否已赋值
        if (moodValueText == null)
        {
            Debug.LogError("MoodBarUI: 请为 Mood Value Text 字段赋值");
        }
    }

    // 增加情绪值（气泡到达出口时调用）
    public void IncreaseMood(int amount = 1)
    {
        currentMood = Mathf.Min(currentMood + amount, maxMood);
        UpdateUI();
    }

    // 减少情绪值（气泡固化时调用）
    public void DecreaseMood(int amount = 1)
    {
        currentMood = Mathf.Max(currentMood - amount, minMood);
        UpdateUI();
    }

    /// <summary>
    /// 统一更新进度条和文本的显示
    /// </summary>
    private void UpdateUI()
    {
        if (moodSlider != null)
        {
            moodSlider.value = currentMood;
        }

        if (moodValueText != null)
        {
            // 更新文本内容，格式为 "当前值 / 最大值"
            moodValueText.text = $"{currentMood}/{maxMood}";
        }

        
    }

    

    // 检查是否达到最大情绪值
    public bool IsMoodMaxed()
    {
        return currentMood >= maxMood;
    }
}