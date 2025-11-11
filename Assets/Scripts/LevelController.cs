using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Level
{
    Lvl1, // sunny
    Lvl2, // rainy
    Lvl3, // Snowing
}
public class LevelController : MonoBehaviour
{
    // Finite State Machine

    public Level curLevel = Level.Lvl1;

    [Header("Lvl_1")]
    public bool isWindOn = false;


    [Header("Lvl_2")]
    public bool triggerLightening = false;



    public GameObject sunnyBoard;
    public GameObject rainnyBoard;
    public GameObject snowingBoard;

    public GameObject curBoard;

    public float timer;
    public float intervalTime;
    public float windStayTime;

    public int bubbleCounter;
    public float curPassingTime;

    // Start is called before the first frame update
    void Start()
    {
        //curLevel = Level.Lvl1;
        curBoard = sunnyBoard;
        ChangeLevel(curLevel);

        intervalTime = Random.Range(0.0f, 3.0f);

    }

    // Update is called once per frame
    void Update()
    {
        switch (curLevel)
        {
            case Level.Lvl1:

                if(!isWindOn && timer > intervalTime)
                {
                    // every random second (4-8s) // timer
                    isWindOn = true;
                    timer = 0.0f;
                }

                if(isWindOn && timer > windStayTime)
                {
                    // wind will stay around 7s
                    isWindOn = false;
                    timer = 0.0f;
                    intervalTime = Random.Range(1.0f, 5.0f);
                }
                

                // logic about how win or lose
                // if(bubble counts > winner number)
                    // ChangeLevel(Level.Lvl2)
                break;
            case Level.Lvl2:



                break;
            case Level.Lvl3:



                break;
        }

        timer += Time.deltaTime;
    }

    void ExitLevel(Level level)
    {
        switch (level)
        {
            case Level.Lvl1:

                // score; / zhuang; win/lose;

                // local computer

                break;
            case Level.Lvl2:

                break;
            case Level.Lvl3:

                break;
        }
    }
    void EnterLevel(Level level)
    {
        switch (level)
        {
            case Level.Lvl1:
                if (curBoard == null)
                    break;
                curBoard.SetActive(false);
                curBoard = sunnyBoard;
                curBoard.SetActive(true);

                break;
            case Level.Lvl2:
                if (curBoard == null)
                    break;
                curBoard.SetActive(false);
                curBoard = rainnyBoard;
                curBoard.SetActive(true);

                break;
            case Level.Lvl3:
                if (curBoard == null)
                    break;
                curBoard.SetActive(false);
                curBoard = snowingBoard;
                curBoard.SetActive(true);

                break;
        }
    }
    void ChangeLevel(Level lvl)
    {
        //
        ExitLevel(curLevel);
        //
        curLevel = lvl;
        //
        EnterLevel(curLevel);
    }
}