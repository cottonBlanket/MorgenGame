using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.Drawing.Drawing2D;
using System.Linq;
using System.IO;

namespace MorgenGame
{
    public partial class Form1 : Form
    {             
        Random rnd = new Random();
        private Pen pen = new Pen(Color.Red);
        private Font font = new Font("MV Boli", 24, FontStyle.Bold);
        private Font gameLabelFont = new Font("Myanmar Text", 9, FontStyle.Bold);
        private GameState gameState;
        private Image backImg;

        #region GameField
        private Player player;
        private Timer gameTimer;
        private Timer headPhoneTimer;
        private int tick = 10;
        private List<Gold> goldList;
        private List<Enemy> enemies;
        private List<SoundPlayer> musics;
        private List<Rectangle> textures;
        private int wallet = 0;
        private int headphone;
        private Timer musicTimer;
        private int numMusic;
        private char lastButton;        
        private Police topPolice;
        private Police bottomPolice;
        private Shop shop;
        private Home home;
        private bool isPlayMusic;
        #endregion GameField

        #region MenuField
        private Timer mainTimer;
        private Label infoMenuLabel;
        private Label infoLabel;
        private Label healthLabel;
        private Label walletLabel;
        private Label headphoneLabel;
        private Label isUsingHeadPhone;
        private Label startLabel;
        private Label winLabel;
        private List<Control> mainMenuControls;
        private List<Control> infoMenuControls;
        private List<Control> activeGameControls;
        private List<Control> winStateControls;
        private Button BackButton;
        private Button useOrBuyHeadPhone;
        private Button startGameButton;
        private Button exitButton;
        private Button infoButton;
        #endregion MenuField


        public Form1()
        {
            InitializeComponent();
            #region FormInit
            DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Icon = Properties.Resources.morgg;
            backImg = Properties.Resources.mainMenuBack;
            //this.BackgroundImage = Properties.Resources.mainMenuBack;
            WindowState = FormWindowState.Maximized;
            #endregion FormInit

            #region InitMenu
            gameState = GameState.MainMenu;
            InitLists();
            InitTimers();
            InitControls();
            mainTimer.Start();
            #endregion InitMenu

            var files = new DirectoryInfo("gameMusic");
            CompleteEnemies();
            CompleteTextures();
            CompleteMusics(files);
          
            var file = files.GetFiles().First();
            var myGame = new SoundPlayer();
            myGame.SoundLocation = file.FullName;
            myGame.Load();
            myGame.Play();

            Invalidate();
        }

        private void InitLists()
        {
            mainMenuControls = new List<Control>();
            infoMenuControls = new List<Control>();
            activeGameControls = new List<Control>();
            winStateControls = new List<Control>();
            goldList = new List<Gold>();
            enemies = new List<Enemy>();
            player = new Player(25, 280);
        }


        #region MenuControl
        private void GameUpdate(object sender, EventArgs e)
        {           
            switch(gameState)
            {
                case GameState.MainMenu :
                    HideListControls(infoMenuControls);
                    HideListControls(activeGameControls);
                    HideListControls(winStateControls);
                    ShowListControls(mainMenuControls);
                    backImg = Properties.Resources.mainMenuBack;
                    break;
                case GameState.InfoMenu:
                    HideListControls(mainMenuControls);
                    HideListControls(activeGameControls);
                    ShowListControls(infoMenuControls);
                    break;
                case GameState.ActiveGame:
                    HideListControls(mainMenuControls);
                    ShowListControls(activeGameControls);
                    Start();
                    break;
                case GameState.WinGame:
                    HideListControls(activeGameControls);
                    ShowListControls(winStateControls);
                    RestartGame();
                    break;
            }
            Invalidate();
        }

        private void RestartGame()
        {
            goldList.Clear();
            player = new Player(25, 280);
            headphone = 0;
            wallet = 0;
            lastButton = default;
        }

        private void HideListControls(List<Control> listControl)
        {
            foreach (var control in listControl)
                control.Hide();
        }

        private void ShowListControls(List<Control> listControl)
        {
            foreach (var control in listControl)
                control.Show();
        }
        #endregion MenuControls

        #region ActiveGame
        public void Start()
        {
            this.KeyDown += new KeyEventHandler(MovePlayer);
            this.KeyUp += new KeyEventHandler(OnKeyUp);
            backImg = Properties.Resources.BackgroundImage;
            
            shop = new Shop(1500, 300);
            home = new Home(50, 280);
           
            gameTimer.Start();
        }

