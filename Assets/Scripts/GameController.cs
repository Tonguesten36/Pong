using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Pong
{
    public class GameController : MonoBehaviour
    {
        #region Variables
        [Space]
        [Header("Game Controller")]
        [Tooltip("Determines if the GameController is in a game or in the game's menu")]
        [SerializeField] private bool isGame; // Determines if the GameController is in a game or in the game's menu
        
        [Tooltip("Check if the GameController is in a PVP match, uncheck if it is in a PVE match")]
        [SerializeField] private bool isPVP;

        private int maxScoreRequired = 5;
        private int scoreBlue;
        private int scorePink;
        private int roundCountdown;

        [Space]
        [Header("UI Manager")]
        private TextMeshProUGUI blueScoreDisplay;
        private TextMeshProUGUI pinkScoreDisplay;
        private TextMeshProUGUI roundCountdownDisplay;

        [Space]
        [Header("Ball")]
        protected Ball ball;

        [Space]
        [Header("Player")]
        private Player playerLeft;
        private Player playerRight;
        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            if (isGame) // This code block will execute when GameController.cs is in the game scene (pvp or pve)
            {
                scoreBlue = 0;
                scorePink = 0;

                ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();

                blueScoreDisplay = GameObject.FindGameObjectWithTag("BlueScoreDisplay").GetComponent<TextMeshProUGUI>();
                pinkScoreDisplay = GameObject.FindGameObjectWithTag("PinkScoreDisplay").GetComponent<TextMeshProUGUI>();

                roundCountdownDisplay = GameObject.FindGameObjectWithTag("RoundCountdownDisplay").GetComponent<TextMeshProUGUI>();

                playerLeft = GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<Player>();
                playerRight = GameObject.FindGameObjectWithTag("PlayerRight").GetComponent<Player>();

                // Check if these GameObjects are found, if not then throw an exception
                NullCheck<GameObject>(playerLeft.gameObject, playerRight.gameObject, ball.gameObject, blueScoreDisplay.gameObject, pinkScoreDisplay.gameObject, roundCountdownDisplay.gameObject);

                // Initiate the first round
                StartCoroutine(NewRoundCountdown());
            }
            else // This code blocks got executed when GameController.cs is in the game's menu scene and not the game scene (either pvp or pve)
            {
                // There is nothing here lol, for now...
            }
        }

        private void Update()
        {
            if (isGame)
            {
                if(scoreBlue == maxScoreRequired || scorePink == maxScoreRequired)
                {
                    if(scoreBlue == maxScoreRequired) 
                    {
                        blueScoreDisplay.text = "Blue wins!";
                        pinkScoreDisplay.text = "";
                    }
                    else if(scorePink == maxScoreRequired)
                    {
                        pinkScoreDisplay.text = "Pink wins!";
                        blueScoreDisplay.text = "";
                    }
                    roundCountdownDisplay.text = "Press F to go back to main menu";
                    ball.BallStandby();
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        SceneManager.LoadScene(0);
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Debug.Log("Quit");
                    Application.Quit();
                }
            }
        }

        #region Main Menu
        public void TwoPlayerButton()
        {
            Debug.Log("2P");
            SceneManager.LoadScene(1);
        }
        public void OnePlayerButton()
        {
            Debug.Log("1P");
            SceneManager.LoadScene(2);
        }
        #endregion

        #region Main Game
        private IEnumerator NewRoundCountdown()
        {
            roundCountdown = 3;
            ball.BallStandby();

            playerLeft.transform.position = new Vector2(playerLeft.transform.position.x, 0);
            playerRight.transform.position = new Vector2(playerRight.transform.position.x, 0);

            for(int i = 0; i < 3; i++)
            {
                roundCountdownDisplay.text = roundCountdown.ToString();
                yield return new WaitForSeconds(1f);
                roundCountdown--;
            }
            if(roundCountdown == 0)
            {
                roundCountdownDisplay.text = "GO!!";
                yield return new WaitForSeconds(1f);
                ball.BallGreenLight();
                roundCountdownDisplay.text = "";
            }
        }
       
        #region Add Score
        public void AddScoreBlue()
        {
            Debug.Log("Blue scored!");
            scoreBlue += 1;
            blueScoreDisplay.text = scoreBlue.ToString();
   
            if(scoreBlue < 5)
            {
                StartCoroutine(NewRoundCountdown());
            }
        }
        public void AddScorePink()
        {
            Debug.Log("Pink scored!");
            scorePink += 1;
            pinkScoreDisplay.text = scorePink.ToString();
            
            if(scorePink < 5) 
            {
                StartCoroutine(NewRoundCountdown());
            }
        }
        #endregion
        #endregion

        public static void NullCheck<T>(params T[] entity)
        {
            int maxArraySize = entity.Length;

            for(int i = 0; i < maxArraySize; i++)
            {
                string entityName = entity[i].ToString();
                if (entity[i] == null)
                {
                    Debug.LogError("Couldn't find " + entityName);
                    throw new UnityException("Couldn't find" + entityName);
                }
            }
        }
    }
}
