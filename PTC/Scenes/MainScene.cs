using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using PTC.GraphicUtils;
using PTC.Particles;
using PTC.Sprites;
using PTC.Text;
using PTC.Utils;
using PTC.Input;

namespace PTC.Scenes
{
    public class MainScene : Scene
    {
        public enum Seasons { summer, autumn, winter, Spring }
        public Seasons season;

        bool seasonActivated = true;
        bool m_gameOver = false;
        private TimeSpan m_LevelStartTime = TimeSpan.MinValue;

        float sceneTime = 0.0f;

        float secondsSeason = 30f;

        //Color summerColor = new Color(144, 238, 144);// Color.LightGreen;//((144 238 144
        Color summerColor = Color.Green;
        Color autumnColor = Color.YellowGreen;
        Color winterColor = new Color(255, 250, 250);
        Color springColor = new Color(0, 255, 127);

        Color seasonTextColor = new Color(136, 255, 255);

        Color BackgroundColor = Color.LightGreen;
        public Color BackGroundColor { get { return BackgroundColor; } }

        Vector3 shiftColorSummerAutumn;
        Vector3 shiftColorAutumnWinter;
        Vector3 shiftColorWinterSpring;
        Vector3 shiftColorSpringSummer;

        List<Blood> blodPletter = new List<Blood>();

        private SmokePlumeParticleSystem m_smokeParticleSystem;
        private BloodsprayParticleSystem m_bloodParticleSystem;

        List<Hole> m_holes = new List<Hole>();
        List<Vector2> holePositions = new List<Vector2>();
        List<MatingRabbits> m_MatingRabbits = new List<MatingRabbits>();
        Timer spawnTimer, m_MatingCollisionCheckTimer;
        BarrelGun m_gun;

        static TimeSpan m_twoSeconds = new TimeSpan(0, 0, 0, 2, 0);
        static TimeSpan m_tenSeconds = new TimeSpan(0, 0, 0, 10, 0);

        public MainScene(Game game)
            : base(game)
        {
        }

        private void MatingTimeCollisionCheckTimesUp(object sender, EventArgs<GameTime> e)
        {
            //checks whether any rabbits are mating.
            //if they do they are removed and the rabbits are searched for more candidates 
            while (CheckIfMating(e.Data)) { }
        }

        public bool CheckIfMating(GameTime time)
        {
            Rabbit rabbit1, rabbit2;

            for (int i = m_Rabbits.Count - 1; i >= 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    //TODO: Optimering
                    rabbit1 = m_Rabbits[i];
                    rabbit2 = m_Rabbits[j];
                    if (!rabbit1.Kid && !rabbit2.Kid && (rabbit1.Position - rabbit2.Position).Length() < rabbit2.TouchDistance && rabbit1.Sex != rabbit2.Sex && !rabbit1.IsDead && !rabbit2.IsDead)
                    {
                        Vector2 position = m_Rabbits[i].Position;

                        RemoveComponentNoUpdate(rabbit1);
                        SpriteFactory.Remove(rabbit1);
                        m_Rabbits.RemoveAt(i);

                        RemoveComponentNoUpdate(rabbit2);
                        SpriteFactory.Remove(rabbit2);
                        m_Rabbits.RemoveAt(j);

                        var mating = SpriteFactory.CreateMatingRabbit(Game, position, time.TotalGameTime);
                        mating.MatingOver += MatingOver;
                        m_MatingRabbits.Add(mating);
                        AddComponentNoUpdate(mating);
                        XACT.KneppeLyd();
                        return true;
                    }
                }
            }
            return false;
        }

        private void MatingOver(object sender, EventArgs e)
        {
            AddRabbitAfterMating(sender as MatingRabbits);
        }

        private void MakeHoles()
        {
            m_holes.Add(new Hole(ThisGame, new Vector2(100, 100)));
            m_holes.Add(new Hole(ThisGame, new Vector2(612, 100)));
            m_holes.Add(new Hole(ThisGame, new Vector2(924, 100)));

            m_holes.Add(new Hole(ThisGame, new Vector2(100, 384)));
            m_holes.Add(new Hole(ThisGame, new Vector2(924, 384)));

            m_holes.Add(new Hole(ThisGame, new Vector2(100, 668)));
            m_holes.Add(new Hole(ThisGame, new Vector2(612, 668)));
            m_holes.Add(new Hole(ThisGame, new Vector2(924, 668)));

            foreach (Hole h in m_holes)
            {
                AddComponentNoUpdate(h);
                holePositions.Add(h.Position);
            }
        }

