using UnityEngine;
using System.Collections;

public class CPlayerCollision : MonoBehaviour {

	public const string TAG_ENEMY = "Enemy";
	public const string TAG_ITEM_SMALL = "Item_small";
	public const string TAG_ITEM_BIG = "Item_big";

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(TAG_ENEMY))
		{
			CGameManager.Instance.on_enemy(other.gameObject);
		}
		else if (other.CompareTag(TAG_ITEM_SMALL))
		{
			CGameManager.Instance.on_item(other.gameObject, false);
		}
		else if (other.CompareTag(TAG_ITEM_BIG))
		{
			CGameManager.Instance.on_item(other.gameObject, true);
		}
	}
}
