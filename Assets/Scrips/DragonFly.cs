using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFly : EnemyMove
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        if(isDead) return;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
}
