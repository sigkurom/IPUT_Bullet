using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//<<注意点!>>
//・プレイヤーにRigidbodyを付け、Constraints→Freeze RotateのX,Zにチェックを入れる。(Y軸回転以外を禁止)
//・適当なフォルダに、右クリック→Create→Pysical Materialを作成。
//　Dynamic Friction(移動してるオブジェクトに対する摩擦)と
//　Static Friction(静止してるオブジェクトに対する摩擦)を0にする。
//　このPysical Materialをプレイヤー、障害物の両方に付ける。
//　(これがないと、オブジェクトに接触した時に摩擦で謎回転する)

//Rigidbodyを使っての前後左右移動、左右回転、ジャンプダッシュまで
//WASDで四方向移動、QEで左右回転、左クリックでジャンプ、右クリックでダッシュ

//ただし他オブジェクトにぶつかった際に、(おそらく)物理演算で謎回転する不具合が発生。(解決済み)
//is Kinetic(物理演算不使用)をonにすれば、接触だけを利用できそうだが、
//transformしか使えなくなるため、プログラム打ち直し。
//→Unity上でPysical Materialを作り、Dynamic Friction(動いてるオブジェクトとの摩擦)と
//Static Friction(静止してるオブジェクトとの摩擦)を0にすれば、角度ズレ問題は解決した。
//プレイヤーとオブジェクトの両方にPysical Materialを付ける必要がある点に注意。

//ver103追記
//unityでのMesh Collidar(オブジェクトの形に合わせた接触判定を自動生成)の微妙な傾き？に対応するため、
//接地判定用のrayの長さを1f→1.01fに変更。
//坂道とか作ると、これを調整しないといけないと思われる。(長くし過ぎると、明らかに空中ジャンプできてしまうため注意)

//天井にぶつかっても上昇速度が殺されない(一瞬天井に当たってから横にずれると、再浮上する)不具合を修正。(上方接触時に、y速度を0にする)
//処理順的に、天井にぶつかったら落下が始まるようになっているはずだが、
//上方向のrayの長さや、周りの状況次第で空中浮遊バグとかの原因になりうるので、要注意。

//(当たり判定的に)中途半端な隙間があると、天井や接地判定が上手く機能しないため、
//場合によっては、あえて当たり判定(collidar)を大きめにする必要もありそう。
//一番マズいのが落下時で、物理的にはプレイヤーが通る隙間が無いので停止するが、
//真下にはオブジェクトが無い(=接地してない)ので、重力がかかり続ける(=内部的には落下速度が増え続ける)ことになる。
//これが続くとそのうち地面等をまとめてすり抜けて、謎空間に落ちていく。

//現在気になる事
//接地判定は、「下方向で他オブジェクトと触れているか」で判定しているため、
//想定外の状況でジャンプできる可能性がある。
//例えば、1.スキルで瓦礫を吹き飛ばす→2.その瓦礫に追いついて、空中で接触(=接地判定)→3.再度ジャンプ、など。
//まあ、1人で出来るかは怪しいし、「仲間の飛ばしたオブジェクト足場に二段ジャンプ」は、
//ある意味SFパルクールらしいかもしれないが。


//ここから細かい話、読み飛ばしてもOK
//Updateは、呼び出し間隔が一定ではないが、呼び出し頻度は高い(毎フレーム)
//FixedUpdateは、間隔は一定だが頻度は低い
//よって、移動キー入力判定はUpdateで、
//移動処理そのものはFixedUpdateで行うのが一般的らしい
//逆だと、移動がカクついたり(Updateの間隔が不定のため)
//キー入力に反応しなかったりする(FixedUpdateは毎フレーム確認するわけではないため)

public class Bullet_Player_0608_ver103 : MonoBehaviour
{
    //ここの変数は一度オブジェクトにスクリプトをくっつけると、
    //以降はunityのInspectorじゃないと変えられないようなので注意。
    //多分、オブジェクトからスクリプトを外して付けなおせば、
    //こちらの修正も反映されるが。
    public float speed; //移動速度
    public float dashspeed = 2; //ダッシュ時の速度倍率

