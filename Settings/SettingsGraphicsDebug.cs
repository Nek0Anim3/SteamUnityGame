using UnityEngine;

public class SettingsGraphicsDebug : MonoBehaviour
{
    private static SettingsGraphicsDebug Instance;

    private readonly int VIDEO_FPS = 90;
    private readonly int VIDEO_VSYNC = 1;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        Application.targetFrameRate = VIDEO_FPS;
        // 1 - overrides VIDEO_FPS
        QualitySettings.vSyncCount = VIDEO_VSYNC;
    }
}
