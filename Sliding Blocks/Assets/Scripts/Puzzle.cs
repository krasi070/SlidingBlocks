using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public int size = 4;

    private void Start()
    {
        InitQuads();
        Camera.main.orthographicSize = size * 0.55f;
    }

    private void InitQuads()
    {
        float start = -size / 2f + 0.5f;
        float end = size / 2f - 0.5f;
        for (float row = start; row <= end; row++)
        {
            for (float col = start; col <= end; col++)
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = new Vector3(col, row , 0f);
                quad.transform.parent = transform;
            }
        }
    }
}