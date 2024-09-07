using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Move : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Think();

        // 5초 후 실행
        Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        //고양이 움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //지형 확인
        //Vector2 frontVec = new Vector2(rigid.position.x)
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null)
        {
            if (rayHit.distance < 0.5f)
                anim.SetBool("isJumping", false);
        }
    }

    //재귀함수(자기 함수가 자신을 한번더 호출)
    void Think()
    {
        nextMove = Random.Range(0, 2);

        Invoke("Think", 5);
    }
}