        private void SpawnTimerTimesUp(object sender, EventArgs e)
        {
            AddRabbit();
        }

        private float m_Points = 0F;
        public int Points
        {
            get { return (int)m_Points; }
        }

        private int m_CurrentLevel = 1;
        public int CurrentLevel
        {
            get { return m_CurrentLevel; }
        }

        private TextUtil m_TextPoints;
        private TextUtil m_TextSeason;

        public override void Draw(GameTime gameTime)
        {

            if (seasonActivated)
                GraphicsDevice.Clear(BackgroundColor);
            else GraphicsDevice.Clear(Color.LightGreen);

            //base.Draw(gameTime);
            DrawGrass(gameTime);
            DrawBloodpool(gameTime);
            DrawHoles(gameTime);
            DrawCarrot(gameTime);
            DrawRabbits(gameTime);
            DrawMatingRabbits(gameTime);
            DrawFence(gameTime);
            DrawDust(gameTime);
            DrawBloodSpray(gameTime);
            DrawSmoke(gameTime);
            DrawGun(gameTime);
            DrawCrossHair(gameTime);
            DrawText(gameTime);
        }


        public void DrawGrass(GameTime time)
        {
            foreach (Grass grass in m_Grasses)
            {
                if (grass.Visible)
                {
                    grass.Draw(time);
                }
            }
        }

        public void DrawBloodpool(GameTime time)
        {
            foreach (DeadController<Blood> blood in m_Blood)
            {
                if (blood.DeadObject.Visible)
                {
                    blood.DeadObject.Draw(time);
                }
            }
        }

        public void DrawHoles(GameTime time)
        {
            foreach (Hole hole in m_holes)
            {
                if (hole.Visible)
                {
                    hole.Draw(time);
                }
            }
        }

        public void DrawCarrot(GameTime time)
        {
            if (m_Carrot.Visible)
            {
                m_Carrot.Draw(time);
            }
        }

        public void DrawRabbits(GameTime time)
        {
            foreach (Rabbit rabbit in m_Rabbits)
            {
                if (rabbit.Visible)
                {
                    rabbit.Draw(time);
                }
            }
        }

        public void DrawMatingRabbits(GameTime time)
        {
            foreach (MatingRabbits rabbit in m_MatingRabbits)
            {
                if (rabbit.Visible)
                {
                    rabbit.Draw(time);
                }
            }
        }

        public void DrawFence(GameTime time)
        {
            foreach (Fence fence in m_Fences)
            {
                if (fence.Visible)
                {
                    fence.Draw(time);
                }
            }
        }

        public void DrawBloodSpray(GameTime time)
        {
            if (m_bloodParticleSystem.Visible)
            {
                m_bloodParticleSystem.Draw(time);
            }
        }

        public void DrawDust(GameTime time)
        {

        }

        public void DrawSmoke(GameTime time)
        {
            if (m_smokeParticleSystem.Visible)
            {
                m_smokeParticleSystem.Draw(time);
            }
        }

        public void DrawGun(GameTime time)
        {
            if (m_gun.Visible)
            {
                m_gun.Draw(time);
            }
        }

        public void DrawCrossHair(GameTime time)
        {
            if (m_Crosshair.Visible)
            {
                m_Crosshair.Draw(time);
            }
        }

        public void DrawText(GameTime time)
        {
            m_TextPoints.Draw(time);
            m_TextSeason.Draw(time);
        }

        public Vector3 ColorSubtract(Color en, Color to)
        {
            int red = (int)(en.R - to.R);
            int green = (int)(en.G - to.G);
            int blue = (int)(en.B - to.B);

            return new Vector3(red, green, blue);
        }

