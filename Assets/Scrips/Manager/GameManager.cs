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
    [SerializeField] GameObject[] enemiesWithItem = null;

    [SerializeField] GameObject Quit;

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
    [Header(" 프리팹")]
    [SerializeField] GameObject enemyDragonFly = null;


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
    [Header("보스 프리팹")]
    [SerializeField]
    private GameObject bossGolem = null;
    private EnemyMove enemyMove;

    private int score = 0;
    private int life = 4;
    private int highScore = 0;
private bool bossActivate = true;
    private Coroutine enemyCoroutine = null;
    [SerializeField] private GameObject mainBody;
    #endregion

    #region 시작, 업데이트
    void Start()
    {
        StartCoroutine(randomItemEnemySpawn());
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
        spawnOne();
        StartCoroutine(SpawnDragonfly());
    }
    public void getLife(int num)
    {
        life = num;
        Shrink();
    }
    void Update()
    {
        
    if(score >= (2000 + highScore/8) && bossActivate) 
    {
        bossGolem.SetActive(true);
        bossActivate = false;
    }
    if(life < 5){
        lifeSpriteRen.sprite = Life[life];
    }
    else lifeSpriteRen.sprite = Life[4];
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
        if (life <= 0){SceneManager.LoadScene("GameOver");
        }
        UpdateUI();
    }
    #endregion

    #region 적생성
    private IEnumerator Wait()
    {
        
        float randomX = 0f;
        float randomY = 0f;
        float randomDelay = 0f;
        int randomEnemy = 0;
        while(true)
        {
            randomY = Random.Range(-2f, MaxPosition.y - 2f);
            randomX = Random.Range(-1.7f, 1.7f);
            randomDelay = Random.Range(1f, 5f);
            randomEnemy = Random.Range(0, 3);
            SpawnEnemy(randomX, randomY,randomEnemy);
            yield return new WaitForSeconds(randomDelay);
        }
    }
    private void SpawnEnemy(float randomX, float randomY, int randomEnemy)
    {
            GameObject result = null;
            if (objectManager.transform.childCount > 0)
            {
                result = objectManager.transform.GetChild(0).gameObject;
                if(result.GetComponent<EnemyMove>().isDragonFly){
                result.transform.position = new Vector2(3f, randomY);
                }
                else{
                result.transform.position = new Vector2(randomX, 6f);
                }
                result.transform.SetParent(null);
                result.SetActive(true);
            }
            else
            {
                switch(randomEnemy)
                {
                    case 0:
                        spawnHelpEnemy(enemyPixi, randomX, 6f);
                        break;
                    case 1:
                        spawnHelpEnemy(enemyStone, randomX, 6f);
                        break;
                    case 2:
                        spawnHelpEnemy(enemyDragonFly, 3f, randomY);
                        break;
                }               
            }
        }
    private void spawnOne()
    {
        spawnHelpEnemy(enemyPixi, 231233f, 6f);
        enemyPixi.SetActive(false);
        spawnHelpEnemy(enemyStone, 3123123f, 6f);
        enemyStone.SetActive(false);
    }
    private IEnumerator SpawnDragonfly()
    {
        float randomY = 0f;
        while (true) 
        {
            randomY = Random.Range(2f, MaxPosition.y -2f);
            Instantiate(enemyDragonFly, new Vector2(3.3f, randomY), Quaternion.identity);
            yield return new WaitForSeconds(8f);
        }
    }
    private void spawnHelpEnemy(GameObject Enemy, float X, float Y)
    {
        Enemy = Instantiate(Enemy);
        Enemy.transform.position = new Vector2(X, Y);
        Enemy.transform.SetParent(null);
    }


    private void Shrink()
    {
        switch (life)
        {
            case 6:
                playerMove.SetDelay(0.8f);
                playerSprite.transform.localScale = new Vector2(2.8f, 2.8f);
                break;
            case 5:
                playerMove.SetDelay(0.6f);
                playerSprite.transform.localScale = new Vector2(2.5f, 2.5f);
                break;
            case 4:
                playerMove.SetDelay(0.5f);
                playerSprite.transform.localScale = new Vector2(2.5f, 2.5f);
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
            resul.transform.position = new Vector2(0f, 5);
            resul.transform.SetParent(null);
            resul.SetActive(true);
        }
        else
        {
            GameObject Wind = Instantiate(windPrefab);
            Wind.transform.position = new Vector2(0f, 5);
            Wind.transform.SetParent(null);
            resul = Wind;
        }
        return resul;
    }
    #endregion
    private IEnumerator randomItemEnemySpawn()
    {
        int randomEnemy = Random.Range(0,3);
        float randomX = Random.Range(-1.7f, 1.7f);
        float randomY=Random.Range(-1.4f,MaxPosition.y -2f);
        float randomDelay1 = Random.Range(3f, 10f);
        float randomDelay_ = Random.Range(3f, 10f);
         yield return new WaitForSeconds(randomDelay1);
        if (randomEnemy == 2)  {Instantiate(enemiesWithItem[randomEnemy], new Vector2(3f, randomY), Quaternion.identity);}
         else {Instantiate(enemiesWithItem[randomEnemy], new Vector2(randomX, 6f), Quaternion.identity);}
         yield return new WaitForSeconds(randomDelay_);
         StartCoroutine(randomItemEnemySpawn());
    }
    public void GoBack()
    {
        Quit.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Yes()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void No()
    {
        Quit.SetActive(false);
        Time.timeScale = 1f;
    }
}
