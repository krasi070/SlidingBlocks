using System;
using System.Collections;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public event Action<PuzzlePiece> OnPuzzlePiecePressed;
    public event Action OnPuzzlePieceFinishedSliding;

    public Vector2Int coordinates;

    public void Init(Vector2Int coordinates, Texture2D image)
    {
        this.coordinates = coordinates;
        GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    public void SlideToPosition(Vector3 target, float speed)
    {
        StartCoroutine(Slide(target, speed));
    }

    private IEnumerator Slide(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            yield return null;
        }

        OnPuzzlePieceFinishedSliding?.Invoke();
    }

    private void OnMouseDown()
    {
        OnPuzzlePiecePressed?.Invoke(this);
    }
}