using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Player.AnimatorState;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Player : MonoBehaviour
    {
        float distToGround;
        private bool isGrounded;

        public Canvas GameOverCanvas;
        
        public bool IsPlaying { get; set; }

        public float fallingSpeed;
        public float moveSpeed;
        public float jumpSpeed;
        public float airSpeed;
        public float speed;
        // public float expectedPosition = 0;
        public float expectedPosition;
        private IState positonState;

        private IAnimatorState animatorState;

        // Animations
        private Animator animator;
        private Rigidbody _body;

        // Use this for initialization
        void Start()
        {
            IsPlaying = true;
            _body = GetComponent<Rigidbody>();
            positonState = new MiddleState(this);
            animator = GetComponent<Animator>();
            animatorState = new OnFloorState(this);
        }
        
        public bool IsGrounded()
        {
            return isGrounded;
        }
        

        void Update(){
            if(Input.GetKeyDown(KeyCode.UpArrow)){
                if(animatorState.CanJump()){
                    animatorState = new OnAscendingState(this);
                }
            }else if(Input.GetKeyDown(KeyCode.LeftArrow)){
                positonState.Left();
            }else if(Input.GetKeyDown(KeyCode.RightArrow)){
                positonState.Right();
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Block"))
            {
                if (transform.position.z < collision.gameObject.transform.position.z)
                {
                    var blockSizeY = collision.collider.bounds.size.y;

                    if (gameObject.transform.position.y < (collision.collider.transform.position.y + (blockSizeY / 2) - 0.3))
                    {
                        GameOverAction();
                    }
                }
            }
        
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Block"))
            {
                if (transform.position.z < collision.gameObject.transform.position.z)
                {
                    var blockSizeY = collision.collider.bounds.size.y;

                    if (gameObject.transform.position.y < (collision.collider.transform.position.y + (blockSizeY / 2)  - 0.3))
                    {
                        GameOverAction();
                    }
                }
            }
            //Debug.Log("Entered");
            //Debug.Log("Collision X: " + collision.transform.position.x);
            if(collision.transform.position.x != positonState.XPosition[IState.POSITION.CENTER]){
                var positions = positonState.XPosition;
                var axeX = collision.transform.position.x;
                positions[IState.POSITION.LEFT] = axeX - 2.5f;
                positions[IState.POSITION.CENTER] = axeX;
                positions[IState.POSITION.RIGHT] = axeX + 2.5f;
                if(collision.transform.position.x > transform.position.x){
                    positonState = new LeftState(this,positions); 
                }else{
                    positonState = new RightState(this,positions);
                }
            }
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        public void GameOverAction()
        {
            animatorState = new OnLoseState(this);
            transform.rotation = new Quaternion(0,180,0,0);
            GameOverCanvas.gameObject.SetActive(true);
            IsPlaying = false;
        }

        void OnCollisionExit(Collision collision)
        {
            //Debug.Log("Exited");
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            animatorState.UpdatePosition();
            // float moveHorizontal = Input.GetAxis ("Horizontal");
            // float moveVertical = Input.GetAxis ("Vertical");
    
            // Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
    
            // _body.AddForce (movement * speed);
        }

        public void UpdateAnimatorState(IAnimatorState _animatorState){
            animatorState = _animatorState;
        }

        public void UpdateExpectedPosition(float dest)
        {
            expectedPosition = dest;
        }

        public void UpdateState(IState newState)
        {
            positonState = newState;
        }

        public Rigidbody GetRigidbody(){
            return _body;
        }

        public Animator GetAnimator(){
            return animator;
        }
    }
}
