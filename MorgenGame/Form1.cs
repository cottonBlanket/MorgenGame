using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Markup;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using WMPLib;
using System.Drawing.Drawing2D;
using System.IO;

namespace MorgenGame
{
    public partial class Form1 : Form
    {
        private Player player;
        private Timer timer;
        private Timer headPhoneTimer;
        private int tick = 10;
        Random rnd = new Random(); 
        private List<Gold> goldList;
        private List<Enemy> enemies;
        private List<SoundPlayer> musics;
        private List<Rectangle> textures;
        private int wallet = 0;
        private int headphone;
        private Timer musicTimer;
        private int numMusic;
        private char lastButton;
        private Pen pen = new Pen(Color.Red);
        private Police topPolice;
        private Police bottomPolice;
        private Shop shop;
        private Home home;
        private Label infoLabel;
        private Label healthLabel;
        private Label walletLabel;
        private Label headphoneLabel;
        private Label isUsingHeadPhone;
        private Button useOrBuyHeadPhone;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.KeyDown += new KeyEventHandler(MovePlayer);
            this.KeyUp += new KeyEventHandler(OnKeyUp);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            var s = new WindowsMediaPlayer();
            Init();

            infoLabel = new Label()
            {
                Font = new Font("Myanmar Text", 14, FontStyle.Bold),
                AutoSize = true, 
                Location = new Point(10, 0),
                Text = "MorgenGame", 
                BackColor = Color.GreenYellow
            };
            Controls.Add(infoLabel);

