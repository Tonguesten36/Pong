using System.Collections;
using UnityEngine;

namespace Pong
{
    // TODO: AI OPPONENT IMPLEMENTATION
    public class Player : MonoBehaviour
    {
        #region Variables
        [Header("General Stats")]
        [SerializeField] private float playerMoveSpeed;
        
        private float computerMoveSpeed;
        private float offset = 1f;
        
        [SerializeField] private KeyCode moveUp;
        [SerializeField] private KeyCode moveDown;


        [Header("Is Human or Computer")]
        [Tooltip("Check this box if the object is the player, uncheck if it is the computer")]
        [SerializeField] private bool isHumanOrComputer; // Human if true, Computer if false
        
        private Ball ball;
        private Player humanPlayer;
        private Rigidbody2D rigidbodyAI;

        #endregion

        private void Start()
        {
            if (!isHumanOrComputer)
            {
                ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
                humanPlayer = GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<Player>();
                rigidbodyAI = GetComponent<Rigidbody2D>();

                computerMoveSpeed = 30f;
                
                if(ball == null)
                {
                    Debug.LogError("whoops");
                    throw new UnityException("Couldn't find" + ball.name);
                }
                if(humanPlayer == null)
                {
                    Debug.LogError("player not found");
                    throw new UnityException("Couldn't find" + humanPlayer.name);
                }
                if(rigidbodyAI == null){
                    Debug.LogError("rigidbody not found on ai");
                }
                else{
                    Debug.Log("Found the rigidbody on ai");
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (isHumanOrComputer)
            {
                if(Input.GetKey(moveUp) || Input.GetKey(moveDown))
                {
                    Movement(true, playerMoveSpeed);
                }
            }
            else
            {
                Movement(false, playerMoveSpeed);
            }
        }

        private void Movement(bool isHumanPlayer, float playerMoveSpeed)
        {
            // Limiting the player's movement within the boundary
            float limitYPos = Mathf.Clamp(this.transform.position.y, -7.5f, 7.5f);
            this.transform.position = new Vector2(this.transform.position.x, limitYPos);

            if (this.gameObject.CompareTag("PlayerRight"))
            {
                if (isHumanPlayer)
                {
                    HumanVelocity("BlueVertical", playerMoveSpeed);
                }
                else
                {
                    ComputerMovement();
                }
            }
            else if(this.gameObject.CompareTag("PlayerLeft"))
            {
                if (isHumanPlayer)
                {
                    HumanVelocity("PinkVertical", playerMoveSpeed);
                }
                else
                {
                    ComputerMovement();
                }
            }
        }

        #region Player
        private void HumanVelocity(string verticalAxis, float moveSpeed)
        {
            float verticalMovement = Input.GetAxis(verticalAxis);
            Vector2 movement = new Vector2(0, verticalMovement);
            Vector2 velocity = movement * moveSpeed;
            transform.Translate(velocity * Time.deltaTime);
        }
        #endregion
        #region Computer      
        //AI Movement
        private void ComputerMovement()
        {
            if (ball.transform.position.y > this.transform.position.y)
            {
                if (rigidbodyAI.velocity.y < 0) rigidbodyAI.velocity = Vector2.zero;
                rigidbodyAI.velocity = Vector2.Lerp(rigidbodyAI.velocity, Vector2.up * computerMoveSpeed * offset, 1f * Time.deltaTime);
            }
            else if (ball.transform.position.y < this.transform.position.y)
            {
                if (rigidbodyAI.velocity.y > 0) rigidbodyAI.velocity = Vector2.zero;
                rigidbodyAI.velocity = Vector2.Lerp(rigidbodyAI.velocity, Vector2.down * computerMoveSpeed * offset, 1f * Time.deltaTime);
            }
            else
            {
                rigidbodyAI.velocity = Vector2.Lerp(rigidbodyAI.velocity, Vector2.zero * computerMoveSpeed * offset, 1f * Time.deltaTime);
            }
        }
        private void OnCollisionEnter(Collision other) 
        {
            if(!isHumanOrComputer)
            {
                offset = Random.Range(0.5f, 1f);
            }
        }

        #endregion

    }
}
