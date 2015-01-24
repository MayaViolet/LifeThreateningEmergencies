using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeightSorter : MonoBehaviour {

	List<SpriteRenderer> sprites;
	SpriteRenderer player;

	// Use this for initialization
	void Start () {
		//get player
		{
			GameObject playerGO = GameObject.FindGameObjectWithTag("PlayerController");
			player = playerGO.GetComponentInChildren<SpriteRenderer>();
		}

		//get sorted sprites
		{
			GameObject[] spriteGOs = GameObject.FindGameObjectsWithTag("HeightSort");
			sprites = new List<SpriteRenderer>(spriteGOs.Length);
			foreach (GameObject GO in spriteGOs)
			{
				sprites.Add(GO.renderer as SpriteRenderer);
			}
		}

		//sort sprites
		sprites.Sort(
			(a,b) => b.transform.position.y.CompareTo(a.transform.position.y)
			);

		AssignOrder();
	}
	
	// Update is called once per frame
	void Update () {
		AssignOrder();
	}

	void AssignOrder()
	{
		int order = 0;
		bool donePlayer = false;
		foreach (SpriteRenderer sprite in sprites)
		{
			sprite.sortingOrder = order++;
			if (!donePlayer && sprite.transform.position.y > player.transform.position.y)
			{
				player.sortingOrder = order++;
			}
		}
	}
}
