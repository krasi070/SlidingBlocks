using System;
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

    private void OnMouseDown()
    {
        OnPuzzlePiecePressed?.Invoke(this);
    }
}