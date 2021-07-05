using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHander : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;

    GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        //gridManager = new GridManager();

        gridManager = GameObject.FindGameObjectWithTag("option").GetComponent<GridManager>();

        player.Setup(gridManager);
        enemy.Setup(gridManager);
        gridManager.Setup(player);
        gridManager.Setup_Centepede(enemy);
    }

}
