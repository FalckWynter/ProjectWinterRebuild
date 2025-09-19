using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateDebugger : MonoBehaviour
{
    private int frameCount = 0;

    void OnEnable()
    {
        Debug.Log($"[UpdateDebugger] OnEnable: {name}, scene={gameObject.scene.name}, sceneLoaded={gameObject.scene.isLoaded}");
        PrintParentChain();
    }

    void OnDisable()
    {
        Debug.Log($"[UpdateDebugger] OnDisable: {name}");
    }

    void Update()
    {
        if (frameCount % 60 == 0) // ÿ�� 60 ֡��ӡһ�Σ�����ˢ������̨
        {
            Debug.Log($"[UpdateDebugger] Update running: {name}, parent={transform.parent?.name ?? "null"}, frame={Time.frameCount}");
        }
        frameCount++;
    }

    private void PrintParentChain()
    {
        Transform t = transform;
        int depth = 0;
        while (t != null)
        {
            GameObject go = t.gameObject;
            string info =
                $"[{depth}] {go.name} | activeSelf={go.activeSelf}, activeInHierarchy={go.activeInHierarchy}, scene={go.scene.name}, loaded={go.scene.isLoaded}";

            // ��鳣�����
            var canvas = go.GetComponent<Canvas>();
            if (canvas != null)
                info += $" | Canvas (renderMode={canvas.renderMode}, enabled={canvas.enabled})";

            var group = go.GetComponent<CanvasGroup>();
            if (group != null)
                info += $" | CanvasGroup (alpha={group.alpha}, interactable={group.interactable}, blocksRaycasts={group.blocksRaycasts})";

            // ��ӡ MonoBehaviour ״̬
            var monos = go.GetComponents<MonoBehaviour>();
            foreach (var mono in monos)
            {
                if (mono == null) continue;
                info += $" | {mono.GetType().Name}(enabled={mono.enabled})";
            }

            Debug.Log(info);

            t = t.parent;
            depth++;
        }
    }
}
