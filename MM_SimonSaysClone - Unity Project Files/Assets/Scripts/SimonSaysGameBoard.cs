using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonSaysGameBoard : MonoBehaviour
{
    private PlayerInputHandler playerInputController;
    private bool hasFailed;

    private SimonSaysCube[] simonCubes;
    private SimonSaysCube nextCube;
    private int numberOfCubes;
    
    public int StartingSequenceLength = 2;
    private int currentSequenceLength;
    private int nextIndexToCheck;
    private int[] currentSequence;
    private int numberOfPlayerInputsReceived;
    private float delayBeforeNewSequencePlays = 1f;
    private int playerScore = 0;

    [Header("UI Components")]
    public GameObject StartScreenGO;
    public GameObject GameOverScreenGO;
    public Text ScoreText;
    public Text GameOverScoreText;
    private string scoreTextBase = "SCORE: ";

    private void Awake()
    {
        SetupGameBoard();

        playerInputController = GetComponent<PlayerInputHandler>();

        playerInputController.SetCanClick(false);
        playerInputController.SetCanType(true);
    }

    private void SetupGameBoard()
    {
        if (StartScreenGO == null)
        {
            Debug.LogError(name + ": Must set StartScreenGO.");
        }

        StartScreenGO.SetActive(true);

        if (GameOverScreenGO == null)
        {
            Debug.LogError(name + ": Must set GameOverScreenGO.");
        }

        GameOverScreenGO.SetActive(false);

        if (ScoreText == null)
        {
            Debug.LogError(name + ": Must set ScoreText.");
        }

        playerScore = 0;
        UpdateScoreText();
        ScoreText.gameObject.SetActive(true);

        simonCubes = GetComponentsInChildren<SimonSaysCube>();

        if (simonCubes.Length <= 0)
        {
            Debug.LogError(name + ": No SimonSaysCube components found in children.");
        }

        for (int i = 0; i < simonCubes.Length; ++i)
        {
            simonCubes[i].SetIndex(i);
        }

        numberOfCubes = simonCubes.Length;

        if (StartingSequenceLength <= 0)
        {
            Debug.LogError(name + ": StartingSequenceLength must be greater than zero.");
        }

        currentSequenceLength = StartingSequenceLength;
    }

    public void StartNewGame()
    {
        currentSequenceLength = StartingSequenceLength;
        playerScore = 0;
        UpdateScoreText();
        ScoreText.gameObject.SetActive(true);

        StartScreenGO.SetActive(false);
        GameOverScreenGO.SetActive(false);

        playerInputController.SetCanClick(false);
        playerInputController.SetCanType(false);

        StartCoroutine(PlaySequenceRoutine());
    }

    private void GenerateSequence()
    {
        currentSequence = new int[currentSequenceLength];

        for (int i = 0; i < currentSequenceLength; ++i)
        {
            currentSequence[i] = Random.Range(0, numberOfCubes);
        }
    }

    private IEnumerator PlaySequenceRoutine()
    {
        yield return new WaitForSeconds(delayBeforeNewSequencePlays);

        playerInputController.SetCanClick(false);
        GenerateSequence();

        for (int i = 0; i < currentSequenceLength; ++i)
        {
            nextCube = simonCubes[currentSequence[i]];

            yield return StartCoroutine(nextCube.SetActiveRoutine());
        }

        StartCoroutine(WaitForAnswerRoutine());
    }

    private IEnumerator WaitForAnswerRoutine()
    {
        nextIndexToCheck = 0;
        numberOfPlayerInputsReceived = 0;
        playerInputController.SetCanClick(true);

        while (numberOfPlayerInputsReceived < currentSequenceLength)
        {
            yield return null;
        }

        //Player finished sequence -- advance
        playerInputController.SetCanClick(false);
        ++playerScore;
        UpdateScoreText();
        currentSequenceLength++;
        StartCoroutine(PlaySequenceRoutine());
    }

    public void RegisterInput(int cubeIndex)
    {
        numberOfPlayerInputsReceived++;

        if (currentSequence[nextIndexToCheck] == cubeIndex)
        {
            //correct -- continue
            nextIndexToCheck++;
        }
        else
        {
            //incorrect -- game over
            StopAllCoroutines();
            ScoreText.gameObject.SetActive(false);
            GameOverScreenGO.SetActive(true);
            playerInputController.SetCanClick(false);
            playerInputController.SetCanType(true);
        }
    }

    private void UpdateScoreText()
    {
        ScoreText.text = scoreTextBase + playerScore;
        GameOverScoreText.text = scoreTextBase + playerScore;
    }
}
