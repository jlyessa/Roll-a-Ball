using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    public GameObject[] GameMain = new GameObject[4];
    public GameObject[] button1 = new GameObject[4];
    public GameObject[] BtnSelectPanel = new GameObject[6];
    public GameObject[] SelectShop = new GameObject[4];
    public GameObject[] ParamsShop = new GameObject[1];
    public GameObject[] ParamsUp = new GameObject[2];
    public GameObject[] Levels = new GameObject[2];
    public Texture[] texturePlayer = new Texture[11];
    public Text[] text = new Text[6];
    public GameObject PanelLevel;
    public GameObject player;
    public GameObject BtnBuy;
    public Camera m_camera;
    static public Texture[] TextureExport = new Texture[11];
    static public int Speed = 1, Skin = 0, Count = 0, Level = 1;
    private int updateCamera, MainCameraUpdate, PlayerSelectSkin, selectSkin, forSelectSkin;
    static private bool[] SelectSkin = new bool[11];
    private const int speedCamera = 1, cameraSpeed = 1;
    static private string str = "SelectSkin_";
    static private string regKeyName = "Software\\Roll a Ball\\BD";
    private int[] CountBuySkin = { 0, 50, 100, 100, 150, 200, 300, 300, 300, 300, 500 };
    private int[] CountBuySpeed = { 0, 0, 20, 35, 60, 105, 145, 200, 280, 390, 500 };
    private string[] NameTexture = { "Обычный", "Красный", "Золотистый", "Синий", "Разноцветный", "Граффити", "Флаг США", "Флаг Украины", "Флаг России", "Флаг Евросоюза", "Планета Земля" };
    private void Start()
    {
        for (int i = 0; i != TextureExport.Length; i++) TextureExport[i] = texturePlayer[i];
        WriteParam(player, Levels);
    }
    private void OnApplicationQuit()
    {
        ReadParams();
    }
    private void FixedUpdate()
    {
        if (MainCameraUpdate == 1)
        {
            m_camera.transform.position += new Vector3(-3f, -2.5f, 10.5f) * Time.deltaTime * speedCamera;
            m_camera.transform.Rotate(0.06f, -30f * cameraSpeed * Time.deltaTime, 0.15f);
            updateCamera++;
            if (updateCamera == 85)
            {
                MainCameraUpdate = 0;
                for (int i = 0; i != button1.Length; i++) button1[i].SetActive(true);
                text[1].text = "Монет: " + Count;
            }
        }
        if (MainCameraUpdate == 2)
        {
            m_camera.transform.position += new Vector3(3f, 2.5f, -10.5f) * Time.deltaTime * speedCamera;
            m_camera.transform.Rotate(-0.06f, 30f * cameraSpeed * Time.deltaTime, -0.15f);
            updateCamera--;
            if (updateCamera == 0)
            {
                MainCameraUpdate = 0;
                ActiveGame(2);
                m_camera.transform.position = new Vector3(566, 129, 230);

            }
        }
        if (PlayerSelectSkin == 1)
        {
            UpdateBuyStart(0.5f);
            if (forSelectSkin == 150) UpdateBuyCenter(-100f);
            if (forSelectSkin == 200) UpdateBuyOver();
        }
        if (PlayerSelectSkin == 2)
        {
            UpdateBuyStart(-0.5f);
            if (forSelectSkin == 50) UpdateBuyCenter(100f);
            if (forSelectSkin == 200) UpdateBuyOver();
        }
    }
    public void StartGame()
    {
        ActiveGame(1);
        PanelLevel.SetActive(true);
    }
    public void ShopGame()
    {
        ActiveGame(1);
        text[3].text = NameTexture[Skin];
        selectSkin = Skin;
        BtnSelectPanel[4].GetComponent<Button>().interactable = false;
        MainCameraUpdate = 1;
    }
    public void ParamsShopPlayer()
    {
        button1[0].GetComponent<Button>().interactable = false;
        if (button1[1].GetComponent<Button>().interactable == false)
        {
            button1[1].GetComponent<Button>().interactable = true;
            for (int i = 0; i != SelectShop.Length; i++) if (SelectShop[i].activeInHierarchy == true) SelectShop[i].SetActive(false);
            if (BtnBuy.activeInHierarchy == true) BtnBuy.SetActive(false);
            if (selectSkin != Skin) NoAcceptShop();
        }
        for(int i = 0; i != ParamsShop.Length; i++) ParamsShop[i].SetActive(true);
        TextUpdateSpeed();
    }
    public void PaintingShop()
    {
        if(button1[0].GetComponent<Button>().interactable == false)
        {
            button1[0].GetComponent<Button>().interactable = true;
            ForParamsShop();
        }
        for (int i = 0; i != SelectShop.Length; i++) SelectShop[i].SetActive(true);
        for (int i = 0; i != ParamsShop.Length; i++) if (ParamsShop[i].GetComponent<Button>().interactable == false) ParamsShop[i].GetComponent<Button>().interactable = true;
        button1[1].GetComponent<Button>().interactable = false;
    }
    public void BackGame()
    {
        for (int i = 0; i != button1.Length; i++) button1[i].SetActive(false);
        for (int i = 0; i != SelectShop.Length; i++) SelectShop[i].SetActive(false);
        if (BtnBuy.activeInHierarchy == true) BtnBuy.SetActive(false);
        ForParamsShop();
        for (int i = 0; i != ParamsShop.Length; i++) if (ParamsShop[i].GetComponent<Button>().interactable == false) ParamsShop[i].GetComponent<Button>().interactable = true;
        for (int i = 0; i != 2; i++) if (button1[i].GetComponent<Button>().interactable == false) button1[i].GetComponent<Button>().interactable = true;
        MainCameraUpdate = 2;
        if (selectSkin != Skin) NoAcceptShop();
    }
    public void NextShop()
    {
        if (selectSkin == 10) selectSkin = 0;
        else selectSkin++;
        SelectBuy(1);
    }
    public void BackShop()
    {
        if (selectSkin == 0) selectSkin = 10;
        else selectSkin--;
        SelectBuy(2);
    }
    public void ShopAccept()
    {
        Skin = selectSkin;
        BtnSelectPanel[4].GetComponent<Button>().interactable = false;
    }
    public void ShopSpeed()
    {
        ParamsShop[0].GetComponent<Button>().interactable = false;
        if (ParamsUp[0].activeInHierarchy == false) for (int i = 0; i != ParamsUp.Length; i++) ParamsUp[i].SetActive(true);
    }
    public void ShopUpSpeed()
    {
        Speed++;
        Count -= CountBuySpeed[Speed];
        text[1].text = "Монет: " + Count;
        TextUpdateSpeed();
        ReadParams();
    }
    public void ShopBuy()
    {
        SelectSkin[selectSkin] = true;
        Count -= CountBuySkin[selectSkin];
        text[1].text = "Монет: " + Count;
        ReadParams();
        BtnSelectPanel[4].GetComponent<Button>().interactable = true;
        BtnSelectPanel[5].SetActive(false);
    }
    public void GameBack()
    {
        PanelLevel.SetActive(false);
        ActiveGame(2);
    }
    public void Level_1()
    {
        SceneManager.LoadScene("1Level");
    }
    public void Level_2()
    {
        SceneManager.LoadScene("2Level");
    }
    private void SelectBuy(int value)
    {
        for (int i = 0; i != BtnSelectPanel.Length; i++) BtnSelectPanel[i].GetComponent<Button>().interactable = false;
        if (SelectSkin[selectSkin] == false)
        {
            if (BtnBuy.activeInHierarchy == false) BtnBuy.SetActive(true);
            text[2].text = "Купить за " + CountBuySkin[selectSkin] + " монет";
        }
        else
        {
            if (BtnBuy.activeInHierarchy == true) BtnBuy.SetActive(false);
        }
        text[3].text = NameTexture[selectSkin];
        PlayerSelectSkin = value;
    }
    private void UpdateBuyOver()
    {
        PlayerSelectSkin = 0;
        forSelectSkin = 0;
        if(CountBuySkin[selectSkin] > Count || Skin == selectSkin) for (int i = 0; i != 4; i++) BtnSelectPanel[i].GetComponent<Button>().interactable = true;
        else for(int i = 0; i != BtnSelectPanel.Length; i++) if(BtnSelectPanel[i].GetComponent<Button>().interactable == false) BtnSelectPanel[i].GetComponent<Button>().interactable = true;
    }
    private void UpdateBuyCenter(float value)
    {
        player.SetActive(false);
        player.transform.position += new Vector3(0, 0, value);
        player.SetActive(true);
        player.GetComponent<Renderer>().material.mainTexture = texturePlayer[selectSkin];
    }
    private void UpdateBuyStart(float value)
    {
        player.transform.position += new Vector3(0, 0, value);
        forSelectSkin++;
    }
    private void NoAcceptShop()
    {
        selectSkin = Skin;
        player.GetComponent<Renderer>().material.mainTexture = texturePlayer[selectSkin];
        text[3].text = NameTexture[selectSkin];
    }
    private void ForParamsShop()
    {
        for (int i = 0; i != ParamsShop.Length; i++) if (ParamsShop[i].activeInHierarchy == true) ParamsShop[i].SetActive(false);
        for (int i = 0; i != ParamsUp.Length; i++) if (ParamsUp[i].activeInHierarchy == true) ParamsUp[i].SetActive(false);
    }
    private void TextUpdateSpeed()
    {
        text[4].text = "Уровень скорости: " + Speed + " / 10";
        if (Speed != 10)
        {
            if (Count < CountBuySpeed[Speed + 1] && ParamsUp[1].GetComponent<Button>().interactable == true) ParamsUp[1].GetComponent<Button>().interactable = false;
            if (ParamsUp[1].GetComponent<Button>().interactable == false && Count >= CountBuySpeed[Speed + 1]) ParamsUp[1].GetComponent<Button>().interactable = true;
            text[5].text = "Повысить за " + CountBuySpeed[Speed + 1] + " монет";
        }
        else
        {
            if (ParamsUp[1].GetComponent<Button>().interactable == true) ParamsUp[1].GetComponent<Button>().interactable = false;
            text[5].text = "Улучшен на максимум";
        }
    }
    private void ActiveGame(int value)
    {
        if(value == 1)
        {
            for(int i = 0; i != GameMain.Length; i++) GameMain[i].SetActive(false);
            text[0].text = "";
        }
        else
        {
            text[0].text = "Roll a Ball";
            for (int i = 0; i != GameMain.Length; i++) GameMain[i].SetActive(true);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public static void WriteParam(GameObject obj, GameObject[] Lvl)
    {
        SelectSkin[0] = true;
        for (int i = 1; i != SelectSkin.Length; i++) SelectSkin[i] = false;
        
        obj.GetComponent<Renderer>().material.mainTexture = TextureExport[Skin];
        for (int i = Level; i != Lvl.Length; i++) Lvl[i].GetComponent<Button>().interactable = false;
    }
    public static void ReadParams()
    {
        
    }
}
