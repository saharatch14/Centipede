
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] public int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    [SerializeField] private GameObject _player;

    [SerializeField] private GameObject _enemy;

    [SerializeField] private GameObject _mushroom;

    [SerializeField] private Player player;

    [SerializeField] private Enemy enemy;

    //private Dictionary<Vector2, Tile> _tiles;

    //private LevelGrid levelGrid;

    private Vector2Int MushroomGridPosition;
    //private GameObject MushroomGameObject;
    private List<Vector2Int> MushroomGridLocate;

    private List<Vector2Int> GridLockLocate;
    private int count = 0;
    private string temp;
    private Vector2Int leftlock  = new Vector2Int(1, 0);
    private Vector2Int rightlock = new Vector2Int(-1, 0);

    public void Setup(Player player)
    {
        this.player = player;
    }

    public void Setup_Centepede(Enemy enemy)
    {
        this.enemy = enemy;
    }

    void Start()
    {
        GridLockLocate = new List<Vector2Int>();
        MushroomGridLocate = new List<Vector2Int>();
        GenerateGrid();
    }

    void GenerateGrid()
    {
        //_tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width ; x++)
        {
            for (int y = 0; y < _height ; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);


                //_tiles[new Vector2(x, y)] = spawnedTile;
                if(x == 0 || x == _width-1)
                {
                    GridLockLocate.Insert(count, new Vector2Int(x,y));
                }
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 , (float)_height / 2 , -10);

        if(_width % 2 == 0)
        {
            _player.transform.position = new Vector3((float)_width / 2, 0, -1);
            Instantiate(_player, _player.transform.position, Quaternion.identity);
        }
        else
        {
            _player.transform.position = new Vector3(((float)_width / 2)- 0.5f, 0, -1);
            Instantiate(_player, _player.transform.position, Quaternion.identity);
        }

        _enemy.transform.position = new Vector3(0, (float)_height, -1);
        Instantiate(_enemy, _enemy.transform.position, Quaternion.identity);


        for (int z = 0; z < _width*_height / _width; z++)
        {
            SpawnMushroom(z);
        }
    }

    private void SpawnMushroom(int num)
    {
        

        MushroomGridPosition = new Vector2Int(Random.Range(0, _width), Random.Range(1, _height-1));

        //MushroomGameObject = new GameObject("Block " + num, typeof(SpriteRenderer));
        //_mushroom = new GameObject("Block " + num, typeof(GameObject));
        //MushroomGameObject.GetComponent<SpriteRenderer>().sprite = Gameassets.i.MusjroomSprite;
        _mushroom.transform.position = new Vector3(MushroomGridPosition.x, MushroomGridPosition.y);
        Instantiate(_mushroom, _mushroom.transform.position, Quaternion.identity);
        MushroomGridLocate.Insert(num, MushroomGridPosition);

    }

    public string CentepedeMoved(Vector2Int CentepedeGridPosition, string command , string way)
    {

        for (int z = 0; z < GridLockLocate.Count; z++)
        {
            if (CentepedeGridPosition == GridLockLocate[z])
            {
                if(CentepedeGridPosition.x == 4 && CentepedeGridPosition.y == 4 && command == "down")
                {
                    temp = "left";
                    return temp;
                }
                else if ((CentepedeGridPosition.x == 0 && CentepedeGridPosition.y == 0) || ((CentepedeGridPosition.x == _width - 1 && CentepedeGridPosition.y == 0) && command == "right"))
                {
                    temp = "up";
                    return temp;
                }
                else if (command == "left" && CentepedeGridPosition == GridLockLocate[z] && CentepedeGridPosition.x > -1)
                {
                    if(CentepedeGridPosition.x > -1 && way == "down")
                    {
                        temp = "down";
                        return temp;
                    }
                    else if((CentepedeGridPosition.x >= 0 && way == "up" && CentepedeGridPosition.y+1 <= _height - 1))
                    {
                        temp = "up";
                        return temp;
                    }
                    /*else if ((CentepedeGridPosition.x >= 0 && way == "up" && CentepedeGridPosition.y + 1 == _height))
                    {
                        temp = "right";
                        return temp;
                    }*/
                }
                else if (command == "down" && CentepedeGridPosition == GridLockLocate[z] && CentepedeGridPosition.y >= 0)
                {
                    if (CentepedeGridPosition.x - 1 > -1)
                    {
                        temp = "left";
                        return temp;
                    }
                    else if(CentepedeGridPosition.x + 1 <= _width - 1)
                    {
                        temp = "right";
                        return temp;
                    }

                }
                else if (command == "right" && CentepedeGridPosition == GridLockLocate[z] && CentepedeGridPosition.x <= _width - 1)
                {
                    if (CentepedeGridPosition.x <= _width-1 && way == "down")
                    {
                        temp = "down";
                        return temp;
                    }
                    else if(CentepedeGridPosition.x <= _width - 1 && way == "up" && CentepedeGridPosition.y + 1 <= _height - 1)
                    {
                        temp = "up";
                        return temp;
                    }
                }
                else if (command == "up" && CentepedeGridPosition == GridLockLocate[z] && CentepedeGridPosition.y <= _height-1)
                {
                    if (CentepedeGridPosition.x - 1 >= 0)
                    {
                        temp = "left";
                        return temp;
                    }
                    if (CentepedeGridPosition.x + 1 <= _width - 1)
                    {
                        temp = "right";
                        return temp;
                    }
                }
            }
            else if(CentepedeGridPosition.x >= _width || CentepedeGridPosition.x < -1)
            {
                if(CentepedeGridPosition.x > _width)
                {
                    temp = "left";
                    return temp;
                }
                else if(CentepedeGridPosition.x < -1)
                {
                    temp = "right";
                    return temp;
                }
            }
            else
            {
                if(command == "downleft")
                {
                    temp = "left";
                    return temp;
                }
                if (command == "downright")
                {
                    temp = "right";
                    return temp;
                }
                /*if (CentepedeGridPosition.x == 4 && CentepedeGridPosition.y == 4 && command == "down")
                {
                    temp = "left";
                    return temp;
                }
                else if ((CentepedeGridPosition.x == 0 && CentepedeGridPosition.y == 0) || ((CentepedeGridPosition.x == _width - 1 && CentepedeGridPosition.y == 0) && command == "right"))
                {
                    temp = "up";
                    return temp;
                }
                else if (command == "left" && CentepedeGridPosition == GridLockLocate[z])
                {
                    if (CentepedeGridPosition.x >= 0 && way == "down")
                    {
                        temp = "down";
                        return temp;
                    }
                    else if ((CentepedeGridPosition.x >= 0 && way == "up" && CentepedeGridPosition.y + 1 <= _height - 1))
                    {
                        temp = "up";
                        return temp;
                    }
                    /*else if ((CentepedeGridPosition.x >= 0 && way == "up" && CentepedeGridPosition.y + 1 == _height))
                    {
                        temp = "right";
                        return temp;
                    }
                }
                else if (command == "down" && CentepedeGridPosition == GridLockLocate[z] && CentepedeGridPosition.y >= 0)
                {
                    if (CentepedeGridPosition.x - 1 >= 0)
                    {
                        temp = "left";
                        return temp;
                    }
                    else if (CentepedeGridPosition.x + 1 <= _width - 1)
                    {
                        temp = "right";
                        return temp;
                    }

                }
                else if (command == "right" && CentepedeGridPosition == GridLockLocate[z])
                {
                    if (CentepedeGridPosition.x <= _width - 1 && way == "down")
                    {
                        temp = "down";
                        return temp;
                    }
                    else if (CentepedeGridPosition.x <= _width - 1 && way == "up" && CentepedeGridPosition.y + 1 <= _height - 1)
                    {
                        temp = "up";
                        return temp;
                    }
                }
                else if (command == "up" && CentepedeGridPosition == GridLockLocate[z] && CentepedeGridPosition.y <= _height - 1)
                {
                    if (CentepedeGridPosition.x - 1 >= 0)
                    {
                        temp = "left";
                        return temp;
                    }
                    if (CentepedeGridPosition.x + 1 <= _width - 1)
                    {
                        temp = "right";
                        return temp;
                    }
                }*/
            }
        }
        return temp;
    }

    /*public Vector2Int CentepedeMovedawayfrommushroom(Vector2Int CentepedeGridPosition)
    {

        for (int z = 0; z < MushroomGridLocate.Count; z++)
        {
            if (CentepedeGridPosition == MushroomGridLocate[z])
            {
                return MushroomGridLocate[z];
            }
            else
            {

            }
        }
        return CentepedeGridPosition;
    }*/

    public string CentepedeMovedawayfrommushroom(Vector2Int CentepedeGridPosition)
    {
        for (int z = 0; z < GridLockLocate.Count; z++)
        {
            if (CentepedeGridPosition == GridLockLocate[z])
            {
                return temp;
            }
            else
            {
                for (int zz = 0; zz < MushroomGridLocate.Count; zz++)
                {
                    if (CentepedeGridPosition + leftlock == MushroomGridLocate[zz])
                    {
                        return "downleft";
                    }
                    else if ((CentepedeGridPosition + rightlock == MushroomGridLocate[zz]))
                    {
                        return "downright";
                    }
                }
            }
        }
        return temp;
    }

    public bool PlayerMoved(Vector2Int PlayerGridPosition)
    {
        for (int z = 0; z < MushroomGridLocate.Count; z++)
        {
            if (PlayerGridPosition == MushroomGridLocate[z])
            {
                return true;
            }
            else
            {
            }
        }
        return false;
    }

    /*public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }*/
}