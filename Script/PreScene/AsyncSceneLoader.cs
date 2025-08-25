using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AsyncSceneLoader : MonoBehaviour
{
    public static AsyncSceneLoader Instance;

    [Header("Loading UI 配置")]
    public GameObject loadingUIPrefab; // 作为Prefab加载的UI
    public GameObject loadingUIInScene; // 场景内默认关闭的UI
    private GameObject currentLoadingUI;

    [Header("进度显示")]
    public Image progressBar;
    public Text progressText;

    [Header("渐入渐出控制")]
    public BlockoutFader blockoutFader;  // 使用Blockout的渐入渐出
    public Animator animatorFader;       // 使用Animator的渐入渐出

    [Header("加载选项")]
    public bool autoEnterScene = true;  // true：加载完成后自动进入，false：等待点击

    private AsyncOperation loadingOperation;
    private bool isReadyToActivate = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void LoadScene()
    {
        LoadScene("GameScene", false);
    }
    /// <summary>
    /// 异步加载场景
    /// </summary>
    public void LoadScene(string sceneName, bool usePrefab = true)
    {
        StartCoroutine(DoLoadScene(sceneName, usePrefab));
    }

    private IEnumerator DoLoadScene(string sceneName, bool usePrefab)
    {
        SetupLoadingUI(usePrefab);

        // 渐入 UI
        yield return StartCoroutine(PlayFadeIn());

        // 开始加载场景，但不立即激活
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        // 更新进度条
        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            UpdateProgress(progress);

            if (progress >= 1f && !isReadyToActivate)
            {
                if (autoEnterScene)
                {
                    loadingOperation.allowSceneActivation = true;
                    yield return StartCoroutine(PlayFadeOut());
                }
                else
                {
                    isReadyToActivate = true;
                }
            }

            yield return null;
        }
    }

    private void SetupLoadingUI(bool usePrefab)
    {
        if (currentLoadingUI != null) Destroy(currentLoadingUI);

        if (usePrefab && loadingUIPrefab != null)
        {
            currentLoadingUI = Instantiate(loadingUIPrefab);
        }
        else if (!usePrefab && loadingUIInScene != null)
        {
            currentLoadingUI = loadingUIInScene;
            currentLoadingUI.SetActive(true);
        }
        else
        {
            Debug.LogError("没有找到可用的Loading UI！");
            return;
        }

        // 获取引用
        progressBar = currentLoadingUI.GetComponentInChildren<Image>();
        progressText = currentLoadingUI.GetComponentInChildren<Text>();
        blockoutFader = currentLoadingUI.GetComponentInChildren<BlockoutFader>();
        animatorFader = currentLoadingUI.GetComponentInChildren<Animator>();
        DontDestroyOnLoad(loadingUIInScene);
    }

    private void UpdateProgress(float progress)
    {
        if (progressBar != null) progressBar.fillAmount = progress;
        if (progressText != null) progressText.text = (int)(progress * 100f) + "%";
    }

    private IEnumerator PlayFadeIn()
    {
        if (blockoutFader != null) yield return StartCoroutine(blockoutFader.FadeIn());
        else if (animatorFader != null)
        {
            animatorFader.SetTrigger("FadeIn");
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator PlayFadeOut()
    {
        if (blockoutFader != null) yield return StartCoroutine(blockoutFader.FadeOut());
        else if (animatorFader != null)
        {
            animatorFader.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        // 玩家手动确认进入场景
        if (!autoEnterScene && isReadyToActivate && Input.anyKeyDown)
        {
            StartCoroutine(PlayFadeOutAndActivate());
        }
    }

    private IEnumerator PlayFadeOutAndActivate()
    {
        yield return StartCoroutine(PlayFadeOut());
        loadingOperation.allowSceneActivation = true;
    }
}
