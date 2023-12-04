using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTextureGenerator : MonoBehaviour
{
    public static Texture2D Generate(bool[,] map, Vector2 playerRoom)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colors = new Color[width * height];

        for (int i = 0; i < colors.Length; i++)
        {
            int x = i % width;
            int y = Mathf.FloorToInt(i / height);
            if (playerRoom == new Vector2(x, y)) {
                colors[i] = Color.green;
            } else {
                colors[i] = map[x, y] == true ? Color.white : Color.clear;
            }
        }

        texture.SetPixels(colors);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;
    }
}
