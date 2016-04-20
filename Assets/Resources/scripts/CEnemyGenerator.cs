using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEnemyGenerator : MonoBehaviour {

	[SerializeField]
	GameObject[] prefab_enemies;

	List<CGameObjectPool<GameObject>> enemy_pools;
	List<GameObject> live_objects;

	CLevelData data_current_level;


	void Awake()
	{
		this.live_objects = new List<GameObject>();
		this.enemy_pools = new List<CGameObjectPool<GameObject>>();
		for (int i = 0; i < this.prefab_enemies.Length; ++i)
		{
			CGameObjectPool<GameObject> pool = new CGameObjectPool<GameObject>(10,
				this.prefab_enemies[i],
			(GameObject original) =>
			{
				GameObject inst = GameObject.Instantiate(original);
				inst.transform.parent = transform;
				inst.SetActive(false);
				return inst;
			});

			this.enemy_pools.Add(pool);
		}
	}


	public void restart()
	{
		StartCoroutine(generator());
		StartCoroutine(dead_enemy_loop());
	}


	IEnumerator dead_enemy_loop()
	{
		while (true)
		{
			restore_dead_enemy_to_pool();
			yield return 0;
		}
	}


	IEnumerator generator()
	{
		while (true)
		{
			spawn();
			yield return new WaitForSeconds(this.data_current_level.gap);
		}
	}


	void spawn()
	{
		int enemy_index = UnityEngine.Random.Range(0, this.prefab_enemies.Length);
		GameObject inst = this.enemy_pools[enemy_index].pop();
		inst.GetComponent<CEnemy>().update_pool_index(enemy_index);
		CMovableObject movable_object = inst.GetComponent<CMovableObject>();
		movable_object.enabled = true;
		movable_object.update_speed_ratio(this.data_current_level.speed_ratio);
		inst.SetActive(true);
		inst.GetComponent<Rigidbody>().useGravity = false;
		inst.GetComponent<Rigidbody>().GetComponent<Rigidbody>().isKinematic = true;
		inst.GetComponent<CRotationObject>().enabled = false;

		if (inst.CompareTag(CPlayerCollision.TAG_ITEM_BIG) ||
			inst.CompareTag(CPlayerCollision.TAG_ITEM_SMALL))
		{
			inst.GetComponent<CRotationObject>().enabled = true;
		}

		int rnd = UnityEngine.Random.Range(0, 2);
		if (rnd == 0)
		{
			inst.transform.localPosition = new Vector3(2.5f, 0.4f, 0.0f);
		}
		else
		{
			inst.transform.localPosition = new Vector3(-3.0f, 0.4f, 0.0f);
		}
		inst.transform.localRotation = Quaternion.AngleAxis(180.0f, Vector3.up);
		this.live_objects.Add(inst);
	}


	void restore_dead_enemy_to_pool()
	{
		List<int> targets = new List<int>();
		for (int i = 0; i < this.live_objects.Count; ++i)
		{
			if (this.live_objects[i].transform.localPosition.z > -150.0f)
			{
				break;
			}


			if (this.live_objects[i].transform.localPosition.y > -10.0f)
			{
				this.live_objects[i].GetComponent<CMovableObject>().update_speed_ratio(0.2f);
				this.live_objects[i].GetComponent<Rigidbody>().useGravity = true;
				this.live_objects[i].GetComponent<Rigidbody>().isKinematic = false;
				this.live_objects[i].GetComponent<CRotationObject>().enabled = true;
			}
			else
			{
				restore_to_pool(this.live_objects[i]);
				targets.Add(i);
			}
		}


		for (int i = 0; i < targets.Count; ++i)
		{
			this.live_objects.RemoveAt(targets[i]);
		}
	}


	void restore_to_pool(GameObject obj)
	{
		int pool_index = obj.GetComponent<CEnemy>().pool_index;
		this.enemy_pools[pool_index].push(obj);
		obj.SetActive(false);
	}


	public void update_level(CLevelData level_data)
	{
		this.data_current_level = level_data;

		for (int i = 0; i < this.live_objects.Count; ++i)
		{
			this.live_objects[i].GetComponent<CMovableObject>().update_speed_ratio(level_data.speed_ratio);
		}
	}


	public void pause()
	{
		StopAllCoroutines();
		for (int i = 0; i < this.live_objects.Count; ++i)
		{
			this.live_objects[i].GetComponent<CMovableObject>().enabled = false;
		}
	}


	public void on_player_eat_item(GameObject item)
	{
		restore_to_pool(item);
		this.live_objects.Remove(item);
	}


	public void destroy_all()
	{
		for (int i = 0; i < this.live_objects.Count; ++i)
		{
			restore_to_pool(this.live_objects[i]);
		}

		this.live_objects.Clear();
	}
}
