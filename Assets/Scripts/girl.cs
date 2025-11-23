using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 新增命名空间引用

public class GirlSleepingController : MonoBehaviour
{
    [Header("UI 引用")]
    public Image girlImage; // 拖拽场景中的 GirlImage 对象到这里

    [Header("图片资源")]
    public Sprite leftLyingSprite;  // 拖拽左侧躺的图片资源到这里
    public Sprite rightLyingSprite; // 拖拽右侧躺的图片资源到这里

    // 可选：添加一个 bool 来防止按键过于频繁切换
    private bool canSwitch = true;
    public float switchCooldown = 0.2f; // 切换冷却时间

    void Update()
    {
        // 检查是否可以切换
        if (!canSwitch)
        {
            return;
        }

        // 检测左键输入
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchToLeft();
            StartCoroutine(SwitchCooldown());
        }
        // 检测右键输入
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchToRight();
            StartCoroutine(SwitchCooldown());
        }
    }

    /// <summary>
    /// 切换到左侧躺的图片
    /// </summary>
    void SwitchToLeft()
    {
        if (girlImage != null && leftLyingSprite != null)
        {
            girlImage.sprite = leftLyingSprite;
        }
        else
        {
            Debug.LogWarning("GirlSleepingController: 图片或 Image 组件未正确赋值！");
        }
    }

    /// <summary>
    /// 切换到右侧躺的图片
    /// </summary>
    void SwitchToRight()
    {
        if (girlImage != null && rightLyingSprite != null)
        {
            girlImage.sprite = rightLyingSprite;
        }
        else
        {
            Debug.LogWarning("GirlSleepingController: 图片或 Image 组件未正确赋值！");
        }
    }

    /// <summary>
    /// 切换冷却的协程，防止按键抖动
    /// </summary>
    IEnumerator SwitchCooldown()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchCooldown);
        canSwitch = true;
    }
}