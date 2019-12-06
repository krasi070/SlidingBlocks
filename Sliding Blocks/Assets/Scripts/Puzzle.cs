using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public Texture2D image;
    public int size = 4;
    public float slideSpeed = 3f;

    private PuzzlePiece _hiddenPiece;
    private Queue<PuzzlePiece> _toSlideQueue;
    private bool _pieceIsSliding;

    private void Start()
    {
        InitQuads();
        Camera.main.orthographicSize = size * 0.55f;
        _toSlideQueue = new Queue<PuzzlePiece>();
    }

    private void InitQuads()
    {
        Texture2D[,] slices = ImageSlicer.GetSlices(image, size);
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = new Vector3(col - (size / 2f) + 0.5f, row - (size / 2f) + 0.5f, 0f);
                quad.transform.parent = transform;

                PuzzlePiece puzzlePiece = quad.AddComponent<PuzzlePiece>();
                puzzlePiece.OnPuzzlePiecePressed += EnqueuePuzzlePiece;
                puzzlePiece.OnPuzzlePieceFinishedSliding += OnPuzzlePieceFinishedMoving;
                puzzlePiece.Init(new Vector2(col, row), slices[row, col]);

                if (row == 0 && col == size - 1)
                {
                    quad.SetActive(false);
                    _hiddenPiece = puzzlePiece;
                }
            }
        }
    }

    private void EnqueuePuzzlePiece(PuzzlePiece puzzlePiece)
    {
        _toSlideQueue.Enqueue(puzzlePiece);
        SlideNextInQueue();
    }

    private void SlideNextInQueue()
    {
        while (_toSlideQueue.Count > 0 && !_pieceIsSliding)
        {
            MovePuzzlePiece(_toSlideQueue.Dequeue());
        }
    }

    private void MovePuzzlePiece(PuzzlePiece puzzlePiece)
    {
        if ((puzzlePiece.coordinates - _hiddenPiece.coordinates).sqrMagnitude == 1f)
        {
            Vector2 tempCoord = _hiddenPiece.coordinates;
            _hiddenPiece.coordinates = puzzlePiece.coordinates;
            puzzlePiece.coordinates = tempCoord;

            Vector3 target = _hiddenPiece.transform.position;
            _hiddenPiece.transform.position = puzzlePiece.transform.position;
            puzzlePiece.SlideToPosition(target, slideSpeed);
            _pieceIsSliding = true;
        }
    }

    private void OnPuzzlePieceFinishedMoving()
    {
        _pieceIsSliding = false;
        SlideNextInQueue();
    }
}