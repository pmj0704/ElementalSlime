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
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject discr;
    [SerializeField] private GameObject setting;
    [SerializeField] private GameObject help;
    [SerializeField] GameObject next;
    [SerializeField] GameObject recent;

        private int num = 0;

    private bool onSetting = true;
    private bool onSound = true;
    private bool onLic = true;

    [SerializeField] Sprite[] SoundSprite;
    [SerializeField] GameObject SoundBT;
    [SerializeField] GameObject Licence;

    [SerializeField] Sprite[] pages;

    void Start()
    {
        textHighScore.text = string.Format("HIGHSCORE {0}", PlayerPrefs.GetInt("HIGHSCORE", 0));
        StartCoroutine(SpawnCloud());
        MinPosition = new Vector2(-2f, -4.3f);
        MaxPosition = new Vector2(2f, 4.3f);
        poolManager = FindObjectOfType<PoolManager>();
    }
    void Update()
    {
        if (num == 0)
        {
            discr.SetActive(true);
            help.GetComponent<Image>().sprite = pages[num];
            help.GetComponent<Image>().color = new Color (1f, 1f, 1f, 0.3f);
            recent.SetActive(false);
            next.SetActive(true);
        }
        else if(num > 0 && num < 3)
        {
            discr.SetActive(false);
            next.SetActive(true);
            recent.SetActive(true);
            help.GetComponent<Image>().sprite = pages[num];
            help.GetComponent<Image>().color = new Color (1f, 1f, 1f, 1f);
        }
        else if (num == 3)
        {
            next.SetActive(false);
            discr.SetActive(false);
            help.GetComponent<Image>().sprite = pages[num];
            help.GetComponent<Image>().color = new Color (1f, 1f, 1f, 1f);
            recent.SetActive(true);
        }
    }
    // Update is called once per frame
    public void ClickToStart()
    {
    mainCamera.GetComponent<AudioListener>().enabled = true;
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
//    public OpenSetting()
//    {

//    }
   public void Sound()
   {
       if(onSound){
    mainCamera.GetComponent<AudioListener>().enabled = false;
    SoundBT.GetComponent<Animator>().Play("NO");
    onSound = false;
       }
       else if(!onSound){
    mainCamera.GetComponent<AudioListener>().enabled = true;
        SoundBT.GetComponent<Animator>().Play("YES");
    onSound = true;
       }
   }

   public void SettingOn()
   {
       if(onSetting){
       setting.SetActive(true);
       onSetting = false;
       }
       else if(!onSetting)
       {
           setting.SetActive(false);
           onSetting=true;
       }
   }
   public void Quit()
   {
       Application.Quit();
   }
   public void Help()
   {
       help.SetActive(true);
   }
   public void HelpOff()
   {
       help.SetActive(false);
   }
   public void nextPage()
   {
       num++;
   }
     public void recentPage()
   {
       num--;
   }
   public void Settingoff()
   {
       setting.SetActive(false);
   }
   public void LicenceOn()
   {
            if(onLic){
       Licence.SetActive(true);
       onLic = false;
       }
       else if(!onLic)
       {
           Licence.SetActive(false);
           onLic=true;
       }
   }
}
