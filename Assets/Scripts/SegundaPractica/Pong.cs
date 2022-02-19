using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Pong : MonoBehaviour
{
    private int ScreenWidth;
    private int ScreenHeight;
    public PongPlayer Player1;
    public PongPlayer Player2;
    public int moveSpeed;
    public PongBall Ball;
    public int ballSpeed;

    public PixelScreen screen;
    public Transformaciones2D T2D;
    public FigureLoader loader;
    public Text Player1PointsTxt;
    public Text Player2PointsTxt;

    private Task ballMove;


    private void Start()
    {
        if (screen.textureSize == Vector2.zero)
        {
            screen.ResetPixels();
        }
        ScreenWidth = screen.textureSize.x;
        ScreenHeight = screen.textureSize.y;

        startGame(0, 0);
    }

    private void OnEnable()
    {
        if (screen.textureSize == Vector2.zero)
        {
            screen.ResetPixels();
        }
        ScreenWidth = screen.textureSize.x;
        ScreenHeight = screen.textureSize.y;

        startGame(0, 0);
    }


    private void FixedUpdate()
    {
        playerMovement();

        screen.clearScreen();
        T2D.drawFigure(Player1.fig);
        T2D.drawFigure(Player2.fig);
        T2D.drawFigure(Ball.fig);
    }

    private void playerMovement()
    {
        if (Input.GetKey(KeyCode.W) && Player1.position.y <= ((ScreenHeight / 2) - 4))
        {
            T2D.TranslateFigure(Player1.fig, Vector2Int.up * moveSpeed);
            Player1.position += Vector2Int.up * moveSpeed;
            Player1.direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) && Player1.position.y >= -((ScreenHeight / 2) - 4))
        {
            T2D.TranslateFigure(Player1.fig, Vector2Int.down * moveSpeed);
            Player1.position += Vector2Int.down * moveSpeed;
            Player1.direction = Vector2.down;
        } else
        {
            Player1.direction = Vector2.zero;
        }

        if (Input.GetKey(KeyCode.UpArrow) && Player2.position.y <= ((ScreenHeight / 2) - 4))
        {
            T2D.TranslateFigure(Player2.fig, Vector2Int.up * moveSpeed);
            Player2.position += Vector2Int.up * moveSpeed;
            Player2.direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Player2.position.y >= -((ScreenHeight / 2) - 4))
        {
            T2D.TranslateFigure(Player2.fig, Vector2Int.down * moveSpeed);
            Player2.position += Vector2Int.down * moveSpeed;
            Player2.direction = Vector2.down;
        } else
        {
            Player2.direction = Vector2.zero;
        }
    }

    private async Task ballMovementAsync()
    {
        await Task.Delay(2000);

        Ball.speed = Vector2Int.right;

        int TimeToWait = ballSpeed * 2;

        while (gameObject.activeSelf)
        {

            await Task.Delay(TimeToWait);

            //Wall Collision
            if (Ball.position.x >= ScreenWidth / 2)
            {
                startGame(Player1.score + 1, Player2.score);
                return;
            }
            else if (Ball.position.x <= -ScreenWidth / 2)
            {
                startGame(Player1.score, Player2.score + 1);
                return;
            }

            if (Ball.position.y >= ScreenHeight / 2 || Ball.position.y <= -ScreenHeight / 2)
            {
                Ball.speed.y *= -1;
            }

            //Paddle Collisions
            if (ballCollides(Player1))
            {
                Ball.speed.x *= -1;
                Ball.speed.y += Ball.speed.y != 0 ? Mathf.Abs(Ball.speed.y) * (int)Player1.direction.y : Random.Range(-1, 2);

            }
            else if (ballCollides(Player2))
            {
                Ball.speed.x *= -1;
                Ball.speed.y += Ball.speed.y != 0 ? Mathf.Abs(Ball.speed.y) * (int)Player2.direction.y : Random.Range(-1, 2);
            }


            T2D.TranslateFigure(Ball.fig, Ball.speed);
            Ball.position += Ball.speed;
        }
    }

    private bool ballCollides(PongPlayer player)
    {
        
        if ((player.position.x == Ball.position.x || player.position.x - 1 == Ball.position.x))
        {
            if (player.fig.vertices[0].y < Ball.position.y && player.fig.vertices[1].y > Ball.position.y)
            {
                return true;
            }
        }
        

        return false;
    }

    private void startGame(int scoreP1, int scoreP2)
    {
        Figura figP1 = new Figura("Player1");
        foreach (var item in loader.loadPongFigure("Player_1").vertices)
        {
            figP1.vertices.Add(item);
        }
        figP1 = T2D.TranslateFigure(figP1, new Vector2Int((-ScreenWidth / 2) + 2, 0));

        Figura figP2 = new Figura("Player2");
        foreach (var item in loader.loadPongFigure("Player_2").vertices)
        {
            figP2.vertices.Add(item);
        }
        figP2 = T2D.TranslateFigure(figP2, new Vector2Int((ScreenWidth / 2) - 2, 0));

        Player1 = new PongPlayer(new Vector2Int((-ScreenWidth / 2) + 2, 0), figP1, moveSpeed, scoreP1);
        Player2 = new PongPlayer(new Vector2Int((ScreenWidth / 2) - 2, 0), figP2, moveSpeed, scoreP2);

        Figura ballFig = new Figura("Ball");
        foreach (var item in loader.loadPongFigure("Ball").vertices)
        {
            ballFig.vertices.Add(item);
        }
        Ball = new PongBall(new Vector2Int(0, 0), ballFig);

        Player1PointsTxt.text = $"{Player1.score}";
        Player2PointsTxt.text = $"{Player2.score}";

        ballMove = ballMovementAsync();
    }
}

public class PongPlayer
{
    public Vector2Int position;
    public Vector2 direction;
    public Figura fig;
    public int speed;
    public int score;

    public PongPlayer(Vector2Int initialPosition, Figura fig, int speed, int score)
    {
        this.position = initialPosition;
        this.fig = fig;
        this.speed = speed;
        this.direction = Vector2.zero;
        this.score = score;
    }
}

public class PongBall
{
    public Vector2Int position;
    public Vector2Int speed;
    public Figura fig;
    public PongBall(Vector2Int initialPosition, Figura fig)
    {
        this.position = initialPosition;
        this.fig = fig;
        speed = Vector2Int.zero;
    }
}
