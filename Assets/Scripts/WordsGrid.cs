using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsGrid : MonoBehaviour
{

    public GameData currentGameData;
    public GameObject gridsSquarePrefab;
    public AlphabetData alphabetData;

    public float squareOffset = 0.0f;
    public float topPosition;

    private List<GameObject> _squareList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void SpawnGridSquares()
    {
        if (currentGameData != null)
        {
            var squareScale = GetSquareScale(new Vector3(1.5f, 1.5f, 0.1f));
            foreach(var squares in currentGameData.selectedBoardData.Board)
            {
                foreach(var squareLetter in squares.Row)
                {
                    var normalLetterData = alphabetData.AlphabetNormal.Find(data => data.letter == squareLetter);
                    var selectedLetterData = alphabetData.AlphabetHighlighted.Find(data => data.letter == squareLetter);
                    var correctLetterData = alphabetData.AlphabetWrong.Find(data => data.letter == squareLetter);

                    if(normalLetterData.image == null || selectedLetterData.image == null || correctLetterData.image == null)
                    {
                        Debug.LogError("All fields in your array should have some letters. Press Fill up with random");
                        #if UNITY_EDITOR
                        if (UnityEditor.EditorApplication.isPlaying)
                        {
                            UnityEditor.EditorApplication.isPlaying = false;
                        }
#endif
                    }
                    else
                    {
                        _squareList.Add(Instantiate(gridsSquarePrefab));
                    }
                }
            }
        }
               
    }
    private Vector3 GetSquareScale(Vector3 defaultScale)
    {
        var finalScale = defaultScale;
        var adjusment = 0.01f;

        while (ShouldScaleDown(finalScale))
        {
            finalScale.x -= adjusment;
            finalScale.y -= adjusment;

            if(finalScale.x <=0 || finalScale.y <= 0)
            {
                finalScale.x = adjusment;
                finalScale.y = adjusment;
                return finalScale;
            }
        }
        return finalScale; 
    }

    private bool ShouldScaleDown(Vector3 targetScale)
    {
        var squareRect = gridsSquarePrefab.GetComponent<SpriteRenderer>().sprite.rect;
        var squareSize = new Vector2(0f, 0f);
        var startPosition = new Vector2(0f, 0f);

        squareSize.x = (squareRect.width * targetScale.x) + squareOffset;
        squareSize.y = (squareRect.height * targetScale.y) + squareOffset;

        var midWidthPosition = ((currentGameData.selectedBoardData.Columns * squareSize.x) / 2) * 0.01f;
        var midWidthHeight = ((currentGameData.selectedBoardData.Rows * squareSize.y) / 2) * 0.01f;

        startPosition.x = (midWidthPosition != 0) ? midWidthPosition * -1 : midWidthPosition;
        startPosition.y = (midWidthHeight);

        return (startPosition.x < GetHalfScreenWidth() * -1 || startPosition.y > topPosition);

    }

    private float GetHalfScreenWidth()
    {
        float height = Camera.main.orthographicSize * 2;
        float width = (1.7f * height) * Screen.width / Screen.height;
        return width / 2;
    }
}
