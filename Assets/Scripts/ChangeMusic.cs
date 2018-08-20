using UnityEngine;

public class ChangeMusic : MonoBehaviour
{
	
	public ToggleMusic MusicButton;

	public AudioClip MusicChoice;

	public string name;
	
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

	private Transform getNext(Transform current)
	{
		return current.parent.GetSiblingIndex() == 0
			? current.parent.parent.GetChild(1)
			: current.parent.parent.GetChild(0);
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
				MusicButton.music0 = MusicChoice;
				MusicButton.IsChanged = true;
				MusicButton.MusicChoice = name;
				transform.parent.GetChild(2).localScale = new Vector3((float)0.03, (float)0.03, 1);
				getNext(transform).GetChild(2).localScale = new Vector3(0, 0, 0);
			}
		}
		else 
		{
			lookTimer = 0f;
			
		}
	}
}
