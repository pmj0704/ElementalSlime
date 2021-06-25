using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    private GameOverManager gameOverManager = null;
    void Start()
    {
        gameOverManager = FindObjectOfType<GameOverManager>();

    }
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x <= gameOverManager.MinPosition.x - 2f) Destroy(gameObject);
    }
    private void CheckLimit()
    {
        if (transform.position.y > gameOverManager.MaxPosition.y + 2f)Despawn();
        if (transform.position.y < gameOverManager.MinPosition.y - 2f)Despawn();
        if (transform.position.x > gameOverManager.MaxPosition.x + 2f)Despawn();
        if (transform.position.x < gameOverManager.MinPosition.x - 2f)Despawn();
    }
    public void Despawn()
    {
        transform.SetParent(gameOverManager.poolManager.transform, false);
        gameObject.SetActive(false);
    }
}
