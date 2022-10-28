using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance
    {
        private set;
        get;
    } = null;
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        scoreText.text= "<sprite=0>";
        gameTimerText.text = TranslateSecToRichText(0.0f);
        StartCoroutine(WaitForBegin());
    }
    private int score = 0;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    public void AddScore(int n)
    {
        score += n;
        scoreText.text = TranslateNumToRichText(score);
    }
    private string TranslateNumToRichText(int n, int len = 1)
    {
        string res = "";
        Stack<int> reverseNum = new Stack<int>();
        while (n > 0)
        {
            reverseNum.Push(n % 10);
            n = n / 10;
        }
        while (reverseNum.Count < len)
            reverseNum.Push(0);
        while (reverseNum.Count > 0)
        {
            res += $"<sprite={reverseNum.Pop()}>";
        }
        return res;
    }
    [SerializeField]
    private Ghost[] ghosts;
    private GhostState ghostState = GhostState.Walking;
    private float powerTime = 10, powerLeftTime = 0;
    [SerializeField]
    private TextMeshProUGUI ghostTimerText;
    public void PowerStart()
    {
        powerLeftTime = powerTime;
        foreach (Ghost ghost in ghosts)
            ghost.SetGhostState(GhostState.Scared);
        ghostTimerText.gameObject.SetActive(true);
    }
    private float gameStartTime;
    private bool isOn = false;
    [SerializeField]
    private TextMeshProUGUI gameTimerText;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private Image startImage;
    private IEnumerator WaitForBegin()
    {
        startPanel.SetActive(true);
        startImage.sprite = sprites[0];
        yield return null;
        yield return new WaitForSeconds(1);
        startImage.sprite = sprites[1];
        yield return null;
        yield return new WaitForSeconds(1);
        startImage.sprite = sprites[2];
        yield return null;
        yield return new WaitForSeconds(1);
        startImage.sprite = sprites[3];
        yield return null;
        yield return new WaitForSeconds(1);
        startPanel.SetActive(false);
        isOn = true;
    }
    private void Update()
    {
        if (!isOn)
            return;
        gameTimerText.text = TranslateSecToRichText(Time.realtimeSinceStartup);
        if (powerLeftTime > 0)
        {
            powerLeftTime -= Time.deltaTime;
            ghostTimerText.text = TranslateSecToRichText(powerLeftTime);
            if (powerLeftTime < 3 && powerLeftTime > 0)
                foreach (Ghost ghost in ghosts)
                    ghost.SetGhostState(GhostState.Recovering);
            else if (powerLeftTime < 0)
            {
                foreach (Ghost ghost in ghosts)
                    ghost.SetGhostState(GhostState.Walking);
                ghostTimerText.gameObject.SetActive(false);
            }
        }
    }
    private string TranslateSecToRichText(float t)
    {
        int sec = Mathf.FloorToInt(t);
        int ms = Mathf.FloorToInt((t - sec) * 100);
        int min = sec / 60;
        sec = sec % 60;
        return TranslateNumToRichText(min,2) + ":" + TranslateNumToRichText(sec,2) + ":" + TranslateNumToRichText(ms,2);
    }
}