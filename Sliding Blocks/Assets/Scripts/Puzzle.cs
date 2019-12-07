using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public Texture2D image;
    public int size = 4;
    public int shuffleMoves = 50;
    public float slideSpeed = 8f;
    public float shuffleSpeed = 24f;

    private enum PuzzleState
    {
        Solved,
        Shuffling,
        InPlay
    };

    private PuzzleState _state;
    private PuzzlePiece[,] _puzzle;
    private PuzzlePiece _hiddenPiece;
    private Queue<PuzzlePiece> _toSlideQueue;
    private bool _pieceIsSliding;

    private void Start()
    {
        _puzzle = new PuzzlePiece[size, size];
        _toSlideQueue = new Queue<PuzzlePiece>();

        InitQuads();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _state == PuzzleState.Solved)
        {
            StartCoroutine(RandomShuffle());
        }
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
                puzzlePiece.Init(new Vector2Int(x, y), slices[y, x]);
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
        if (_state == PuzzleState.InPlay)
        {
            _toSlideQueue.Enqueue(puzzlePiece);
            SlideNextInQueue();
        }
    }

    private void SlideNextInQueue()
    {
        while (_toSlideQueue.Count > 0 && !_pieceIsSliding)
        {
            SlidePuzzlePiece(_toSlideQueue.Dequeue());
        }
    }

    private void SlidePuzzlePiece(PuzzlePiece puzzlePiece)
    {
        if ((puzzlePiece.coordinates - _hiddenPiece.coordinates).sqrMagnitude == 1f)
        {
            _puzzle[puzzlePiece.coordinates.x, puzzlePiece.coordinates.y] = _hiddenPiece;
            _puzzle[_hiddenPiece.coordinates.x, _hiddenPiece.coordinates.y] = puzzlePiece;

            Vector2Int tempCoord = _hiddenPiece.coordinates;
            _hiddenPiece.coordinates = puzzlePiece.coordinates;
            puzzlePiece.coordinates = tempCoord;

            Vector3 target = _hiddenPiece.transform.position;
            _hiddenPiece.transform.position = puzzlePiece.transform.position;

            if (_state == PuzzleState.Shuffling)
            {
                puzzlePiece.SlideToPosition(target, shuffleSpeed);
            }
            else if (_state == PuzzleState.InPlay)
            {
                puzzlePiece.SlideToPosition(target, slideSpeed);
            }
            
            _pieceIsSliding = true;
        }
    }

    private void OnPuzzlePieceFinishedMoving()
    {
        _pieceIsSliding = false;

        if (_state == PuzzleState.InPlay && IsSolved())
        {
            ShowHiddenPiece();
            _state = PuzzleState.Solved;
        }
        else
        {
            SlideNextInQueue();
        }
    }

    private IEnumerator RandomShuffle()
    {
        _state = PuzzleState.Shuffling;

        _hiddenPiece.gameObject.SetActive(false);
        int x = _hiddenPiece.coordinates.x;
        int y = _hiddenPiece.coordinates.y;
        int prevX = -1;
        int prevY = -1;

        for (int i = 0; i < shuffleMoves; i++)
        {
            PuzzlePiece toSlide = GetRandomAdjecentPiece(x, y, prevX, prevY);

            prevX = x;
            prevY = y;
            x = toSlide.coordinates.x;
            y = toSlide.coordinates.y;

            SlidePuzzlePiece(toSlide);

            while (_pieceIsSliding)
            {
                yield return null;
            }
        }

        _state = PuzzleState.InPlay;
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
        if (prevX == randomPiece.coordinates.x && prevY == randomPiece.coordinates.y)
        {
            candidates.Remove(randomPiece);
            return candidates[Random.Range(0, candidates.Count)];
        }

        return randomPiece;
    }

    private bool IsSolved()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if ((_puzzle[x, y].coordinates - _puzzle[x, y].originalCoordinates).magnitude != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void ShowHiddenPiece()
    {
        _hiddenPiece.gameObject.SetActive(true);
    }
}