        public Color ColorShift(Color start, Vector3 shift, float temp_time, Seasons season)
        {
            switch (season)
            {
                case Seasons.summer:
                    start.R -= (byte)((float)sceneTime / secondsSeason * shift.X);
                    start.G -= (byte)((float)sceneTime / secondsSeason * shift.Y);
                    start.B -= (byte)((float)sceneTime / secondsSeason * shift.Z);
                    break;
                case Seasons.autumn:
                    start.R -= (byte)(((float)sceneTime - secondsSeason) / (secondsSeason) * shift.X);
                    start.G -= (byte)(((float)sceneTime - secondsSeason) / (secondsSeason) * shift.Y);
                    start.B -= (byte)(((float)sceneTime - secondsSeason) / (secondsSeason) * shift.Z);
                    break;
                case Seasons.winter:
                    start.R -= (byte)(((float)sceneTime - 2 * secondsSeason) / (secondsSeason) * shift.X);
                    start.G -= (byte)(((float)sceneTime - 2 * secondsSeason) / (secondsSeason) * shift.Y);
                    start.B -= (byte)(((float)sceneTime - 2 * secondsSeason) / (secondsSeason) * shift.Z);
                    break;
                case Seasons.Spring:
                    start.R -= (byte)(((float)sceneTime - 3 * secondsSeason) / (secondsSeason) * shift.X);
                    start.G -= (byte)(((float)sceneTime - 3 * secondsSeason) / (secondsSeason) * shift.Y);
                    start.B -= (byte)(((float)sceneTime - 3 * secondsSeason) / (secondsSeason) * shift.Z);
                    break;


            }


            return new Color(start.R, start.G, start.B);
        }



        public override void Update(GameTime gameTime)
        {
            sceneTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (sceneTime < secondsSeason)
            {


                BackgroundColor = ColorShift(summerColor, shiftColorSummerAutumn, sceneTime, Seasons.summer);
                Grass.grassColor = BackgroundColor;

            }

            else if (sceneTime >= secondsSeason && sceneTime < 2 * secondsSeason)
            {
                season = Seasons.autumn;
                BackgroundColor = ColorShift(autumnColor, shiftColorAutumnWinter, sceneTime, Seasons.autumn);
                Grass.grassColor = BackgroundColor;

            }
            else if (sceneTime >= 2 * secondsSeason && sceneTime < 3 * secondsSeason)
            {
                season = Seasons.winter;
                BackgroundColor = ColorShift(winterColor, shiftColorWinterSpring, sceneTime, Seasons.winter);
                Grass.grassColor = BackgroundColor;



            }

            else if (sceneTime >= 3 * secondsSeason && sceneTime < 4 * secondsSeason)
            {
                season = Seasons.Spring;
                BackgroundColor = ColorShift(springColor, shiftColorSpringSummer, sceneTime, Seasons.Spring);
                Grass.grassColor = BackgroundColor;

            }
            else
            {
                if (season != Seasons.summer)
                {
                    if (spawnTimer.DelayInMilliseconds > 100)
                    {
                        spawnTimer.DelayInMilliseconds -= 100;
                    }
                    season = Seasons.summer;
                }
                sceneTime = 0.0f;

            }

            CheckForColision(gameTime);

            UpdateAllComponents(gameTime);
            RemoveKilledRabbits(gameTime);
            RemoveOldBlood(gameTime);
            m_TextPoints.SetText(string.Format("Points: {0}", ThisGame.CurrentPoints));
            m_TextSeason.SetText(season.ToString());
            //if (!DoOnce)
            //{
            //    DoOnce = true;
            //    var client = new HighScoreProxy.HighScoreServiceClient();
            //    m_TextSeason.SetText(client.GetPublicKey().Key);
            //}
        }

        //private bool DoOnce = false;

        private void UpdateAllComponents(GameTime time)
        {
            for (int i = Components.Count - 1; i >= 0; i--)
            {
                if (Components[i].Enabled)
                {
                    Components[i].Update(time);
                }
            }
        }

        private bool CheckForColision(GameTime time)
        {

            foreach (Rabbit rabbit in m_Rabbits)
            {
                if (!rabbit.IsDead && (rabbit.Position - rabbit.Target).Length() < rabbit.TouchDistance / 2)
                {
                    if (rabbit.Target == m_Carrot.Position)
                    {

                        m_gameOver = true;
                    }
                    rabbit.Target = GetRandomOtherHole(rabbit.Target);
                }
            }
            return false;
        }

