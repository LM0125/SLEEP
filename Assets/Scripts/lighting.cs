using UnityEngine;

public class Lightning : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        // 检测碰撞对象是否为气泡
        Bubble bubble = other.GetComponent<Bubble>();
        if (bubble != null && !bubble.isLocked)
        {
            bubble.LockThisBubble(); // 固化气泡
        }
    }
}