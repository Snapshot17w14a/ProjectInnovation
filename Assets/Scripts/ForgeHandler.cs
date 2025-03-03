using UnityEngine;
using TMPro;

[RequireComponent(typeof(BlowDetector))]
public class ForgeHandler : CraftingProcess, ICraftingProcess
{
    private BlowDetector blowDetector;
    [Header("Interaction settings")]
    [SerializeField] [Tooltip("The threshold from which it is considered to be loud enough")] float microphoneThreshold;
    [SerializeField] [Tooltip("The amount the score should increase per second when the needle is in the correct state")] float increaseAmount = 2;
    [SerializeField] [Tooltip("The amount the score should decrease per second when the needle is in the incorrect state")] float decreaseAmount = 1;
    [SerializeField] [Tooltip("The amount the needle should rise per second when the volume is high enough")] private float activeIncrease = 1;
    [SerializeField] [Tooltip("The amount the needle should fall per second when the volume is not high enough")] private float activeDecrease = 0.5f;

    private float score = 0;
    private float itemScore = 0;
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
        var craftingManager = ServiceLocator.GetService<CraftingManager>();
        maxScore = craftingManager != null ? craftingManager.MaxScorePerProcess : 5;
        blowDetector = GetComponent<BlowDetector>();

        forgedItem = new Item();
        StartProcess(ref forgedItem);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isForgingStarted) return;

        float micVolume = blowDetector.RMSVolumeValue;

        if (!isTimerStarted && micVolume > microphoneThreshold) isTimerStarted = true;

        if (isTimerStarted)
        {
            //Change score depending on volume
            score += Time.deltaTime * ((micVolume > microphoneThreshold) ? activeIncrease : -activeDecrease);
            score = Mathf.Clamp(score, 0, maxScore);

            //Increase item score when in the correct spot
            itemScore = Mathf.Clamp(itemScore + ((progressDisplay.IsNeedlePositionCorrect() ? increaseAmount : -decreaseAmount) * Time.deltaTime), 0, maxScore);
            Debug.Log($"score: {score}, itemScore: {itemScore}");

            //Display values on UI
            progressDisplay.SetNeedleProgress(score / maxScore);
            progressDisplay.DisplayMicrophoneVolume(micVolume, microphoneThreshold);
            progressDisplay.UpdateFireParticles(micVolume, microphoneThreshold);

            timerDisplay.text = timer + "s of " + allowedTime + "s";

            //Keep track of passed time and end when over alowed time
            if (timer < allowedTime) timer += Time.deltaTime;
            else if (timer >= allowedTime) DisplayFinalScore();
        }
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
        finalScore.text = "Score: " + Mathf.RoundToInt(itemScore);
        forgedItem.SetForgeResult(Mathf.RoundToInt(itemScore));
        DestroyImmediate(blowDetector.gameObject);
        isForgingDone = true;
    }
}
