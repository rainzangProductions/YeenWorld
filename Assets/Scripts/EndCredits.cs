using UnityEngine;

public class EndCredits : MonoBehaviour {
    public float scrollSpeed = 20f;
    public float resetY = -300f;
    public float startY = -600f;

    private RectTransform rectTransform;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,startY);
    }

    void Update() {
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (rectTransform.anchoredPosition.y > resetY) {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,startY);
        }
    }
}