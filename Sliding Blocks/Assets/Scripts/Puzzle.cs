using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public Texture2D image;
    public int size = 4;
    public int numberOfShuffleMoves = 50;
    public float slideSpeed = 3f;

    private PuzzlePiece[,] _puzzle;
    private PuzzlePiece _hiddenPiece;
    private Queue<PuzzlePiece> _toSlideQueue;
    private bool _pieceIsSliding;

    private void Start()
    {
        _puzzle = new PuzzlePiece[size, size];
        _toSlideQueue = new Queue<PuzzlePiece>();

        InitQuads();
        StartCoroutine(Shuffle());
    }

    private void InitQuads()
    {
        Texture2D[,] slices = ImageSlicer.GetSlices(image, size);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.position = new Vector3(x - (size / 2f) + 0.5f, y - (size / 2f) + 0.5f, 0f);
                quad.transform.parent = transform;

                PuzzlePiece puzzlePiece = quad.AddComponent<PuzzlePiece>();
                puzzlePiece.OnPuzzlePiecePressed += EnqueuePuzzlePiece;
                puzzlePiece.OnPuzzlePieceFinishedSliding += OnPuzzlePieceFinishedMoving;
                puzzlePiece.Init(new Vector2(x, y), slices[y, x]);
                _puzzle[x, y] = puzzlePiece;

                if (y == 0 && x == size - 1)
                {
                    quad.SetActive(false);
                    _hiddenPiece = puzzlePiece;
                }
            }
        }

        Camera.main.orthographicSize = size * 0.55f;
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
            PuzzlePiece temp = _puzzle[(int)puzzlePiece.coordinates.x, (int)puzzlePiece.coordinates.y];
            _puzzle[(int)puzzlePiece.coordinates.x, (int)puzzlePiece.coordinates.y] = _puzzle[(int)_hiddenPiece.coordinates.x, (int)_hiddenPiece.coordinates.y];
            _puzzle[(int)_hiddenPiece.coordinates.x, (int)_hiddenPiece.coordinates.y] = temp;

            Vector2 tempCoord = _hiddenPiece.coordinates;
            _hiddenPiece.coordinates = puzzlePiece.coordinates;
            puzzlePiece.coordinates = tempCoord;

            Vector3 target = _hiddenPiece.transform.position;
            _hiddenPiece.transform.position = puzzlePiece.transform.position;
            puzzlePiece.SlideToPosition(target, slideSpeed);
            _pieceIsSliding = true;
        }
    }

    private IEnumerator Shuffle()
    {
        int x = (int)_hiddenPiece.coordinates.x;
        int y = (int)_hiddenPiece.coordinates.y;
        int prevX = -1;
        int prevY = -1;

        for (int i = 0; i < numberOfShuffleMoves; i++)
        {
            PuzzlePiece toSlide = GetRandomAdjecentPiece(x, y, prevX, prevY);

            prevX = x;
            prevY = y;
            x = (int)toSlide.coordinates.x;
            y = (int)toSlide.coordinates.y;

            EnqueuePuzzlePiece(toSlide);

            while (_toSlideQueue.Count > 0 || _pieceIsSliding)
            {
                yield return null;
            }
        }
    }

    private void OnPuzzlePieceFinishedMoving()
    {
        _pieceIsSliding = false;
        SlideNextInQueue();
    }

    private PuzzlePiece GetRandomAdjecentPiece(int x, int y, int prevX, int prevY)
    {
        List<PuzzlePiece> candidates = new List<PuzzlePiece>();

        if (x > 0)
        {
            candidates.Add(_puzzle[x - 1, y]);
        }

        if (x < size - 1)
        {
            candidates.Add(_puzzle[x + 1, y]);
        }

        if (y > 0)
        {
            candidates.Add(_puzzle[x, y - 1]);
        }

        if (y < size - 1)
        {
            candidates.Add(_puzzle[x, y + 1]);
        }

        PuzzlePiece randomPiece = candidates[Random.Range(0, candidates.Count)];
        if (prevX == (int)randomPiece.coordinates.x && prevY == (int)randomPiece.coordinates.y)
        {
            candidates.Remove(randomPiece);
            return candidates[Random.Range(0, candidates.Count)];
        }

        return randomPiece;
    }
}