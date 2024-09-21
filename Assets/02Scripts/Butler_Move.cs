using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Butler_Move : MonoBehaviour
{
    public GameManager gamerManager;
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;
    public float maxSpeed;
    public float jumpPower;
    //public int butlerCoins;
    public int itemPrice = 3000;
    public GameObject obj;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0);

        //���� ����
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            PlaySound("JUMP");
        }

        //���ǵ� ����
        if (Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        //���� ���� ��ȯ
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //������ ����(Animation)
        if (Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
    }

    private void FixedUpdate()
    {
        //Ű���� ������ / A = ����, D = ������
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)//������ �ִ� �ӷ�
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1))//���� �ִ� �ӷ�
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //���� Ȯ��
        if (rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag =="Enemy")
        {
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else//Debug.Log("���Ϳ� ����");
                OnDamaged(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            //����
            gamerManager.stagePoint += 100;
            //���� ������ �������
            collision.gameObject.SetActive(false);

            PlaySound("ITEM");
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //���� ���������� �̵�
            gamerManager.NextStage();

            PlaySound("FINISH");
        }

        if(collision.CompareTag("ShopItem"))
        {
            PurchaseItem();
        }
    }

    private void PurchaseItem()
    {
        if(gamerManager.stagePoint >= itemPrice)
        {
            gamerManager.stagePoint -= itemPrice;
            Debug.Log("���� ���� �Ϸ�, ���� �� : " + gamerManager.stagePoint);
        }
        else
        {
            Debug.Log("���� ����");
        }
    }

    void OnAttack(Transform enemy)
    {
        //����
        gamerManager.stagePoint += 100;
        //������ �ڼ�
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        //���� ����
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();

        PlaySound("ATTACK");
    }

    void OnDamaged(Vector2 targetPos)
    {
        //ü�� �����
        gamerManager.HealthDown();
        //���̾� ����
        gameObject.layer = 11;

        //�÷��̾� �� ����
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //���Ϳ� �پ��� �� ƨ���� ����
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7, ForceMode2D.Impulse);

        //���ϸ��̼�
        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 3);

        PlaySound("DAMAGED");
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        PlaySound("DIE");
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }


    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
                
        }
        audioSource.Play();
    }


}
