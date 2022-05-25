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
        public Player player;
        public Timer timer;
        Random rnd = new Random(); 
        public List<Gold> goldList;
        public List<Enemy> enemies;
        public List<SoundPlayer> musics;
        public List<Rectangle> textures;
        public MenuStrip menu;
        public int wallet = 0;
        public Timer musicTimer;
        public int numMusic;
        public char lastButton;
        public Pen pen = new Pen(Color.Red);

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.KeyDown += new KeyEventHandler(MovePlayer);
            this.KeyUp += new KeyEventHandler(OnKeyUp);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            var s = new WindowsMediaPlayer();
            Init();

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
            player = new Player() { posX = 20, posY =  340 };
            timer = new Timer();
            this.components.Add(timer);
            goldList = new List<Gold>();
            enemies = new List<Enemy>();
            CompleteEnemies();
            CompleteTextures();
            CompleteMusics();
                        
            musicTimer = new Timer();
            musicTimer.Tick += new EventHandler(PlayMusic);
            musicTimer.Interval = 30000;
            

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
            //if(!musicTimer.Enabled)
            //    PlayMusic();
            StartMusicTimer();
            TakeGold();
            SpawnGold();
            MoveEnemy();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            //g.DrawImage(Map.BackgroundImage, new Rectangle(new Point(0, 0),
                //new Size(ClientRectangle.Width, ClientRectangle.Height)));
            

            foreach(var gold in goldList)
                gold.PlayAnimation(g);
            foreach (var enemy in enemies)
                enemy.PlayAnimation(g);

            var a = player.health / 10;
            g.DrawString(player.health > 0 ?
                a.ToString() : "GameOver", Font, new SolidBrush(System.Drawing.Color.Black), 100, 100);
            g.DrawString(wallet.ToString(), Font, new SolidBrush(System.Drawing.Color.Black), 150, 100);

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
    }
}
