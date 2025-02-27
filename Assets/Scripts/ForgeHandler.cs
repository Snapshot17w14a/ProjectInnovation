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

    // Update is called once per frame
    void Update()
    {
        float micVolume = blowDetector.RMSVolumeValue;
        if (micVolume > microphoneThreshold) fireIntensity += Time.deltaTime;
        else fireIntensity = Mathf.Max(0, fireIntensity - Time.deltaTime);
        score += Time.deltaTime * ((micVolume > microphoneThreshold) ? increaseAmount : -decreaseAmount);
        score = Mathf.Clamp(score, 0, maxScore);
        Debug.Log($"intensity: {fireIntensity}, score: {score}");
        progressDisplay.SetNeedleProgress(score / maxScore);
    }
}
