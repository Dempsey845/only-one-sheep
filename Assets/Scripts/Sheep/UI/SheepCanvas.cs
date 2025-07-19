using UnityEngine;
using UnityEngine.UI;

public class SheepCanvas : MonoBehaviour
{
    [SerializeField] private Image emojiImage;

    private void Update()
    {
        if (SheepManager.Instance != null)
        {
            Vector3 sheepCanvasPosition = SheepManager.Instance.GetSheepCanvasPosition();
            transform.position = sheepCanvasPosition;

            if (PlayerManager.Instance != null)
            {
                transform.LookAt(Camera.main.transform);
                transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            }
        }
    }

    public void SetEmojiSprite(Sprite newEmojiSprite)
    {
        emojiImage.sprite = newEmojiSprite;
    }
}
