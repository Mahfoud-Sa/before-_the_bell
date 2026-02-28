using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    [SerializeField] AudioSource bellSound; // Add this to play the bell sound
    [SerializeField] bool soundPlayed = false; // To ensure sound only plays once

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }

        // Check if timer has reached zero and sound hasn't been played yet
        if (remainingTime <= 0 && !soundPlayed)
        {
            remainingTime = 0;
         

            // Play the bell sound if AudioSource is assigned
            if (bellSound != null)
            {
                bellSound.Play();
                soundPlayed = true; // Mark sound as played to prevent repeated playing
            }
        }

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
