using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace PID
{
    public static class PatrolPathHelper
    {
        const int INF = 99999999;
        public static void ShortestPath(in Vector3[] graph, in Vector3 start, out int[] distance, out Vector3[] path)
        {
            int size = graph.Length;
            bool[] visited = new bool[size];
            distance = new int[size];
            path = new Vector3[size];

            for (int i = 0; i < size; i++)
            {
                distance[i] = INF;
                path[i] = Vector3.zero; 
                visited[i] = false;
            }
            distance[0] = 0;

            for (int i = 0; i < size; i++)
            {
                // 1. �湮���� ���� ���� �� ���� ����� �������� Ž��
                int next = -1;
                int minCost = INF;
                for (int j = 0; j < size; j++)
                {
                    if (!visited[j] &&
                        distance[j] < minCost)
                    {
                        next = j;
                        minCost = distance[j];
                    }
                }
                if (next < 0)
                    break;

                // 2. ��������� �Ÿ����� ���ļ� �� ª�����ٸ� ����.
                //for (int j = 0; j < size; j++)
                //{
                //    if (i == j)
                //        return; 
                //    // distance[j] : ���������� ���� ����� �Ÿ�
                //    // distance[next] : Ž������ �������� �Ÿ�
                //    // graph[next, j] : Ž������ �������� �������� �Ÿ�
                //    if (distance[j] > distance[next] + graph[next, j])
                //    {
                //        distance[j] = distance[next] + graph[next, j];
                //        path[j] = next;
                //    }
                //}
                visited[next] = true;
            }
        }
    }
}