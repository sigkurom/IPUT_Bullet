using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Player_0518_ver001 : MonoBehaviour
{
    public float movespeed = 20;

    private Vector3 jumping;

    public float timecount = 0;

    // Start is called before the first frame update
    void Start()
    {
        ;
    }

    // Update is called once per frame
    void Update()
    {

        timecount += Time.deltaTime;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                movespeed = 14f;
            }
            ///�΂߈ړ��̑��x�ݒ�
            else
            {
                movespeed = 20;
            }
            ///�ʏ�̈ړ����x
        }
        else
        {
            movespeed = 20;
        }
        if (Input.GetKey(KeyCode.L))
        {
            movespeed = movespeed * 2;
        }
        ///�_�b�V��

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, movespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -movespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(movespeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-movespeed * Time.deltaTime, 0, 0);
        }
        ///�ړ��R�R�܂�

        ///�W�����v
        ///if(Input.GetKey("space"))
        ///{
        /// Vector3 force = new Vector3(0, 1, 0);
        /// this.GetComponent<Rigidbody>().AddForce(force, ForceMode.Force);
        ///}


    }


}
