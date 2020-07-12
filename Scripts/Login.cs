using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
    public GameObject[] Inputs = new GameObject[3];
    public GameObject[] Buttons = new GameObject[3];
    private bool reg = false;
    private static string connStr = @"server=q92563sd.beget.tech;user=q92563sd_ball;database=q92563sd_ball;port=3306;password=40izugob;";
    private MySqlConnection MySqL = new MySqlConnection(connStr);
    public Text Error;
	void Start () {
        IPStatus status = IPStatus.TimedOut;
        try
        {
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            PingReply pr = p.Send(@"google.com");
            status = pr.Status;
        }
        catch { }
        if(status != IPStatus.Success)
        {
            for (int i = 0; i < Inputs.Length; i++) Inputs[i].GetComponent<InputField>().interactable = false;
            for (int i = 0; i < Buttons.Length; i++) Buttons[i].GetComponent<Button>().interactable = false;
            Error.text = "Отсутствует соединение";
        }
        else
        {
            try
            {
                MySqL.Open();
            }
            catch
            {
                for (int i = 0; i < Inputs.Length; i++) Inputs[i].GetComponent<InputField>().interactable = false;
                for (int i = 0; i < Buttons.Length; i++) Buttons[i].GetComponent<Button>().interactable = false;
                Error.text = "Не удалось подключиться";
            }
            finally
            {
               if(MySqL != null) MySqL.Close();
            }
        }
    }
    public void ClickbtnLogin()
    {
        if(Inputs[0].GetComponent<Text>().text == "" || Inputs[1].GetComponent<Text>().text == "") Error.text = "Введены не все параметры";
        else
        {
            for (int i = 0; i < 2; i++) Inputs[i].GetComponent<InputField>().interactable = false;
            Buttons[0].GetComponent<Button>().interactable = false;
            string res = "";
            try
            {
                MySqL.Open();
                MySqlCommand com = new MySqlCommand("SELECT `Login` FROM `accounts` WHERE `Login` = " + Inputs[0].GetComponent<Text>().text, MySqL);
                if (com.ExecuteScalar() == null) res = "null";
                else res = com.ExecuteScalar().ToString();
            }
            catch
            {
                res = "error";
            }
            finally
            {
                if (MySqL != null) MySqL.Close();
            }
            if(res == Inputs[0].GetComponent<Text>().text)
            {
                try
                {
                    MySqL.Open();
                    MySqlCommand com = new MySqlCommand("SELECT `Pass` FROM `accounts` WHERE `Login` = " + Inputs[0].GetComponent<Text>().text, MySqL);
                    if (com.ExecuteScalar() == null) res = "null";
                    else res = com.ExecuteScalar().ToString();
                }
                catch
                {
                    res = "error";
                }
                finally
                {
                    if (MySqL != null) MySqL.Close();
                }
                if(res == Inputs[1].GetComponent<Text>().text)
                {
                    SceneManager.LoadScene("Main");
                }
                else
                {
                    for (int i = 0; i < 2; i++) Inputs[i].GetComponent<InputField>().interactable = true;
                    Buttons[0].GetComponent<Button>().interactable = true;
                    Error.text = "Неверный пароль";
                }
            }
            else
            {
                for (int i = 0; i < 2; i++) Inputs[i].GetComponent<InputField>().interactable = true;
                Buttons[0].GetComponent<Button>().interactable = true;
                Error.text = "Аккаунт не зарегестрирован";
            }
        }
    }
    public void ClickBtnReg()
    {
        if(!reg)
        {
            Buttons[0].SetActive(false);
            Buttons[2].SetActive(true);
            Inputs[2].SetActive(true);
        }
        reg = !reg;
    }
    public void ClickbtnBack()
    {
        reg = !reg;
        Inputs[2].SetActive(false);
        Buttons[2].SetActive(false);
        Buttons[0].SetActive(true);
    }
}
