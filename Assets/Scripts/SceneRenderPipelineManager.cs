using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SceneRenderPipelineManager : MonoBehaviour
{
    [System.Serializable]
    public class ScenePipelineSettings
    {
        public string sceneName;
        public RenderPipelineAsset pipelineAsset;
    }

    public ScenePipelineSettings[] sceneSettings;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        // 监听场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("3DSleep");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("2DSleep");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 根据场景名称查找对应的渲染管线设置
        foreach (var setting in sceneSettings)
        {
            if (setting.sceneName == scene.name && setting.pipelineAsset != null)
            {
                GraphicsSettings.renderPipelineAsset = setting.pipelineAsset;
                QualitySettings.renderPipeline = setting.pipelineAsset;
                Debug.Log($"场景 {scene.name} 切换到指定的渲染管线");
                break;
            }
        }
    }
}