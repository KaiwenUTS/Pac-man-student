using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI highestText;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            int score = PlayerPrefs.GetInt("Score");
            float time = 0;
            if (PlayerPrefs.HasKey("Time"))
                time = PlayerPrefs.GetFloat("Time");
            highestText.text = Utils.TranslateNumToRichText(score) + "--" + Utils.TranslateSecToRichText(time);
        }
        else
            highestText.text = "-------------------";
    }
    public void OnLevel1BtnClicked(Button button)
    {
        button.interactable = false;
        SceneManager.LoadScene("PacStudent");
    }
    public void OnLevel2BtnClicked(Button button)
    {
        button.interactable = false;
        SceneManager.LoadScene("GameScene");
    }
}
