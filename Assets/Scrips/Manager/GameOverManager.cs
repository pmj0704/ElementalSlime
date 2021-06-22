using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Vector2 MinPosition { get; private set; }
    public Vector2 MaxPosition { get; private set; }
    [SerializeField]
    private Text textHighScore = null;
    [SerializeField]
    private GameObject[] spriteCloud = null;
    public PoolManager poolManager { get; private set; }

    void Start()
    {
        textHighScore.text = string.Format("HIGHSCORE {0}", PlayerPrefs.GetInt("HIGHSCORE", 0));
        StartCoroutine(SpawnCloud());
        MinPosition = new Vector2(-2f, -4.3f);
        MaxPosition = new Vector2(2f, 4.3f);
        poolManager = FindObjectOfType<PoolManager>();
    }

    // Update is called once per frame
    public void ClickToStart()
    {
        SceneManager.LoadScene("Main");
    }

    private IEnumerator SpawnCloud()
    {
        float cloudDelay;
        float cloudY;
        int randomCloud;
        while (true)
        {
            cloudDelay = Random.Range(1f, 3f);
            randomCloud = Random.Range(0, 2);
            cloudY = Random.Range(-3f, 7f);
            Instantiate(spriteCloud[randomCloud], new Vector2(2.7f, cloudY), Quaternion.identity);
            yield return new WaitForSeconds(2.3f);
        }
    }
   
}
