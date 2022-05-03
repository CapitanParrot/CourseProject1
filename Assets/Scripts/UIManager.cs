using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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


    void Start()
    {
        Cursor.SetCursor(CursorTexture, HotSpotCursor, CursorMode.Auto);
    }

    void Awake()
    {
        Instance = this;
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
        Time.timeScale = 0.1f;
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

    IEnumerator InfoOff(int time)
    {
        yield return new WaitForSeconds(time);
        InfoPanel.SetActive(false);
    }

    public void ShowEndScreen()
    {
        isGameActive = false;
        EndScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowDeathScreen()
    {
        isGameActive = false;
        DeathScreen.SetActive(true);
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
