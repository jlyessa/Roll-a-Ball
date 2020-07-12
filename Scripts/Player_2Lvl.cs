using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_2Lvl : MonoBehaviour
{
    public Text[] text = new Text[7];
    public AudioClip[] Clip = new AudioClip[4];
    public Camera m_camera;
    public GameObject TimeStop;
    public GameObject GameCheck;
    public GameObject ExcapePanel;
    private Rigidbody rb;
    private Vector3 offset;
    private const int maxCount = 20;
    private int count = 0, start = 0, gametime, timeost;
    private float time = 1, timeupdate = 1;
    private bool stopstart = false, checkTimeStop = false, checkGameWin = false, excapebool = false, TimeEsc = false;
    private bool isUp = false, isDown = false, isLeft = false, isRight = false;
    public void BackGame()
    {
        SceneManager.LoadScene("Main");
    }
    public void AgainGame()
    {
        SceneManager.LoadScene("2Level");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void OnApplicationQuit()
    {
        MainGame.ReadParams();
    }
    public void Win()
    {
        if (MainGame.Level == 1) MainGame.Level++;
        MainGame.Count += maxCount;
        MainGame.ReadParams();
        SceneManager.LoadScene("Main");
    }
    private void Start ()
    {
        AudioSource.PlayClipAtPoint(Clip[0], transform.position);
        text[3].text = "3";
        text[1].text = "Монет: " + MainGame.Count.ToString();
        text[2].text = "Осталось: " + Convert.ToString(maxCount - count);
        rb = GetComponent<Rigidbody>();
        offset = m_camera.transform.position - transform.position;
        GetComponent<Renderer>().material.mainTexture = MainGame.TextureExport[MainGame.Skin];
        switch (MainGame.Speed)
        {
            case 1:
                gametime = 150;
                break;
            case 2:
                gametime = 140;
                break;
            case 3:
                gametime = 130;
                break;
            case 4:
                gametime = 120;
                break;
            case 5:
                gametime = 110;
                break;
            case 6:
                gametime = 100;
                break;
            case 7:
                gametime = 90;
                break;
            case 8:
                gametime = 80;
                break;
            case 9:
                gametime = 70;
                break;
            case 10:
                gametime = 60;
                break;
        }
        timeost = gametime;
        TimerUpdate();
    }
	private void FixedUpdate ()
    {
        m_camera.transform.position = transform.position + offset;
        if (start < 5 && !stopstart)
        {
            if (time > 0) time -= Time.deltaTime;
            else TimerStart(start);
            if (start == 5) stopstart = true;
        }
        if (start > 3 && !checkTimeStop && !checkGameWin && !TimeEsc)
        {
            if (isUp) rb.AddForce(new Vector3(0.0f, 0.0f, 1.0f) * MainGame.Speed);
            if (isDown) rb.AddForce(new Vector3(0.0f, 0.0f, -1.0f) * MainGame.Speed);
            if (isLeft) rb.AddForce(new Vector3(-1.0f, 0.0f, 0.0f) * MainGame.Speed);
            if (isRight) rb.AddForce(new Vector3(1.0f, 0.0f, 0.0f) * MainGame.Speed);
            if (timeupdate > 0) timeupdate -= Time.deltaTime;
            else
            {
                gametime--;
                TimerUpdate();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        count++;
        if (count == maxCount)
        {
            if (!TimeStop.activeInHierarchy)
            {
                checkGameWin = true;
                int minute = (timeost - gametime) / 60;
                int second = (timeost - gametime) % 60;
                if (second < 10) text[4].text = "Прошел за 0" + minute + ":0" + second + " минуты";
                else text[4].text = "Прошел за 0" + minute + ":" + second + " минуты";
                text[5].text = "Собрал: " + maxCount + " монет";
                int counter = MainGame.Count + count;
                text[6].text = "Монет: " + counter;
                text[2].text = "Осталось: " + Convert.ToString(maxCount - count);
                GameCheck.SetActive(true);
            }
        }
        else text[2].text = "Осталось: " + Convert.ToString(maxCount - count);
        if (other.tag == "Cube" && !checkTimeStop && !TimeEsc)
        {
            other.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(Clip[1], transform.position);
        }
    }
    private void TimerStart(int obj)
    {
        start++;
        time = 1;
        if (obj == 0)
        {
            text[3].text = "2";
            AudioSource.PlayClipAtPoint(Clip[0], transform.position);
        }
        else if (obj == 1)
        {
            text[3].text = "1";
            AudioSource.PlayClipAtPoint(Clip[0], transform.position);
        }
        else if (obj == 2)
        {
            text[3].text = "0";
            AudioSource.PlayClipAtPoint(Clip[0], transform.position);
        }
        else if (obj == 3)
        {
            text[3].text = "Старт!";
            AudioSource.PlayClipAtPoint(Clip[0], transform.position);
        }
        else if (obj == 4) text[3].text = "";
    }
    private void TimerUpdate()
    {
        if (gametime <= 20) AudioSource.PlayClipAtPoint(Clip[2], transform.position);
        timeupdate = 1;
        int minute = gametime / 60;
        int second = gametime % 60;
        if (second < 10) text[0].text = "Время: 0" + minute + ":0" + second;
        else text[0].text = "Время: 0" + minute + ":" + second;
        if (gametime == 0)
        {
            if (!GameCheck.activeInHierarchy)
            {
                checkTimeStop = true;
                TimeStop.SetActive(true);
            }
        }
    }
    public void ClickbtnEsc()
    {
        if (start == 5 && !GameCheck.activeInHierarchy && !TimeStop.activeInHierarchy)
        {
            if (!excapebool && !ExcapePanel.activeInHierarchy)
            {
                ExcapePanel.SetActive(true);
                TimeEsc = true;
                AudioSource.PlayClipAtPoint(Clip[3], transform.position);
                excapebool = true;
            }
            else if (excapebool && ExcapePanel.activeInHierarchy)
            {
                ExcapePanel.SetActive(false);
                TimeEsc = false;
                AudioSource.PlayClipAtPoint(Clip[3], transform.position);
                excapebool = false;
            }
        }
    }
    public void ClickUp(bool res)
    {
        isUp = res;
    }
    public void ClickDown(bool res)
    {
        isDown = res;
    }
    public void ClickLeft(bool res)
    {
        isLeft = res;
    }
    public void ClickRight(bool res)
    {
        isRight = res;
    }
}
