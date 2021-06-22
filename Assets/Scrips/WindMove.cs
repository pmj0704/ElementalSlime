using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMove : MonoBehaviour
{

    #region 변수 목록
    [SerializeField]
    private float speed = 1f;

    private GameManager gameManager = null;
    #endregion

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (transform.position.y < gameManager.MinPosition.y - 1f)
           gameObject.SetActive(false);
    }
}
