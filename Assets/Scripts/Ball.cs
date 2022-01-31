using UnityEngine;
using System.Collections;

namespace Pong
{
    public class Ball : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float ballMoveSpeed;
        
        private float originalMoveSpeed; // Used for storing the default ballMoveSpeed value before a round start
        private bool greenLight; // When the ball will be allowed to move
        
        private int ballMoveDirectionY, ballMoveDirectionX;

        private GameController gameController;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            // Assign the default ballMoveSpeed value to a temporary field
            originalMoveSpeed = ballMoveSpeed;

            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

            BallStartingDirectionX();
            BallStartingDirectionY();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (greenLight)
            {
                BallMovement(ballMoveSpeed, ballMoveDirectionX, ballMoveDirectionY);
            }
        }

        private void BallMovement(float moveSpeed, int moveDirectionX, int moveDirectionY)
        {
            Vector2 movementBall = new Vector2(moveSpeed * moveDirectionX, (moveSpeed * moveDirectionY) / 2);
            transform.Translate(movementBall * Time.deltaTime);

            BallCollidingNorthSouth();
        }

        #region StartingDirection
        private int BallStartingDirectionX()
        {
            int randomHorizontalDirection = Random.Range(0, 2);

            if (randomHorizontalDirection > 0)
            {
                ballMoveDirectionX = 1;
            }
            else
            {
                ballMoveDirectionX = -1;
            }

            return ballMoveDirectionX;
        }
        private int BallStartingDirectionY()
        {
            int randomVerticalDirection = Random.Range(0, 2);

            if (randomVerticalDirection > 0)
            {
                ballMoveDirectionY = 1;
            }
            else
            {
                ballMoveDirectionY = -1;
            }

            return ballMoveDirectionY;
        }
        #endregion

        private void BallCollidingNorthSouth()
        {
            if (transform.position.y > 9.25f)
            {
                Debug.Log("Touched north");
                ballMoveDirectionY = -1;
            }
            if (transform.position.y < -9.25f)
            {
                Debug.Log("Touched south");
                ballMoveDirectionY = 1;
            }
        }

        #region Ball's status
        public void BallStandby()
        {
            greenLight = false;
            StopCoroutine(SpeedAccelerator());
        }
        public void BallGreenLight()
        {
            greenLight = true;
            StartCoroutine(SpeedAccelerator());
        }

        private IEnumerator SpeedAccelerator()
        {
            while (greenLight)
            {
                ballMoveSpeed += 0.5f;
                yield return new WaitForSeconds(0.5f);
            }
        }
        #endregion

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("PlayerRight"))
            {
                Debug.Log("Touched blue");
                ballMoveDirectionX = -1;
            }
            if (collision.collider.CompareTag("PlayerLeft"))
            {
                Debug.Log("Touched pink");
                ballMoveDirectionX = 1;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("BoundaryWest"))
            {
                gameController.AddScoreBlue();
                this.transform.position = new Vector2(0, 0);
                ballMoveSpeed = originalMoveSpeed;
            }
            if (collision.CompareTag("BoundaryEast"))
            {
                gameController.AddScorePink();
                this.transform.position = new Vector2(0, 0);
                ballMoveSpeed = originalMoveSpeed;
            }
        }
    }

}
