using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rigidbodyを使っての前後左右移動ジャンプダッシュまで
//WASDで四方向移動、左クリックでジャンプ、右クリックでダッシュ

//ここから細かい話、読み飛ばしてもOK
//Updateは、呼び出し間隔が一定ではないが、呼び出し頻度は高い(毎フレーム)
//FixedUpdateは、間隔は一定だが頻度は低い
//よって、移動キー入力判定はUpdateで、
//移動処理そのものはFixedUpdateで行うのが一般的らしい
//逆だと、移動がカクついたり(Updateの間隔が不定のため)
//キー入力に反応しなかったりする(FixedUpdateは毎フレーム確認するわけではないため)

public class Bullet_Player_0525_ver100 : MonoBehaviour
{
    //ここの変数は一度オブジェクトにスクリプトをくっつけると、
    //以降はunityのInspectorじゃないと変えられないようなので注意。
    //多分、オブジェクトからスクリプトを外して付けなおせば、
    //こちらの修正も反映されるが。
    public float speed; //移動速度
    public float dashspeed = 2; //ダッシュ時の速度倍率
    public Vector3 pm = new Vector3(0f, 0f, 0f); //プレイヤー移動用速度
    public Vector3 w = new Vector3(0f, 0f, 1f); //前移動ベクトル
    public Vector3 s = new Vector3(0f, 0f, -1f); //後ろ移動ベクトル
    public Vector3 d = new Vector3(1f, 0f, 0f); //右移動ベクトル
    public Vector3 a = new Vector3(-1f, 0f, 0f); //左移動用ベクトル
    public Vector3 jumping = new Vector3(0f, 30f, 0f); //ジャンプ用ベクトル
    public Vector3 gravity = new Vector3(0f, -1f, 0f); //重力用ベクトル
    bool forward; //前キー押し判定
    bool back; //後ろキー押し判定
    bool right; //右キー押し判定
    bool left; //左キー押し判定
    bool jump; //ジャンプキー押し判定
    bool dash; //ダッシュキー押し判定

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //各キー入力判定の初期化
        forward = false;
        back = false;
        right = false;
        left = false;
        jump = false;
        dash = false;

        //各種キーを押している場合、入力判定をtrueにする
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
        //rigidbodyで速度を変化させる下準備?
        Rigidbody rb1 = this.transform.GetComponent<Rigidbody>(); //velocity用にrigidbodyを取得

        //斜め移動時の速度調整
        if (forward||back)
        {
            if (right||left)
            {
                speed = 7; //斜め移動用のspeed
            }
            else
            {
                speed = 10; //四方移動のspeed
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

        //(キーを押していない場合の)四方向移動速度の初期化
        //ジャンプや重力による速度は維持しないといけないので、yは初期化しない
        pm.x = 0f;
        pm.z = 0f;

        //四方向移動の処理
        //押したキー(の入力判定)に応じて速度をベクトルpmに追加
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

        //ジャンプの処理
        //ray(不可視の光線)を下方向に発射し、地面と接触するかどうかで接地判定を行う
        Ray ray = new Ray(transform.position, -transform.up);
        {
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 1f)) //rayが接地してるか判定
            {
                pm.y = 0f; //接地中は落下速度を0にする

                if(jump)
                {
                    pm += jumping; //pmにジャンプ速度を加える
                }
            }
            else
            {
                pm += gravity; //非接地時は重力(相当の速度)を加える
                //AddForceは上手く使えなかったため、速度で代用
                //プログラム内で重力の処理を行っているため、
                //unityのrigidbodyのuse gravityはoffにすること
            }
        }

        rb1.velocity = pm; //ベクトルpmを速度として与える
    }
}

//現状だと、空中でも方向転換やダッシュのON・OFFが出来るので、
//必要かつ余裕があれば、その辺調整してもいいかも。
