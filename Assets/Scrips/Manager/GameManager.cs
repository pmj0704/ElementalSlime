using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 변수 목록
    [SerializeField]
    private Text textScore = null;
    [SerializeField]
    private Text textHighScore = null;


    [Header("바람 프리팹")]
    [SerializeField]
    private GameObject windPrefab = null;
    [SerializeField]
    private float windDealy = 0f;
    [Header("돌 프리팹")]
    [SerializeField]
    private GameObject enemyStone = null;
    [Header("픽시 프리팹")]
    [SerializeField]
    private GameObject enemyPixi = null;
[Header("보스 프리팹")]
    [SerializeField]
    private GameObject bossGolem = null;

    [SerializeField] private SpriteRenderer lifeSpriteRen = null;
    private SpriteRenderer spriteRenderer = null;
    [SerializeField] private SpriteRenderer playerSprite = null;
    [SerializeField]
    private Sprite[] Life = null;
    private PlayerMove playerMove = null;
    public Vector2 MinPosition { get; private set; }
    public Vector2 MaxPosition { get; private set; }
    public PoolManager poolManager { get; private set; }
    public ObjectManager objectManager { get; private set; }
    public EnemyBulletManager enemyBulletManager { get; private set; }

    private EnemyMove enemyMove;

    private int score = 0;
    private int life = 4;
    private int highScore = 0;

    private Coroutine enemyCoroutine = null;
    [SerializeField] private GameObject mainBody;

    #endregion

    #region 시작, 업데이트
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HIGHSCORE");
        enemyMove = FindObjectOfType<EnemyMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        poolManager = FindObjectOfType<PoolManager>();
        objectManager = FindObjectOfType<ObjectManager>();
        enemyBulletManager = FindObjectOfType<EnemyBulletManager>();

        playerMove = FindObjectOfType<PlayerMove>();
        MinPosition = new Vector2(-2f, -4.3f);
        MaxPosition = new Vector2(2f, 4.3f);
        StartCoroutine(SpawnWind());
        enemyCoroutine = StartCoroutine(Wait());
        UpdateUI();
        hangaesik();
    }
    void Update()
    {
        if(!mainBody.activeInHierarchy){
            bossGolem.SetActive(false);
        }
        
    if(score >= 1000) 
    {
    bossGolem.SetActive(true);
    }
        lifeSpriteRen.sprite = Life[life];
        Shrink();
    }
    private void UpdateUI()
    {
        textScore.text = string.Format("SCORE: {0}", score);
        textHighScore.text = string.Format("HIGHSCORE: {0}", highScore);
    }
    public void AddScore(int addscore)
    {
        score += addscore;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HIGHSCORE", highScore);
        }
        UpdateUI();
    }
    public void Dead()
    {
        life--;
        if (life <= 0) SceneManager.LoadScene("GameOver");
        UpdateUI();
    }
    #endregion

    #region 적생성
    private IEnumerator Wait()
    {
        
        float randomX = 0f;
        float randomDelay = 0f;
        int randomEnemy = 0;
        while(true)
        {
            randomX = Random.Range(-1.7f, 1.7f);
            randomDelay = Random.Range(1f, 5f);
            randomEnemy = Random.Range(0, 2);
            SpawnEnemy(randomX, randomEnemy);
            yield return new WaitForSeconds(randomDelay);
        }
    }
    private void SpawnEnemy(float randomX, int randomEnemy)
    {
            GameObject result = null;
            if (objectManager.transform.childCount > 0)
            {
                result = objectManager.transform.GetChild(0).gameObject;
                result.transform.position = new Vector2(randomX, 6f);
                result.transform.SetParent(null);
                 result.SetActive(true);
            }
            else
            {
                switch(randomEnemy)
                {
                    case 0:
                        spawnHelpEnemy(enemyPixi, randomX);
                        break;
                    case 1:
                        spawnHelpEnemy(enemyStone, randomX);
                        break;
                }               
            }
        }
    private void hangaesik()
    {
        spawnHelpEnemy(enemyPixi, 231233f);
        spawnHelpEnemy(enemyStone, 3123123f);
    }
    private void spawnHelpEnemy(GameObject Enemy, float randomX)
    {
        Enemy = Instantiate(Enemy);
        Enemy.transform.position = new Vector2(randomX, 6f);
        Enemy.transform.SetParent(null);
    }


    private void Shrink()
    {
        switch (life)
        {
            case 4:
                break;
            case 3:
                playerMove.SetDelay(0.3f);
                playerSprite.transform.localScale = new Vector2(2f, 2f);

                break;
            case 2:
                playerMove.SetDelay(0.2f);
                playerSprite.transform.localScale = new Vector2(1.5f, 1.5f);
                break;
            case 1:
                playerMove.SetDelay(0.1f);
                playerSprite.transform.localScale = new Vector2(1f, 1f);
                break;
        }
    }
    #endregion

    #region 바람 생성
    private IEnumerator SpawnWind()
    {
        while (true)
        {
            InstantiateWind();
            yield return new WaitForSeconds(windDealy);
        }
    }
    private GameObject InstantiateWind()
    {
        GameObject resul = null;
        if (objectManager.transform.childCount > 0)
        {
            resul = objectManager.transform.GetChild(0).gameObject;
            resul.transform.position = new Vector2(0f, 5.3f);
            resul.transform.SetParent(null);
            resul.SetActive(true);
        }
        else
        {
            GameObject Wind = Instantiate(windPrefab);
            Wind.transform.position = new Vector2(0f, 5.3f);
            Wind.transform.SetParent(null);
            resul = Wind;
        }
        return resul;
    }
    #endregion

}
