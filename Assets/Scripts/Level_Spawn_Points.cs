using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Spawn_Points : MonoBehaviour
{
    public List<Transform> spawn_points = new List<Transform>();

    public Transform Get_Point(int _tmp)
    {
        return spawn_points[_tmp];
    }
}