using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[HideInInspector]
public struct Cell
{
    public float left;
    public float top;
    public float centreX;
    public float centreY;
    public int index;
}

public class Board01 : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    Text timer;
    [SerializeField]
    Text expression;
    [SerializeField]
    Button buttonPress;
    [SerializeField]
    Image resultsSprite;
    [SerializeField]
    SpriteRenderer gameOverImage;
    [SerializeField]
    AudioSource inCorrectSound;
    [SerializeField]
    Sprite correct;
    [SerializeField]
    Sprite incorrect;
    
    private BoxCollider2D box;
    private Transform boardPosition;
    private Color color = new Color(1f, 1f, 1f);
    private Score score;//Player score
    private Timer gameTime;//Game clock
    private const int COLUMNS = 10;
    private const int ROWS = 10;
    private Cell[,] board = new Cell[ROWS, COLUMNS];//Level playing board
    private float startX, startY;//Intial coordinates of top left corner of current cell
    private float cellWidth, cellHeight;//Size of each cell on board
    private int correctAnswers, incorrectAnswers;//Number of correct and incorrect answers
    private int correctIndex = 0;//Index of correct cell that player should select
	private int currentIndex = 1;//Indexof current cell in which player toekn is located
    private int playerRow = 0, playerCol = 0;//Row and column indices of correct cell clicked by player
    private float max = 7f, min = -7f;//Minimum and maximum integer values for integer expression
    private bool isPressed = false;//Indicates whether button generating integer expression was pressed 
	private bool gameOver = false;//Indicates whether the game has ended

    void Awake()
    {
        box = GetComponent<BoxCollider2D>();
        boardPosition = GetComponent<Transform>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        gameTime = GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>();
        SetRange();
        SetupBoard();
        SetGameInfo();
    }

    void Update ()
    {
         if((gameTime.count == 0 || currentIndex == 100) & !gameOver)
        {
            gameOver = true;
            buttonPress.enabled = false;
            PlayerPrefs.SetInt("NewGame", 1);
            gameOverImage.color = color;
            GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().SaveScore();
            PlayerPrefs.SetInt("Time", (int)gameTime.count);
            PlayerPrefs.SetInt("CorrectAnswers", correctAnswers);
            PlayerPrefs.SetInt("WrongAnswers", incorrectAnswers);
            gameTime.enabled = false;
            Invoke("Wait", 4f);
        }
        PlayerPrefs.SetInt("Row", playerRow);
        PlayerPrefs.SetInt("Column", playerCol);
        PlayerPrefs.SetInt("CorrectAnswers", correctAnswers);
        PlayerPrefs.SetInt("WrongAnswers", incorrectAnswers);
    }

    /*Calculates the position,centre and index of each cell on the board*/
    private void SetupBoard()
    {
        float sizeX = box.size.x;
        float sizeY = box.size.y;
        float scaleX = box.GetComponent<Transform>().lossyScale.x;
        float scaleY = box.GetComponent<Transform>().lossyScale.y;
        float halfwayRatio = 0.5f;
        startX = boardPosition.position.x - (sizeX * scaleX * halfwayRatio); //x coordinate of top left corner of the board
        startY = boardPosition.position.y + (sizeY * scaleY * halfwayRatio) ; //y coordinate of top left corner of the board
        float xPos = startX; //x coordinate of top left corner of current cell
        float yPos = startY; //y coordinate of top left corner of current cell
        int startIndex = 90;
        int index = 0;
        float centreX, centreY;
        cellWidth = sizeX * scaleX / ROWS;
        cellHeight = sizeY * scaleY / COLUMNS;
        for (int col = 0; col < COLUMNS; col++)
        {
            index = startIndex + col + 1;
            for (int row = ROWS - 1; row >= 0; row--)
            {
                centreX = xPos + (cellWidth * halfwayRatio);
                centreY = yPos - (cellHeight * halfwayRatio);
                board[row,col] = new Cell();
                board[row, col].left = xPos;
                board[row, col].top = yPos;
                board[row, col].centreX = centreX;
                board[row, col].centreY = centreY;
                board[row, col].index = index;
                index -= 10;
                yPos -= cellHeight;    
            }
            xPos += cellWidth;
            yPos = startY;
        }
    }
	
	//Intializes level properties
    private void SetGameInfo()
    {
        PlayerPrefs.SetInt("NewGame", 0);
        playerRow = PlayerPrefs.GetInt("Row", 0);
        playerCol = PlayerPrefs.GetInt("Column", 0);
        player.position = new Vector2(board[playerRow, playerCol].centreX, board[playerRow, playerCol].centreY);
        correctAnswers = PlayerPrefs.GetInt("CorrectAnswers", 0);
        incorrectAnswers = PlayerPrefs.GetInt("WrongAnswers", 0);
        currentIndex = board[playerRow, playerCol].index;
    }

    /*Places the player on the board based on where mouse click was deted inside the board*/
    void OnMouseDown()
    {
        if (!gameOver)
        {
            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float rightSideX = startX + box.size.x;
            float bottomSideY = startY - box.size.y;
            float touchX = touchPoint.x;
            float touchY = touchPoint.y;
            int playerChoice = -1;
            if (touchX > startX && touchX < rightSideX && touchY < startY && touchY > bottomSideY)
            {
                float rightX, bottomY;
                for (int row = 0; row < ROWS; row++)
                {
                    for (int col = 0; col < COLUMNS; col++)
                    {
                        rightX = board[row, col].left + cellWidth;
                        bottomY = board[row, col].top - cellHeight;
                        if (touchX > board[row, col].left && touchX < rightX && touchY < board[row, col].top && touchY > bottomY)
                        {
                            playerChoice = board[row, col].index;
                            checkIndex(playerChoice);
                            return;
                        }//if
                    }//for
                }//for
            }//if
        }//if
    }//end OnMouseDown
	
	//Check whether player clicked correct cell
    void checkIndex(int playerChoice)
    {
        buttonPress.enabled = true;
        if (isPressed)
        {
            if (playerChoice == correctIndex)
            {
                resultsSprite.sprite = correct;
                correctAnswers += 1;
                int temp = score.calculateScore(playerChoice);
                findCell(temp);
            }
            else if (playerChoice != correctIndex)
            {
                resultsSprite.sprite = incorrect;
                incorrectAnswers += 1;
                inCorrectSound.Play();
            }
           isPressed = false;
        }

    }
	
	//Finds the cell represnents by parm index
    void findCell(int index)
    {
        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col < COLUMNS; col++)
            {
                if (board[row, col].index == index)
                {
                    player.position = new Vector2(board[row, col].centreX, board[row, col].centreY);
                    playerRow = row;
                    playerCol = col;
                    currentIndex = index;
                    return;
                }
            }
        }
    }
	
	//Calculates the integer expression used to determine player movement
    public void GenerateExpression()
    {
        if (!isPressed)
        {
            resultsSprite.sprite = null;
            int ranNum1, ranNum2, tempNum, tempNum2, answer;
            do
            {
                ranNum1 = (int)Random.Range(min, max);
                ranNum2 = (int)Random.Range(min, max);
                answer = ranNum1 + ranNum2;
                tempNum = Mathf.Abs(ranNum1);
                tempNum2 = Mathf.Abs(ranNum2);
                correctIndex = currentIndex + answer;
            } while (correctIndex < 1 || correctIndex > 100);

            string exp = "";

            if (ranNum1 >= 0)
            {
                exp = "   " + tempNum;
            }
            else if (ranNum1 < 0)
            {
                exp = " - " + tempNum;
            }
            if (ranNum2 >= 0)
            {
                exp += " + " + tempNum2 + " = ";
            }
            else if (ranNum2 < 0)
            {
                exp += " - " + tempNum2 + " = ";
            }
            expression.text = exp;
            buttonPress.enabled = false; 
            isPressed = true;
        }//if
    }
	//Sets the range of integer values used to generate integer expression
    void SetRange()
    {
        if (PlayerPrefs.GetString("Difficulty").CompareTo("easy") == 0)
        {
            max = 7f;
            min = -7f;
        }
        else
        {
            max = 13f;
            min = -13f;
        }
    }
    
    //Time used to delay the next screen loading
    void Wait()
    {
        SceneManager.LoadScene("Results");
    }
}