        #region Paint
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawImage(backImg, new Rectangle(new Point(0, 0),
                new Size(ClientRectangle.Width, ClientRectangle.Height)));
            if (gameState == GameState.ActiveGame)
            {
                foreach (var gold in goldList)
                    gold.PlayAnimation(g);
                foreach (var enemy in enemies)
                    enemy.PlayAnimation(g);

                g.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(0, 0, 1550, 35));
                //if (topPolice != null)
                //    g.DrawImage(topPolice.picture, new Rectangle(topPolice.posX, topPolice.posY, topPolice.sizeX, topPolice.sizeY));
                g.SmoothingMode = SmoothingMode.AntiAlias;
                player.PlayAnimation(g, lastButton);
            }
        }
        #endregion Paint

        private void Update(object sender, EventArgs e)
        {
            MovePlayer();
            IsCollideTextures();
            IsAroundShop();            
            StopHeadPhoneTimer();            
            UpdateLabels();           
            TakeGold();
            SpawnGold();
            MoveEnemy();
            WinGame();
            //PoliceMove();
            //SpawnPolice();
            //StartMusicTimer();
            Invalidate();
        }

        #region UpdateMethods
        private void MovePlayer()
        {
            if (player.isMoving && player.health > 0)
                player.Move();
        }

        private void IsCollideTextures()
        {
            foreach (var texture in textures)
                if (texture.IntersectsWith(new Rectangle(player.posX, player.posY, player.sizeX, player.sizeY)))
                    CollideTextures();

            if (player.isMoving)
            {
                if (player.posX > 0) player.posX -= 5;
                if (player.posX + player.sizeX < 1552) player.posX += 5;
                if (player.posY > 0) player.posY -= 5;
                if (player.posY + player.sizeY < 880) player.posY += 5;
                player.Move();
            }
        }

        private void IsAroundShop()
        {
            if (Collide(shop))
            {
                useOrBuyHeadPhone.Click -= UseHeadPhone;
                useOrBuyHeadPhone.Click += new EventHandler(Buy);
                useOrBuyHeadPhone.Text = "Купить наушники";
            }
            else
            {
                useOrBuyHeadPhone.Click -= Buy;
                useOrBuyHeadPhone.Click += new EventHandler(UseHeadPhone);
                useOrBuyHeadPhone.Text = "Использовать наушники";
            }
        }

        private void StopHeadPhoneTimer()
        {
            if (tick < 0)
            {
                headPhoneTimer.Stop();
                tick = 10;
            }
        }

        //private void PoliceMove()
        //{
        //    if (bottomPolice != null)
        //        bottomPolice.Move();
        //    if (topPolice != null)
        //        topPolice.Move();
        //}

        //private void SpawnPolice()
        //{
        //    if (rnd.Next(1) == 0)
        //        topPolice = new Police(500, 30, 0);
        //    else if(rnd.Next(1) == 1)
        //        bottomPolice = new Police(1650, 800, 1);
        //}

        private void UpdateLabels()
        {
            healthLabel.Text = "Здоровье: " + (player.health / 5).ToString();
            walletLabel.Text = "Золото: " + wallet.ToString();
            headphoneLabel.Text = "Наушники: " + headphone.ToString();
            if (!headPhoneTimer.Enabled)
                isUsingHeadPhone.Text = "Наушники не используются";
        }

        //private void StartMusicTimer()
        //{
        //    if (Collide(player, 25))
        //        musicTimer.Start();
        //    else
        //    {
        //        musicTimer.Stop();
        //        musics[numMusic].Stop();
        //    }
        //}

        private void TakeGold()
        {
            var takingGold = new List<Gold>();
            foreach (var gold in goldList)
                if (Collide(gold))
                {
                    takingGold.Add(gold);
                    wallet++;
                }
            
            foreach (var gold in takingGold)
                goldList.Remove(gold);
        }

        private void SpawnGold()
        {
            if (rnd.Next(10) == 0 && goldList.Count < 20)
                goldList.Add(new Gold(rnd.Next(300, 1300), rnd.Next(230, 620)));
        }

        public void MoveEnemy()
        {
            foreach (var enemy in enemies)
            {
                if (Collide(enemy, 50))
                    AttackPlayer(enemy);
                else
                {
                    enemy.posX += enemy.movements[rnd.Next(4)].X;
                    enemy.posY += enemy.movements[rnd.Next(4)].Y;
                }
            }
            Invalidate();
        }

        private void AttackPlayer(Enemy enemy)
        {
            if (!headPhoneTimer.Enabled)
                player.health -= 1;
            else if (Collide(enemy, -20))
                player.health -= 1;
            var right = player.posX > enemy.posX;
            var left = player.posX < enemy.posX;
            var top = player.posY < enemy.posY;
            var down = player.posY > enemy.posY;
            enemy.posX += right ? 3 : left ? -3 : 0;
            enemy.posY += top ? -3 : down ? 3 : 0;
        }

        private void WinGame()
        {
            if(Collide(home))
                if(headphone >= 3 && player.health > 0)
                    gameState = GameState.WinGame;               
        }
        #endregion UpdateMethods

        private void GoTimer(object sender, EventArgs e)
        {
            isUsingHeadPhone.Text = "Наушники использованы. Время: " + tick.ToString();
            tick--;
        }

        private void Buy(object sender, EventArgs e)
        {
            if (wallet >= 10)
            {
                wallet -= 10;
                headphone++;
            }
            Invalidate();
        }
        public void MovePlayer(object sender, KeyEventArgs e)
        {
            player.isMoving = true;
            switch (e.KeyCode)
            {
                case Keys.A:
                    lastButton = 'A';
                    player.moveX = -5;
                    break;
                case Keys.D:
                    lastButton = 'D';
                    player.moveX = 5;
                    break;
                case Keys.W:
                    lastButton = 'W';
                    player.moveY = -5;
                    break;
                case Keys.S:
                    lastButton = 'S';
                    player.moveY = 5;
                    break;
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    player.moveX = 0;
                    Invalidate();
                    break;
                case Keys.D:
                    player.moveX = 0;
                    Invalidate();
                    break;
                case Keys.W:
                    player.moveY = 0;
                    Invalidate();
                    break;
                case Keys.S:
                    player.moveY = 0;
                    Invalidate();
                    break;
            }
            player.isMoving = false;
        }

        private void CollideTextures()
        {
            player.isMoving = false;
            switch (lastButton)
            {
                case 'A':
                    player.posX += 3;
                    break;
                case 'D':
                    player.posX -= 3;
                    break;
                case 'W':
                    player.posY += 3;
                    break;
                case 'S':
                    player.posY -= 3;
                    break;
            }
            player.isMoving = true;
        }

        //недоработанная музыка
        //private void PlayMusic(object sender, EventArgs e)
        //{
        //    foreach (var enemy in enemies)
        //    {
        //        numMusic = rnd.Next(5);
        //        if (Collide(enemy, 25))
        //        {           
        //            if(!isPlayMusic)
        //                musics[numMusic].Play();
        //            isPlayMusic = true;
        //        }
        //        else
        //        {
        //            musics[numMusic].Stop();
        //            isPlayMusic = false;
        //        }
        //    }    
        //}

        private bool Collide(IGameObject obj, int border = 0)
        {
            var playerRect = new Rectangle(player.posX, player.posY, player.sizeX, player.sizeY);
            var enemyRect = new Rectangle(obj.posX - border, obj.posY - border, obj.sizeX + border, obj.sizeY + border);

            return playerRect.IntersectsWith(enemyRect);
        }

        private void UseHeadPhone(object sender, EventArgs e)
        {
            if (headphone > 0)
            {
                headphone--;
                headPhoneTimer.Start();
            }
        }
        #endregion ActiveGame
        
        #region Completed
        private void CompleteTextures()
        {
            textures = new List<Rectangle>()
            {
            new Rectangle(157, 140, 1180, 20),
            new Rectangle(300, 200, 250, 20),
            new Rectangle(950, 200, 250, 20),
            new Rectangle(670, 200, 160, 20),
            new Rectangle(157, 680, 1180, 15)
            };
        }

        private void CompleteEnemies()
        {
            enemies.Add(new Enemy(420, 180));
            enemies.Add(new Enemy(700, 180));
            enemies.Add(new Enemy(980, 180));
            enemies.Add(new Enemy(600, 500));
            enemies.Add(new Enemy(700, 450));
            enemies.Add(new Enemy(800, 500));
        }

        private void CompleteMusics(DirectoryInfo files)
        {
            var musicFiles = files.GetFiles();

            musics = new List<SoundPlayer>
            {
                new SoundPlayer(Properties.Resources.gameMusic1)
                { SoundLocation = musicFiles[1].FullName },
                new SoundPlayer(Properties.Resources.gameMusic2)
                { SoundLocation = musicFiles[2].FullName },
                new SoundPlayer(Properties.Resources.gameMusic3)
                { SoundLocation = musicFiles[3].FullName },
                new SoundPlayer(Properties.Resources.gameMusic4)
                { SoundLocation = musicFiles[4].FullName },
                new SoundPlayer(Properties.Resources.gameMusic5)
                { SoundLocation = musicFiles[5].FullName },
            };
        }

        #endregion Completed

        #region ChangerGameState
        private void InfoButtonClick(object sender, EventArgs e)
        {           
            gameState = GameState.InfoMenu;
            Invalidate();
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            gameState = GameState.MainMenu;
            Invalidate();
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void StartGameClick(object sender, EventArgs e)
        {
            gameState = GameState.ActiveGame;
            Invalidate();
        }
        #endregion ChangeGameState

        #region Start
        private void InitControls()
        {
            infoLabel = ControlExtention.InitLabel
            (infoLabel, 10, 0, new Font("Myanmar Text", 14, FontStyle.Bold), Color.Gray, Color.Transparent, "MorgenGame");
            Controls.Add(infoLabel);
            activeGameControls.Add(infoLabel);

            healthLabel = ControlExtention.InitLabel
                (healthLabel, 200, 10, gameLabelFont, Color.Gray, Color.Transparent);
            Controls.Add(healthLabel);
            activeGameControls.Add(healthLabel);

            walletLabel = ControlExtention.InitLabel
                (walletLabel, 400, 10, gameLabelFont, Color.Gray, Color.Transparent);
            Controls.Add(walletLabel);
            activeGameControls.Add(walletLabel);

            headphoneLabel = ControlExtention.InitLabel
                (headphoneLabel, 600, 10, gameLabelFont, Color.Gray, Color.Transparent);
            Controls.Add(headphoneLabel);
            activeGameControls.Add(headphoneLabel);

            isUsingHeadPhone = ControlExtention.InitLabel
                (isUsingHeadPhone, 800, 10, gameLabelFont, Color.Gray, Color.Transparent);
            Controls.Add(isUsingHeadPhone);
            activeGameControls.Add(isUsingHeadPhone);

            useOrBuyHeadPhone = ControlExtention.InitButton
                (useOrBuyHeadPhone, 1340, 5, new Font("MV Boli", 10, FontStyle.Regular),
                BackColor = Color.Gray, Color.Black, "Использовать наушники", UseHeadPhone);
            Controls.Add(useOrBuyHeadPhone);
            activeGameControls.Add(useOrBuyHeadPhone);

            startGameButton = ControlExtention.InitButton
                (startGameButton, 650, 670, font, Color.Transparent, Color.Black, "Начать игру", StartGameClick);
            Controls.Add(startGameButton);
            mainMenuControls.Add(startGameButton);

            infoButton = ControlExtention.InitButton
                (infoButton, 660, 730, font, Color.Transparent, Color.Black, "Как играть", InfoButtonClick);
            Controls.Add(infoButton);
            mainMenuControls.Add(infoButton);

            exitButton = ControlExtention.InitButton
                (exitButton, 690, 790, font, Color.Transparent, Color.Black, "Выход", ExitButtonClick);
            Controls.Add(exitButton);
            mainMenuControls.Add(exitButton);

            startLabel = ControlExtention.InitLabel
                (startLabel, 430, 100, new Font("MV Boli", 72, FontStyle.Bold), Color.Transparent, Color.Black, "Morgen Game");
            mainMenuControls.Add(startLabel);
            Controls.Add(startLabel);

            BackButton = ControlExtention.InitButton
                (BackButton, 100, 30, font, Color.Transparent, Color.Blue, "В главное меню", BackButtonClick);
            Controls.Add(BackButton);
            infoMenuControls.Add(BackButton);
            winStateControls.Add(BackButton);

            infoMenuLabel = ControlExtention.InitLabel
                (infoMenuLabel, 700, 100, font, Color.Blue, Color.Gray, "Супер игра");
            Controls.Add(infoMenuLabel);
            infoMenuControls.Add(infoMenuLabel);

            winLabel = ControlExtention.InitLabel
                (winLabel, 700, 100, font, Color.Blue, Color.Gray, "Вы выиграли");
            Controls.Add(winLabel);
            winStateControls.Add(winLabel);
            winLabel.Hide();
        }

        private void InitTimers()
        {
            headPhoneTimer = new Timer();
            headPhoneTimer.Interval = 1000;
            headPhoneTimer.Tick += new EventHandler(GoTimer);

            //musicTimer = new Timer();
            //musicTimer.Tick += new EventHandler(PlayMusic);
            //musicTimer.Interval = 20;

            gameTimer = new Timer();
            gameTimer.Tick += new EventHandler(Update);
            gameTimer.Interval = 20;
            this.components.Add(gameTimer);

            mainTimer = new Timer();
            mainTimer.Interval = 10;
            mainTimer.Tick += new EventHandler(GameUpdate);
            this.components.Add(mainTimer);
        }

        #endregion Start
    }
}
