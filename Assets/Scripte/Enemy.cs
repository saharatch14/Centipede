using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    public int centepedeBodySize=15;
    private List<Vector2Int> centepedeMovePositionList;
    private string command;
    private string way;
    private string newlocate;
    private string temptest;

   [SerializeField] private GameObject _body;

    // Start is called before the first frame update

    private GridManager gridManager;

    public void Setup(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    void Awake()
    {
        centepedeMovePositionList = new List<Vector2Int>();
    }

    void Start()
    {
        gridManager = GameObject.FindGameObjectWithTag("option").GetComponent<GridManager>();
        gridPosition = new Vector2Int(-Mathf.Abs(centepedeBodySize), (int)gridManager._height);
        gridMoveTimerMax = .2f/*(gridManager._height * gridManager._width) / 60f*/;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        HandleAI();
        HandleGridMovment();
    }

    void UpAI()
    {
        gridMoveDirection.x = 0;
        gridMoveDirection.y = +1;
    }

    void DownAI()
    {
        gridMoveDirection.x = 0;
        gridMoveDirection.y = -1;
    }

    void LeftAI()
    {
        gridMoveDirection.x = -1;
        gridMoveDirection.y = 0;
    }

    void RightAI()
    {
        gridMoveDirection.x = +1;
        gridMoveDirection.y = 0;
    }

    private void HandleAI()
    {
        if (gridPosition == new Vector2Int(gridManager._width - 1, gridManager._height))
        {
            DownAI();
            command = "down";
            way = "down";
        }
        else
        {
            if (newlocate == "down")
            {
                DownAI();
                command = "down";
                way = "down";
            }
            else if (newlocate == "left")
            {
                LeftAI();
                command = "left";
            }
            else if (newlocate == "right")
            {
                RightAI();
                command = "right";
            }
            else if (newlocate == "up")
            {
                UpAI();
                command = "up";
                way = "up";
            }
            else if(newlocate == "downright")
            {
                DownAI();
                //newlocate = "left";
                command = "downright";
                way = "down";
            }
            else if (newlocate == "downleft")
            {
                DownAI();
                //newlocate = "right";
                command = "downleft";
                way = "down";
            }
        }
    }
    private void HandleGridMovment()
    {
        gridMoveTimer += Time.deltaTime;
        //gridMoveTimer = (gridManager._height * gridManager._width) / (gridMoveTimer % 60);
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            centepedeMovePositionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;

            if (centepedeMovePositionList.Count >= centepedeBodySize + 1)
            {
                centepedeMovePositionList.RemoveAt(centepedeMovePositionList.Count - 1);
            }

            for (int i = 0; i < centepedeMovePositionList.Count; i++)
            {
                Vector2Int centepedeMovePosition = centepedeMovePositionList[i];

                Vector3 convert = new Vector3(centepedeMovePosition.x, centepedeMovePosition.y, -1);

                GameObject tmpObj = GameObject.Instantiate(_body, convert, Quaternion.identity) as GameObject;
                convert += new Vector3(1f, 0f, 0f);
                Destroy(tmpObj, gridMoveTimerMax);
            }

            //Vector2Int newlocate = gridManager.CentepedeMoved(gridPosition,command);
            newlocate = gridManager.CentepedeMoved(gridPosition, command, way);
            newlocate = gridManager.CentepedeMovedawayfrommushroom(gridPosition);
            //Debug.Log(newlocate);
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection) - 90);
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Bullet")
        {
            Destroy(this.gameObject);
            FindObjectOfType<GameManager>().CompleteLevel();
        }
    }
}
