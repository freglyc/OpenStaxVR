using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

	// Gets the settings gameobject
	public GameObject bookCover;

	public string BookName;

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
				transform.parent.localPosition = new Vector3(transform.parent.localPosition.x, 
					(float)(transform.parent.localPosition.y + 3), transform.parent.localPosition.z);
				// The path of the txt file storing game status.
				string currentDir = Application.persistentDataPath;
				string filename = "bookName.txt";
				string fullPath = currentDir + "/" + filename;
				try
				{
					File.WriteAllText(fullPath, BookName);

				}
				catch (IOException e)
				{
					File.Create(fullPath);
					File.WriteAllText(fullPath, BookName);
				}

				SceneManager.LoadScene("scene");
			}
		} else {
			lookTimer = 0f;
		}

	}
}
