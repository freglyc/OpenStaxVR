using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMusic : MonoBehaviour
{
	[NonSerializedAttribute]
	public Boolean IsOn;

	[NonSerializedAttribute]
	public Boolean IsChanged = false;

	public Image StartImage;

	public Sprite Off;
	public Sprite On;
	
	public AudioClip music0;
	
	[NonSerializedAttribute]
	public String MusicChoice = "Classical";

	public AudioSource AudioSource;

	private bool _isPlaying = false;

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
		AudioSource.clip = music0;

	}

	// Update is called once per frame
	void Update()
	{
		if (IsChanged && _isPlaying)
		{
			AudioSource.Stop();
			AudioSource.clip = music0;
			AudioSource.Play();
			IsChanged = false;
		}
		else
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
					_isPlaying = !_isPlaying;
					IsOn = !IsOn;
					if (_isPlaying)
					{

						AudioSource.clip = music0;
						AudioSource.Play();
						if (MusicChoice == "Classical")
						{
							transform.parent.parent.parent.parent.GetChild(1).GetChild(0)
								.GetChild(2).localScale = new Vector3((float)0.03, (float)0.03, 1);
						
						} else if (MusicChoice == "WhiteNoise")
						{
							transform.parent.parent.parent.parent.GetChild(1).GetChild(1)
								.GetChild(2).localScale = new Vector3((float)0.03, (float)0.03, 1);
							
						}
						

					}
					else
					{
						AudioSource.Pause();
						transform.parent.parent.parent.parent.GetChild(1).GetChild(0).GetChild(2)
								.localScale = new Vector3(0, 0, 0);
						transform.parent.parent.parent.parent.GetChild(1).GetChild(1).GetChild(2)
							.localScale = new Vector3(0, 0, 0);
					}

					StartImage.sprite = IsOn ? On : Off;
				}


			}
			else
			{
				lookTimer = 0f;

			}
		}


	}
}