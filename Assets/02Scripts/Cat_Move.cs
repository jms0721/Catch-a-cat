using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat_Move : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator anim;
    public int nextMove;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // ���� �� ����
        Invoke("Think", 1);
    }

    private void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);
    }

    private void FixedUpdate()
    {
        //����� ������
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        //���� Ȯ��
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rayHit.collider == null)
        {
            //Debug.Log("���! �� ���� ��������"); //�տ� ���������� �ִٸ� �����
            nextMove *= 0;
            CancelInvoke();
            Invoke("Think", 1);
        }
    }

    //����Լ�(�ڱ� �Լ��� �ڽ��� �ѹ��� ȣ��)
    //����� ������
    void Think()
    {
        nextMove = Random.Range(1, 2);

        Invoke("Think", 1);

        anim.SetInteger("WalkSpeed", nextMove);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}