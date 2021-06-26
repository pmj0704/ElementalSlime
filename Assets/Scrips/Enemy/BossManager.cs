using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    #region 보스 파트
    [Header("보스 팔")] [SerializeField] private GameObject[] arm;
    [Header("보스 몸")] [SerializeField] private GameObject mainBody;
    [Header("보스 새로 생성 할 팔")] [SerializeField] private GameObject[] newArm;
    [Header("큰 스파이크")] [SerializeField] private GameObject[] spikeL;
    [Header("중간 크기 스파이크")] [SerializeField] private GameObject[] spikeM;
    [Header("작은 스파이크")] [SerializeField] private GameObject[] spikeS;
    [Header("바위")] [SerializeField] private GameObject Bullder;
    [Header("보스 다리")] [SerializeField] private GameObject[] Leg;
    [Header("보스 총알")] [SerializeField] private GameObject newBullet;
    [Header("보스 총알 생성 위치")] [SerializeField] private Transform[] ShootingPos;
    [Header("보스 팔 위치")] [SerializeField] private Transform[] armPos;
    #endregion 

    #region  플레이어 추적
    [Header("플레이어")][SerializeField] private Transform player;
    private Vector3 diff = Vector3.zero;
    private Vector3 targetPosition = new Vector3(-0.81f, 0.13f, 0);
    private float rotationZ = 0f;
    private Transform targetPos;
    #endregion

    #region 컴포턴트와 오브젝트 타입
    private Animator animator;
    private GameManager gameManager = null;
    #endregion

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        #region 공격
        Bullder.SetActive(false);
        StartCoroutine(moveByPlayer(arm[0],(-220)));
        StartCoroutine(moveByPlayer(arm[1],25));
        StartCoroutine(ShootArm());
        StartCoroutine(SpikeAttack());
        StartCoroutine(KeepShooting());
        #endregion
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5 * Time.deltaTime);
        
    }

    #region 플레이어 추적
    public IEnumerator moveByPlayer(GameObject one, int Dir)
    {
        while(true)
        {
            diff = player.transform.position - transform.position;
            diff.Normalize();
            rotationZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            one.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ +Dir);
            yield return 0;
        }
    }
    #endregion

    #region 공격
    private IEnumerator ShootArm()
    {
        yield return new WaitForSeconds (2f);
        Instantiate(newArm[0]);
        Instantiate(newArm[1]);
        newArm[0].transform.position = armPos[0].position;
        newArm[1].transform.position = armPos[1].position;
        arm[0].SetActive(false);        
        arm[1].SetActive(false);
        yield return 0;
    }
    private IEnumerator SpikeAttack()
    {
        float randomD1;
        while(true)
        {
            randomD1 = Random.Range(2f, 10f);
            break; 
        }
            yield return new WaitForSeconds(randomD1);
            StartCoroutine(SpikeAt(spikeL[0], "Spike Attack"));
            yield return new WaitForSeconds(randomD1);
            StartCoroutine(SpikeAt(spikeL[1], "SpikeL_R"));
            yield return new WaitForSeconds(randomD1);
            StartCoroutine(SpikeAt(spikeM[0], "MSpike"));
            yield return new WaitForSeconds(randomD1);
            StartCoroutine(SpikeAt(spikeM[1], "MSpike_R"));
            StartCoroutine(makeBulder(randomD1));
    }
    private IEnumerator SpikeAt(GameObject Object, string Ani)
    {
            spikeMove(Object,Ani);
            yield return new WaitForSeconds(2.2f);
            Idle(Object);
    }
private void spikeMove(GameObject Object, string Anime)
{
    animator.Play(Anime);
}
private void Idle(GameObject Object)
{
    animator.Play("Idle");
    Object.SetActive(false);
}
private IEnumerator makeBulder(float Delay)
{
    yield return new WaitForSeconds (Delay);
    Bullder.SetActive(true);
    animator.Play("Charge");
    yield return new WaitForSeconds (Delay);
    StartCoroutine(makeBulder(Delay));
}
private IEnumerator KeepShooting()
{
    while(true){
    yield return new WaitForSeconds(2.5f);
    StartCoroutine(Shoot());
    yield return new WaitForSeconds(2.5f);
    }
}
private IEnumerator Shoot()
{
    int time =0;
    int time1 = 0;
    while(Leg[1].activeInHierarchy)
    {
        ShootPool(1);
        time++;
        yield return new WaitForSeconds(0.2f);

        if(time >= 5) break;
    }
    while(Leg[0].activeInHierarchy)
    {
        ShootPool(0);
        time1++;
        yield return new WaitForSeconds(0.2f);
        
        if(time1 >= 5) break;
    }

}
#endregion

#region  총알 풀링
private GameObject ShootPool(int num)
    {
        GameObject result = null;
        if (gameManager.enemyBulletManager.transform.childCount > 0)
        {
            result = gameManager.enemyBulletManager.transform.GetChild(0).gameObject;
            result.transform.position = ShootingPos[num].position;
            result.transform.SetParent(null);
            result.SetActive(true);

        }
        else
        {
            GameObject EBullet = Instantiate(newBullet, ShootingPos[num]);
            EBullet.transform.position = ShootingPos[num].position;
            EBullet.transform.SetParent(null);
            result = EBullet;
        }
        return result;
    }
    #endregion
}
