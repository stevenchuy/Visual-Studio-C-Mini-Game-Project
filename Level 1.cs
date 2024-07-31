using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace MultiLevel_Game_Project_V._3
{
    public partial class Level_1 : Form
    {
        int playerScore = 0; // variable to track player score
        int robotScore = 0; // variable to track robot score
        private HighestScore highestScore; // field for HighestScore
        float timeLeft = 30f;
        private List<Coin> coinsList = new List<Coin>();
        private Player player;
        private Robot robot;
        public Level_1()
        {
            InitializeComponent();
            FormSetUp();
            MakeCoin();
            player = new Player(this.ClientSize);
            robot = new Robot (this. ClientSize);
            highestScore = new HighestScore(); // initializing highestScore
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            player.Move();
            robot.Move(coinsList); // Update robot movement towards nearest coin
            CheckPlayerCoinGrab();
            CheckRobotCoinGrab(); // Check if the robot collects any coins

            playerLabel.Text = "Player Score: " + playerScore;
            robotLabel.Text = "Robot Score: " + robotScore;
            timerLabel.Text = "Time Left: " + timeLeft.ToString("#") + ".s";
            highestScoreLabel.Text = "Highest Score: " + highestScore.Score; // Display the highest score
            timeLeft -= 0.0333f;
            
            if (timeLeft <= 0 || playerScore == 10 || robotScore == 10)
            {
                GameOver();                
            }

            this.Invalidate(); // Ensure form gets redrawn
        }

        private void GameOver()
        {
            // Show a prompt for player name
            string playerName = Prompt.ShowDialog("Enter your name:", "Game Over");

            // Update high scores with the current player's score
            int finalScore = playerScore;
            highestScore.Update(playerName, finalScore);

            // Display top 3 high scores
            var topScores = highestScore.GetTopHighScores(3);
            DisplayHighScores(topScores); 

            this.Hide();

        }

        private void DisplayHighScores(List<HighScoreEntry> topScores)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Top 3 High Scores: ");

            foreach (var entry in topScores)
            {
                message.AppendLine($"{entry.PlayerName}: {entry.Score}");
            }
            MessageBox.Show(message.ToString(), "High Scores", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            player.HandleKeyPress(e.KeyCode);
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            player.HandleKeyRelease(e.KeyCode);
        }

        private void PaintEvent(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            ImageAnimator.UpdateFrames();
            player.Draw(canvas);
            robot.Draw(canvas);
            foreach (var coin in coinsList)
            {
                coin.Draw(canvas);
            }
        }

        private void FormSetUp()
        {
            this.BackgroundImage = Image.FromFile("backgroundLevel1.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
            //ImageAnimator.Animate(Player.PlayerImage, this.OnFrameChangedHandler);
            //ImageAnimator.Animate(Robot.RobotImage, this.OnFrameChangedHandler);
           // ImageAnimator.Animate(Coin.CoinImage, this.OnFrameChangedHandler);
            this.DoubleBuffered = true;
        }

        private void MakeCoin()
        {
            Random random = new Random();

            if (playerScore == 0 && robotScore == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Coin newCoin = new Coin(random.Next(10, this.ClientSize.Width - 10),
                                            random.Next(10, this.ClientSize.Height - 10));
                    coinsList.Add(newCoin);
                }
            }
            else
            {
                Coin newCoin = new Coin(random.Next(10, this.ClientSize.Width - 10),
                                            random.Next(10, this.ClientSize.Height - 10));
                coinsList.Add(newCoin);
            }

        }

        private void CheckPlayerCoinGrab()
        {

            foreach (var coin in coinsList.ToList())
            {
                if (player.IsCollidingWith(coin))
                {
                    coinsList.Remove(coin);
                    playerScore++;
                    highestScore.Update(playerScore); // update the highest score
                    MakeCoin();
                }
            }
        }
        private void CheckRobotCoinGrab()
        {
            foreach (var coin in coinsList.ToList())
            {
                if (robot.IsCollidingWith(coin))
                {
                    coinsList.Remove(coin);
                    robotScore++;
                    MakeCoin();
                    Debug.WriteLine("New coins created. Total coins: " + coinsList.Count);

                }
            }
        }

        private void OnFrameChangedHandler(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label { Left = 20, Top = 20, Text = text };
            TextBox textBox = new TextBox { Left = 20, Top = 50, Width = 250 };
            Button confirmation = new Button { Text = "Ok", Left = 200, Width = 70, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }

    [Serializable]
    public class HighScoreEntry
    {
        public string PlayerName { get; set; }
        public int Score { get; set; }
    }

    [Serializable]
    public class HighestScore
    {
        private List<HighScoreEntry> _highScores; // variable to track highest score
        private const string HighScoreFile = "highscore.json";

        //Constructor
        public HighestScore()
        {
            _highScores = new List<HighScoreEntry>(); 
            Load();            // Initialize highestScore or load it from a file
        }

        public void Update(string playerName, int newScore)
        {
            _highScores.Add(new HighScoreEntry
            {
                PlayerName = playerName,
                Score = newScore
            });

            _highScores = _highScores
                .OrderBy(entry => entry.Score)
                .Take(3)
                .ToList();

             Save();
          
        }

        private void Save()
        {
            string json = JsonSerializer.Serialize(_highScores);
            File.WriteAllText(HighScoreFile, json);
        }

        private void Load()
        {
            if (File.Exists(HighScoreFile))
            {
                string json = File.ReadAllText(HighScoreFile);
                _highScores = JsonSerializer.Deserialize<List<HighScoreEntry>>(json) ?? new List<HighScoreEntry> ();
            }
        }

        public List<HighScoreEntry> GetTopHighScores(int topN)
        {
            return _highScores.Take(topN).ToList();
        }
    }
    public class Player
    {
        public static Image PlayerImage = Image.FromFile("Space Ghost.gif");
        private int playerX, playerY, playerWidth, playerHeight, playerSpeed;
        private bool isUp, isDown, isLeft, isRight;
        private Size formSize;

        public Player(Size formSize)
        {
            this.formSize = formSize;
            playerX = 0;
            playerY = 0;
            playerWidth = 200;
            playerHeight = 100;
            playerSpeed = 35;
        }

        public void Move()
        {
            if (isUp && playerY > 0) playerY -= playerSpeed;
            if (isDown && playerY < formSize.Height - playerHeight) playerY += playerSpeed;
            if (isLeft && playerX > 0) playerX -= playerSpeed;
            if (isRight && playerX < formSize.Width - playerWidth) playerX += playerSpeed;
        }

        public void HandleKeyPress(Keys key)
        {
            switch (key)
            {
                case Keys.Up: isUp = true; break;
                case Keys.Down: isDown = true; break;
                case Keys.Left: isLeft = true; break;
                case Keys.Right: isRight = true; break;
            }
        }

        public void HandleKeyRelease(Keys key)
        {
            switch (key)
            {
                case Keys.Up: isUp = false; break;
                case Keys.Down: isDown = false; break;
                case Keys.Left: isLeft = false; break;
                case Keys.Right: isRight = false; break;
            }
        }

        public void Draw(Graphics canvas)
        {
            canvas.DrawImage(PlayerImage, playerX, playerY, playerWidth, playerHeight);
        }

        public bool IsCollidingWith(Coin coin)
        {
            Rectangle playerRect = new Rectangle(playerX, playerY, playerWidth - 100, playerHeight - 25);
            Rectangle coinRect = new Rectangle(coin.CoinX, coin.CoinY, coin.CoinWidth, coin.CoinHeight);
            return playerRect.IntersectsWith(coinRect);
        }
    }

    public class Robot
    {
        public static Image RobotImage { get; private set; }
        private int robotX, robotY, robotWidth, robotHeight, robotSpeed;
        private Size formSize;

        public Robot(Size formSize)
        {
            this.formSize = formSize;
            RobotImage = Image.FromFile("robot.gif"); // Load robot image
            robotX = 100; // Initial position
            robotY = 100; // Initial position
            robotWidth = 100; // Set dimensions
            robotHeight = 100;
            robotSpeed = 35; // Set speed
        }

        public void Move(List<Coin> coinsList)
        {
            if (coinsList.Count == 0)
                return;

            // Find the nearest coin
            Coin nearestCoin = FindNearestCoin(coinsList);

            // Move towards the nearest coin
            MoveTowards(nearestCoin);
        }

        private Coin FindNearestCoin(List<Coin> coinsList)
        {
            Coin nearestCoin = null;
            double minDistance = double.MaxValue;

            foreach (var coin in coinsList)
            {
                double distance = Math.Sqrt(Math.Pow(coin.CoinX - robotX, 2) + Math.Pow(coin.CoinY - robotY, 2));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCoin = coin;
                }
            }

            return nearestCoin;
        }

        private void MoveTowards(Coin coin)
        {
            if (coin == null)
                return;

            int deltaX = coin.CoinX - robotX;
            int deltaY = coin.CoinY - robotY;
            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (distance == 0)
                return; // Already at the coin

            // Move the robot towards the coin
            robotX += (int)(robotSpeed * (deltaX / distance));
            robotY += (int)(robotSpeed * (deltaY / distance));

            // Ensure the robot stays within bounds
            robotX = Math.Max(0, Math.Min(formSize.Width - robotWidth, robotX));
            robotY = Math.Max(0, Math.Min(formSize.Height - robotHeight, robotY));
        }

        public void Draw(Graphics canvas)
        {
            canvas.DrawImage(RobotImage, robotX, robotY, robotWidth, robotHeight);
        }

        public bool IsCollidingWith(Coin coin)
        {
            Rectangle robotRect = new Rectangle(robotX, robotY, robotWidth, robotHeight);
            Rectangle coinRect = new Rectangle(coin.CoinX, coin.CoinY, coin.CoinWidth, coin.CoinHeight);
            return robotRect.IntersectsWith(coinRect);
        }
    }
    public class Coin
    {
        public int CoinX { get; set; }
        public int CoinY { get; set; }
        public int CoinWidth { get; set; } = 100;
        public int CoinHeight { get; set; } = 100;

        public static Image CoinImage = Image.FromFile("coin.gif");
        public Coin(int x, int y)
        {
            CoinX = x;
            CoinY = y;
        }

        public void Draw(Graphics canvas)
        {
            canvas.DrawImage(CoinImage, CoinX, CoinY, CoinWidth, CoinHeight);
        }
    }

}
