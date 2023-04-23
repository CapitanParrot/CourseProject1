using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

// ���� ���� ����, ���� ��������� �������� �� ���.
public class UIManager : MonoBehaviour
{
    public GameObject HealthPanel;

    public Image FullHeart;
    public Image HalfHeart;
    public Image HollowHeart;

    public static UIManager Instance;

    public GameObject BossHealthBar;

    private Slider bossHealthSlider;

    public TextMeshProUGUI bossNameText;

    public static bool IsGamePaused = false;

    public GameObject PauseMenuUI;

    // ��������� ����� ��������, ����� ������ ���� ��������.
    public GameObject InsertMenu;
    public TextMeshProUGUI InsertMenuText;

    public string MainMenuName = "Menu";

    public GameObject InfoPanel;

    public TextMeshProUGUI InfoArtifactName;
    public TextMeshProUGUI InfoArtifactDescription;
    public int InfoDuration = 4;

    public GameObject EndScreen;

    private bool isGameActive = true;

    public GameObject DeathScreen;

    // ��������� ������� �������, ������� ��������� ����� ������ ������.
    public GameObject Story;
    public int StoryTime = 5;

    public Texture2D CursorTexture;

    public Vector2 HotSpotCursor;

    // NEW ��������� ������� ���������� ���� �����
    void Start()
    {
        Cursor.SetCursor(CursorTexture, HotSpotCursor, CursorMode.Auto);
        GameManager.Instance.InitEvent += Init;
        GameManager.Instance.RestartEvent += RestartUI;
        GameManager.Instance.NextLevelEvent += ActivateInsert;
        GameManager.Instance.FinishGameEvent += ShowEndScreen;
    }

    void Awake()
    {
        Instance = this;
        
    }

    public void DeathUI(object sender, PlayerDeathArgs e)
    {
        ShowDeathScreen();
    }

    public void RestartUI(object sender, RestartArgs args)
    {
        DeactivateBossHealth();
    }

    public void Init(object sender, EventArgs args)
    {
        PlayerManager.Instance.InitPlayer += DrawHealth;
        PlayerManager.Instance.ChangeHealth += DrawHealth;
        PlayerManager.Instance.PlayerDeathEvent += DeathUI;
        ArtifactManager.Instance.PickArtifact += SetArtifactDescription;
    }

    // ��������� ����������� ������� �������� �����.
    public void DrawBossHealth(string bossName, int health)
    {
        BossHealthBar.SetActive(true);
        bossHealthSlider = BossHealthBar.GetComponent<Slider>();
        bossHealthSlider.maxValue = health;
        bossHealthSlider.value = health;
        bossNameText.text = bossName;
    }

    public void ChangeBossHealth(int newHealth)
    {
        bossHealthSlider.value = newHealth;
    }

    public void DeactivateBossHealth()
    {
        BossHealthBar.SetActive(false);
    }

    // ��������� ��������� ������.
    public void DrawHealth(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < HealthPanel.transform.childCount; i++)
        {
            Destroy(HealthPanel.transform.GetChild(i).gameObject);
        }

        // ������ ��� ��� ��������� �������� ������ ��� ������.
        int fullHeartsCount = currentHealth / 2;
        int halfHeartsCount = currentHealth % 2;
        int hollowHeartsCount = (maxHealth - currentHealth) / 2;

