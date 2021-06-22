using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] private GameObject[] arm;
    [SerializeField] private GameObject mainBody;

    [SerializeField] private GameObject[] newArm;
    [SerializeField] private GameObject[] spikeL;
    [SerializeField] private GameObject[] spikeM;
    [SerializeField] private GameObject[] spikeS;
    [SerializeField] private GameObject Bullder;
    [SerializeField] private GameObject[] Leg;
    [SerializeField] private GameObject newBullet;

    [SerializeField] private Transform[] ShootingPos;
    
    [SerializeField] private Transform[] armPos;
    [SerializeField]private Transform player;
    private Animator animator;
    private Vector3 diff = Vector3.zero;
    private float rotationZ = 0f;
    Vector3 targetPosition = new Vector3(-0.81f, 0.13f, 0);
    private Transform targetPos;
    private GameManager gameManager = null;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Bullder.SetActive(false);
        animator = GetComponent<Animator>();
        StartCoroutine(moveByPlayer(arm[0],(-220)));
        StartCoroutine(moveByPlayer(arm[1],25));
        StartCoroutine(ShootArm());
        StartCoroutine(SpikeAttack());
        StartCoroutine(KeepShooting());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5 * Time.deltaTime);
        
    }
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
            randomD1 = Random.Range(5f, 10f);
            break; 
        }
            yield return new WaitForSeconds(1f);
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
}
