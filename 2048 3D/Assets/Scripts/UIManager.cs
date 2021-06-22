using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] TextMeshProUGUI TextLevel;
    [SerializeField] TextMeshProUGUI TextCurrentExp;
    public GameObject ButtonGameOver;
    [SerializeField] Image fillProgressBar;

    public GameObject ToolDev_Pannel;
    public InputField gravityInput;
    private bool checkOpenTool = false;

    [SerializeField] GameObject sliderBall;

    private void Awake()
    {
        Instance = this;
        ToolDev_Pannel.SetActive(false);
    }
    void Start()
    {
        UpdateTextLevel();
        UpdateExpLevel();
    }

    public void UpdateTextLevel()
    {
        TextLevel.text = GameManager.Instance.Level.ToString();
    }
    public void UpdateExpLevel()
    {
        TextCurrentExp.text = GameManager.Instance.CurrentExp.ToString();
        float fillAmount = (float)GameManager.Instance.CurrentExp / (float)GameManager.Instance.TargetExp;
        fillProgressBar.fillAmount = fillAmount;
    }

    public void GameOver()
    {
        sliderBall.SetActive(false);
        ButtonGameOver.SetActive(true);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ToolDev()
    {
        ToolDev_Pannel.SetActive(!checkOpenTool);
        checkOpenTool = !checkOpenTool;
    }

    public void ChangeGravity(string _gra)
    {
        float gra = float.Parse(_gra);
        GameManager.Instance.GravityBall = gra;
    }
}
