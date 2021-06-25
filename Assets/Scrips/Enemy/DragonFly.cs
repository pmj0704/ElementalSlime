using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFly : EnemyMove
{
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    
    {
        if(isDead) return;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < gameManager.MinPosition.x - 2f || transform.position.y > gameManager.MaxPosition.y)
        {
            gameObject.SetActive(false);
        }
    }
}
