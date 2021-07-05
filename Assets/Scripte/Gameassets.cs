using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameassets : MonoBehaviour
{
    public static Gameassets i;

    private void Awake()
    {
        i = this;
    }

    public Sprite MusjroomSprite;
}
