using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    //���� ����
    //�������� ����

    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public Butler_Move butler_Move;
    public GameObject[] Stages;

    public void NextStage()
    {
        //�������� ����
        if(stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
        }
        else
        {
            //���� �Ϸ�
            Time.timeScale = 0;
            Debug.Log("���� �Ϸ�");
        }
        totalPoint += stagePoint;
        stagePoint = 0;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            health--;

            collision.attachedRigidbody.velocity = Vector2.zero;
            collision.transform.position = new Vector3(0, 0, -1);
        }
            
    }

    void PlayerReposition()
    {
        butler_Move.transform.position = new Vector3(0, 0, -1);
        butler_Move.VelocityZero();
    }

}
