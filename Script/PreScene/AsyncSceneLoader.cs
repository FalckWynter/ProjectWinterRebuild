using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace PlentyFishFramework
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        [Header("UI 控制器")]
        public LoadingUIMono loadingUI;

        [Header("淡入淡出参数")]
        public float fadeInDuration = 0.5f;
        public float fadeOutDuration = 0.5f;

        [Header("完成后切换模式")]
        public bool autoSwitchScene = true; // true = 自动进入新场景，false = 等待玩家输入
        public float minStayTime = 1.0f;    // 至少等待多久后才允许继续

        [Header("淡出设置")]
        public bool fadeOutWhenFinish = true; // true = 渐出，false = 立即消失

        private AsyncOperation asyncOperation;
        private bool isReadyToSwitch = false;
        private bool sceneActivated = false; // 标记是否已完成场景切换

        public bool isCloseMusicWhenLoad = true;

        /// <summary>
        /// 异步加载场景
        /// </summary>
        public void LoadSceneAsync(string sceneName)
        {
            if (isCloseMusicWhenLoad)
                AudioManagerSystem.StopCurrentMusic();
            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            // 打开 UI 并淡入
            yield return loadingUI.FadeIn(fadeInDuration);

            yield return new WaitForSeconds(0.1f);

            // 开始异步加载
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;
            DontDestroyOnLoad(loadingUI.gameObject);
            float timer = 0f;

            while (!asyncOperation.isDone)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                loadingUI.UpdateProgress(progress);

                if (asyncOperation.progress >= 0.9f && !isReadyToSwitch && timer >= minStayTime)
                {
                    if (autoSwitchScene)
                    {
                        isReadyToSwitch = true;
                        asyncOperation.allowSceneActivation = true;
                        // 由 UI 自己负责淡出和销毁
                        loadingUI.StartExitAndSelfDestroy(fadeOutDuration, fadeOutWhenFinish,0.5f);
                    }
                    else
                    {
                        isReadyToSwitch = true;
                        loadingUI.ShowPressAnyKeyHint();

                        yield return new WaitUntil(() => Input.anyKeyDown);
                        asyncOperation.allowSceneActivation = true;
                        // 由 UI 自己负责淡出和销毁
                        loadingUI.StartExitAndSelfDestroy(fadeOutDuration, fadeOutWhenFinish,0.5f);
                    }
                }

                yield return null;
            }

            // 到这里说明场景已经切换
            sceneActivated = true;
            yield return null; // 等一帧确保新场景加载完成


        }



        private IEnumerator ExitLoadingUI()
        {
            if (fadeOutWhenFinish)
            {
                yield return loadingUI.FadeOut(fadeOutDuration);
            }
            else
            {
                loadingUI.SetAlpha(0);
            }
        }

        public void LoadGameScene()
        {
            LoadSceneAsync(GameSceneManager.gameSceneName);
        }
        public void LoadEndingScene()
        {
            LoadSceneAsync(GameSceneManager.endingSceneName);

        }
        public void LoadMainTopicScene()
        {
            LoadSceneAsync(GameSceneManager.mainTopicSceneName);

        }
    }
}
//public class AsyncSceneLoader : MonoBehaviour
//{
//    public static AsyncSceneLoader Instance;

//    [Header("Loading UI 配置")]
//    public GameObject loadingUIPrefab; // 作为Prefab加载的UI
//    public GameObject loadingUIInScene; // 场景内默认关闭的UI
//    private GameObject currentLoadingUI;

//    [Header("进度显示")]
//    public Image progressBar;
//    public Text progressText;

//    [Header("渐入渐出控制")]
//    public BlockoutFader blockoutFader;  // 使用Blockout的渐入渐出
//    public Animator animatorFader;       // 使用Animator的渐入渐出

//    [Header("加载选项")]
//    public bool autoEnterScene = true;  // true：加载完成后自动进入，false：等待点击

//    private AsyncOperation loadingOperation;
//    private bool isReadyToActivate = false;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//    }
//    public void LoadScene()
//    {
//        LoadScene("GameScene", false);
//    }
//    /// <summary>
//    /// 异步加载场景
//    /// </summary>
//    public void LoadScene(string sceneName, bool usePrefab = true)
//    {
//        StartCoroutine(DoLoadScene(sceneName, usePrefab));
//    }

//    private IEnumerator DoLoadScene(string sceneName, bool usePrefab)
//    {
//        SetupLoadingUI(usePrefab);

//        // 渐入 UI
//        yield return StartCoroutine(PlayFadeIn());

//        // 开始加载场景，但不立即激活
//        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
//        loadingOperation.allowSceneActivation = false;

//        // 更新进度条
//        while (!loadingOperation.isDone)
//        {
//            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
//            UpdateProgress(progress);

//            if (progress >= 1f && !isReadyToActivate)
//            {
//                if (autoEnterScene)
//                {
//                    loadingOperation.allowSceneActivation = true;
//                    yield return StartCoroutine(PlayFadeOut());
//                }
//                else
//                {
//                    isReadyToActivate = true;
//                }
//            }

//            yield return null;
//        }
//    }

//    private void SetupLoadingUI(bool usePrefab)
//    {
//        if (currentLoadingUI != null) Destroy(currentLoadingUI);

//        if (usePrefab && loadingUIPrefab != null)
//        {
//            currentLoadingUI = Instantiate(loadingUIPrefab);
//        }
//        else if (!usePrefab && loadingUIInScene != null)
//        {
//            currentLoadingUI = loadingUIInScene;
//            currentLoadingUI.SetActive(true);
//        }
//        else
//        {
//            Debug.LogError("没有找到可用的Loading UI！");
//            return;
//        }

//        // 获取引用
//        progressBar = currentLoadingUI.GetComponentInChildren<Image>();
//        progressText = currentLoadingUI.GetComponentInChildren<Text>();
//        blockoutFader = currentLoadingUI.GetComponentInChildren<BlockoutFader>();
//        animatorFader = currentLoadingUI.GetComponentInChildren<Animator>();
//        //DontDestroyOnLoad(loadingUIInScene);
//    }

//    private void UpdateProgress(float progress)
//    {
//        if (progressBar != null) progressBar.fillAmount = progress;
//        if (progressText != null) progressText.text = (int)(progress * 100f) + "%";
//    }

//    private IEnumerator PlayFadeIn()
//    {
//        if (blockoutFader != null) yield return StartCoroutine(blockoutFader.FadeIn());
//        else if (animatorFader != null)
//        {
//            animatorFader.SetTrigger("FadeIn");
//            yield return new WaitForSeconds(1f);
//        }
//    }

//    private IEnumerator PlayFadeOut()
//    {
//        if (blockoutFader != null) yield return StartCoroutine(blockoutFader.FadeOut());
//        else if (animatorFader != null)
//        {
//            animatorFader.SetTrigger("FadeOut");
//            yield return new WaitForSeconds(1f);
//        }
//    }

//    void Update()
//    {
//        // 玩家手动确认进入场景
//        if (!autoEnterScene && isReadyToActivate && Input.anyKeyDown)
//        {
//            StartCoroutine(PlayFadeOutAndActivate());
//        }
//    }

//    private IEnumerator PlayFadeOutAndActivate()
//    {
//        yield return StartCoroutine(PlayFadeOut());
//        loadingOperation.allowSceneActivation = true;
//    }
//}
