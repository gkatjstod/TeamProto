using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class MoveAnimation : MonoBehaviour
{
    ///애니메이션 상태를 enum으로 체크
    enum ACTION_TYPE
    {
        ch_idle =0, //기본상태

        ch_leftmove, //왼쪽움직임
        ch_rightmove, //오른쪽 움직임
        ch_upmove,
        ch_downmove,

        ch_jump, //점프
        ch_attack, //공격
        ch_damage, //피격
        ch_dead, //뒤짐ㅎ
    }

    public enum ANGLE_TYPE
    {
        front = 0,
        right = 1,
        back = 2,
        left = 3,
    }
  
   
    public ANGLE_TYPE angletype;



    SkeletonAnimation SkeletonAnimation;
   
    Rigidbody Myrigidbody;
    BoxCollider HitboxCollider;

    public float speed; //캐릭터 속도
    public float force; //캐릭터 점프
    public int jumpcount = 2; //2단점프
    private bool isground = true; //바닥에 닿는지 검사
    private bool lookright = true; //캐릭터의 방향


    private float PrePadH;//이전LeftStickHorizontal
    private float PrePadV;//이전LeftStickVertical
    private float Intervel;//딜레이타임




    ///
    public float rotspeed = 5.0f;
    public float angle = 90.0f;
    private Quaternion _targetRot = Quaternion.identity;
    private bool _isForward = true;
    private bool _isReverse = false;//애니메이션 좌우반전 확인용
                                    /// 
    public int PlayerNumber;//플레이어넘버

    ACTION_TYPE atype;
    //현재는 쓰지않는 조건
    /*
      bool IsPadCenter() //L스틱중립
    {return ((PrePadH == 0) && (Input.GetAxisRaw("LeftStickHorizontal") == 0) && (Input.GetAxisRaw("LeftStickVertical") == 0) && (PrePadV == 0));}

        bool IsPadLPressing()//L스틱 좌입력중
    {return ((PrePadH == -1) && (Input.GetAxisRaw("LeftStickHorizontal") == -1));}
        bool IsPadRPressing()//L스틱 우입력중
    {return ((PrePadH == 1) && (Input.GetAxisRaw("LeftStickHorizontal") == 1));}
     */

        //사용중
    bool IsPadLDown()//L스틱 좌입력
    {
        switch (PlayerNumber)
        {
            case 1:
                return ((PrePadH > Input.GetAxisRaw("P1LeftStickHorizontal")) && (Input.GetAxisRaw("P1LeftStickHorizontal") < 0));
                break;
            case 2:
                return ((PrePadH > Input.GetAxisRaw("P2LeftStickHorizontal")) && (Input.GetAxisRaw("P2LeftStickHorizontal") < 0));
                break;
            case 3:
                return ((PrePadH > Input.GetAxisRaw("P3LeftStickHorizontal")) && (Input.GetAxisRaw("P3LeftStickHorizontal") < 0));
                break;
            case 4:
                return ((PrePadH > Input.GetAxisRaw("P4LeftStickHorizontal")) && (Input.GetAxisRaw("P4LeftStickHorizontal") < 0));
                break;
            default: break;
        }
        return false;
      //  return ((PrePadH > Input.GetAxisRaw("LeftStickHorizontal")) && (Input.GetAxisRaw("LeftStickHorizontal") < 0));
    }

    bool IsPadLUp()//L스틱 좌입력 해제
    {
        switch (PlayerNumber)
        {
            case 1:
                return ((PrePadH < 0) && (Input.GetAxisRaw("P1LeftStickHorizontal") == 0));
                break;
            case 2:
                return ((PrePadH < 0) && (Input.GetAxisRaw("P2LeftStickHorizontal") == 0));
                break;
            case 3:
                return ((PrePadH < 0) && (Input.GetAxisRaw("P3LeftStickHorizontal") == 0));
                break;
            case 4:
                return ((PrePadH < 0) && (Input.GetAxisRaw("P4LeftStickHorizontal") == 0));
                break;
            default: break;
        }
        return false;
        //return ((PrePadH < 0) && (Input.GetAxisRaw("LeftStickHorizontal")==0) );
    }

    

    bool IsPadRDown()//L스틱 우입력
    {
        switch (PlayerNumber)
        {
            case 1:
                return ((PrePadH < Input.GetAxisRaw("P1LeftStickHorizontal")) && (Input.GetAxisRaw("P1LeftStickHorizontal") > 0));
                break;
            case 2:
                return ((PrePadH < Input.GetAxisRaw("P2LeftStickHorizontal")) && (Input.GetAxisRaw("P2LeftStickHorizontal") > 0));
                break;
            case 3:
                return ((PrePadH < Input.GetAxisRaw("P3LeftStickHorizontal")) && (Input.GetAxisRaw("P3LeftStickHorizontal") > 0));
                break;
            case 4:
                return ((PrePadH < Input.GetAxisRaw("P4LeftStickHorizontal")) && (Input.GetAxisRaw("P4LeftStickHorizontal") > 0));
                break;
            default: break;
        }
        return false;
        //   return ((PrePadH < Input.GetAxisRaw("LeftStickHorizontal")) && (Input.GetAxisRaw("LeftStickHorizontal") >0));
    }

    bool IsPadRUp()//L스틱 우입력 해제
    {
        switch (PlayerNumber)
        {
            case 1:
                return ((PrePadH > 0) && (Input.GetAxisRaw("P1LeftStickHorizontal") == 0));
                break;
            case 2:
                return ((PrePadH > 0) && (Input.GetAxisRaw("P2LeftStickHorizontal") == 0));
                break;
            case 3:
                return ((PrePadH > 0) && (Input.GetAxisRaw("P3LeftStickHorizontal") == 0));
                break;
            case 4:
                return ((PrePadH > 0) && (Input.GetAxisRaw("P4LeftStickHorizontal") == 0));
                break;
            default: break;
        }
        return false;
        // return ((PrePadH > 0) && (Input.GetAxisRaw("LeftStickHorizontal") == 0));
    }



  

   

    
    void Start()
    {
        SkeletonAnimation = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        HitboxCollider = transform.GetChild(1).GetComponent<BoxCollider>();
        Myrigidbody = GetComponent<Rigidbody>();

        SetActionType(ACTION_TYPE.ch_idle);
        switch (PlayerNumber)
        {
            case 1:
                PrePadH = Input.GetAxisRaw("P1LeftStickHorizontal");
                break;
            case 2:
                PrePadH = Input.GetAxisRaw("P2LeftStickHorizontal");
                break;
            case 3:
                PrePadH = Input.GetAxisRaw("P3LeftStickHorizontal");
                break;
            case 4:
                PrePadH = Input.GetAxisRaw("P4LeftStickHorizontal");
                break;
            default: break;
        }
        
        PrePadV = Input.GetAxisRaw("LeftStickVertical");
        Intervel = 0.0f;
       // PlayerNumber = 0;
        angletype = ANGLE_TYPE.front;
        //angletype = front;
    }

    ///ACTION_TYPE별 애니메이션 정의
    void SetActionType(ACTION_TYPE type)
    {
        atype = type;
        switch(atype)
        {
            case ACTION_TYPE.ch_idle:
                { SkeletonAnimation.state.SetAnimation(0, "idle", true); }
                break;

            case ACTION_TYPE.ch_leftmove:
                {
                    SkeletonAnimation.skeleton.FlipX = true; ///좌우반전
                    SkeletonAnimation.state.SetAnimation(0, "run", true);
                }
                break;

            case ACTION_TYPE.ch_rightmove:
                {
                    SkeletonAnimation.skeleton.FlipX = false; ///좌우반전
                    SkeletonAnimation.state.SetAnimation(0, "run", true);
                }
                break;

            case ACTION_TYPE.ch_upmove:
                {
                    SkeletonAnimation.skeleton.FlipX = false; ///좌우반전
                    SkeletonAnimation.state.SetAnimation(0, "run", true);
                }
                break;

            case ACTION_TYPE.ch_downmove:
                {
                    SkeletonAnimation.skeleton.FlipX = true; ///좌우반전
                    SkeletonAnimation.state.SetAnimation(0, "run", true);
                }
                break;
                

            default: break;
        }
    }

    ///실질적 액션 변경함수, 한 번만 실행
    private void InputAction()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)|| IsPadLDown())//L스틱 좌입력
        {
            lookright = false;
            SetActionType(ACTION_TYPE.ch_leftmove);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow)|| IsPadRDown())//L스틱 우입력
        {
            lookright = true;
            SetActionType(ACTION_TYPE.ch_rightmove);
        }

        //if (Input.GetKeyDown(KeyCode.UpArrow) || IsPadLDown())//L스틱 좌입력
        //{
        //    lookright = false;
        //    SetActionType(ACTION_TYPE.ch_upmove);
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow) || IsPadRDown())//L스틱 우입력
        //{
        //    lookright = true;
        //    SetActionType(ACTION_TYPE.ch_downmove);
        //}
        /*
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (lookright==true) HitboxCollider.center = new Vector3(1.5f, 1.5f, 0f);
            else HitboxCollider.center = new Vector3(-1.5f, 1.5f, 0f);
            Hitbox.SetActive(true);
            SetActionType(ACTION_TYPE.ch_attack);
            Invoke("MoveReset", 0.4f);
        }
        */

        if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)||
           Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || IsPadLUp()||IsPadRUp())   //L스틱 좌우입력해제시
        {
            if (atype == ACTION_TYPE.ch_jump) return;
            SetActionType(ACTION_TYPE.ch_idle);
        }
    }

    ///움직임 업데이트
    void ActionUpdate()
    {
        Vector3 pos = transform.position;
        switch (atype)
        {
            case ACTION_TYPE.ch_leftmove:
                {
                    if(angletype==ANGLE_TYPE.front)
                        pos.x -= Time.deltaTime * speed;
                    if (angletype == ANGLE_TYPE.right)
                        pos.z -= Time.deltaTime * speed;
                    if (angletype == ANGLE_TYPE.back)
                        pos.x += Time.deltaTime * speed;
                    if (angletype == ANGLE_TYPE.left)
                        pos.z += Time.deltaTime * speed;
                }
                break;
            case ACTION_TYPE.ch_rightmove:
                {
                    if (angletype == ANGLE_TYPE.front)
                       pos.x += Time.deltaTime * speed;
                    if (angletype == ANGLE_TYPE.right)
                        pos.z += Time.deltaTime * speed;
                    if (angletype == ANGLE_TYPE.back)
                        pos.x -= Time.deltaTime * speed;
                    if (angletype == ANGLE_TYPE.left)
                        pos.z -= Time.deltaTime * speed;
                }
                break;

            case ACTION_TYPE.ch_upmove:
                {
                    pos.z -= Time.deltaTime * speed;
                }
                break;
            case ACTION_TYPE.ch_downmove:
                {
                    pos.z += Time.deltaTime * speed;
                }
                break;

                
            default: break;
        }
        transform.position = pos;
    }

    // 좌우 캐릭터 방향전환
    private void ChangeAngle()
    {
        if (Input.GetKeyDown(KeyCode.Q)||Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (angletype == ANGLE_TYPE.front)
            {
                if (_isReverse ^ false)//정상
                    _targetRot *= Quaternion.AngleAxis(_isForward ? -90 : 90, Vector3.up);
                else//반전
                    _targetRot *= Quaternion.AngleAxis(_isForward ? -270 : 270, Vector3.up);
                angletype = ANGLE_TYPE.left;
            }
            else
            {
                if (_isReverse ^ false)//정상
                    _targetRot *= Quaternion.AngleAxis(_isForward ? -90 : 90, Vector3.up);
                else//반전
                    _targetRot *= Quaternion.AngleAxis(_isForward ? -270 : 270, Vector3.up);
                angletype -= 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (angletype == ANGLE_TYPE.left)
            {
                if (_isReverse ^ false)//정상
                    _targetRot *= Quaternion.AngleAxis(_isForward ? -90 : 90, Vector3.down);
                else//반전
                    _targetRot *= Quaternion.AngleAxis(_isForward ? -270 : 270, Vector3.down);
                angletype = ANGLE_TYPE.front;
            }

            else
            {
                if (_isReverse ^ false)//정상
                    _targetRot *= Quaternion.AngleAxis(_isForward ? -90 : 90, Vector3.down);
                else//반전
                    _targetRot *= Quaternion.AngleAxis(_isForward ? -270 : 270, Vector3.down);
                angletype += 1;

            }
        }

    }

    void Update()
    {
        Intervel += Time.deltaTime;
        InputAction();
        ActionUpdate();
        if (Intervel > 0.1f)
        {

            switch (PlayerNumber)
            {
                case 1:
                    PrePadH = Input.GetAxisRaw("P1LeftStickHorizontal");
                    break;
                case 2:
                    PrePadH = Input.GetAxisRaw("P2LeftStickHorizontal");
                    break;
                case 3:
                    PrePadH = Input.GetAxisRaw("P3LeftStickHorizontal");
                    break;
                case 4:
                    PrePadH = Input.GetAxisRaw("P4LeftStickHorizontal");
                    break;
                default: break;
            }
            PrePadV = Input.GetAxisRaw("LeftStickVertical");
            Intervel = 0.0f;
        }


        ///
        ChangeAngle();
        

        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, rotspeed * Time.deltaTime);
        ///

        Debug.Log(PrePadH);
        Debug.Log(PrePadV);

        Debug.Log(angletype);


    }

    
}
