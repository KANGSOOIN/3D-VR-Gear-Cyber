using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    /// <summary>
    /// ���� �ִ밡�� ũ�� ���� ũ�� ���� �迭����
    /// �迭�� ���� ���� ũ�� ������ ������ �������� ����
    /// �� �������� ������ ���� ����� ���� �������� �迭������ �� �������� �ϳ��� �����´�
    /// 
    /// 
    /// </summary>
    [SerializeField] int roomMaxSize;
    [SerializeField] int roomMinSize;

    //test
    [SerializeField] GameObject wall;
    [SerializeField] GameObject door;
    [SerializeField] GameObject smallWall;
    [SerializeField] GameObject smallTile;
    [SerializeField] Vector3[] position;
    [SerializeField]
    GameObject[] array = new GameObject[2];
    //room1
    [SerializeField] GameObject room;

    //room2
    [SerializeField] GameObject room2;
    private void Start()
    {
        array[0] = room;
        array[1] = room2;
        position = new Vector3[10];
        position[0] = new Vector3(0, 0, 0);
        position[1] = new Vector3(24, 0, 0);
        int x = 3;
        Instantiate(room).transform.position = position[0];
        Instantiate(room2).transform.position = position[1];
        Debug.Log(room.transform.position);
        while (true)
        {
            Instantiate(smallTile, room.transform.position+ new Vector3(x,0,0), Quaternion.identity);
            x += 3;
            if (x >= position[1].x)
            {
                break;
            }
        }
    }
}
