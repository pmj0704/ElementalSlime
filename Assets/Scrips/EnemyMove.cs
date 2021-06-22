using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    #region 변수 목록

    private GameManager gameManager = null;
    private Animator animator = null;
    private Collider2D col = null;
    private SpriteRenderer spriteRenderer = null;
    protected bool isDead = false;
    private bool isDamaged = false;

    [Header("획득 점수")]
    [SerializeField]
    private int score = 100;
    [Header("적 HP")]
    [SerializeField]
    private int hp = 7;
    [Header("적 이동속도")]
    [SerializeField]
    protected float speed = 5f;
    [SerializeField]
    private bool sizeChange = false;
    [SerializeField]
    private bool hasAni = false;
    [SerializeField] private Sprite[] sprite = null;
    [SerializeField] private bool isVertical = false;
    [SerializeField] private bool isFire = false;
    private int direction = 0;
    [SerializeField] GameObject lazer = null;
    private bool isStop = false;
    private float shotDeley = 1.3f;
    private float shotingtime;
    private Transform player;
    private BossManager bossManager = null;
    [SerializeField]
    private int Dir;
    [SerializeField]
    private bool Udo = false;

    #endregion

    protected virtual void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        bossManager = FindObjectOfType<BossManager>();
        Started();
        if(Udo)StartCoroutine(bossManager.moveByPlayer(gameObject, Dir));
        player = FindObjectOfType<PlayerMove>().transform;
    }
    private void Started()
    {
        direction = Random.Range(-1, 2);
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        if (sizeChange && this.gameObject.activeInHierarchy) StartCoroutine(ChangeScale());
    }

    protected virtual void Update()
    {
        if(Udo)Udotan();
        if(isFire)startFire();
        if (!isVertical)
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        else {
            if (isStop) return;
            transform.Translate(Vector2.right * speed * direction * Time.deltaTime + Vector2.down * speed * Time.deltaTime);
        if (transform.position.x > gameManager.MaxPosition.x) direction = -1;
        if (transform.position.x < gameManager.MinPosition.x) direction = 1;
    }
        if (transform.position.y <= gameManager.MinPosition.y) Despawn(gameObject);
        if (!isDamaged) speed += 0.01f; 
    }

    private void startFire()
    {
          shotingtime+=Time.deltaTime;
        if(shotingtime>shotDeley){
            shotingtime=0f;
            StartCoroutine(Fire());
        }
    }
    
    private IEnumerator ChangeScale()
    {
        Vector3 scale = Vector3.zero;
        float randomScale = 0f;
            randomScale = Random.Range(0.3f, 1f);
            transform.localScale = new Vector3(randomScale, randomScale);
        yield return 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Bullet"))
        {
            if (isDamaged) return;



            DespawnB(collision.gameObject);
            StartCoroutine(Damaged());
            if (hp <= 0)
            {
                if (isDead) return;
                isDead = true; //1번 실행
                gameManager.AddScore(score);
                StartCoroutine(Dead());
            }
        }
    }
    private IEnumerator Damaged()
    {
        if(speed > 0.3f)
        speed -= 0.5f;
        hp--;
        if (hasAni)
        {
            spriteRenderer.sprite = sprite[hp];
        }
        spriteRenderer.material.SetColor("_Color", new Color(1f, 0f, 0f, 0f));
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        isDamaged = false;
    }
    private IEnumerator Dead()
    {
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        animator.enabled = true;
        animator.Play("Boom");
        yield return new WaitForSeconds(0.4f);
        Despawn(gameObject);
    }
    private IEnumerator Fire()
    {
        isStop = true;
        lazer.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        lazer.SetActive(false);
        isStop = false;
    }
    public void Despawn(GameObject Object)
    {
        Object.transform.SetParent(gameManager.objectManager.transform, false);
        Object.SetActive(false);
        State();
    }
    
         private void DespawnB(GameObject Object)
    {
        Object.transform.SetParent(gameManager.poolManager.transform, false);
        Object.SetActive(false);
    }
    
    public void State()
    {
        speed = 1f;
        Started();
        direction = 0;
        hp = 7;
        isDead = false;
        isDamaged = false;
        col.enabled = true;
        if (hasAni)
        {
            animator.enabled = false;
            spriteRenderer.sprite = sprite[6];
        }
    }
    private void Udotan()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
 }
