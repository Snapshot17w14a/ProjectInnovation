using TMPro;
using UnityEngine;

public class ForgeHandler : MonoBehaviour
{
    [SerializeField] BlowDetector blowDetector;
    [SerializeField] float microphoneThreshold;
    [SerializeField] float increaseAmount = 2;
    [SerializeField] float decreaseAmount = 1;
    private float fireIntensity = 0;
    private float score = 0;
    [SerializeField] private float maxScore = 5;

    [SerializeField] private ForgeProgressDisplay progressDisplay;
    [SerializeField] private TextMeshProUGUI timerDisplay;
    [SerializeField] private TextMeshProUGUI finalScore;

    private bool isTimerStarted = false;
    [SerializeField] private float allowedTime = 5;
    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
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

    void DisplayFinalScore()
    {
        isTimerStarted = false;
        finalScore.text = "Score: " + Mathf.RoundToInt(score);
    }
}
