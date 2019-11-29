using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public int size = 4;

    private void Start()
    {
        InitQuads();
        SetCamera();
    }

    private void SetCamera()
    {
        Camera.main.transform.position += new Vector3(size / 2f, size / 2f, 0f);
        Camera.main.orthographicSize = size / 2f + 1;
    }

    private void InitQuads()
    {
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = new Vector3(col + 0.5f, row + 0.5f, 0f);
                quad.transform.parent = transform;
            }
        }
    }
}