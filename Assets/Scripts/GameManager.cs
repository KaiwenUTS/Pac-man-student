using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        gameTimerText.text = Utils.TranslateSecToRichText(0.0f);
        startPanel.SetActive(true);
        endPanel.SetActive(false);
        if (IsOn)
            return;
        StartCoroutine(WaitForBegin());
    }
    private int pelletLeft = 0;
    public void SetPelletNum(int n)
    {
        pelletLeft = n;
    }
    public void EatPellet()
    {
        --pelletLeft;
        if (pelletLeft == 0)
            GameOver();
    }
    private int score = 0;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    public void AddScore(int n)
    {
        score += n;
        scoreText.text = Utils.TranslateNumToRichText(score);
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
    public bool IsOn { get => isOn; private set => isOn = value; }
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
        IsOn = true;
        gameStartTime = Time.realtimeSinceStartup;
    }
    [SerializeField]
    private GameObject endPanel;
    public void GameOver()
    {
        if (!IsOn)
            return;
        IsOn = false;
        endPanel.SetActive(true);
        if (!PlayerPrefs.HasKey("Score") || (PlayerPrefs.HasKey("Score") && PlayerPrefs.GetInt("Score")<score))
        {
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.SetFloat("Time", Time.realtimeSinceStartup - gameStartTime);
        }
        StartCoroutine(WaitForExit());
    }
    private IEnumerator WaitForExit()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("StartScene");
    }
    public void OnExitBtnClicked()
    {
        SceneManager.LoadScene("StartScene");
    }
    [SerializeField]
    private GameObject[] heartImgs;
    public void UpdateLife(int n)
    {
        if (n <= 2)
            heartImgs[2].SetActive(false);
        if (n <= 1)
            heartImgs[1].SetActive(false);
        if (n <= 0)
            heartImgs[0].SetActive(false);
    }
    [SerializeField]
    private AudioController audioController;
    private void Update()
    {
        if (!IsOn)
            return;
        gameTimerText.text = Utils.TranslateSecToRichText(Time.realtimeSinceStartup - gameStartTime);
        if (powerLeftTime > 0)
        {
            if(audioController.GetAudioIndex()!=2)
                audioController.SetAudio(1);
            powerLeftTime -= Time.deltaTime;
            ghostTimerText.text = Utils.TranslateNumToRichText((int)powerLeftTime,2);
            if (powerLeftTime < 3 && powerLeftTime > 0)
                foreach (Ghost ghost in ghosts)
                    ghost.SetGhostState(GhostState.Recovering);
            else if (powerLeftTime < 0)
            {
                if (audioController.GetAudioIndex() != 2)
                    audioController.SetAudio(0);
                foreach (Ghost ghost in ghosts)
                    ghost.SetGhostState(GhostState.Walking);
                ghostTimerText.gameObject.SetActive(false);
            }
        }
        if (cherrySpawnTimeLeft > 0)
        {
            cherrySpawnTimeLeft -= Time.deltaTime;
        }
        else
        {
            SpawnCherry();
            cherrySpawnTimeLeft = cherrySpawnTime;
        }
    }
    [SerializeField]
    private float cherrySpawnTime = 10, cherrySpawnTimeLeft = 1;
    [SerializeField]
    private GameObject cherryPrefab;
    private void SpawnCherry()
    {
        Vector3 spawnPos = Vector3.zero;
        int theta = Random.Range(0, 360);
        spawnPos.x = 15 * Mathf.Sin(theta);
        spawnPos.y = 15 * Mathf.Cos(theta);
        Instantiate(cherryPrefab, spawnPos, Quaternion.identity);
    }
}
