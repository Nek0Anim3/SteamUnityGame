using TMPro;
using UnityEngine;

public class DebugVersionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text UI_VersionText;
    private string text;

    private void Awake()
    {
        text = Application.productName + " v" + Application.version + "-dev (July 2026)";
    }

    private void Start()
    {
        UI_VersionText.text = text;
    }
}
