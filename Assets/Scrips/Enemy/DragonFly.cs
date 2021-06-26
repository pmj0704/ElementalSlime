using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFly : EnemyMove
{
    protected override void Start(){base.Start();}
    protected override void Update()
    {
        if (transform.position.y <= gameManager.MinPosition.y) gameObject.SetActive(false);
        if(isDead) return;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x < gameManager.MinPosition.x  || transform.position.y > gameManager.MaxPosition.y)
        {
            gameObject.SetActive(false);
        }
    }
}
