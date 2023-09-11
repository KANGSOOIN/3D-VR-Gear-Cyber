using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace PM
{
    public class Map : MonoBehaviour
    {
        /// <summary>
        /// ���� �ִ밡�� ũ�� ���� ũ�� ���� �迭����
        /// �迭�� ���� ���� ũ�� ������ ������ �������� ����
        /// �� �������� ������ ���� ����� ���� �������� �迭������ �� �������� �ϳ��� �����´�
        /// ������ room0 {2,2}, room1{2,3}, room2{3,2}, room3{3,3}, room4{2,4}, room5{4,2}, room6{3,4}, room7{4,3}, room8{4,4}
        /// array ������ ���� ���ӿ��� ��Ʈ�� ����
        /// </summary>
        [SerializeField] int maxXSize;
        [SerializeField] int maxYSize;
        [SerializeField] int RoomMaxSize;
        enum arrow { up, down, left, right };

        [SerializeField] GameObject wall;
        [SerializeField] GameObject roomDoor;
        [SerializeField] GameObject passageTile;
        [SerializeField]
        GameObject[,] array;
        RoomData[,] roomData;

        [SerializeField] GameObject[] room0;
        [SerializeField] GameObject[] room1;
        [SerializeField] GameObject[] room2;
        [SerializeField] GameObject[] room3;
        [SerializeField] GameObject[] room4;
        [SerializeField] GameObject[] room5;
        [SerializeField] GameObject[] room6;
        [SerializeField] GameObject[] room7;
        [SerializeField] GameObject[] room8;
        [SerializeField] GameObject bossRoom;

        Transform rooms;
        Transform passages;
        Vector3[,] position;


        private void Start()
        {
            rooms = transform.GetChild(0);
            passages = transform.GetChild(1);

            PositionSetting();
            RandomPrefab();
            SpawnPrefab();
            EllerAlghorithm();
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
                    position[j, i] = new Vector3(i * 30, 0, -j * 30);
                }
            }
        }

        // �������� �������� �޾� array�� ���� �� x,y ���� ����
        private void RandomPrefab()
        {
            array = new GameObject[maxYSize, maxXSize];
            roomData = new RoomData[maxYSize, maxXSize];
            // �ʱ⿡�� x,y 2~4���� ����
            for (int j = 0; j < maxYSize; j++)
            {
                for (int i = 0; i < maxXSize; i++)
                {
                    int x = Random.Range(2, RoomMaxSize + 1);
                    int z = Random.Range(2, RoomMaxSize + 1);
                    // ������ �迭�濡�� �������� ����
                    if (j == maxYSize - 1 && i == maxXSize - 1)
                    {
                        x = 4;
                        z = 4;
                        array[j, i] = Instantiate(bossRoom, rooms);
                    }
                    else
                    {
                        if (x == 2 && z == 2)
                            array[j, i] = Instantiate(room0[Random.Range(0, 1)], rooms);
                        else if (x == 2 && z == 3)
                            array[j, i] = Instantiate(room1[Random.Range(0, 1)], rooms);
                        else if (x == 3 && z == 2)
                            array[j, i] = Instantiate(room2[Random.Range(0, 1)], rooms);
                        else if (x == 3 && z == 3)
                            array[j, i] = Instantiate(room3[Random.Range(0, 1)], rooms);
                        else if (x == 2 && z == 4)
                            array[j, i] = Instantiate(room4[Random.Range(0, 1)], rooms);
                        else if (x == 4 && z == 2)
                            array[j, i] = Instantiate(room5[Random.Range(0, 1)], rooms);
                        else if (x == 3 && z == 4)
                            array[j, i] = Instantiate(room6[Random.Range(0, 1)], rooms);
                        else if (x == 4 && z == 3)
                            array[j, i] = Instantiate(room7[Random.Range(0, 1)], rooms);
                        else if (x == 4 && z == 4)
                            array[j, i] = Instantiate(room8[Random.Range(0, 1)], rooms);
                    }
                    roomData[j, i] = array[j, i].GetComponent<RoomData>();
                    roomData[j, i].x = x;
                    roomData[j, i].z = z;
                    roomData[j, i].aggregate = maxXSize * j + i;
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

        private void EllerAlghorithm()
        {
            for (int j = 0; j < maxYSize; j++)
            {
                RightAggregater(j);
                DownAggregater(j);
            }
        }

        // array�迭�� ���� �Ű������� ����
        private void RightAggregater(int j)
        {
            for (int i = 0; i < maxXSize - 1; i++)
            {
                // j�� roomData�� ������ ���� �ƴҶ�
                if (j != roomData.GetLength(1) - 1)
                {
                    // ��ĭ�� ������ų Ȯ�� ������ 2���� 1
                    if (Random.Range(0, 2) == 0)
                    {
                        // ũ�ų� �۰ų� ������
                        if (roomData[j, i].aggregate < roomData[j, i + 1].aggregate)
                            roomData[j, i + 1].aggregate = roomData[j, i].aggregate;
                        else if (roomData[j, i].aggregate > roomData[j, i + 1].aggregate)
                            roomData[j, i].aggregate = roomData[j, i + 1].aggregate;
                        else
                            continue;
                        PassageRightCreate(j, i);
                    }
                }
                else
                {
                    // �����濡 ���� �̹� ���� ���� �Ǿ������� ���� ����� ���� �������� ������� Ȯ�� �� ���� �ȵǾ� ������
                    // ���� ����� ���� �ϳ��� �������� ����
                    if (i == roomData.GetLength(0) - 2)
                    {
                        if (roomData[j, i + 1].up)
                        {
                            for (int m = 1; m <= roomData.GetLength(1); m++)
                            {
                                if (roomData[j - m, i + 1].left)
                                {
                                    break;
                                }
                                else if (!roomData[j - m, i + 1].up)
                                {
                                    PassageRightCreate(j - Random.Range(1, m + 1), i);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (roomData[j, i].aggregate != roomData[j, i + 1].aggregate)
                            {
                                roomData[j, i + 1].aggregate = roomData[j, i].aggregate;
                                PassageRightCreate(j, i);
                            }
                        }
                    }
                    else
                    {
                        if (roomData[j, i].aggregate != roomData[j, i + 1].aggregate)
                        {
                            roomData[j, i + 1].aggregate = roomData[j, i].aggregate;
                            PassageRightCreate(j, i);
                        }
                    }
                }
            }
        }
        private void DownAggregater(int j)
        {
            if (j == roomData.GetLength(1) - 1)
                return;

            for (int i = 0; i < maxXSize; i++)
            {
                // i�� roomData ���� �������� �ƴҶ�
                if (i != roomData.GetLength(0) - 1)
                {
                    Debug.Log(i + " " + (roomData.GetLength(0) - 1));
                    if (Random.Range(0, 2) == 0 || roomData[j, i].aggregate != roomData[j, i + 1].aggregate)
                    {
                        roomData[j + 1, i].aggregate = roomData[j, i].aggregate;
                        PassageDownCreate(j, i);
                    }
                }
                else
                {
                    if (Random.Range(0, 2) == 0 || roomData[j, i].aggregate != roomData[j, i - 1].aggregate)
                    {
                        roomData[j + 1, i].aggregate = roomData[j, i].aggregate;
                        PassageDownCreate(j, i);
                    }
                }

            }
            // ���� ���� ���� �ٰ����� �Ʒ� ��θ� �ȸ���� ������ ���ǹ����� �߰�
            int sum = 0;
            for (int i = 0; i < maxXSize; i++)
            {
                if (roomData[j, i].down)
                    sum++;
                // ������ ���� ���� ���� ���� ���� ���� ���� �Ʒ���ΰ� ���ų� �밪�� ���� �迭�� ���� �ٸ��� �Ʒ������ ���� 0�̸�
                // ���� �Ʒ��� ��� ����
                if (i == roomData.GetLength(0) - 1 || roomData[j, i].aggregate != roomData[j, i + 1].aggregate)
                {
                    if (sum == 0)
                    {
                        roomData[j + 1, i].aggregate = roomData[j, i].aggregate;
                        PassageDownCreate(j, i);
                    }
                    sum = 0;
                }
            }
        }
        /// <summary>
        /// ��� �Լ��� ����
        /// i, j�� array �迭�� ��ȸ
        /// j,i�� �迭�� �ε����� ������ return
        /// </summary>
        //private void PassageCreate(int m, int n)
        //{
        //    if (n + 1 < maxXSize && array[m, n].GetComponent<RoomData>().right == false)
        //    {
        //        PassageRightCreate(m, n);
        //        PassageCreate(m, n + 1);
        //    }
        //    if (m + 1 < maxYSize && array[m, n].GetComponent<RoomData>().down == false)
        //    {
        //        PassageDownCreate(m, n);
        //        PassageCreate(m + 1, n);
        //    }
        //}
        private void PassageRightCreate(int m, int n)
        {
            int x = 6;
            while (position[m, n].x + x < position[m, n + 1].x)
            {
                GameObject tile = Instantiate(passageTile, passages);
                tile.transform.position = position[m, n] + new Vector3(x, 0, 0);
                tile.GetComponent<Passage>().arrow = Passage.Arrow.right;
                x += 6;
            }
            roomData[m, n].right = true;
            roomData[m, n + 1].left = true;
        }
        private void PassageDownCreate(int m, int n)
        {
            int z = 6;
            while (position[m, n].z - z > position[m + 1, n].z)
            {
                GameObject tile = Instantiate(passageTile, passages);
                tile.transform.position = position[m, n] + new Vector3(0, 0, -z);
                tile.GetComponent<Passage>().arrow = Passage.Arrow.down;
                z += 6;
            }
            roomData[m, n].down = true;
            roomData[m + 1, n].up = true;
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
                    ArrowWallCreater(roomData.x, roomData.z, roomData.up, arrow.up, roomData.transform);
                    ArrowWallCreater(roomData.x, roomData.z, roomData.right, arrow.right, roomData.transform);
                    ArrowWallCreater(roomData.x, roomData.z, roomData.down, arrow.down, roomData.transform);
                    ArrowWallCreater(roomData.x, roomData.z, roomData.left, arrow.left, roomData.transform);
                }
            }
        }

        private void ArrowWallCreater(int x, int y, bool door, arrow arrow, Transform room)
        {
            GameObject leftWall = array[0, 0];
            GameObject rightWall = array[0, 0];
            switch (arrow)
            {
                case arrow.up:
                    if (door)
                        Instantiate(roomDoor, room.transform.position + new Vector3(0, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                    else
                        Instantiate(wall, room.transform.position + new Vector3(0, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                    for (int i = 1; i <= x * 0.5f; i++)
                    {
                        rightWall = Instantiate(wall, room.transform.position + new Vector3(i * 6, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                        leftWall = Instantiate(wall, room.transform.position + new Vector3(-i * 6, 0, y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
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
                        Instantiate(roomDoor, room.transform.position + new Vector3(0, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 270, 0), room.GetChild(2));
                    else
                        Instantiate(wall, room.transform.position + new Vector3(0, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                    for (int i = 1; i <= x * 0.5f; i++)
                    {
                        rightWall = Instantiate(wall, room.transform.position + new Vector3(i * 6, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
                        leftWall = Instantiate(wall, room.transform.position + new Vector3(-i * 6, 0, -y * 6 * 0.5f), Quaternion.Euler(0, 90, 0), room.GetChild(2));
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
                        Instantiate(roomDoor, room.transform.position + new Vector3(x * 6 * 0.5f, 0, 0), Quaternion.Euler(0, 180, 0), room.GetChild(2));
                    else
                        Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, 0), Quaternion.identity, room.GetChild(2));
                    for (int i = 1; i <= y * 0.5f; i++)
                    {
                        rightWall = Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, i * 6), Quaternion.identity, room.GetChild(2));
                        leftWall = Instantiate(wall, room.transform.position + new Vector3(x * 6 * 0.5f, 0, -i * 6), Quaternion.identity, room.GetChild(2));
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
                    if (door)
                        Instantiate(roomDoor, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, 0), Quaternion.identity, room.GetChild(2));
                    else
                        Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, 0), Quaternion.identity, room.GetChild(2));
                    for (int i = 1; i <= y * 0.5f; i++)
                    {
                        rightWall = Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, i * 6), Quaternion.identity, room.GetChild(2));
                        leftWall = Instantiate(wall, room.transform.position + new Vector3(-x * 6 * 0.5f, 0, -i * 6), Quaternion.identity, room.GetChild(2));
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

        //private void DoorConnectings()
        //{
        //    for (int j = 0; j < maxYSize; j++)
        //    {
        //        for (int i = 0; i < maxXSize; i++)
        //        {
        //            RoomData roomData = array[j, i].GetComponent<RoomData>();
                    
        //            if(roomData.up || roomData.down || roomData.right || roomData.left)
        //            {
        //                SyberDoor[] doors = array[j, i].GetComponentsInChildren<SyberDoor>();
        //                foreach(SyberDoor door in doors)
        //                {
        //                    if(door.arrow == SyberDoor.Arrow.up)
        //                        //door.connetingDoor = array[j-1, i];
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
