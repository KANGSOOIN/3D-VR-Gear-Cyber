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
    /// ������ room0 {2,2}, room1{3,2}, room2{2,3}, room3{3,3}
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
    enum arrow { up, down, left, right };

    [SerializeField] GameObject wall;
    [SerializeField] GameObject roomDoor;
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
        WallCreate();
    }

    // �������� ������ ��ġ�� ����
    private void PositionSetting()
    {
        position = new Vector3[maxXSize, maxYSize];
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                position[j,i] = new Vector3(i * 30, 0, -j * 30);
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
            PassageDownCreate(m, n);
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
        array[m, n].GetComponent<RoomData>().right = true;
        array[m, n+1].GetComponent<RoomData>().left = true;
    }
    private void PassageDownCreate(int m, int n)
    {
        int z = 6;
        while (position[m, n].z - z > position[m+1, n].z)
        {
            GameObject tile = Instantiate(passageTile);
            tile.transform.position = position[m, n] + new Vector3(0, 0, -z);
            tile.GetComponent<Passage>().arrow = Passage.Arrow.down;
            z += 6;
        }
        array[m, n].GetComponent<RoomData>().down = true;
        array[m+1, n].GetComponent <RoomData>().up = true;
    }
    
    // �迭�� ����� ���ӿ�����Ʈ�� ��ȸ�ϸ� RoomData ��ũ��Ʈ�� Ȯ���Ͽ� ��ΰ� ���� ���⿡�� �׳� �� �ִ� ���⿡�� ���� ���� ����� �°� ����
    // ��ΰ� �ֳ� ���Ŀ� ���� �ٸ��� ���� �ϸ� ������ 
    private void WallCreate()
    {
        for (int j = 0; j < maxYSize; j++)
        {
            for (int i = 0; i < maxXSize; i++)
            {
                RoomData roomData = array[j, i].GetComponent<RoomData>();
                ArrowWallCreater(roomData.x, roomData.y, roomData.up, arrow.up, roomData.transform);
                ArrowWallCreater(roomData.x, roomData.y, roomData.right, arrow.right, roomData.transform);
                ArrowWallCreater(roomData.x, roomData.y, roomData.down, arrow.down, roomData.transform);
                ArrowWallCreater(roomData.x, roomData.y, roomData.left, arrow.left, roomData.transform);
            }
        }
    }

    private void ArrowWallCreater(int x, int y, bool door, arrow arrow, Transform room)
    {
        GameObject leftWall = new GameObject();
        GameObject rightWall = new GameObject();
        switch (arrow)
        {
            case arrow.up:
                if (door)
                    Instantiate(roomDoor, room.transform.position + new Vector3(0, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                else
                    Instantiate(wall, room.transform.position + new Vector3(0, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                for (int i = 1; i <= x * 0.5f; i++)
                {
                    rightWall = Instantiate(wall, room.transform.position + new Vector3(i * 6, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                    leftWall = Instantiate(wall, room.transform.position + new Vector3(-i * 6, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                }
                if (x % 2 == 0)
                {
                    rightWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    leftWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    rightWall.transform.position -= new Vector3(1.5f, 0, 0);
                    leftWall.transform.position += new Vector3(1.5f, 0, 0);
                }
                break;
            case arrow.down:
                if (door)
                    Instantiate(roomDoor, room.transform.position + new Vector3(0, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                else
                    Instantiate(wall, room.transform.position + new Vector3(0, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                for (int i = 1; i <= x * 0.5f; i++)
                {
                    rightWall = Instantiate(wall, room.transform.position + new Vector3(i * 6, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                    leftWall = Instantiate(wall, room.transform.position + new Vector3(-i * 6, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room);
                }
                if (x % 2 == 0)
                {
                    rightWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    leftWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    rightWall.transform.position -= new Vector3(1.5f, 0, 0);
                    leftWall.transform.position += new Vector3(1.5f, 0, 0);
                }
                break;
            case arrow.right:
                if (door)
                    Instantiate(roomDoor, room.transform.position + new Vector3(x * 6 * 0.5f, 0, 0), Quaternion.identity, room);
                else
                    Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, 0), Quaternion.identity, room);
                for (int i = 1; i <= y * 0.5f; i++)
                {
                    rightWall = Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, i * 6), Quaternion.identity, room);
                    leftWall = Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, -i * 6), Quaternion.identity, room);
                }
                if (y % 2 == 0)
                {
                    rightWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    leftWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    rightWall.transform.position -= new Vector3(0, 0, 1.5f);
                    leftWall.transform.position += new Vector3(0, 0, 1.5f);
                }
                break;
            case arrow.left:
                if(door)
                    Instantiate(roomDoor, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, 0), Quaternion.identity, room);
                else
                    Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, 0), Quaternion.identity, room);
                for (int i = 1; i <= y * 0.5f; i++)
                {
                    rightWall = Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, i * 6), Quaternion.identity, room);
                    leftWall = Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, -i * 6), Quaternion.identity, room);
                }
                if (y % 2 == 0)
                {
                    rightWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    leftWall.transform.localScale = new Vector3(1, 1, 0.5f);
                    rightWall.transform.position -= new Vector3(0, 0, 1.5f);
                    leftWall.transform.position += new Vector3(0, 0, 1.5f);
                }
                break;
        }
    }
    private void RightWallCreater(int x, int y)
    {

    }
    private void LeftWallCreater(int x, int y)
    {

    }
}
