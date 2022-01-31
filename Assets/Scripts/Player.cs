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
        
        [SerializeField] private KeyCode moveUp;
        [SerializeField] private KeyCode moveDown;


        [Header("Is Human or Computer")]
        [Tooltip("Check this box if the object is the player, uncheck if it is the computer")]
        [SerializeField] private bool isHumanOrComputer; // Human if true, Computer if false
        
        private Ball ball;
        private Player humanPlayer;
        #endregion

        private void Start()
        {
            if (!isHumanOrComputer)
            {
                ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
                humanPlayer = GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<Player>();
                computerMoveSpeed = playerMoveSpeed - 2f;
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
                    ComputerVelocity(computerMoveSpeed);
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
                    ComputerVelocity(computerMoveSpeed);
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
        private void ComputerVelocity(float moveSpeed)
        {
            Vector2 movement = new Vector2(0, ball.transform.position.y);
            Vector2 velocity = movement;
            transform.Translate(velocity * Time.deltaTime);
        }
        // This coroutine will be activated after it touched the ball (**only use for the AI**)
        //private IEnumerator RandomMovement()
        //{
        //    for(int i = 0; i < 2; i++)
        //    {
        //        float randomMoveDirection = Random.Range(0f, 1f);
        //        if(randomMoveDirection < 0.5f)
        //        {
        //            computerMoveDirection = -1;
        //        }
        //        else
        //        {
        //            computerMoveDirection = -1f;
        //        }
        //    }
        //        yield return new WaitForSeconds(2f);
        //}
        #endregion

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //if (!isHumanOrComputer)
            //{
            //    if (collision.collider.CompareTag("Ball"))
            //    {
            //        computerMoveSpeed = 10f;
            //        StartCoroutine(RandomMovement());
            //    }
            //}
        }

    }
}
