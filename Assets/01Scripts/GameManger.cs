using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    //점수 관리
    //스테이지 변경

    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public void NextStage()
    {
        stageIndex++;

        totalPoint += stagePoint;
        stagePoint = 0;
    }
}
