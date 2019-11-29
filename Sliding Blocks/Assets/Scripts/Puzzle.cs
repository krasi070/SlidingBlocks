﻿using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public int size = 4;

    private PuzzlePiece _hiddenPiece;

    private void Start()
    {
        InitQuads();
        Camera.main.orthographicSize = size * 0.55f;
    }

    private void InitQuads()
    {
        for (float row = 0; row < size; row++)
        {
            for (float col = 0; col < size; col++)
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = new Vector3(col - (size / 2f) + 0.5f, row - (size / 2f) + 0.5f, 0f);
                quad.transform.parent = transform;

                PuzzlePiece puzzlePiece = quad.AddComponent<PuzzlePiece>();
                puzzlePiece.OnPuzzlePiecePressed += MovePuzzlePiece;
                puzzlePiece.coordinates = new Vector2(row, col);

                if (row == 0 && col == size - 1)
                {
                    quad.SetActive(false);
                    _hiddenPiece = puzzlePiece;
                }
            }
        }
    }

    private void MovePuzzlePiece(PuzzlePiece puzzlePiece)
    {
        if ((puzzlePiece.coordinates - _hiddenPiece.coordinates).sqrMagnitude == 1f)
        {
            Vector2 tempCoord = _hiddenPiece.coordinates;
            _hiddenPiece.coordinates = puzzlePiece.coordinates;
            puzzlePiece.coordinates = tempCoord;

            Vector3 tempPos = _hiddenPiece.transform.position;
            _hiddenPiece.transform.position = puzzlePiece.transform.position;
            puzzlePiece.transform.position = tempPos;
        }
    }
}