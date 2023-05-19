using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Space_Race
{
    public partial class Spacerace : Form
    {

        string state = "waiting";
        string winner;

        //P1
        Rectangle player1 = new Rectangle(150, 250, 10, 10);
        
        //P2
        Rectangle player2 = new Rectangle(450, 250, 10, 10);

        //timer line
        Rectangle timerLine = new Rectangle(300,0,10,300);

        Rectangle timerLine2 = new Rectangle(300, 0, 10, 300);

        // player variables
        int playerSpeed = 8;
        int playerSize = 15;

        // Player score
        int p1score = 1;
        int p2score = 1;
        

        // meteor meteor
        int meteorSpeed = 5;
        int meteorhigth = 15;
        int meteorswidth = 5; 

        // list of meteor
        List<Rectangle> meteorList = new List<Rectangle>();

        int score = 0;
        int time = 1;

             
        bool sDown = false; 
        bool wDown = false;
        bool arrowupDown = false;
        bool arrowdownDown = false;


        SolidBrush yellowBrush = new SolidBrush(Color.FromArgb(255,237, 255, 0));
        SolidBrush blackBrush = new SolidBrush(Color.Black);
        SolidBrush pinkBrush = new SolidBrush(Color.FromArgb(255,255,62,165));

        Random randGen = new Random();
        int randValue = 0;

        public Spacerace()
        {
            InitializeComponent();
        }

        public void InitializeGame()
        {
            time = 1;
            score = 0;
            

            meteorList.Clear();
            //P1
            player1 = new Rectangle(150, 250, 10, 10);
            //P2
            player2 = new Rectangle(450, 250, 10, 10);
            //timer line
            timerLine = new Rectangle(300, 0, 10, 300);


            // player 1 side
            Rectangle meteor1 = new Rectangle(0, randValue, meteorhigth, meteorswidth);
            meteorList.Add(meteor1);
            //player 2 side
            Rectangle meteor2 = new Rectangle(310, randValue, meteorhigth, meteorswidth);
            meteorList.Add(meteor2);


            titleLabel.Text = "";
            subtitleLabel.Text = "";
            gameTimer.Enabled = true;
            state = "playing";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown= true;
                    break;
                case Keys.Up:
                    arrowupDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Down:
                    arrowdownDown = true;
                    break ;
                case Keys.Space:
                    if (state == "waiting" || state == "over")
                    {
                        InitializeGame();
                    }
                    break;
                case Keys.Escape:
                    if (state == "waiting" || state == "over")
                    {
                        Application.Exit();
                    }
                    break ;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.Up:
                    arrowupDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Down:
                    arrowdownDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // game timer

            timerLine.Y += time;

            //move player1
            if (wDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y -= playerSpeed;
            }
            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += playerSpeed;
            }
            // move player2 
            if (arrowupDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y -= playerSpeed;

            }
            if (arrowdownDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += playerSpeed;

            }

            // rest players when they get to the top
            for (int i = 0; i < meteorList.Count; i++)
            {
                if (player1.Y < 0)
                {
                    player1.Y = 200;

                    P1score.Text = $"{p1score++}";

                }

                if (player2.Y < 0)
                {
                    player2.Y = 200;

                    P2score.Text = $"{p2score++}";
                }
            }

            // move the meteor
            for (int i = 0; i < meteorList.Count; i++)
            {
                int x = meteorList[i].X + meteorSpeed;
                meteorList[i] = new Rectangle(x, meteorList[i].Y, meteorhigth, meteorswidth);
            }

            // generate new meteor
            randValue = randGen.Next(0, 10);

            if (randValue <= 10)
            {
                randValue = randGen.Next(0, 250 - meteorhigth - 20);
                // player 1 side
                Rectangle meteor1 = new Rectangle(0, randValue, meteorhigth, meteorswidth);
                meteorList.Add(meteor1);
                //player 2 side
                Rectangle meteor2 = new Rectangle(310, randValue, meteorhigth, meteorswidth);
                meteorList.Add(meteor2);
            }

            //remove meteor at half
            for (int i = 0; i < meteorList.Count; i++)
            {
                if (meteorList[i].IntersectsWith(timerLine2))
                {
                    meteorList.RemoveAt(i);
                }
            }


            // check to see if meteor hits player
            for (int i = 0; i < meteorList.Count; i++)
            {
                if (meteorList[i].IntersectsWith(player1))
                {
                    player1.Y = 250;
                }

                if (meteorList[i].IntersectsWith(player2))
                {
                    player2.Y = 250;
                }
            }

            //time go down
            for (int i = 0; i < meteorList.Count; i++)
            {
               if(timerLine.Y > 300)
                {
                    state = "over";
                }
            }
           
            // check plaeyr
            if(p1score > p2score)
            {
                winner = "PLAYER 1 IS TEH WINNER";
            }
            else
            {
                winner = "PLAYER 2 IS TEH WINNER";
            }

            if(p1score == p2score)
            {
                winner = "THE GAME IS A TIE";
            }

            //redraw the screen
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            if (state == "waiting")
            {
                titleLabel.Text = "Space Race";
                subtitleLabel.Text = "Press Space to Play and Esc to Exit";
            }

            if (state == "playing")
            {
                //paint 1
                e.Graphics.FillRectangle(pinkBrush, player1);
                // paint 2
                e.Graphics.FillRectangle(pinkBrush, player2);
                // timer line
                e.Graphics.FillRectangle(pinkBrush, timerLine2);
                // timer line
                e.Graphics.FillRectangle(blackBrush, timerLine);
                // draw meteor
                for (int i = 0; i < meteorList.Count; i++)
                {
                    e.Graphics.FillRectangle(yellowBrush, meteorList[i]);
                }
            }

            if (state == "over")
            {
                titleLabel.Text = "GAME OVER";
                subtitleLabel.Text = $"{winner}";
                subtitleLabel.Text += "\nPress Space to Play or Esc to Exit";
            }
        }
    }
}
