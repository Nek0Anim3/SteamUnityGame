using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICrosshair : MonoBehaviour
{
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Sprite[] crosshairSprite = new Sprite[3];
    // 0 - Point
    // 1 - Active
    // 2 - Grab
    private void Start()
    {
        crosshairImage.sprite = crosshairSprite[0];
    }
    
    public void SetCrosshair(int ID)
    {
        if (ID > crosshairSprite.Length || ID < 0) return;
        crosshairImage.sprite = crosshairSprite[ID];
    }
    
}
