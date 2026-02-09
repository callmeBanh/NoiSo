using UnityEngine;
using UnityEngine.UI;
public class NewMonoBehaviourScript : MonoBehaviour
{
    public Image icon;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    void Start()
    {
        UpdateIcon();
    }

    public void ToggleSound()
    {
        if(sound.instance != null)
        {
            sound.instance.ToggleSound();
            UpdateIcon();
        }
    }

    void UpdateIcon()
    {
        if(sound.instance == null || icon == null) return;
        icon.sprite = sound.instance.IsMuted() ? soundOffSprite : soundOnSprite;
    }
}
