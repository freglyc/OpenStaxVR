using UnityEngine;
using UnityEngine.SceneManagement;

public class Library : MonoBehaviour
{

	// Whether the Google Cardboard user is gazing at this button.
	private bool isLookedAt = false;

	// How long the user can gaze at this before the button is clicked.
	public float timerDuration = 1f;

	// Count time the player has been gazing at the button.
	private float lookTimer = 0f;

	public void SetGazedAt(bool gazedAt)
	{
		isLookedAt = gazedAt;
	}

	// Update is called once per frame
	void Update()
	{
		if (isLookedAt)
		{

			// Increment the gaze timer.
			lookTimer += Time.deltaTime;

//			gazeTimer.GetComponent<Renderer>().material.SetFloat("_Cutoff", lookTimer / timerDuration);

			// Gaze time exceeded limit - button is considered clicked.
			if (lookTimer > timerDuration)
			{
				lookTimer = 0f;


				SceneManager.LoadScene("scene");
			}
		}
		else
		{
			lookTimer = 0f;
		}

	}
}

