using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PTC.Sequencer;
using System.Diagnostics;
using PTC.Utils;
using PTC.GraphicUtils;

namespace PTC.Sprites
{
    public class Rabbit : MovingSprite
    {
        public bool IsDead { get; set; }
        private Vector2 m_target;
        public Vector2 Target { get { return m_target; } set { m_target = value; PointTowards(Target); } }
        private Sex m_sex;
        public int PointsForKill { get; set; }
        public Sex Sex
        {
            get { return m_sex; }
            set
            {
                m_sex = value;
                switch (Sex)
                {
                    case Sex.Male:
                        TheColor = new Color(0x88, 0xFF, 0xFF);
                        break;
                    case Sex.Female:
                        TheColor = new Color(0xFF, 0x88, 0xFF);
                        break;
                }
            }
        }

        public Rabbit(Game game, Vector2 startPos, Sex sex, bool kid)
            : base(game, startPos)
        {
            Reset(startPos, sex, kid);
        }

        public void Reset(Vector2 startPos, Sex sex, bool kid)
        {
            Reset(startPos);
            if (kid)
            {
                Scale = 0.5F;
            }
            else
            {
                Scale = 1.5F;
            }
            Sex = sex;
            PointsForKill = 1;
            m_Kid = kid;
            IsDead = false;
            Target = Vector2.Zero;
            m_WasPaused = false;
            m_KidStageTime = TimeSpan.Zero;
            State = RabbitState.Jumping;
        }

        public override void Reset(Vector2 startPos)
        {
            base.Reset(startPos);
        }

        private bool m_Kid;
        public bool Kid
        {
            get { return m_Kid; }
            set { m_Kid = value; }
        }

        private SequencedIterator<int> m_DelayIterator = new SequencedIterator<int>(new RepeatingSequencer(0, 6), 10, 80, 20, 40, 80, 100, 10);
        private GameState m_RabbitGameState;

        protected override GameState GetImage()
        {
            m_RabbitGameState =
                new GameState(
                    new List<GameImage>() {
                        GetImageJumping(),
                        GetImageDead1(),
                        GetImageDead2(),
                        GetImageDead3()
                    }
                    );
            return m_RabbitGameState;
        }

        private GameImage GetImageDead1()
        {
            return new GameImage(
                        new SequencedIterator<Texture2D>(new ForwardingSequencer(0, 2),
                       new List<Texture2D>()
                    {
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_1"),
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_1"),
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_on_back")
                      }), new SequencedIterator<int>(new ForwardingSequencer(0, 2), 500, 500, 2000));
        }

        private GameImage GetImageDead2()
        {
            return new GameImage(
                        new SequencedIterator<Texture2D>(new ForwardingSequencer(0, 2),
                       new List<Texture2D>()
                    {
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_2"),
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_2"),
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_on_back")
                      }), new SequencedIterator<int>(new ForwardingSequencer(0, 2), 500, 500, 2000));
        }

        private GameImage GetImageDead3()
        {
            return new GameImage(
                        new SequencedIterator<Texture2D>(new ForwardingSequencer(0, 2),
                       new List<Texture2D>()
                    {
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_3"),
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_3"),
                        Game.Content.Load<Texture2D>(@"Rabbits\dead_rabbit_on_back")
                      }), new SequencedIterator<int>(new ForwardingSequencer(0, 2), 500, 500, 2000));
        }

        private GameImage GetImageJumping()
        {
            return new GameImage(
                        new SequencedIterator<Texture2D>(new RepeatingSequencer(0, 6),
                       new List<Texture2D>()
                    {
                        Game.Content.Load<Texture2D>(@"Rabbits\kanin_hopper_01_50"),
                        Game.Content.Load<Texture2D>(@"Rabbits\kanin_hopper_02_50"),
                        Game.Content.Load<Texture2D>(@"Rabbits\kanin_hopper_03_50"),
                        Game.Content.Load<Texture2D>(@"Rabbits\kanin_hopper_04_50"),
                        Game.Content.Load<Texture2D>(@"Rabbits\kanin_hopper_05_50"),
                        Game.Content.Load<Texture2D>(@"Rabbits\kanin_hopper_06_50"),
                        Game.Content.Load<Texture2D>(@"Rabbits\kanin_hopper_07_50")
                      }),
                      m_DelayIterator);
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position + JumpVector;
            }
            set
            {
                base.Position = value;
            }
        }

        public Vector2 JumpVector
        {
            get { return new Vector2(0F, -7F * JumpIndex * ZScale); }
        }

        public int JumpIndex
        {
            get
            {
                if (m_DelayIterator.CurrentIndex == 6)
                    return 0;
                if (m_DelayIterator.CurrentIndex == 5)
                    return 2;
                return m_DelayIterator.CurrentIndex;
            }
        }

        public override bool IsPaused
        {
            get
            {
                return (JumpIndex == 1 || State != RabbitState.Jumping || Kid);
            }
        }

        private bool m_WasPaused = false;

        public void CheckForBoing()
        {
            bool isPaused = IsPaused;
            if (isPaused && !m_WasPaused)
            {
                XACT.RabbitLands();
            }
            m_WasPaused = isPaused;
        }

        private TimeSpan m_KidStageTime;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CheckForBoing();
            if (!IsDead && Kid && gameTime.TotalGameTime.Subtract(m_KidStageTime) > new TimeSpan(0,0,0,0,600))
            {
                m_Scale += 0.1F;
                m_KidStageTime = gameTime.TotalGameTime; 
                if (m_Scale >= 1.5)
                {
                    m_Kid = false;
                }
            }
        }

        public static Sex GetRandomSex()
        {
            switch (RealRandom.Create(0, 2).Next())
            {
                case 0:
                    return Sex.Male;
                case 1:
                    return Sex.Female;
                default:
                    throw new InvalidOperationException("The Random Sex can only be Male or Female");
            }
        }

        public RabbitState State
        {
            get { return (RabbitState)m_RabbitGameState.State; }
            set { m_RabbitGameState.State = (int)value; }
        }

        public void Kill()
        {
            int newState = RealRandom.Create(1, 4).Next();
            IsDead = true;
            switch (newState)
            {
                case (int)RabbitState.Dead1:
                    State = RabbitState.Dead1;
                    break;
                case (int)RabbitState.Dead2:
                    State = RabbitState.Dead2;
                    break;
                case (int)RabbitState.Dead3:
                    State = RabbitState.Dead3;
                    break;
                default:
                    State = RabbitState.Dead1;
                    break;

            }
        }

    }
}
