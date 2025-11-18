using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 关卡类型枚举
public enum Level
{
    Lvl1, // 晴天
    Lvl2, // 雨天
    Lvl3  // 雪天
}

public class LevelController : MonoBehaviour
{
    [Header("面板")]
    public GameObject sunnyBoard;   // 晴天面板
    public GameObject rainnyBoard;  // 雨天面板
    public GameObject snowingBoard; // 雪天面板
    public GameObject curBoard;     // 当前显示的面板
    public Level curLevel = Level.Lvl1; // 当前关卡
    [Header("计时器")]
    public float timer;
    public float intervalTime;
    public int bubbleCounter;       // 气泡计数器
    public float curPassingTime;    // 当前关卡经过时间

    [Header("第一关设置")]
    public bool isWindOn = false;       // 风是否激活
    public GameObject leftWindEffect;   // 左侧风效果
    public GameObject rightWindEffect;  // 右侧风效果
    public float windForce = 5f;        // 风力大小
    private bool isLeftWind = false;    // 当前是否为左侧风
    
    public float windStayTime;
   

    [Header("第二关设置")]
    public bool isLightningActive = false; // 标记当前是否处于闪电生效阶段
    private List<LightningWarning> activeWarnings = new List<LightningWarning>();
    public float lightningInterval = 30f;   // 闪电间隔时间
    public float lightningWarningTime = 2f; // 预警时间
    public int lightningCount = 3;         // 每次闪电数量
    public float lightningDuration = 0.5f;  // 闪电持续时间
    public Vector2 lightningAreaMin;       // 闪电区域最小值
    public Vector2 lightningAreaMax;       // 闪电区域最大值
    public GameObject lightningPrefab;     // 闪电效果预制体
    public GameObject warningPrefab; // 闪电预警预制体
    // 闪电计时器
    private float lightningTimer;
    private float warningTimer;
    private bool isWarning;
    private List<Vector2> lightningPositions = new List<Vector2>();
    // 关卡UI面板

    [Header("Reverse计时器")]
    public float reverseTimer;
    public float reverseIntervalTime;
    public float reverseStayTime;
    public BubbleSpawner bs;


    void Start()
    {
        curBoard = sunnyBoard;
        ChangeLevel(curLevel);

        intervalTime = Random.Range(0.0f, 3.0f);

        // 初始化风效果状态
        if (leftWindEffect != null) leftWindEffect.SetActive(false);
        if (rightWindEffect != null) rightWindEffect.SetActive(false);

        // 初始化闪电计时器
        lightningTimer = 0;
        warningTimer = 0;
        isWarning = false;
    }

    void Update()
    {
        switch (curLevel)
        {
            case Level.Lvl1:
                HandleLevel1Logic();
                break;
            case Level.Lvl2:
                HandleLevel2Logic();
                break;
            case Level.Lvl3:
                
                break;
        }

        {
            timer += Time.deltaTime;
            reverseTimer += Time.deltaTime;
        }
    }
   

    // 结束控制互换
    

    /// <summary>
    /// 处理第一关逻辑（风系统）
    /// </summary>
    private void HandleLevel1Logic()
    {
        if (!isWindOn && timer > intervalTime)
        {
            // 随机时间后激活风
            isWindOn = true;
            timer = 0.0f;
            isLeftWind = Random.Range(0, 2) == 0; // 随机风向

            // 显示对应风效果
            if (isLeftWind)
            {
                leftWindEffect?.SetActive(true);
                leftWindEffect.transform.GetComponent<DOTweenAnimation>().DOPlay();
            }
            else
            {
                rightWindEffect?.SetActive(true);
                rightWindEffect.transform.GetComponent<DOTweenAnimation>().DOPlay();
            }
        }

        if (isWindOn && timer > windStayTime)
        {
            // 风持续一段时间后关闭
            isWindOn = false;
            timer = 0.0f;
            intervalTime = Random.Range(1.0f, 5.0f); // 随机下一次风的间隔

            // 隐藏风效果
            //leftWindEffect.transform.GetComponent<DOTweenAnimation>().DORewind();
            //rightWindEffect.transform.GetComponent<DOTweenAnimation>().DORewind();
            leftWindEffect?.SetActive(false);
            rightWindEffect?.SetActive(false);
        }
    }

