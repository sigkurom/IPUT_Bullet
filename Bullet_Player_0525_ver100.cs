using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rigidbody���g���Ă̑O�㍶�E�ړ��W�����v�_�b�V���܂�
//WASD�Ŏl�����ړ��A���N���b�N�ŃW�����v�A�E�N���b�N�Ń_�b�V��

//��������ׂ����b�A�ǂݔ�΂��Ă�OK
//Update�́A�Ăяo���Ԋu�����ł͂Ȃ����A�Ăяo���p�x�͍���(���t���[��)
//FixedUpdate�́A�Ԋu�͈�肾���p�x�͒Ⴂ
//����āA�ړ��L�[���͔����Update�ŁA
//�ړ��������̂��̂�FixedUpdate�ōs���̂���ʓI�炵��
//�t���ƁA�ړ����J�N������(Update�̊Ԋu���s��̂���)
//�L�[���͂ɔ������Ȃ������肷��(FixedUpdate�͖��t���[���m�F����킯�ł͂Ȃ�����)

public class Bullet_Player_0525_ver100 : MonoBehaviour
{
    //�����̕ϐ��͈�x�I�u�W�F�N�g�ɃX�N���v�g����������ƁA
    //�ȍ~��unity��Inspector����Ȃ��ƕς����Ȃ��悤�Ȃ̂Œ��ӁB
    //�����A�I�u�W�F�N�g����X�N���v�g���O���ĕt���Ȃ����΁A
    //������̏C�������f����邪�B
    public float speed; //�ړ����x
    public float dashspeed = 2; //�_�b�V�����̑��x�{��
    public Vector3 pm = new Vector3(0f, 0f, 0f); //�v���C���[�ړ��p���x
    public Vector3 w = new Vector3(0f, 0f, 1f); //�O�ړ��x�N�g��
    public Vector3 s = new Vector3(0f, 0f, -1f); //���ړ��x�N�g��
    public Vector3 d = new Vector3(1f, 0f, 0f); //�E�ړ��x�N�g��
    public Vector3 a = new Vector3(-1f, 0f, 0f); //���ړ��p�x�N�g��
    public Vector3 jumping = new Vector3(0f, 30f, 0f); //�W�����v�p�x�N�g��
    public Vector3 gravity = new Vector3(0f, -1f, 0f); //�d�͗p�x�N�g��
    bool forward; //�O�L�[��������
    bool back; //���L�[��������
    bool right; //�E�L�[��������
    bool left; //���L�[��������
    bool jump; //�W�����v�L�[��������
    bool dash; //�_�b�V���L�[��������

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�e�L�[���͔���̏�����
        forward = false;
        back = false;
        right = false;
        left = false;
        jump = false;
        dash = false;

        //�e��L�[�������Ă���ꍇ�A���͔����true�ɂ���
        if(Input.GetKey(KeyCode.W))
        {
            forward = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            back = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            right = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            left = true;
        }

        if (Input.GetMouseButton(1))
        {
            jump = true;
        }

        if (Input.GetMouseButton(0))
        {
            dash = true;
        }
    }

    void FixedUpdate()
    {
        //rigidbody�ő��x��ω������鉺����?
        Rigidbody rb1 = this.transform.GetComponent<Rigidbody>(); //velocity�p��rigidbody���擾

        //�΂߈ړ����̑��x����
        if (forward||back)
        {
            if (right||left)
            {
                speed = 7; //�΂߈ړ��p��speed
            }
            else
            {
                speed = 10; //�l���ړ���speed
            }
        }
        else
        {
            speed = 10;
        }

        if(dash)
        {
            speed *= dashspeed; 
        }

        //(�L�[�������Ă��Ȃ��ꍇ��)�l�����ړ����x�̏�����
        //�W�����v��d�͂ɂ�鑬�x�͈ێ����Ȃ��Ƃ����Ȃ��̂ŁAy�͏��������Ȃ�
        pm.x = 0f;
        pm.z = 0f;

        //�l�����ړ��̏���
        //�������L�[(�̓��͔���)�ɉ����đ��x���x�N�g��pm�ɒǉ�
        if (forward)
        {
            pm += w * speed;
        }

        if (back)
        {
            pm += s * speed;
        }

        if (right)
        {
            pm += d * speed;
        }

        if (left)
        {
            pm += a * speed;
        }

        //�W�����v�̏���
        //ray(�s���̌���)���������ɔ��˂��A�n�ʂƐڐG���邩�ǂ����Őڒn������s��
        Ray ray = new Ray(transform.position, -transform.up);
        {
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1f)) //ray���ڒn���Ă邩����
            {
                pm.y = 0f; //�ڒn���͗������x��0�ɂ���

                if(jump)
                {
                    pm += jumping; //pm�ɃW�����v���x��������
                }
            }
            else
            {
                pm += gravity; //��ڒn���͏d��(�����̑��x)��������
                //AddForce�͏�肭�g���Ȃ��������߁A���x�ő�p
                //�v���O�������ŏd�͂̏������s���Ă��邽�߁A
                //unity��rigidbody��use gravity��off�ɂ��邱��
            }
        }

        rb1.velocity = pm; //�x�N�g��pm�𑬓x�Ƃ��ė^����
    }
}

//���󂾂ƁA�󒆂ł������]����_�b�V����ON�EOFF���o����̂ŁA
//�K�v���]�T������΁A���̕Ӓ������Ă����������B
