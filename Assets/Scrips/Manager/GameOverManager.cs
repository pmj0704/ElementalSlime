using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    #region 변수 목록
    public Vector2 MinPosition { get; private set; }
    public Vector2 MaxPosition { get; private set; }
    public PoolManager poolManager { get; private set; }
    [Header("구름 모양")] [SerializeField] private GameObject[] spriteCloud = null;
    #endregion

    #region UI
    private int num = 0;
    private bool onSetting = true;
    public bool onSound = true;
    private bool onLic = true;
    
    [Header("소리")] [SerializeField] private AudioListener audioListener;
    [Header("설명")] [SerializeField] private GameObject discr;
    [Header("설정")] [SerializeField] private GameObject setting;
    [Header("설명 버튼")] [SerializeField] private GameObject help;
    [Header("다음 장 버튼")] [SerializeField] GameObject next;
    [Header("이전 장 버튼")] [SerializeField] GameObject recent;
    [Header("최고 점수")] [SerializeField] private Text textHighScore = null;
    [Header("소리 버튼")] [SerializeField] GameObject SoundBT;
    [Header("라이선스 버튼")] [SerializeField] GameObject Licence;
    [Header("설명 페이지")] [SerializeField] Sprite[] pages;
    #endregion

    void Start()
    {
        StartCoroutine(SpawnCloud());
        textHighScore.text = string.Format("HIGHSCORE {0}", PlayerPrefs.GetInt("HIGHSCORE", 0));
        MinPosition = new Vector2(-2f, -4.3f);
        MaxPosition = new Vector2(2f, 4.3f);
        poolManager = FindObjectOfType<PoolManager>();
        audioListener = FindObjectOfType<AudioListener>();

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

    #region UI
    public void ClickToStart()
    {
        SceneManager.LoadScene("Main");
    }

   public void Sound()
   {
       if(onSound){
    AudioListener.pause = true;
    SoundBT.GetComponent<Animator>().Play("NO");
    onSound = false;
       }
       else if(!onSound){
        AudioListener.pause = false;
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
   #endregion

   #region BG
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
    #endregion
}