            healthLabel = new Label()
            {
                Font = new Font("Myanmar Text", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(200, 10),
                BackColor = Color.GreenYellow
            };
            Controls.Add(healthLabel);

            walletLabel = new Label()
            {
                Font = new Font("Myanmar Text", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(400, 10),                
                BackColor = Color.GreenYellow
            };
            Controls.Add(walletLabel);

            headphoneLabel = new Label()
            {
                Font = new Font("Myanmar Text", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(600, 10),               
                BackColor = Color.GreenYellow
            };
            Controls.Add(headphoneLabel);

            isUsingHeadPhone = new Label()
            {
                Font = new Font("Myanmar Text", 9, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(800, 10),
                BackColor = Color.GreenYellow
            };
            Controls.Add(isUsingHeadPhone);

            useOrBuyHeadPhone = new Button()
            {
                Location = new Point(1340, 5),
                AutoSize = true,
                Text = "Использовать наушники",
                Font = new Font("MV Boli", 10 ,FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.GreenYellow,
                FlatStyle = FlatStyle.Flat
            };
            Controls.Add(useOrBuyHeadPhone);
            useOrBuyHeadPhone.Click += new EventHandler(UseHeadPhone);
            

            var files = new DirectoryInfo("gameMusic");
            var file = files.GetFiles().First();
            var Audio = new SoundPlayer();
            Audio.SoundLocation = file.FullName;
            Audio.Load();
            Audio.Play();

            WindowState = FormWindowState.Maximized;
            
            Invalidate();
        }

        public void Init()
        {
            player = new Player() { posX = 20, posY =  240 };
            timer = new Timer();
            this.components.Add(timer);
            goldList = new List<Gold>();
            enemies = new List<Enemy>();
            CompleteEnemies();
            CompleteTextures();
            CompleteMusics();

            shop = new Shop(1500, 300);
            home = new Home(50, 280);

            headPhoneTimer = new Timer();
            headPhoneTimer.Interval = 1000;
            headPhoneTimer.Tick += new EventHandler(GoTimer);
                        
            musicTimer = new Timer();
            musicTimer.Tick += new EventHandler(PlayMusic);
            musicTimer.Interval = 20;
            

            timer.Interval = 20;
            timer.Tick += new EventHandler(Update);
            timer.Start();
        }

        private void Update(object sender, EventArgs e)
        {
            if (player.isMoving && player.health > 0)
                player.Move();
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
            if (tick < 0)
            {
                headPhoneTimer.Stop();
                tick = 10;
            }
            if(bottomPolice != null)
                bottomPolice.Move();
            if (topPolice != null)
                topPolice.Move();
            topPolice = new Police(500, 30, 0);
            topPolice.Move();
            //SpawnPolice();
            UpdateLabels();
            StartMusicTimer();
            TakeGold();
            SpawnGold();
            MoveEnemy();
            Invalidate();
        }

        private void SpawnPolice()
        {
            if (rnd.Next(1) == 0)
                topPolice = new Police(500, 30, 0);
            else if(rnd.Next(1) == 1)
                bottomPolice = new Police(1650, 800, 1);
        }

        private void GoTimer(object sender, EventArgs e)
        {
            isUsingHeadPhone.Text = "Наушники использованы. Время: " + tick.ToString();
            tick--;    
        }

        private void Buy(object sender, EventArgs e)
        {
            if(wallet >= 10)
            {
                wallet -= 10;
                headphone++;
            }
            Invalidate();
        }

        private void UseHeadPhone(object sender, EventArgs e)
        {
            if(headphone > 0)
            {
                headphone--;
                headPhoneTimer.Start();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawImage(Map.BackgroundImage, new Rectangle(new Point(0, 0),
                new Size(ClientRectangle.Width, ClientRectangle.Height)));

            foreach (var gold in goldList)
                gold.PlayAnimation(g);
            foreach (var enemy in enemies)
                enemy.PlayAnimation(g);
            
            g.FillRectangle(new SolidBrush(Color.GreenYellow), new RectangleF(0, 0, 1550, 35));
            if(topPolice != null)
                g.DrawImage(topPolice.picture, new Rectangle(topPolice.posX, topPolice.posY, topPolice.sizeX, topPolice.sizeY));
            g.SmoothingMode = SmoothingMode.AntiAlias;
            player.PlayAnimation(g, lastButton);
        }

        public void MovePlayer(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    lastButton = 'A';
                    player.isMoving = true;
                    player.moveX = -3;
                    break;
                case Keys.D:
                    lastButton = 'D';
                    player.isMoving = true;
                    player.moveX = 3;
                    break;
                case Keys.W:
                    lastButton = 'W';
                    player.isMoving = true;
                    player.moveY = -3;
                    break;
                case Keys.S:
                    lastButton = 'S';
                    player.isMoving = true;
                    player.moveY = 3;
                    break;
            }
        }

        public void MoveEnemy()
        {
            foreach (var enemy in enemies)
            {
                if (Collide(enemy, 25))
                {
                    AttackPlayer(enemy);
                }
                else
                {
                    enemy.posX += enemy.movements[rnd.Next(4)].X;
                    enemy.posY += enemy.movements[rnd.Next(4)].Y;
                }
            }
            Invalidate();
        }

        private void CollideTextures()
        {
            switch(lastButton)
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

        private void StartMusicTimer()
        {
            if (Collide(player, 25))
                musicTimer.Start();
            else
            {
                musicTimer.Stop();
                musics[numMusic].Stop();
            }
        }
        private void PlayMusic(object sender, EventArgs e)
        {
            foreach (var enemy in enemies)
                if (Collide(enemy, 25))
                {
                    numMusic = rnd.Next(5);
                    musics[numMusic].Play();
                }
        }

        private bool Collide(IGameObject obj, int border = 0)
        {
            var playerRect = new Rectangle(player.posX, player.posY, player.sizeX, player.sizeY);
            var enemyRect = new Rectangle(obj.posX - border, obj.posY - border, obj.sizeX + border, obj.sizeY + border);

            return playerRect.IntersectsWith(enemyRect);
        }

        private void TakeGold()
        {
            var takingGold = new List<Gold>();
            foreach(var gold in goldList)
            {
                if(Collide(gold))
                {
                    takingGold.Add(gold);
                    wallet++;
                }
            }

            foreach(var gold in takingGold)
                goldList.Remove(gold);
        }

        private void CompleteTextures()
        {
            textures = new List<Rectangle>()
            {
            new Rectangle(157, 140, 1180, 60),
            new Rectangle(300, 200, 250, 60),
            new Rectangle(950, 200, 250, 60),
            new Rectangle(670, 200, 160, 60),
            new Rectangle(157, 670, 1180, 75)
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

        private void CompleteMusics()
        {
            musics = new List<SoundPlayer>
            {
                new SoundPlayer(Map.gameMusic1),
                new SoundPlayer(Map.gameMusic2),
                new SoundPlayer(Map.gameMusic3),
                new SoundPlayer(Map.gameMusic4),
                new SoundPlayer(Map.gameMusic5),
            };
        }

        private void AttackPlayer(Enemy enemy)
        {
            if(!headPhoneTimer.Enabled)
                player.health -= 1;
            var right = player.posX > enemy.posX;
            var left = player.posX < enemy.posX;
            var top = player.posY < enemy.posY;
            var down = player.posY > enemy.posY;
            enemy.posX += right ? 3 : left ? -3 : 0;
            enemy.posY += top ? -3 : down ? 3 : 0;
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

        private void SpawnGold()
        {
            if (rnd.Next(10) == 0 && goldList.Count < 20)
                goldList.Add(new Gold(rnd.Next(300, 1300), rnd.Next(230, 620)));               
        }

        private void UpdateLabels()
        {
            healthLabel.Text = "Здоровье: " + (player.health / 5).ToString();
            walletLabel.Text = "Золото: " + wallet.ToString();
            headphoneLabel.Text = "Наушники: " + headphone.ToString();
            if(!headPhoneTimer.Enabled)
                isUsingHeadPhone.Text = "Наушники не используются";
        }
    }
}
