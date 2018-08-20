using UnityEngine;

public class SettingsToggle : MonoBehaviour {

	// Whether the Google Cardboard user is gazing at this button.
	private bool isLookedAt = false;

	// How long the user can gaze at this before the button is clicked.
	public float timerDuration = 1f;

	// Count time the player has been gazing at the button.
	private float lookTimer = 0f;

	// Current state of the settings
	private bool _isShowing = false;

	public void SetGazedAt(bool gazedAt) {
		isLookedAt = gazedAt;
	}

	private void helper()
	{
		Transform settings = transform.parent.parent.GetChild(1);
		foreach (Transform child in settings)
		{
			if (!child.CompareTag("Music"))
			{
				child.gameObject.SetActive(_isShowing);
			}
			else
			{
				if (!_isShowing)
				{
					child.localScale = new Vector3(0, 0, 0);
				}
				else
				{
					child.localScale = new Vector3(1, 1, 1);
							
				}
			}
			
		}
		
	}

	// Use this for initialization
	void Start () {
		helper();
	}
	
	// Update is called once per frame
	void Update () {
		if (isLookedAt) {

			// Increment the gaze timer.
			lookTimer += Time.deltaTime;

//			gazeTimer.GetComponent<Renderer>().material.SetFloat("_Cutoff", lookTimer / timerDuration);

			// Gaze time exceeded limit - button is considered clicked.
			if (lookTimer > timerDuration) {
				lookTimer = 0f;
				_isShowing = !_isShowing;
				helper();
			}
		} else {
			lookTimer = 0f;
		}

	}
}
