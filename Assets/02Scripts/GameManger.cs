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
    public Butler_Move butler_Move;
    public GameObject[] Stages;

    public void NextStage()
    {
        //스테이지 변경
        if(stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
        }
        else
        {
            //게임 완료
            Time.timeScale = 0;
            Debug.Log("게임 완료");
        }
        totalPoint += stagePoint;
        stagePoint = 0;

    }

    //void ButlerReposition()
    //{
    //    butler_Move.transform.position = new Vector3(0, 0, -1);
    //    butler_Mo
    //}
}