    //transform.forwardによりお役御免
    //public float angle; //プレイヤーの角度取得用
    //public float p_rad; //プレイヤーの角度のラジアン変換用

    public Vector3 pm = new Vector3(0f, 0f, 0f); //プレイヤー移動用速度

    //transform.forward(正面ベクトル)等が使用できたため、コメントアウト
    //public Vector3 w = new Vector3(0f, 0f, 1f); //前移動ベクトル
    //public Vector3 s = new Vector3(0f, 0f, -1f); //後ろ移動ベクトル
    //public Vector3 d = new Vector3(1f, 0f, 0f); //右移動ベクトル
    //public Vector3 a = new Vector3(-1f, 0f, 0f); //左移動用ベクトル

    public Vector3 e = new Vector3(0f, 3f, 0f); //右回転用ベクトル
    public Vector3 q = new Vector3(0f, -3f, 0f); //左回転用ベクトル
    public Vector3 jumping = new Vector3(0f, 30f, 0f); //ジャンプ用ベクトル
    public Vector3 gravity = new Vector3(0f, -1f, 0f); //重力用ベクトル
    bool forward; //前キー押し判定
    bool back; //後ろキー押し判定
    bool right; //右キー押し判定
    bool left; //左キー押し判定
    bool rightturn; //右回転押し判定
    bool leftturn; //左回転押し判定
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
        rightturn = false;
        leftturn = false;
        jump = false;
        dash = false;

        //各種キーを押している場合、入力判定をtrueにする
        if (Input.GetKey(KeyCode.W))
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
        //rigidbodyで速度を変化させる下準備?
        Rigidbody rb0 = this.GetComponent<Rigidbody>(); //angularVelocity(回転)用のrigidbodyを取得?
        Rigidbody rb1 = this.transform.GetComponent<Rigidbody>(); //velocity用にrigidbodyを取得

        //斜め移動時の速度調整
        if (forward || back)
        {
            if (right || left)
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

        if (dash)
        {
            speed *= dashspeed;
        }

        //(キーを押していない場合の)四方向移動速度や回転速度の初期化
        //ジャンプや重力による速度は維持しないといけないので、yは初期化しない
        pm.x = 0f;
        pm.z = 0f;
        rb0.angularVelocity = new Vector3(0f, 0f, 0f);

        //回転の処理
        if (rightturn)
        {
            rb0.angularVelocity = e;
        }

        if (leftturn)
        {
            rb0.angularVelocity = q;
        }

        //左右同時に回転しようとすると停止
        if (rightturn && leftturn)
        {
            rb0.angularVelocity = new Vector3(0f, 0f, 0f);
        }

        //四方向移動の処理
        //押したキー(の入力判定)に応じて速度をベクトルpmに追加
        if (forward)
        {
            pm += this.transform.forward * speed;
        }

        if (back)
        {
            pm += -this.transform.forward * speed;
        }

        if (right)
        {
            pm += this.transform.right * speed;
        }

        if (left)
        {
            pm += -this.transform.right * speed;
        }

        Debug.Log(rightturn);

        //ジャンプの処理
        //ray(不可視の光線)を下方向に発射し、地面と接触するかどうかで接地判定を行う
        Ray under_ray = new Ray(transform.position, -transform.up);
        Ray upper_ray = new Ray(transform.position, transform.up);
        {
            RaycastHit hit;
            if(Physics.Raycast(upper_ray, out hit, 1.01f))
            {
                pm.y = 0f; //天井にぶつかった際に、上昇速度を0にする。下降時や空中浮遊に悪用されないか要テスト。
            }

            if (Physics.Raycast(under_ray, out hit, 1.01f)) //rayが接地してるか判定、unityのMesh collidarの微妙な傾きを無視するために、誤差レベルで接地判定を大きくしている。
            {
                pm.y = 0f; //接地中は落下速度を0にする

                if (jump)
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

                //想定外の落下速度上昇や、それに伴うすり抜け防止のために、速度上限を付けた方が良い？
            }
        }

        rb1.velocity = pm; //ベクトルpmを速度として与える
    }
}

//現状だと、空中でも方向転換やダッシュのON・OFFが出来るので、
//必要かつ余裕があれば、その辺調整してもいいかも。