        for (int i = 0; i < fullHeartsCount; i++)
        {
            Instantiate(FullHeart, HealthPanel.transform);
        }
        for (int i = 0; i < halfHeartsCount; i++)
        {
            Instantiate(HalfHeart, HealthPanel.transform);
        }
        for (int i = 0; i < hollowHeartsCount; i++)
        {
            Instantiate(HollowHeart, HealthPanel.transform);
        }
    }

    public void DrawHealth(object sender, InitPlayerArgs args)
    {
        for (int i = 0; i < HealthPanel.transform.childCount; i++)
        {
            Destroy(HealthPanel.transform.GetChild(i).gameObject);
        }

        // ������ ��� ��� ��������� �������� ������ ��� ������.
        int fullHeartsCount = args.CurrentHealth / 2;
        int halfHeartsCount = args.CurrentHealth % 2;
        int hollowHeartsCount = (args.MaxHealth - args.CurrentHealth) / 2;

        for (int i = 0; i < fullHeartsCount; i++)
        {
            Instantiate(FullHeart, HealthPanel.transform);
        }
        for (int i = 0; i < halfHeartsCount; i++)
        {
            Instantiate(HalfHeart, HealthPanel.transform);
        }
        for (int i = 0; i < hollowHeartsCount; i++)
        {
            Instantiate(HollowHeart, HealthPanel.transform);
        }
    }

    public void DrawHealth(object sender, PlayerHealthArgs args)
    {
        for (int i = 0; i < HealthPanel.transform.childCount; i++)
        {
            Destroy(HealthPanel.transform.GetChild(i).gameObject);
        }

        // ������ ��� ��� ��������� �������� ������ ��� ������.
        int fullHeartsCount = args.CurrentHealth / 2;
        int halfHeartsCount = args.CurrentHealth % 2;
        int hollowHeartsCount = (args.MaxHealth - args.CurrentHealth) / 2;

        for (int i = 0; i < fullHeartsCount; i++)
        {
            Instantiate(FullHeart, HealthPanel.transform);
        }
        for (int i = 0; i < halfHeartsCount; i++)
        {
            Instantiate(HalfHeart, HealthPanel.transform);
        }
        for (int i = 0; i < hollowHeartsCount; i++)
        {
            Instantiate(HollowHeart, HealthPanel.transform);
        }
    }

    // ������ ��� ������
    void Pause()
    {
        PauseMenuUI.SetActive(true);
        IsGamePaused = true;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        IsGamePaused = false;
        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuName);
    }

    // �������� ���������.
    public void ActivateInsert(int level)
    {
        isGameActive = false;
        InsertMenu.SetActive(true);
        InsertMenuText.text = "���� " + level;
        Time.timeScale = 0f;
    }

    public void ActivateInsert(object sender, int level)
    {
        isGameActive = false;
        InsertMenu.SetActive(true);
        InsertMenuText.text = "���� " + level;
        Time.timeScale = 0f;
    }

    // ��������� ��������� � ���������� �������.
    public void DeactivateInsert()
    {
        isGameActive = true;
        InsertMenu.SetActive(false);
        Time.timeScale = 1f;
        AstarPath.active.Scan();
        if(GameManager.Instance.DeathCounter == 1 && GameManager.Instance.LevelCounter == 1)
        {
            ShowStory();
        }
    }

    // ���������� �������� ��������� ����� �������.
    public void SetArtifactDescription(string name, string description)
    {
        InfoPanel.SetActive(true);
        InfoArtifactDescription.text = description;
        InfoArtifactName.text = name;
        StartCoroutine(InfoOff(InfoDuration));
    }

    public void SetArtifactDescription(object sender, ArtifactArgs args)
    {
        InfoPanel.SetActive(true);
        InfoArtifactDescription.text = args.ArtifactDescription;
        InfoArtifactName.text = args.ArtifactName;
        StartCoroutine(InfoOff(InfoDuration));
    }


    IEnumerator InfoOff(int time)
    {
        yield return new WaitForSeconds(time);
        InfoPanel.SetActive(false);
    }

    private void DeactivatePopUpPanels()
    {
        InfoPanel.SetActive(false);
        Story.SetActive(false);
    }

    public void ShowEndScreen()
    {
        isGameActive = false;
        EndScreen.SetActive(true);
        DeactivatePopUpPanels();
        Time.timeScale = 0f;
    }

    public void ShowEndScreen(object sender, int DeathCount)
    {
        isGameActive = false;
        EndScreen.SetActive(true);
        DeactivatePopUpPanels();
        Time.timeScale = 0f;
    }

    public void ShowDeathScreen()
    {
        isGameActive = false;
        DeathScreen.SetActive(true);
        DeactivatePopUpPanels();
        Time.timeScale = 0f;
    }

    public void HideDeathScreen()
    {
        isGameActive = true;
        DeathScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowStory()
    {
        Story.SetActive(true);
        StartCoroutine(CloseStory(StoryTime));
    }

    IEnumerator CloseStory(int time)
    {
        yield return new WaitForSeconds(time);
        Story.SetActive(false);
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
}
