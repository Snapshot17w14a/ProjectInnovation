using TMPro;
using UnityEngine;

[RequireComponent(typeof(BlowDetector))]
public class ForgeHandler : MonoBehaviour, ICraftingProcess
{
    private BlowDetector blowDetector;
    [Header("Interaction settings")]
    [SerializeField] [Tooltip("The threshold from which it is considered to be loud enough")] float microphoneThreshold;
    [SerializeField] [Tooltip("The amount the score should increase per second when the needle is in the correct state")] float increaseAmount = 2;
    [SerializeField] [Tooltip("The amount the score should decrease per second when the needle is in the incorrect state")]  float decreaseAmount = 1;
    private float fireIntensity = 0;
    private float score = 0;
    private float maxScore;
    [SerializeField] private float allowedTime = 5;

    [SerializeField] private ForgeProgressDisplay progressDisplay;
    [SerializeField] private TextMeshProUGUI timerDisplay;
    [SerializeField] private TextMeshProUGUI finalScore;

    private bool isTimerStarted = false;
    private float timer = 0;

    private Item forgedItem;
    private bool isForgingDone = false;
    private bool isForgingStarted = false;

    public bool IsProcessDone => isForgingDone;

    private void Start()
    {
        maxScore = ServiceLocator.GetService<CraftingManager>().MaxScorePerProcess;
        blowDetector = GetComponent<BlowDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isForgingStarted) return;
        float micVolume = blowDetector.RMSVolumeValue;
        if (isTimerStarted)
        {
            if (micVolume > microphoneThreshold) fireIntensity += Time.deltaTime;
            else fireIntensity = Mathf.Max(0, fireIntensity - Time.deltaTime);
            score += Time.deltaTime * ((micVolume > microphoneThreshold) ? increaseAmount : -decreaseAmount);
            score = Mathf.Clamp(score, 0, maxScore);
            Debug.Log($"intensity: {fireIntensity}, score: {score}");
            progressDisplay.SetNeedleProgress(score / maxScore);

            timerDisplay.text = timer + "s of " + allowedTime + "s";
        }

        if (!isTimerStarted && micVolume > microphoneThreshold) isTimerStarted = true;
        if (isTimerStarted && timer < allowedTime) timer += Time.deltaTime;
        else if (timer >= allowedTime) DisplayFinalScore();
    }

    public void StartProcess(ref Item item)
    {
        forgedItem = item;
        isForgingStarted = true;
    }

    void DisplayFinalScore()
    {
        isTimerStarted = false;
        isForgingStarted = false;
        finalScore.text = "Score: " + Mathf.RoundToInt(score);
        forgedItem.SetForgeResult(Mathf.RoundToInt(score));
        DestroyImmediate(blowDetector.gameObject);
        isForgingDone = true;
    }
}
