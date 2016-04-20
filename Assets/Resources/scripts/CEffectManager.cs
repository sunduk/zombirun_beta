using UnityEngine;
using System.Collections;

public class CEffectManager : MonoBehaviour
{
	[SerializeField]
	GameObject donuts_small_ef_prefab;

	[SerializeField]
	GameObject donuts_big_ef_prefab;

	CGameObjectPool<GameObject> donuts_small_pool;
	CGameObjectPool<GameObject> donuts_big_pool;


	void Awake()
	{
		this.donuts_small_pool = new CGameObjectPool<GameObject>(3, this.donuts_small_ef_prefab,
			(GameObject original) =>
			{
				GameObject obj = GameObject.Instantiate(original) as GameObject;
				obj.SetActive(false);
				return obj;
			});

		this.donuts_big_pool = new CGameObjectPool<GameObject>(3, this.donuts_big_ef_prefab,
			(GameObject original) =>
			{
				GameObject obj = GameObject.Instantiate(original) as GameObject;
				obj.SetActive(false);
				return obj;
			});
	}


	public void play_donuts_effect(Vector3 position, bool is_big)
	{
		StartCoroutine(run_donuts_effect(position, is_big));
	}


	IEnumerator run_donuts_effect(Vector3 position, bool is_big)
	{
		CGameObjectPool<GameObject> pool;
		GameObject obj;
		if (is_big)
		{
			pool = this.donuts_big_pool;
		}
		else
		{
			pool = this.donuts_small_pool;
		}

		obj = pool.pop();
		obj.SetActive(true);
		position.y = 3.8f;
		obj.transform.position = position;

		yield return new WaitForSeconds(2.0f);
		obj.SetActive(false);
		pool.push(obj);
	}
}
