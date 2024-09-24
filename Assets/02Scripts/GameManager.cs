using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //점수 관리
    //스테이지 변경

    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public int sanckPoint;
    public Butler_Move butler_Move;
    public GameObject[] Stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UISanck;
    public Text UIStage;
    public GameObject UIRestartBtn;
    public GameObject UIRestartFinish;

    private void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
        UISanck.text = sanckPoint.ToString();
    }

    public void NextStage()
    {
        //스테이지 변경
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else
        {
            //게임 완료
            Time.timeScale = 0;
            //Debug.Log("게임 완료");
            //butten UI
            if (UIRestartFinish != null)
            {
                UIRestartFinish.SetActive(true);
            }
        }
        //포인트
        totalPoint += stagePoint;
        stagePoint = 0;

    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;
            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            //마지막 플레이어 이미지 삭제
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);
            //플레이어 사망
            butler_Move.OnDie();
            //UI
            //Debug.Log("사망");
            //butten UI
            Invoke("ViewBtn", 3);
        }
    }

    void ViewBtn()
    {
        if (UIRestartBtn != null)
        {
            UIRestartBtn.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //플레이어 리스폰
            if (health > 1)
                PlayerReposition();

            HealthDown();
        }

    }

    void PlayerReposition()
    {
        butler_Move.transform.position = new Vector3(-7, 0, 2);
        butler_Move.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

}