using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Buttons{
	towardPlayer, awayPlayer, rotateTopBook, rotateBotBook, toIntro,
	toggleHide, toggleMusic
 }

public class BookSettings : MonoBehaviour {

	// Gets the book
	public GameObject BookInterface;
	public Book Book;

	private Boolean _isHiden = false;
	// The Gameobjects to be activated.
	private bool[,] recover = new bool[7, 21];
	
	// Button choices
	public Buttons current;

	// How long the user can gaze at this before the button is clicked.
	public float timerDuration = 1f;

	// Count time the player has been gazing at the button.
	private float lookTimer = 0f;

	// Whether the Google Cardboard user is gazing at this button.
	private bool isLookedAt = false;

	public void SetGazedAt(bool gazedAt) {
		isLookedAt = gazedAt;
	}


	private void ToggleHide()
	{
		_isHiden = !_isHiden;
		Transform parent = BookInterface.gameObject.transform;
		if (_isHiden)
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform child = parent.GetChild(i);
				for (int j = 0; j < child.childCount; j++)
				{
					Transform grandChild = child.GetChild(j);
					if (grandChild.gameObject.activeSelf && !grandChild.gameObject.CompareTag("Hide"))
					{
						if (!grandChild.gameObject.CompareTag("Music"))
						{
							recover[i, j] = true;
							grandChild.gameObject.SetActive(false);
						}
						else
						{
							grandChild.gameObject.transform.localScale = new Vector3(0, 0, 0);
						}
					}
				}
								
			}

		}
		else
		{
			parent.GetChild(1).GetChild(5).transform.localScale = new Vector3(1, 1, 1);
			for (int i = 0; i < parent.childCount; i++)
			{
				for (int j = 0; j < parent.GetChild(i).childCount; j++)
				{
					if (recover[i, j] == true)
					{
						parent.GetChild(i).GetChild(j).gameObject.SetActive(true);
						recover[i, j] = false;
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {

		// Player looking at the buttons
		if(isLookedAt){

			// Increment the gaze timer.
			lookTimer += Time.deltaTime;

			// Gaze time exceeded limit - button is considered clicked.
			if (lookTimer > timerDuration) {
				lookTimer = 0f;

				switch (current){
					case Buttons.towardPlayer: // Moves the book toward the user
						BookInterface.transform.position += new Vector3(-1,0,0);
						break;
					case Buttons.awayPlayer: // Moves the book away from the user
						BookInterface.transform.position += new Vector3(1,0,0);
						break;
					case Buttons.rotateTopBook: // Rotates the top of the book toward the user
						BookInterface.transform.Rotate(-10,0,0);
						break;
					case Buttons.rotateBotBook: // Rotates the bottom of the book toward the user
						BookInterface.transform.Rotate(10,0,0);
						break;
					case Buttons.toIntro:
//					    Book.UnloadBundles(Book);
						SceneManager.LoadScene("intro");
						break;
					case Buttons.toggleHide:
						ToggleHide();
						break;
					case Buttons.toggleMusic:
						break;
				}
			}
		} else {
			lookTimer = 0f;
		}
	}
}
