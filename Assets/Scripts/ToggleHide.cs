using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleHide : MonoBehaviour
{
    public Boolean IsOn;

    public Image StartImage;

    public Sprite Off;
    public Sprite On;

    // How long the user can gaze at this before the button is clicked.
    public float timerDuration = 1f;

    // Count time the player has been gazing at the button.
    private float lookTimer = 0f;

    // Whether the Google Cardboard user is gazing at this button.
    private bool isLookedAt = false;

    public void SetGazedAt(bool gazedAt)
    {
        isLookedAt = gazedAt;
    }


    // Use this for initialization
    void Start()
    {
        StartImage.sprite = IsOn ? On : Off;
    }

    // Update is called once per frame
    void Update()
    {
        // Player looking at the buttons
        if (isLookedAt)
        {
            // Increment the gaze timer.
            lookTimer += Time.deltaTime;

            // Gaze time exceeded limit - button is considered clicked.
            if (lookTimer > timerDuration)
            {
                lookTimer = 0f;
                IsOn = !IsOn;
                StartImage.sprite = IsOn ? On : Off;
            }
        }
        else 
        {
            lookTimer = 0f;
			
        }
    }
}