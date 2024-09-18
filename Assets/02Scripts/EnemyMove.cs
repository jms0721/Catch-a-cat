using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    public int nextMove;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Think();

        Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        //움직임
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //지형확인
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            //Debug.Log("경고! 낭떨어지");
            Turn();
        }
    }

    //재귀 함수
    void Think()
    {
        //다음 활동
        nextMove = Random.Range(-1, 2);

        //에니메이션 실행
        anim.SetInteger("WalkSpeed", nextMove);

        //방향
        if(nextMove != 0)
            spriteRenderer.flipX = nextMove == -1;

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == -1;

        CancelInvoke();
        Invoke("Think", 2);
    }
}
