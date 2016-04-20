using UnityEngine;
using System.Collections;

public class CSoundManager : CSingletonMonobehaviour<CSoundManager>
{
	[SerializeField]
	AudioSource item_get;


	public void play_on_item()
	{
		this.item_get.Play();
	}
}
