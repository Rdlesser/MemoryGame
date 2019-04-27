using UnityEngine;

public class SoundManager : MonoBehaviour
{

	[SerializeField] private AudioClip cardFlipSound;
	
	private AudioSource source
	{
		get { return GetComponent<AudioSource>(); }
	}

	private void Awake()
	{
		gameObject.AddComponent<AudioSource>();
	}

	public void OnCardFlipped()
	{
		source.clip = cardFlipSound;
		source.playOnAwake = false;
		source.PlayOneShot(cardFlipSound);
	}
}