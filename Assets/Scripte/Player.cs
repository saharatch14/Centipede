using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]

public class Player : MonoBehaviour
{
    //private Vector2Int gridPosition;
    [SerializeField]
    private GameObject player_Bullet;

    [SerializeField]
    private Transform attack_Point;

    private bool isMoving;
    private Vector3 origPos, targetPos;
    private Vector2Int targetPosSent;
    private float timeToMove = 0.2f;
    public float attack_Timer = 0.35f;
    private float current_Attack_Timer;
    private bool canAttack;
    public float min_X, max_X, min_Y, max_Y;
    public int life = 3;

    GridManager gridManager;

    private bool recicer;

    public void Setup(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    // Start is called before the first frame update

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        gridManager = GameObject.FindGameObjectWithTag("option").GetComponent<GridManager>();
        current_Attack_Timer = attack_Timer;
        min_X = 0;
        max_X = gridManager._width;
        min_Y = 0;
        max_Y = gridManager._height/1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.up));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.down));
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.right));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving)
        {
            StartCoroutine(MovePlayer(Vector3.left));
        }
    }

    void Attack()
    {
        attack_Timer += Time.deltaTime;
        if(attack_Timer > current_Attack_Timer)
        {
            canAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(canAttack)
            {
                canAttack = false;
                attack_Timer = 0f;

                Instantiate(player_Bullet, attack_Point.position, Quaternion.identity);
            }    
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        int locatex = (int)targetPos.x;
        int locatey = (int)targetPos.y;

        targetPosSent = new Vector2Int(locatex, locatey);

        recicer = gridManager.PlayerMoved(targetPosSent);

        if (recicer == true)
        {
                isMoving = false;
        }
        else
        {
            if (targetPos.x >= min_X && targetPos.y >= min_Y && targetPos.x < (int)max_X && targetPos.y < (int)max_Y)
            {
                while (elapsedTime < timeToMove)
                {
                    transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                transform.position = targetPos;
                isMoving = false;
            }
            else
            {
                isMoving = false;
            }
        }
    }
	
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Enemy")
        {
            life = life - 1;
            if (life > 0)
            {
                gameObject.SetActive(false);
                FindObjectOfType<GameManager>().EndGame();
            }
            else
            {
                Destroy(gameObject);
                FindObjectOfType<GameManager>().CompleteLevel();
            }
        }
    }
}
