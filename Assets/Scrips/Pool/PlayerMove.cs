using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    #region 변수 목록
    private GameManager gameManager = null;
    private Vector2 targetPosition = Vector2.zero;
    private bool isDamaged = false;
    private SpriteRenderer spriteRenderer = null;

    [Header("이동속도")]
    [SerializeField]
    private float speed = 5f;

    [Header("총알 발사 위치")]
    [SerializeField]
    private Transform bulletPosition = null;

    [Header("총알 프리팹")]
    [SerializeField]
    private GameObject bulletPrefab = null;

    [Header("총알 발사간격")]
    [SerializeField]
    private float bulletDelay = 0.5f;
    #endregion


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.x = Mathf.Clamp(targetPosition.x, gameManager.MinPosition.x, gameManager.MaxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, gameManager.MinPosition.y, gameManager.MaxPosition.y);
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
        }

    }
    private IEnumerator Fire()
    {
        while (true)
        {
            InstantiateOrPool();
            yield return new WaitForSeconds(bulletDelay);
        }
    }
    private GameObject InstantiateOrPool()
    {
        GameObject result = null;
        if (gameManager.poolManager.transform.childCount > 0)
        {
            result = gameManager.poolManager.transform.GetChild(0).gameObject;
            result.transform.position = bulletPosition.position;
            result.transform.SetParent(null);
            result.SetActive(true);

        }
        else
        {
            GameObject Bullet = Instantiate(bulletPrefab, bulletPosition);
            Bullet.transform.position = bulletPosition.position;
            Bullet.transform.SetParent(null);
            result = Bullet;
        }
        result.transform.localScale = bulletPosition.lossyScale;
        return result;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDamaged) return;
        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        gameManager.Dead();
        isDamaged = true;
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        isDamaged = false;
    }
    public void SetDelay(float delay)
    {
        bulletDelay = delay; 
    }
}

