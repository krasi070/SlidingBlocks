using System;
using System.Collections;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public event Action<PuzzlePiece> OnPuzzlePiecePressed;

    public Vector2 coordinates;

    public void Init(Vector2 coordinates, Texture2D image)
    {
        this.coordinates = coordinates;
        GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    public IEnumerator Slide(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            yield return null;
        }
    }

    private void OnMouseDown()
    {
        OnPuzzlePiecePressed?.Invoke(this);
    }
}