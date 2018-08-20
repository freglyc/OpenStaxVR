using UnityEngine;

public class ToCToggle : MonoBehaviour {

	// Gets the settings gameobject
	public GameObject ToC;

	public GameObject backGround;

	public Book book;

	// Whether the Google Cardboard user is gazing at this button.
	private bool isLookedAt = false;

	// How long the user can gaze at this before the button is clicked.
	public float timerDuration = 1f;

	// Count time the player has been gazing at the button.
	private float lookTimer = 0f;


	public void SetGazedAt(bool gazedAt) {
		isLookedAt = gazedAt;
	}

	// Use this for initialization
	void Start () {
		ToC.SetActive(book.IsTable);
	}
	
	// Update is called once per frame
	void Update () {
		if (isLookedAt) {

			// Increment the gaze timer.
			lookTimer += Time.deltaTime;

			// Gaze time exceeded limit - button is considered clicked.
			if (lookTimer > timerDuration) {
				lookTimer = 0f;
				book.IsTable = !book.IsTable;

				ToC.SetActive(book.IsTable);

				foreach (Transform child in ToC.transform)
				{
					child.gameObject.SetActive(book.IsTable);

				}
				backGround.SetActive(book.IsTable);
			}
			
		} 
		else {
			lookTimer = 0f;
		}

	}
}
