using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AsyncSceneLoader : MonoBehaviour
{
    public static AsyncSceneLoader Instance;

    [Header("Loading UI ����")]
    public GameObject loadingUIPrefab; // ��ΪPrefab���ص�UI
    public GameObject loadingUIInScene; // ������Ĭ�Ϲرյ�UI
    private GameObject currentLoadingUI;

    [Header("������ʾ")]
    public Image progressBar;
    public Text progressText;

    [Header("���뽥������")]
    public BlockoutFader blockoutFader;  // ʹ��Blockout�Ľ��뽥��
    public Animator animatorFader;       // ʹ��Animator�Ľ��뽥��

    [Header("����ѡ��")]
    public bool autoEnterScene = true;  // true��������ɺ��Զ����룬false���ȴ����

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
    /// �첽���س���
    /// </summary>
    public void LoadScene(string sceneName, bool usePrefab = true)
    {
        StartCoroutine(DoLoadScene(sceneName, usePrefab));
    }

    private IEnumerator DoLoadScene(string sceneName, bool usePrefab)
    {
        SetupLoadingUI(usePrefab);

        // ���� UI
        yield return StartCoroutine(PlayFadeIn());

        // ��ʼ���س�����������������
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        // ���½�����
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
            Debug.LogError("û���ҵ����õ�Loading UI��");
            return;
        }

        // ��ȡ����
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
        // ����ֶ�ȷ�Ͻ��볡��
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
