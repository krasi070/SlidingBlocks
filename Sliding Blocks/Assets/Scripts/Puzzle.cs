using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public int size = 4;

    private GameObject _hiddenPiece;

    private void Start()
    {
        InitQuads();
        Camera.main.orthographicSize = size * 0.55f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                float distance = Mathf.Abs(Vector3.Distance(_hiddenPiece.transform.position, hit.transform.position));
                if (Mathf.Approximately(distance, 1f))
                {
                    Vector3 tempPos = _hiddenPiece.transform.position;
                    _hiddenPiece.transform.position = hit.transform.position;
                    hit.transform.position = tempPos;
                }
            }
        }
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

                if (row == start && col == end)
                {
                    _hiddenPiece = quad;
                    _hiddenPiece.SetActive(false);
                }
            }
        }
    }
}