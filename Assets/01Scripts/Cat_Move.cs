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

        // 5�� �� ����
        Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        //����� ������
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //���� Ȯ��
        //Vector2 frontVec = new Vector2(rigid.position.x)
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider != null)
        {
            if (rayHit.distance < 0.5f)
                anim.SetBool("isJumping", false);
        }
    }

    //����Լ�(�ڱ� �Լ��� �ڽ��� �ѹ��� ȣ��)
    void Think()
    {
        nextMove = Random.Range(0, 2);

        Invoke("Think", 5);
    }
}