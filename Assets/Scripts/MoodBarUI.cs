using UnityEngine;
using UnityEngine.UI;

public class MoodBarUI : MonoBehaviour
{
    [Header("UI 组件")]
    public Slider moodSlider; // 关联场景中的情绪进度条Slider

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

            // 更新初始颜色
            UpdateBarColor();
        }
        else
        {
            Debug.LogError("MoodBarUI: 未为 Mood Slider 字段赋值");
        }
    }

    // 增加情绪值（气泡到达出口时调用）
    public void IncreaseMood(int amount = 1)
    {
        currentMood = Mathf.Min(currentMood + amount, maxMood);
        moodSlider.value = currentMood;
        UpdateBarColor();
    }

    // 减少情绪值（气泡固化时调用）
    public void DecreaseMood(int amount = 1)
    {
        currentMood = Mathf.Max(currentMood - amount, minMood);
        moodSlider.value = currentMood;
        UpdateBarColor();
    }

    // 根据当前情绪值更新进度条颜色
    private void UpdateBarColor()
    {
        if (moodSlider.fillRect == null) return;

        Image fillImage = moodSlider.fillRect.GetComponent<Image>();
        if (fillImage != null)
        {
            // 情绪从低到高的颜色变化：红 -> 黄 -> 绿
            if (currentMood <= maxMood * 0.3f)
            {
                fillImage.color = Color.red;
            }
            else if (currentMood <= maxMood * 0.7f)
            {
                fillImage.color = Color.yellow;
            }
            else
            {
                fillImage.color = Color.green;
            }
        }
    }

    // 检查是否达到最大情绪值
    public bool IsMoodMaxed()
    {
        return currentMood >= maxMood;
    }
}