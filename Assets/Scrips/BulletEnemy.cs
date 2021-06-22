using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : BulletMove
{
    private EnemyBulletManager enemyBulletManager { get; set; }
    protected override void Start()
    {
        base.Start();
        enemyBulletManager = FindObjectOfType<EnemyBulletManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        CheckLimit();
    }
    protected override void CheckLimit()
    {
        if (transform.position.y > gameManager.MaxPosition.y + 1f) Despawn();
        if (transform.position.y < gameManager.MinPosition.y - 2f) Despawn();
        if (transform.position.x > gameManager.MaxPosition.x + 2f) Despawn();
        if (transform.position.x < gameManager.MinPosition.x - 2f) Despawn();
    }
    private void Despawn()
    {
        transform.SetParent(gameManager.enemyBulletManager.transform, false);
        gameObject.SetActive(false);
    }
}
