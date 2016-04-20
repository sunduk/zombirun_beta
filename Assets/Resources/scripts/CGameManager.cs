using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
 * 게임의 요소들(Implemented elements).
 * 1. 무한 맵 스크롤(Endless map scroll).
 * 2. 적군 및 아이템 등장과 이동(Spawn enemies, items).
 * 3. 플레이어 조작(Player control).
 * 4. 게임 오버와 재시작(Gameover restart).
 * */


public class CGameManager : CSingletonMonobehaviour<CGameManager> {

	// 테이블 데이터(Table data).
	List<CLevelData> levels;


	// 게임 플레이 관련 변수(Variables about game playing).
	int current_level_index;
	CLevelData data_current_level;
	int score;
	int best_score;
	CEffectManager effect_manager;


	// 레벨링 데이터에 영향을 받는 변수들(Influence of level data).
	[SerializeField]
	CRoadScroller road_scroller;

	[SerializeField]
	CEnemyGenerator enemy_generator;

	[SerializeField]
	CPlayerMovement player_movement;


	void Awake()
	{
		hardcoding_table_data();
		this.effect_manager = gameObject.GetComponent<CEffectManager>();
	}


	void Start()
	{
		restart();
	}


	void hardcoding_table_data()
	{
		this.levels = new List<CLevelData>();

		/*
		 * 밸런스 조절용 파라미터 설명.
		 * CLevelData(레벨 플레이 시간, 좀비 이동 속도 배율, 좀비 등장 간격);
		 * 
		 *   레벨 플레이 시간동안 현재 레벨을 지속함.
		 *   시간이 다 지나면 다음 레벨로 자동으로 넘어감.
		 *   다음 레벨이 없을 경우 현재 레벨이 최대 레벨이 됨.
		 * 
         * Parameters about levels.
         * CLevelData(playing time, speed ratio of enemy, emeny interval)
         *   Keep current level during level time.
         *   Move to next level automatically when over the level time.
         *   If next level is empty, the current level is max level.
		 * */

		this.levels.Add(new CLevelData(10, 1.25f, 0.3f));
		this.levels.Add(new CLevelData(10, 1.25f, 0.2f));
		this.levels.Add(new CLevelData(10, 1.25f, 0.15f));

		this.levels.Add(new CLevelData(10, 1.3f, 0.3f));
		this.levels.Add(new CLevelData(10, 1.3f, 0.2f));
		this.levels.Add(new CLevelData(10, 1.3f, 0.15f));
	}


	void restart()
	{
		reset_playing_datas();

		StopAllCoroutines();
		StartCoroutine(game_loop());

		this.road_scroller.restart();
		this.enemy_generator.restart();
		this.player_movement.restart();
	}


	void reset_playing_datas()
	{
		this.current_level_index = 0;
		refresh_current_level();

		this.score = 0;
		apply_level_data();
	}


	void refresh_current_level()
	{
		this.data_current_level = this.levels[this.current_level_index];
	}


	void level_up()
	{
		if (this.current_level_index < this.levels.Count - 1)
		{
			++this.current_level_index;
			refresh_current_level();

			apply_level_data();
		}
	}


	/// <summary>
	/// 레벨이 변화할 때 마다 처리해야 할 내용들.
    /// Run when level changed.
	/// </summary>
	void apply_level_data()
	{
		this.road_scroller.update_speed_ratio(this.data_current_level.speed_ratio);
		this.enemy_generator.update_level(this.data_current_level);
		this.player_movement.update_speed_ratio(this.data_current_level.speed_ratio);
	}


	IEnumerator game_loop()
	{
		float begin = Time.time;

		while (true)
		{
			if (Time.time - begin >= this.data_current_level.goal)
			{
				begin = Time.time;
				level_up();
			}

			yield return 0;
		}
	}


	public void on_item(GameObject item, bool is_big)
	{
		this.enemy_generator.on_player_eat_item(item);
		this.effect_manager.play_donuts_effect(this.player_movement.transform.position, is_big);
		CSoundManager.Instance.play_on_item();

		if (is_big)
		{
			this.score += 3;
		}
		else
		{
			++this.score;
		}
		if (this.score >= this.best_score)
		{
			this.best_score = this.score;
		}
	}


	public void on_enemy(GameObject enemy)
	{
		gameover();
	}


	void gameover()
	{
		// 게임 루프 중지.
        // Stop game loop.
		StopAllCoroutines();

		// 도로 스크롤 중지.
        // Stop to scroll the road.
		this.road_scroller.pause();

		// 적군 생성 및 이동 중지.
        // Stop to generate enemy.
		this.enemy_generator.pause();

		// 플레이어 조작 중지.
        // Stop player movement.
		this.player_movement.pause();

		StartCoroutine(run_gameover_action());
	}


	IEnumerator run_gameover_action()
	{
		yield return new WaitForSeconds(2.0f);

		this.enemy_generator.destroy_all();
		restart();
	}


	void OnGUI()
	{
		GUI.Button(new Rect(0, 0, 100, 100), string.Format("level {0}\nScore {1}\nBest {2}", 
			(this.current_level_index + 1), this.score, this.best_score));
	}
}