        private int m_gunHeight = 450;
        private Crosshair m_Crosshair;
        private List<Grass> m_Grasses = new List<Grass>();
        private Carrot m_Carrot;
        private List<Rabbit> m_Rabbits = new List<Rabbit>();
        private List<DeadController<Rabbit>> m_DeadRabbits = new List<DeadController<Rabbit>>();
        private List<DeadController<Blood>> m_Blood = new List<DeadController<Blood>>();
        private List<Fence> m_Fences = new List<Fence>();

        protected override void LoadContent()
        {
            base.LoadContent();

            Song gameSong = ThisGame.Content.Load<Song>("PTC_theme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(gameSong);

            shiftColorSummerAutumn = ColorSubtract(summerColor, autumnColor);
            shiftColorAutumnWinter = ColorSubtract(autumnColor, winterColor);
            shiftColorWinterSpring = ColorSubtract(winterColor, springColor);
            shiftColorSpringSummer = ColorSubtract(springColor, summerColor);

            m_TextPoints = new TextUtil(ThisGame, Vector2.Zero, FontMedium, Color.Violet, Color.Black, new Vector2(-10, 0),
                HorizontalAlignment.Right, VerticalAlignment.Top);
            m_TextSeason = new TextUtil(ThisGame, Vector2.Zero, FontMedium, seasonTextColor, Color.Black, new Vector2(10, 0),
                HorizontalAlignment.Left, VerticalAlignment.Top);

            m_Crosshair = new Crosshair(ThisGame, new Vector2(50, 50));
            AddComponentNoUpdate(m_Crosshair);
            m_Crosshair.GunFired += CrosshairGunFired;


        }
        private void AddRabbit()
        {
            Sprite startHole = m_holes[RealRandom.Create(0, m_holes.Count).Next()];
            Rabbit rabbit = SpriteFactory.CreateRabbit(ThisGame, startHole.Position, Rabbit.GetRandomSex(), false);
            rabbit.Target = GetRandomTarget(startHole.Position);

            rabbit.PointTowards(rabbit.Target);
            AddComponentNoUpdate(rabbit);
            m_Rabbits.Add(rabbit);
        }


        private void AddRabbitAfterMating(MatingRabbits matingRabbits)
        {
            //Two Addults
            var spread = new Vector2(30, 0);
            var malePos = matingRabbits.Position + spread;
            var femalePos = matingRabbits.Position - spread;
            var male = SpriteFactory.CreateRabbit(ThisGame, malePos, Sex.Male, false);
            male.Target = GetRandomTarget(malePos);
            male.PointTowards(male.Target);
            AddComponentNoUpdate(male);
            m_Rabbits.Add(male);

            var female = SpriteFactory.CreateRabbit(ThisGame, femalePos, Sex.Female, false);
            female.Target = GetRandomTarget(femalePos);
            female.PointTowards(female.Target);
            AddComponentNoUpdate(female);
            m_Rabbits.Add(female);

            //Add Kid
            AddRabbitKid(female.Position);

            RemoveMating(matingRabbits);
        }

        private void AddRabbitKid(Vector2 femalePos)
        {
            Rabbit rabbit = SpriteFactory.CreateRabbit(ThisGame, femalePos, Rabbit.GetRandomSex(), true);
            rabbit.Target = GetRandomTarget(femalePos);

            rabbit.PointTowards(rabbit.Target);
            AddComponentNoUpdate(rabbit);
            m_Rabbits.Add(rabbit);
        }

        private void RemoveMating(MatingRabbits matingRabbits)
        {
            m_MatingRabbits.Remove(matingRabbits);
            RemoveComponentNoUpdate(matingRabbits);
        }

        private Vector2 GetRandomTarget(Vector2 notThisPosition)
        {
            //return m_holes[0].Position;
            if (RealRandom.Create(0, 2).Next() == 0)
            {
                return m_Carrot.Position;
            }
            else
            {
                return GetRandomOtherHole(notThisPosition);
            }
        }

        private void CrosshairGunFired(object sender, EventArgs<GameTime> e)
        {
            m_smokeParticleSystem.AddParticles(m_gun.Position);
            Rabbit rabbitToCheck = null;
            for (int i = m_Rabbits.Count - 1; i >= 0; i--)
            {

                rabbitToCheck = m_Rabbits[i];

                if ((rabbitToCheck.Position - m_Crosshair.Position).Length() < (rabbitToCheck.TouchDistance + m_Crosshair.TouchDistance))
                {

                    rabbitToCheck.Position += new Vector2(RealRandom.Create(0, 20).Next() - 10, -RealRandom.Create(0, 8).Next());

                    if (!rabbitToCheck.IsDead)
                    {
                        KillRabbit(rabbitToCheck, e.Data);
                    }

                    ThisGame.CurrentPoints += rabbitToCheck.PointsForKill;
                    rabbitToCheck.PointsForKill++;
                    XACT.PlayerHit();


                    SquirtBlood(rabbitToCheck.Position - new Vector2(0, -10), e.Data);


                    return;
                }
                else
                {
                    for (int f = m_MatingRabbits.Count - 1; f >= 0; f--)
                    {
                        MatingRabbits matingRabbitsToCheck = m_MatingRabbits[f];

                        if ((matingRabbitsToCheck.Position - m_Crosshair.Position).Length() < matingRabbitsToCheck.TouchDistance
                            )
                        {
                            m_MatingRabbits.Remove(matingRabbitsToCheck);
                            RemoveComponentNoUpdate(matingRabbitsToCheck);
                            SquirtBlood(matingRabbitsToCheck.Position, e.Data);
                            XACT.PlayerHit();

                            Rabbit rabbit = SpriteFactory.CreateRabbit(ThisGame, matingRabbitsToCheck.Position + new Vector2(-10, RealRandom.Create(0, 15).Next()), Sex.Female, false);
                            AddComponentNoUpdate(rabbit);
                            m_Rabbits.Add(rabbit);
                            KillRabbit(rabbit, e.Data);
                            ThisGame.CurrentPoints += rabbit.PointsForKill;
                            rabbit.PointsForKill++;

                            rabbit = SpriteFactory.CreateRabbit(ThisGame, matingRabbitsToCheck.Position + new Vector2(10, RealRandom.Create(0, 15).Next()), Sex.Male, false);
                            AddComponentNoUpdate(rabbit);
                            m_Rabbits.Add(rabbit);
                            KillRabbit(rabbit, e.Data);
                            ThisGame.CurrentPoints += rabbit.PointsForKill;
                            rabbit.PointsForKill++;
                            return;
                        }
                    }

                }
            }
            XACT.PlayerMiss();
        }

        private void KillRabbit(Rabbit rabbitToKill, GameTime time)
        {
            rabbitToKill.Kill();
            m_DeadRabbits.Add(new DeadController<Rabbit>()
            {
                DeadObject = rabbitToKill,
                TimeOfDeath = time.TotalGameTime
            });

        }

        private void SquirtBlood(Vector2 position, GameTime time)
        {
            Blood blod = SpriteFactory.CreateBlood(ThisGame, position);
            blodPletter.Add(blod);
            AddComponentNoUpdate(blod);
            m_bloodParticleSystem.AddParticles(position);
            m_Blood.Add(new DeadController<Blood>() { DeadObject = blod, TimeOfDeath = time.TotalGameTime });
        }


        private void RemoveKilledRabbits(GameTime time)
        {
            TimeSpan twoSecondsAgo = time.TotalGameTime - m_twoSeconds;
            for (int i = m_DeadRabbits.Count - 1; i >= 0; i--)
            {
                if (m_DeadRabbits[i].TimeOfDeath < twoSecondsAgo)
                {
                    RemoveComponentNoUpdate(m_DeadRabbits[i].DeadObject);
                    m_Rabbits.Remove(m_DeadRabbits[i].DeadObject);
                    SpriteFactory.Remove(m_DeadRabbits[i].DeadObject);
                    m_DeadRabbits.RemoveAt(i);
                }
            }
        }



        private void RemoveOldBlood(GameTime time)
        {
            TimeSpan tenSecondsAgo = time.TotalGameTime - m_tenSeconds;
            for (int i = m_Blood.Count - 1; i >= 0; i--)
            {
                if (m_Blood[i].TimeOfDeath < tenSecondsAgo)
                {
                    RemoveComponentNoUpdate(m_Blood[i].DeadObject);
                    blodPletter.Remove(m_Blood[i].DeadObject);
                    SpriteFactory.Remove(m_Blood[i].DeadObject);
                    m_Blood.RemoveAt(i);
                }
            }
        }


        public bool IsGameOver()
        {
            return m_gameOver;
        }

        Vector2 GetRandomOtherHole(Vector2 notThisPosition)
        {
            Vector2 randomPositionFound = notThisPosition;
            while (randomPositionFound == notThisPosition)
            {
                randomPositionFound = holePositions[RealRandom.Create(0, holePositions.Count).Next()];
            }
            return randomPositionFound;
        }

        public override void Reset()
        {
            if (spawnTimer == null)
            {
                spawnTimer = new Timer(ThisGame, 800);
                spawnTimer.Repeat = true;
                spawnTimer.TimesUp += SpawnTimerTimesUp;
                this.AddComponentNoUpdate(spawnTimer);
            }

            if (m_MatingCollisionCheckTimer == null)
            {
                m_MatingCollisionCheckTimer = new Timer(ThisGame, 500);
                m_MatingCollisionCheckTimer.Repeat = true;
                m_MatingCollisionCheckTimer.TimesUp += MatingTimeCollisionCheckTimesUp;
                this.AddComponentNoUpdate(m_MatingCollisionCheckTimer);
            }
            MakeHoles();

            m_smokeParticleSystem = new SmokePlumeParticleSystem(ThisGame, 50, ThisGame.CurrentSpriteBatch);
            m_smokeParticleSystem.Initialize();
            AddComponentNoUpdate(m_smokeParticleSystem);

            m_bloodParticleSystem = new BloodsprayParticleSystem(ThisGame, 40, ThisGame.CurrentSpriteBatch);
            m_bloodParticleSystem.Initialize();
            AddComponentNoUpdate(m_bloodParticleSystem);
            season = Seasons.summer;
            seasonActivated = true;
            m_gameOver = false;
            m_LevelStartTime = TimeSpan.MinValue;
            sceneTime = 0.0f;
            secondsSeason = 30;
            summerColor = Color.Green;
            autumnColor = Color.YellowGreen;
            winterColor = new Color(255, 250, 250);
            springColor = new Color(0, 255, 127);
            BackgroundColor = Color.LightGreen;
            shiftColorSummerAutumn = Vector3.Zero;
            shiftColorAutumnWinter = Vector3.Zero;
            shiftColorWinterSpring = Vector3.Zero;
            shiftColorSpringSummer = Vector3.Zero;
            blodPletter = new List<Blood>();

            m_MatingRabbits = new List<MatingRabbits>();
            m_gun = null;
            m_Points = 0F;
            m_CurrentLevel = 1;
            m_gunHeight = 450;
            m_Grasses = new List<Grass>();
            m_Carrot = null;
            m_Rabbits = new List<Rabbit>();
            m_DeadRabbits = new List<DeadController<Rabbit>>();
            m_Blood = new List<DeadController<Blood>>();
            m_Fences = new List<Fence>();


            m_Carrot = new Carrot(ThisGame, ThisGame.GraphicsDevice.Viewport.GetCenter());
            AddComponentNoUpdate(m_Carrot);

            m_gun = new BarrelGun(ThisGame, new Vector2(0, m_gunHeight));
            AddComponentNoUpdate(m_gun);

            Fence fence = null;
            float width = 0;

            for (int i = 0; i < 25; i++)
            {
                m_Grasses.Add(new Grass(ThisGame,
                    new Vector2(
                        RealRandom.Create(0, (int)ThisGame.GraphicsDevice.Viewport.GetRight()).Next(),
                        RealRandom.Create(0, (int)ThisGame.GraphicsDevice.Viewport.GetBottom()).Next())));
            }
            foreach (var grass in m_Grasses)
            {
                AddComponentNoUpdate(grass);
            }

            for (int i = 0; i < 10; i++)
            {
                if (fence != null)
                {
                    width = fence.Width - 5;
                }

                fence = new Fence(ThisGame, i * width);
                m_Fences.Add(fence);
                AddComponentNoUpdate(fence);
            }


            ThisGame.CurrentPoints = 0;
        }
    }
}
