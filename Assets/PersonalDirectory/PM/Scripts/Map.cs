using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Map : MonoBehaviour
{
    /// <summary>
    /// ���� �ִ밡�� ũ�� ���� ũ�� ���� �迭����
    /// �迭�� ���� ���� ũ�� ������ ������ �������� ����
    /// �� �������� ������ ���� ����� ���� �������� �迭������ �� �������� �ϳ��� �����´�
    /// ������ room0 {2,2}, room1{2,3}, room2{3,2}, room3{3,3}
    /// array ������ ���� �������� ��Ʈ�� ����
    /// 
    /// ����
    /// maxXSize, maxYSize
    /// Array ũ�� ����
    /// position ����
    /// ���� ������ ���� Array�� ����
    /// �����յ� ���̿� �� ���� , �ٴ� , ����, õ��
    /// �����յ� �� ����
    /// </summary>
    [SerializeField] int maxXSize;
    [SerializeField] int maxYSize;
    [SerializeField] int RoomMaxSize;
    int x;
    int y;

    //test
    [SerializeField] GameObject wall;
    [SerializeField] GameObject door;
    [SerializeField] GameObject passageTile;
    [SerializeField]
    GameObject[,] array;
    Vector3[,] position;

    [SerializeField] GameObject[] room0;
    [SerializeField] GameObject[] room1;
    [SerializeField] GameObject[] room2;
    [SerializeField] GameObject[] room3;

    private void Start()
    {
        PositionSetting();
        RandomPrefab();
        SpawnPrefab();
        PassageCreate(0, 0);
    }

    // �������� ������ ��ġ�� ����
    private void PositionSetting()
    {
        position = new Vector3[maxXSize, maxYSize];
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                position[j,i] = new Vector3(i * 30, 0, j * 30);
            }
        }
    }

    // �������� �������� �޾� array�� ���� �� x,y ���� ����
    private void RandomPrefab()
    {
        array = new GameObject[maxYSize, maxXSize];
        // �ʱ⿡�� x,y 2~3���� ����
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                x = Random.Range(2, RoomMaxSize+1);
                y = Random.Range(2, RoomMaxSize+1);
                if (x == 2 && y == 2)
                    array[j, i] = Instantiate(room0[Random.Range(0, 1)], transform);
                else if(x == 2 && y == 3)
                    array[j, i] = Instantiate(room1[Random.Range(0, 1)], transform);
                else if(x == 3 && y == 2)
                    array[j, i] = Instantiate(room2[Random.Range(0, 1)], transform);
                else if(x == 3 && y == 3)
                    array[j, i] = Instantiate(room3[Random.Range(0, 1)], transform);

                array[j, i].GetComponent<RoomData>().x = x;
                array[j, i].GetComponent<RoomData>().y = y;
            }
        }
    }

    // �������� position ��ġ�� ����
    private void SpawnPrefab()
    {
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                array[j, i].transform.position = position[j, i] + new Vector3(0, 0.1f, 0);
            }
        }
    }

    /// <summary>
    /// ��� �Լ��� ����
    /// i, j�� array �迭�� ��ȸ
    /// j,i�� �迭�� �ε����� ������ return
    /// </summary>
    private void PassageCreate(int m, int n)
    {
        if (n + 1 < maxXSize && array[m, n].GetComponent<RoomData>().right == false)
        {
            PassageRightCreate(m, n);
            PassageCreate(m, n + 1);
        }
        if (m+1 < maxYSize && array[m, n].GetComponent<RoomData>().down == false)
        {
            PassageLeftCreate(m, n);
            PassageCreate(m + 1, n);
        }
    }
    private void PassageRightCreate(int m, int n)
    {
        int x = 6;
        while (position[m, n].x +x < position[m, n+1].x)
        {
            GameObject tile = Instantiate(passageTile);
            tile.transform.position = position[m, n] + new Vector3(x,0,0);
            tile.GetComponent<Passage>().arrow = Passage.Arrow.right;
            x += 6;
        }
        array[m,n].GetComponent<RoomData>().right = true;
    }
    private void PassageLeftCreate(int m, int n)
    {
        int z = 6;
        while (position[m, n].z + z < position[m+1, n].z)
        {
            GameObject tile = Instantiate(passageTile);
            tile.transform.position = position[m, n] + new Vector3(0, 0, z);
            tile.GetComponent<Passage>().arrow = Passage.Arrow.down;
            z += 6;
        }
        array[m, n].GetComponent<RoomData>().down = true;
    }
    //private void Start()
    //{
    //    array[0] = room[0];
    //    array[1] = room2;
    //    position = new Vector3[10];
    //    position[0] = new Vector3(0, 0, 0);
    //    position[1] = new Vector3(24, 0, 0);
    //    int x = 3;
    //    Instantiate(room[0]).transform.position = position[0];
    //    Instantiate(room2).transform.position = position[1];
    //    Debug.Log(room[0].transform.position);
    //    while (true)
    //    {
    //        Instantiate(smallTile).transform.position = room[0].transform.position + new Vector3(x, 0, 0);
    //        x += 3;
    //        if (x >= position[1].x)
    //        {
    //            break;
    //        }
    //    }
    //}
}
