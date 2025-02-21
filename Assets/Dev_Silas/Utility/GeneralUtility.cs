using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class GeneralUtility
{
    public class WorldTextGenerator
    {
        private const int kSortingDefault = 5000;

        public int sortingOrderDefault;

        public WorldTextGenerator(int sortingOrder = kSortingDefault)
        {
            sortingOrderDefault = sortingOrder;
        }
        
        // Create Text in the World
        public static TextMesh CreateWorldText(string text, Transform parent = null,
            Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null,
            TextAnchor textAnchor = TextAnchor.UpperRight, TextAlignment textAlignment = TextAlignment.Right,
            int sortingOrder = 5000)
        {
            color ??= Color.magenta;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment,
                sortingOrder);
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
            Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }

    }

    public class MouseUtility
    {


        public static Vector3 GetWorldPosition()
        {
            Vector3 v = GetWorldPositionWithZ(Input.mousePosition, Camera.main);
            v.z = 0.0f;
            return v;
        }

        public static Vector3 GetWorldPositionWithZ(Camera worldCamera)
        {
            return GetWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            // Ensure the Z position is set to a proper value
            screenPosition.z = worldCamera.nearClipPlane; // Set to the near clipping plane for proper conversion

            // Convert screen position to world position
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}
