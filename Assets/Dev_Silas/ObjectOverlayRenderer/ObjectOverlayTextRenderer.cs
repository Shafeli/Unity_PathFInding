using UnityEngine;

public class ObjectOverlayTextRenderer : MonoBehaviour
{

    public string TopText = "Object Name";
    public string BottomText = "Object State";

    private GUIStyle TextStyle = new();

    [SerializeField] private const float TextWidth = 200f;
    [SerializeField] private const float TextHeight = 50f;
    [SerializeField] private const float VerticalSpacing = 20f;

    void Start()
    {
        TextStyle.normal.textColor = Color.white;
    }

    private void OnGUI()
    {
        // Convert the world position of the object to screen position
        Vector3 worldPosition = transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        // Offset the text slightly
        screenPosition.y = Screen.height - screenPosition.y;

        // Calculate the position for the text above the object based on the object's transform
        float objectHeight = transform.localScale.y; // Assuming Y-axis represents the height of the object
        float textYPosition = screenPosition.y - VerticalSpacing - TextHeight - objectHeight;

        // Render the first line
        GUI.Label(new Rect(screenPosition.x, textYPosition, TextWidth, TextHeight), TopText, TextStyle);

        // Render the second line
        GUI.Label(new Rect(screenPosition.x, textYPosition + VerticalSpacing, TextWidth, TextHeight), BottomText,
            TextStyle);
    }
}