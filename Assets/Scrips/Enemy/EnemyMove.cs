using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    #region 변수 목록
    private bool isItem = true;
    protected GameManager gameManager = null;
    private Animator animator = null;
    private Collider2D col = null;
    private SpriteRenderer spriteRenderer = null;
    private BossManager bossManager = null;
    private AudioSource audioSource = null;
    #endregion

    #region 불변수들
    protected bool isDead = false;
    private bool isDamaged = false;
    private bool isStop = false;

    [Header("총알, 레이저를 쏘는가?")] [SerializeField] private bool isFire = false;
    [Header("크기를 바꾸는가?")] [SerializeField]private bool sizeChange = false;
    [Header("맞았을 때 애니메이션이 있는가?")] [SerializeField] private bool hasAni = false;
    [Header("옆으로 가는가?")] [SerializeField] private bool isVertical = false;
    
    [Header("맞을 때 느려지는가?")] [SerializeField] private bool Slow = true;
    [Header("아이템이 있는가?")] [SerializeField] private bool hasItem = false;
    [Header("혹시 잠자리인가?")] public bool isDragonFly = false;
    #endregion

    #region 적 정보
    [Header("획득 점수")] [SerializeField] protected int score = 100;
    [Header("적 HP")] [SerializeField] protected int hp = 7;
    [Header("적 이동속도")] [SerializeField] protected float speed = 0f;
    [Header("아이템")] [SerializeField] private GameObject item = null;
    [Header("맞았을 때 바뀌는 모습")] [SerializeField] private Sprite[] sprite = null;
    [Header("레이저")] [SerializeField] GameObject lazer = null;

    private float shotDeley = 1.3f;
    private float shotingtime;
    private int direction = 0;
    private float ownSpeed;
    private int ownHp;
    #endregion

    protected virtual void Start()
    {
        ownSpeed = speed;
        ownHp = hp;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        bossManager = FindObjectOfType<BossManager>();
        audioSource = GetComponent<AudioSource>();
        Started();
    }
    private void Started()
    {
        direction = Random.Range(-1, 2);
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        if (sizeChange && this.gameObject.activeInHierarchy) StartCoroutine(ChangeScale());
    }

    protected virtual void Update()
    {
        if(isFire)startFire();
        #region 좌우로 움직이는 적
        if (!isVertical)transform.Translate(Vector2.down * ownSpeed * Time.deltaTime);
        else {
            if (isStop) return;
            transform.Translate(Vector2.right * ownSpeed/2 * direction * Time.deltaTime + Vector2.down * ownSpeed/2 * Time.deltaTime);
            if (transform.position.x > gameManager.MaxPosition.x) direction = -1;
            if (transform.position.x < gameManager.MinPosition.x) direction = 1;
        }
        #endregion
        if (!isDamaged) ownSpeed += 0.01f; 
    }
    #region 총알 발사
    private void startFire()
    {
          shotingtime+=Time.deltaTime;
        if(shotingtime>shotDeley){
            shotingtime=0f;
            StartCoroutine(Fire());
        }
    }
     private IEnumerator Fire()
    {
        isStop = true;
        lazer.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        lazer.SetActive(false);
        isStop = false;
    }
    #endregion
    
    private IEnumerator ChangeScale()
    {
        Vector3 scale = Vector3.zero;
        float randomScale = 0f;
            randomScale = Random.Range(0.3f, 1f);
            transform.localScale = new Vector3(randomScale, randomScale);
        yield return 1;
    }
    #region 적이 피해를 입을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Bullet"))
        {
            if (isDamaged) return;



            DespawnB(collision.gameObject);
            StartCoroutine(Damaged());
            if (hp < 0)
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
        if(ownSpeed > 0.3f && !Slow)
        ownSpeed -= 0.5f;
        hp--;
        if (hasAni && hp>0)
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
        if(hasItem)
        {
            DropItem();
        }
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        animator.enabled = true;
        animator.Play("Boom");
        audioSource.Play();
        yield return new WaitForSeconds(0.4f);
        Despawn(gameObject);
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
        ownSpeed = speed;
        Started();
        direction = 0;
        hp = ownHp;
        isDead = false;
        isDamaged = false;
        col.enabled = true;
        if (hasAni)
        {
            animator.enabled = false;
            spriteRenderer.sprite = sprite[6];
        }
    }
        private void DropItem()
    {
        if(isItem)
        {
        item.SetActive(true);
        item.transform.SetParent(null);
        isItem = false;
        }
    }
    #endregion
 }
