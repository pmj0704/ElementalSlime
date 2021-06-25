using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPArt : MonoBehaviour
{

    private GameManager gameManager = null;
    private Animator animator = null;
    private Collider2D col = null;
    private SpriteRenderer spriteRenderer = null;
    private BossManager bossManager = null;
    [Header("적 HP")]
    [SerializeField]
    private int hp = 7;
    
    [Header("적 이동속도")]
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int Dir;
    private Transform player;
    private bool isDead = false;
    private bool isDamaged = false;
    private EnemyMove enemyMove = null;
    [SerializeField]private GameObject[] newArm;
    [SerializeField]private bool isMove = false;
    [SerializeField]private bool mainPart = false;
    private AudioSource audioSource;
[Header("보스 프리팹")]
    [SerializeField]
    private GameObject bossGolem = null;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemyMove = FindObjectOfType<EnemyMove>();
        if(isMove){
        gameObject.transform.SetParent(null);
        }
        player = FindObjectOfType<PlayerMove>().transform;
        bossManager = FindObjectOfType<BossManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        if(isMove)
        {StartCoroutine(bossManager.moveByPlayer(gameObject, Dir));}
    }

    void Update()
    {
        if(mainPart && isDead)
        {
            StartCoroutine(killBoss());
        }
        if(isMove){
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
    }
     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;
        if (collision.CompareTag("Bullet"))
        {
            if (isDamaged) return;
            if(enemyMove == null) enemyMove = FindObjectOfType<EnemyMove>();
            DespawnB(collision.gameObject);
            StartCoroutine(Damaged());
            if (hp <= 0)
            {
                if (isDead) return;
                isDead = true; 
                StartCoroutine(Dead());
            }
        }
    }
    private IEnumerator killBoss(){
            bossGolem.GetComponent<SpriteRenderer>().material.SetColor("_Color", new Color(0.5f, 0.4f, 0f ,0f));
            bossGolem.GetComponent<Animator>().Play("Boom");
            audioSource.Play();
            yield return new WaitForSeconds(0.4f);
                gameManager.getLife(4);
            bossGolem.SetActive(false);
    }
    private IEnumerator Dead()
    {
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        col.enabled = false;
        yield return new WaitForSeconds(0.4f);
        
        if(mainPart)
        {
            audioSource.Play();
            yield return new WaitForSeconds (4f);
            bossGolem.SetActive(false);
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(4f);
            audioSource.Play();
        gameObject.SetActive(false);
        
    }
    private IEnumerator Damaged()
    {
        hp--;
        spriteRenderer.material.SetColor("_Color", new Color(1f, 0f, 0f, 0f));
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetColor("_Color", new Color(0f, 0f, 0f, 0f));
        isDamaged = false;
    }
     private void DespawnB(GameObject Object)
    {
        Object.transform.SetParent(gameManager.poolManager.transform, false);
        Object.SetActive(false);
    }
}

