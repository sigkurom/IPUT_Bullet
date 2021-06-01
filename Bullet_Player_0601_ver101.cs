using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<<���ӓ_!>>
//�E�v���C���[��Rigidbody��t���AConstraints��Freeze Rotate��X,Z�Ƀ`�F�b�N������B(Y����]�ȊO���֎~)
//�E�K���ȃt�H���_�ɁA�E�N���b�N��Create��Pysical Material���쐬�B
//�@Dynamic Friction(�ړ����Ă�I�u�W�F�N�g�ɑ΂��門�C)��
//�@Static Friction(�Î~���Ă�I�u�W�F�N�g�ɑ΂��門�C)��0�ɂ���B
//�@����Pysical Material���v���C���[�A��Q���̗����ɕt����B
//�@(���ꂪ�Ȃ��ƁA�I�u�W�F�N�g�ɐڐG�������ɖ��C�œ��]����)

//Rigidbody���g���Ă̑O�㍶�E�ړ��A���E��]�A�W�����v�_�b�V���܂�
//WASD�Ŏl�����ړ��AQE�ō��E��]�A���N���b�N�ŃW�����v�A�E�N���b�N�Ń_�b�V��

//���������I�u�W�F�N�g�ɂԂ������ۂɁA(�����炭)�������Z�œ��]����s��������B(�����ς�)
//is Kinetic(�������Z�s�g�p)��on�ɂ���΁A�ڐG�����𗘗p�ł����������A
//transform�����g���Ȃ��Ȃ邽�߁A�v���O�����ł������B
//��Unity���Pysical Material�����ADynamic Friction(���I�u�W�F�N�g�Ƃ̖��C)��0�ɂ���΁A�p�x�Y�����͉��������B
//�v���C���[�ƃI�u�W�F�N�g�̗�����Pysical Material��t����K�v������_�ɒ��ӁB


//��������ׂ����b�A�ǂݔ�΂��Ă�OK
//Update�́A�Ăяo���Ԋu�����ł͂Ȃ����A�Ăяo���p�x�͍���(���t���[��)
//FixedUpdate�́A�Ԋu�͈�肾���p�x�͒Ⴂ
//����āA�ړ��L�[���͔����Update�ŁA
//�ړ��������̂��̂�FixedUpdate�ōs���̂���ʓI�炵��
//�t���ƁA�ړ����J�N������(Update�̊Ԋu���s��̂���)
//�L�[���͂ɔ������Ȃ������肷��(FixedUpdate�͖��t���[���m�F����킯�ł͂Ȃ�����)

public class Bullet_Player_0601_ver101 : MonoBehaviour
{
    //�����̕ϐ��͈�x�I�u�W�F�N�g�ɃX�N���v�g����������ƁA
    //�ȍ~��unity��Inspector����Ȃ��ƕς����Ȃ��悤�Ȃ̂Œ��ӁB
    //�����A�I�u�W�F�N�g����X�N���v�g���O���ĕt���Ȃ����΁A
    //������̏C�������f����邪�B
    public float speed; //�ړ����x
    public float dashspeed = 2; //�_�b�V�����̑��x�{��

    //transform.forward�ɂ�肨�����
    //public float angle; //�v���C���[�̊p�x�擾�p
    //public float p_rad; //�v���C���[�̊p�x�̃��W�A���ϊ��p

    public Vector3 pm = new Vector3(0f, 0f, 0f); //�v���C���[�ړ��p���x

    //transform.forward(���ʃx�N�g��)�����g�p�ł������߁A�R�����g�A�E�g
    //public Vector3 w = new Vector3(0f, 0f, 1f); //�O�ړ��x�N�g��
    //public Vector3 s = new Vector3(0f, 0f, -1f); //���ړ��x�N�g��
    //public Vector3 d = new Vector3(1f, 0f, 0f); //�E�ړ��x�N�g��
    //public Vector3 a = new Vector3(-1f, 0f, 0f); //���ړ��p�x�N�g��

    public Vector3 e = new Vector3(0f, 3f, 0f); //�E��]�p�x�N�g��
    public Vector3 q = new Vector3(0f, -3f, 0f); //����]�p�x�N�g��
    public Vector3 jumping = new Vector3(0f, 30f, 0f); //�W�����v�p�x�N�g��
    public Vector3 gravity = new Vector3(0f, -1f, 0f); //�d�͗p�x�N�g��
    bool forward; //�O�L�[��������
    bool back; //���L�[��������
    bool right; //�E�L�[��������
    bool left; //���L�[��������
    bool rightturn; //�E��]��������
    bool leftturn; //����]��������
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
        rightturn = false;
        leftturn = false;
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

        if (Input.GetKey(KeyCode.E))
        {
            rightturn = true;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            leftturn = true;
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
        Rigidbody rb0 = this.GetComponent<Rigidbody>(); //angularVelocity(��])�p��rigidbody���擾?
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

        //(�L�[�������Ă��Ȃ��ꍇ��)�l�����ړ����x���]���x�̏�����
        //�W�����v��d�͂ɂ�鑬�x�͈ێ����Ȃ��Ƃ����Ȃ��̂ŁAy�͏��������Ȃ�
        pm.x = 0f;
        pm.z = 0f;
        rb0.angularVelocity = new Vector3 (0f, 0f, 0f);

        //��]�̏���
        if (rightturn)
        {
            rb0.angularVelocity = e;
        }

        if (leftturn)
        {
            rb0.angularVelocity = q;
        }

        //���E�����ɉ�]���悤�Ƃ���ƒ�~
        if (rightturn && leftturn)
        {
            rb0.angularVelocity = new Vector3(0f, 0f, 0f);
        }

        //�l�����ړ��̏���
        //�������L�[(�̓��͔���)�ɉ����đ��x���x�N�g��pm�ɒǉ�
        if (forward)
        {
            pm += this.transform.forward * speed;
        }

        if (back)
        {
            pm += - this.transform.forward * speed;
        }

        if (right)
        {
            pm += this.transform.right * speed;
        }

        if (left)
        {
            pm += - this.transform.right * speed;
        }

        Debug.Log(rightturn);

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