    /// <summary>
    /// 处理第二关逻辑（闪电系统）
    /// </summary>
    private void HandleLevel2Logic()
    {
        if (!isWarning)
        {
            lightningTimer += Time.deltaTime;
            if (lightningTimer >= lightningInterval)
            {
                // 预警开始，重置闪电生效状态
                isLightningActive = false;
                isWarning = true;
                warningTimer = 0;
                GenerateLightningPositions();
            }
        }
        else
        {
            warningTimer += Time.deltaTime;
            if (warningTimer >= lightningWarningTime)
            {
                // 闪电生效，设置状态为true
                isLightningActive = true;
                StrikeLightning();
                isWarning = false;
                lightningTimer = 0;

                // 启动协程，在闪电持续时间后重置状态
                StartCoroutine(DeactivateLightningAfterDelay(lightningDuration));
            }
        }

        if (reverseTimer > reverseIntervalTime)
        {
            // show up some sprites event
            bs.ReversCurBubbleControl();
            reverseTimer = 0.0f;
        }

        if (bs.IsCurReversing() && reverseTimer > reverseStayTime)
        {
            // show up some sprites event
            bs.RecoverCurBubbleControl();
            reverseTimer = 0.0f;
        }
    }

    // 新增协程：闪电持续时间后重置生效状态
    IEnumerator DeactivateLightningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isLightningActive = false;
    }

    /// <summary>
    /// 获取当前风力（仅第一关有效）
    /// </summary>
    public Vector2 GetWindForce()
    {
        if (curLevel != Level.Lvl1 || !isWindOn) return Vector2.zero;

        // 左侧风向右，右侧风向左
        return isLeftWind ? new Vector2(windForce, 0) : new Vector2(-windForce, 0);
    }

    /// <summary>
    /// 退出关卡时的清理逻辑
    /// </summary>
    void ExitLevel(Level level)
    {
        switch (level)
        {
            case Level.Lvl1:
                // 第一关退出逻辑
                break;
            case Level.Lvl2:
                // 第二关退出逻辑
                break;
            case Level.Lvl3:
                // 第三关退出逻辑
                break;
        }
    }

    /// <summary>
    /// 生成闪电位置
    /// </summary>
    private void GenerateLightningPositions()
    {
        // 先清除旧的预警

        lightningPositions.Clear();
        for (int i = 0; i < 3; i++)
        {
            // 生成随机位置
            float x = Random.Range(lightningAreaMin.x, lightningAreaMax.x);
            float y = Random.Range(lightningAreaMin.y, lightningAreaMax.y);
            Vector2 pos = new Vector2(x, y);
            lightningPositions.Add(pos);

            // 生成预警
            if (warningPrefab != null)
            {
                GameObject warningObj = Instantiate(warningPrefab, pos, Quaternion.identity);
                LightningWarning warning = warningObj.GetComponent<LightningWarning>();
                if (warning != null)
                {
                    Debug.Log("预警实例化成功，位置：" + pos);
                    warning.StartFlicker();
                    activeWarnings.Add(warning);
                }
                else
                {
                    Debug.LogError("预警预制体缺少LightningWarning组件！");
                    Destroy(warningObj);
                }
            }
            else
            {
                Debug.LogError("请给LevelController赋值warningPrefab！");
            }
        }
    }

    /// <summary>
    /// 执行闪电打击
    /// </summary>
    void StrikeLightning()
    {
        foreach (var pos in lightningPositions)
        {
            // 实例化闪电效果
            if (lightningPrefab != null)
            {
                GameObject lightning = Instantiate(lightningPrefab, pos, Quaternion.identity);
                Destroy(lightning, lightningDuration);
            }

        }
        // 清理所有预警对象
        foreach (var warning in activeWarnings)
        {
            if (warning != null)
                Destroy(warning.gameObject);
        }
        activeWarnings.Clear();
    }

    /// <summary>
    /// 进入关卡时的初始化逻辑
    /// </summary>
    void EnterLevel(Level level)
    {
        switch (level)
        {
            case Level.Lvl1:
                curBoard?.SetActive(false);
                curBoard = sunnyBoard;
                curBoard?.SetActive(true);
                break;
            case Level.Lvl2:
                curBoard?.SetActive(false);
                curBoard = rainnyBoard;
                curBoard?.SetActive(true);
                break;
            case Level.Lvl3:
                curBoard?.SetActive(false);
                curBoard = snowingBoard;
                curBoard?.SetActive(true);
                break;
        }
    }
    public void ChangeLevel(Level lvl)
    {
        // 关卡切换逻辑（如面板切换、状态重置等）
        ExitLevel(curLevel);
        curLevel = lvl;
        EnterLevel(curLevel);

    }
}