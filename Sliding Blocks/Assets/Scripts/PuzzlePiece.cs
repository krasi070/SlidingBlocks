using System;
using System.Collections;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public event Action<PuzzlePiece> OnPuzzlePiecePressed;
    public event Action OnPuzzlePieceFinishedSliding;

    public Vector2Int coordinates;

    private Vector2Int _originalCoordinates;

    public void Init(Vector2Int coordinates, Texture2D image)
    {
        this.coordinates = coordinates;
        this._originalCoordinates = coordinates;
        GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/Texture");
        GetComponent<MeshRenderer>().material.mainTexture = image;
    }

    public void SlideToPosition(Vector3 target, float speed)
    {
        StartCoroutine(Slide(target, speed));
    }

    public bool IsAtOriginalCoordinates()
    {
        return coordinates == _originalCoordinates;
    }

    private IEnumerator Slide(Vector3 target, float speed)
    {
        while ((transform.position - target).sqrMagnitude > 0)
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