using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [SerializeField] Player playerControll;
    [Header("Abilities_1")]
    public Image AbilitiImage;
    public Text timeText;
    public float coolDown = 5;
    public bool clickItem;
    bool isCoolDown = false;
    [SerializeField] Button item_1;

    [Header("Abilities_2")]
    public Image AbilitiImage2;
    public Text timeText2;
    public float coolDown2 = 5;
    public bool clickItem2;
    bool isCoolDown2 = false;
    [SerializeField] Button item_2;

    [Header("Abilities_3")]
    public Image AbilitiImage3;
    public Text timeText3;
    public float coolDown3 = 5;
    public bool clickItem3;
    bool isCoolDown3 = false;
    [SerializeField] Button item_3;

    [Header("Abilities_4")]
    public Image AbilitiImage4;
    public Text timeText4;
    public float coolDown4 = 5;
    public bool clickItem4;
    bool isCoolDown4 = false;
    [SerializeField] Button item_4;
    void Start()
    {
        AbilitiImage.fillAmount = 0;
        timeText.text = "";

        AbilitiImage2.fillAmount = 0;
        timeText2.text = "";

        AbilitiImage3.fillAmount = 0;
        timeText3.text = "";

        AbilitiImage4.fillAmount = 0;
        timeText4.text = "";
    }

    void Update()
    {
        Ability1();
        Ability2();
        Ability3();
        Ability4();
    }

    void Ability1()
    {
        if (!isCoolDown && clickItem)
        {
            isCoolDown = true;
            AbilitiImage.fillAmount = 1;
            clickItem = false;
        }

        if (isCoolDown)
        {
            coolDown -= Time.deltaTime;
            AbilitiImage.fillAmount -= 1 / coolDown * Time.deltaTime;
            if (AbilitiImage.fillAmount <= 0)
            {
                AbilitiImage.fillAmount = 0;
                isCoolDown = false;
                coolDown = 5;
                item_1.interactable = true;
            }
            float minutes = Mathf.FloorToInt(coolDown / 60);
            float seconds = Mathf.FloorToInt(coolDown % 60);

            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (coolDown <= 0)
                timeText.text = "";
        }
    }

    void Ability2()
    {
        if (!isCoolDown2 && clickItem2)
        {
            isCoolDown2 = true;
            AbilitiImage2.fillAmount = 1;
            clickItem2 = false;
        }

        if (isCoolDown2)
        {
            coolDown2 -= Time.deltaTime;
            AbilitiImage2.fillAmount -= 1 / coolDown2 * Time.deltaTime;
            if (AbilitiImage2.fillAmount <= 0)
            {
                AbilitiImage2.fillAmount = 0;
                isCoolDown2 = false;
                coolDown2 = 5;
                item_2.interactable = true;
            }
            float minutes = Mathf.FloorToInt(coolDown2 / 60);
            float seconds = Mathf.FloorToInt(coolDown2 % 60);

            timeText2.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (coolDown2 <= 0)
                timeText2.text = "";
        }
    }

    void Ability3()
    {
        if (!isCoolDown3 && clickItem3)
        {
            isCoolDown3 = true;
            AbilitiImage3.fillAmount = 1;
            clickItem3 = false;
        }

        if (isCoolDown3)
        {
            coolDown3 -= Time.deltaTime;
            AbilitiImage3.fillAmount -= 1 / coolDown3 * Time.deltaTime;
            if (AbilitiImage3.fillAmount <= 0)
            {
                AbilitiImage3.fillAmount = 0;
                isCoolDown3 = false;
                coolDown3 = 5;
                item_3.interactable = true;
            }
            float minutes = Mathf.FloorToInt(coolDown3 / 60);
            float seconds = Mathf.FloorToInt(coolDown3 % 60);

            timeText3.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (coolDown3 <= 0)
                timeText3.text = "";
        }
    }

    void Ability4()
    {
        if (!isCoolDown4 && clickItem4)
        {
            isCoolDown4 = true;
            AbilitiImage4.fillAmount = 1;
            clickItem4 = false;
        }

        if (isCoolDown4)
        {
            coolDown4 -= Time.deltaTime;
            AbilitiImage4.fillAmount -= 1 / coolDown4 * Time.deltaTime;
            if (AbilitiImage4.fillAmount <= 0)
            {
                AbilitiImage4.fillAmount = 0;
                isCoolDown4 = false;
                coolDown4 = 5;
                item_4.interactable = true;
            }
            float minutes = Mathf.FloorToInt(coolDown4 / 60);
            float seconds = Mathf.FloorToInt(coolDown4 % 60);

            timeText4.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            if (coolDown4 <= 0)
                timeText4.text = "";
        }
    }


    public void ClickButton()
    {
        clickItem = true;
        item_1.interactable = false;
        playerControll.SetWhiteBall();
    }
    public void ClickButton2()
    {
        clickItem2 = true;
        item_2.interactable = false;
        playerControll.MagnetBall();
    }
    public void ClickButton3()
    {
        clickItem3 = true;
        item_3.interactable = false;
        StartCoroutine(playerControll.DesBallSmall());
    }
    public void ClickButton4()
    {
        clickItem4 = true;
        item_4.interactable = false;
        playerControll.SetClickDestroyBall();
    }
    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (coolDown <= 0)
            timeText.text = "";
    }
}
