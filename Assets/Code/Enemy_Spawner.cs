using UnityEngine;
using System.Collections;

public class Enemy_Spawner : MonoBehaviour {

enum SpawnType{land,air}
enum spawnDirection {left,right,up,down,portal}


Game_Manager gameController;
//var spawnerSprite			: SpriteSheet;

int unitMax;
int maxWaves;
GameObject enemyUnit_one;
int  Health;
bool spawnTrigger;

private GameObject enemyUnitClone;
private int unitIndex;
private int waveIndex;

//variables for spawned enemy
//enemyState eState; 						//enemy State
//enemyType  eType;						//Enemy Type
// enemyDirection Direction;					//Direction

 int health; 								//health
 float patrolRange; 							//patrolRange
 float moveSpeed; 							//moveSpeed
 float chaseSpeed; 							//chaseSpeed
 float fallSpeed; 							//fallSpeed
 float jumpHeight; 							//jumpHeight
 float maxHeight; 							//maxHeight

 bool Attack = false; 					//Attack
 bool canShoot = true; 					//canShoot
 Transform homePosition;						//startPosition
 Vector3 velocity; 							//velocity
 float gravity;							//gravity
 
public void Start()
{
	 unitIndex = 0;
	 waveIndex = 0;
     gameController = GetComponent<Game_Manager>();
}

public void FixedUpdate ()
 {
 	
	if(spawnTrigger == true && waveIndex <= maxWaves)
	{
		
		
		SpawnWave();
		spawnTrigger = false;

	}
	//gameController.aniSprite(spawnerSprite);
	
}

public void SpawnWave()
{
	/*
	for(var i = 0;i < unitMax;i++ )
	{
	  enemyUnitClone = Instantiate(enemyUnit_one, transform.position , transform.rotation);

      enemyUnitClone.GetComponent<Enemy_Controller>().initEnemy(eState,
															   eType,
															   Direction,
															   
															   health, 
															   patrolRange,
															   moveSpeed,
															   chaseSpeed,
															   fallSpeed,
															   jumpHeight,
															   maxHeight,
															   
															   false,
															   false,
															   false,
															   false,
															   false,
															   true,
															   
															   transform.position,
															   velocity,
															   gravity );


        yield return new WaitForSeconds(1.5f);
	}
  */   
}


}
