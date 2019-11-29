using System;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public event Action<PuzzlePiece> OnPuzzlePiecePressed;

    public Vector2 coordinates;

    private void OnMouseDown()
    {
        OnPuzzlePiecePressed?.Invoke(this);
    }
}