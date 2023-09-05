using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class Passage : MonoBehaviour
{
    // Ÿ�� ���⿡ ���� ���� ����� ���� enum ���
    public enum Arrow { right, down};
    [SerializeField] GameObject passageWall;
    public Arrow arrow;
    // Ÿ���� �� �������� ���̸� ���� ���ǿ� ���� ��κ��� ����
    Ray rightRay;
    Ray leftRay;
    Ray upRay;
    Ray downRay;

    bool right;
    bool left;
    bool up;
    bool down;
    private void Start()
    {
        rightRay = new Ray(transform.position, transform.up + transform.right);
        leftRay = new Ray(transform.position, transform.up - transform.right);
        upRay = new Ray(transform.position, transform.up + transform.forward);
        downRay = new Ray(transform.position, transform.up - transform.forward);
        RayCast();
        CreateWall();
    }
    
    private void RayCast()
    {
        right = Physics.Raycast(rightRay, 1);
        left = Physics.Raycast(leftRay, 1);
        up = Physics.Raycast(upRay, 1);
        down = Physics.Raycast(downRay, 1);
    }

    private void CreateWall()
    {
        if (right && left && up && down)
        {
            Instantiate(passageWall).transform.position = transform.position + new Vector3(0,3,0);
        }

    }
}
