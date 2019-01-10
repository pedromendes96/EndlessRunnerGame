using System.Collections;
using Client;
using Player.AnimatorState;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Player : MonoBehaviour, Subscriber
    {
        float distToGround;
        private bool isGrounded;

        private const string MOBILE_LEFT = "LEFT";
        private const string MOBILE_RIGHT = "RIGHT";
        private const string MOBILE_DOWN = "DOWN";
        private const string MOBILE_TOP = "TOP";
        private const string MOBILE_STOPPED = "STOPPED";
        private bool restart;

        public Canvas GameOverCanvas;

        public SceneLoaderMain SceneLoaderMain;
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
        
        private bool updatedData = false;
        private int verticalDirection = 0;
        private int horizontalDirection = 0;
        
        // Use this for initialization
        void Start()
        {
            restart = false;
            IsPlaying = true;
            _body = GetComponent<Rigidbody>();
            positonState = new MiddleState(this);
            animator = GetComponent<Animator>();
            animatorState = new OnFloorState(this);
            TCPClient.Instance.AddSubscriber(this);
            Debug.Log("TCPClient.Instance.IsConnected -> " + TCPClient.Instance.IsConnected);
        }
        
        public bool IsGrounded()
        {
            return isGrounded;
        }
        

        void Update(){
            if (restart)
            {
                SceneLoaderMain.LoadSceneAsync("Main");
            }
            if (TCPClient.Instance.IsConnected)
            {
                if(verticalDirection == 1){
                    if (!animatorState.CanJump()) return;
                    animatorState = new OnAscendingState(this);
                }else switch (horizontalDirection)
                {
                    case -1:
                        positonState.Left();
                        break;
                    case 1:
                        positonState.Right();
                        break;
                }
            }
            else
            {
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

        public void Update(string data)
        {
            Debug.Log("Received data: " + data);
            var position = data.Trim();
            switch (position)
            {
                case MOBILE_TOP:
                    verticalDirection = 1;
                    horizontalDirection = 0;
                    updatedData = true;
                    break;
                case MOBILE_DOWN:
                    verticalDirection = 0;
                    horizontalDirection = 0;
                    updatedData = true;
                    break;
                case MOBILE_RIGHT:
                    verticalDirection = 0;
                    horizontalDirection = 1;
                    updatedData = true;
                    break;
                case MOBILE_LEFT:
                    verticalDirection = 0;
                    horizontalDirection = -1;
                    updatedData = true;
                    break;
                case MOBILE_STOPPED:
                    verticalDirection = 0;
                    horizontalDirection = 0;
                    updatedData = true;
                    break;
                case "PLAY":
                    if (!IsPlaying)
                    {
                        restart = true;
                    }
                    break;
                default:
                    Debug.Log("Error in message -> " + position);
                    break;
            }
        }
        
        IEnumerator LoadYourAsyncScene(string name)
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        
        void OnApplicationQuit()
        {
            TCPClient.Instance.Close();
        }
    }
}
