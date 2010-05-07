using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PTC.Utils;

namespace PTC.Text
{
    public class TextUtil : DrawableGameComponent
    {
        private SpriteFont m_Font;
        private Color m_Color;
        private Color m_ShadowColor;
        private Vector2 m_Offset;
        private HorizontalAlignment m_HAlign;
        private VerticalAlignment m_VAlign;
        private string[] m_Texts;

        private Rectangle m_View = Rectangle.Empty;
        private Resetable<Vector2> m_CurrentScrollPosition = new Resetable<Vector2>();
        //private Vector2 m_CurrentScrollPosition = Vector2.Zero;
        private Vector2 m_ScrollSpeed;
        private bool m_Repeating = false;
        private bool m_UseShadow = false;
        private int m_ScrollboxWidth = 0; //Width of the Box measured in X's in the current font
        private int m_ScrollboxHeight = 0; //Height of the Box measured in X's in the current font
        private float m_TextHeight;
        private float m_TextWidth;

        public TextUtil(PTCGame game, SpriteFont font,
            Color color, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va, params string[] texts) : base(game)
        {
            m_Font = font;
            m_Color = color;
            m_Offset = offset;
            m_HAlign = ha;
            m_VAlign = va;
            m_Texts = texts;
            m_TextHeight = TextHeight(font, texts);
            m_TextWidth = TextWidth(font, texts);
        }

        public TextUtil(PTCGame game, int scrollBoxHeight, int scrollBoxWidth, Vector2 scrollSpeed, bool repeating, bool startOusideScrollbox, 
            SpriteFont font, Color color, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va, params string[] texts)
            : this(game, font, color, offset, ha, va, texts)
        {
            m_View = GetView(scrollBoxHeight, scrollBoxWidth, font, ha, va, offset, texts);
            m_Offset.X -= m_View.X;
            m_Offset.Y -= m_View.Y;
            if (startOusideScrollbox)
            {
                m_CurrentScrollPosition.Value = StartOutsideScrollbox(m_View, m_CurrentScrollPosition.Value, scrollSpeed);
                m_CurrentScrollPosition.Set();
            }
            m_ScrollSpeed = scrollSpeed;
            m_Repeating = repeating;
            m_ScrollboxHeight = scrollBoxHeight;
            m_ScrollboxWidth = scrollBoxWidth;
        }

        public TextUtil(PTCGame game, SpriteFont font,
            Color color, Color shadowColor, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va,
            params string[] texts)
            : this(game, font, color, offset, ha, va, texts)
        {
            m_ShadowColor = shadowColor;
            m_UseShadow = true;
        }

        public TextUtil(PTCGame game, int scrollBoxHeight, int scrollBoxWidth, Vector2 scrollSpeed, bool repeating, bool startOusideScrollbox, SpriteFont font,
            Color color, Color shadowColor, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va,
            params string[] texts)
            : this(game, scrollBoxHeight, scrollBoxWidth, scrollSpeed, repeating, startOusideScrollbox, font, color, offset, ha, va, texts)
        {
            m_ShadowColor = shadowColor;
            m_UseShadow = true;
        }

        public void Reset()
        {
            m_CurrentScrollPosition.Reset();
        }

        private Vector2 StartOutsideScrollbox(Rectangle scrollBox, Vector2 currentOffset, Vector2 scrollSpeed)
        {
            var scrollBoxVector = new Vector2(scrollBox.Width, scrollBox.Height);
            //The antiSpeedUnitVector represents the opposite direction of the speed vector, but expressed as -1,0,1 in each direction.
            //To be used for multiplying with the scrollBoxVector
            var antiSpeedUnitVector = new Vector2(-Math.Sign(scrollSpeed.X), -Math.Sign(scrollSpeed.Y));
            return currentOffset + scrollBoxVector * antiSpeedUnitVector;
        }

        private Rectangle GetView(int scrollBoxHeight, int scrollBoxWidth, SpriteFont font, 
            HorizontalAlignment ha, VerticalAlignment va, Vector2 offSet, string[] texts)
        {
            Vector2 startPos = GetTopLeftMostPosition(font, ha, va, offSet, texts);
            Vector2 measure = font.MeasureString("X");
            return new Rectangle((int)startPos.X, (int)startPos.Y, (int)(measure.X * scrollBoxWidth), (int)(measure.Y * scrollBoxHeight));  
        }

        private Vector2 GetTopLeftMostPosition(SpriteFont font, HorizontalAlignment ha, VerticalAlignment va, Vector2 offSet, params string[] texts)
        {
            Vector2 pos = new Vector2(float.MaxValue);
            foreach (Vector2 position in GetPositions(font, ha, va, texts))
            {
                if (pos.X > position.X)
                {
                    pos.X = position.X;
                }

                if (pos.Y > position.Y)
                {
                    pos.Y = position.Y;
                }
            }
            return pos + offSet;
        }

        public PTCGame ThisGame
        {
            get { return Game as PTCGame; }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            Write(m_Font, m_Color, m_ShadowColor, m_Offset, m_HAlign, m_VAlign, m_Texts);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_CurrentScrollPosition.Value += m_ScrollSpeed * gameTime.ElapsedGameTime.Milliseconds / 17F;
            CheckBounds();
        }
        
        private void Write(SpriteFont font, Color color, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va, params string[] texts)
        {
            int index = 0;
            foreach (Vector2 pos in GetPositions(font, ha, va, texts))
            {
                Viewport savedViewport = new Viewport();
                if (m_View != Rectangle.Empty)
                {
                    ThisGame.CurrentSpriteBatch.End();
                    ThisGame.CurrentSpriteBatch.Begin();
                    savedViewport = ThisGame.GraphicsDevice.Viewport;
                    Viewport currentViewPort = ThisGame.GraphicsDevice.Viewport;
                    currentViewPort.Width = m_View.Width;
                    currentViewPort.Height = m_View.Height;
                    currentViewPort.X = m_View.X;
                    currentViewPort.Y = m_View.Y;
                    ThisGame.GraphicsDevice.Viewport = currentViewPort;
                }

                m_PositionTopLeft = pos + offset + m_CurrentScrollPosition;
                ThisGame.CurrentSpriteBatch.DrawString(font, texts[index], m_PositionTopLeft, color, 0F, Vector2.Zero,
                    Vector2.One, SpriteEffects.None, 0F);

                if (m_View != Rectangle.Empty)
                {
                    ThisGame.CurrentSpriteBatch.End();
                    ThisGame.CurrentSpriteBatch.Begin();
                    ThisGame.GraphicsDevice.Viewport = savedViewport;
                }

                index++;
            }
        }

        private Vector2 m_PositionTopLeft;
        protected Vector2 PositionTopLeft
        {
            get { return m_PositionTopLeft; }
        }

        private void Write(SpriteFont font, Color color, Color shadowColor, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va, params string[] texts)
        {
            if (m_UseShadow)
            {
                //First the Shadow
                Write(font, shadowColor, offset + new Vector2(2), ha, va, texts);
            }
            //Then the real color
            Write(font, color, offset, ha, va, texts);
        }

        private void CheckBounds()
        {
            if (!m_Repeating)
                return;

            if (m_CurrentScrollPosition.Value.X > m_View.Width)
                m_CurrentScrollPosition.Value.X = -m_TextWidth;

            if (m_CurrentScrollPosition.Value.Y > m_View.Height)
                m_CurrentScrollPosition.Value.Y = -m_View.Height;

            if (m_CurrentScrollPosition.Value.X < -m_TextWidth)
                m_CurrentScrollPosition.Value.X = m_View.Width;

            if (m_CurrentScrollPosition.Value.Y < -m_TextHeight)
                m_CurrentScrollPosition.Value.Y = m_View.Height;
        }

        private IEnumerable<Vector2> GetPositions(SpriteFont font, HorizontalAlignment ha, VerticalAlignment va, params string[] texts)
        {
            float verticalPos = GetVerticalPosition(font, va, texts);
            foreach(string text in texts)
            {
                yield return
                    new Vector2(
                        GetHorizontalPosition(font, ha, text),
                        verticalPos);
                verticalPos += TextHeight(font, text);
            }
        }

        public void SetText(params string[] texts)
        {
            m_Texts = texts;
            m_TextHeight = TextHeight(m_Font, texts);
            m_TextWidth = TextWidth(m_Font, texts);
        }

        private float GetVerticalPosition(SpriteFont font, VerticalAlignment va, string[] texts)
        {
            switch (va)
            {
                case VerticalAlignment.Center:
                    return (GameHeight- TextHeight(font, texts)) / 2F;
                case VerticalAlignment.Top:
                    return 0F;
                case VerticalAlignment.Bottom:
                    return GameHeight - TextHeight(font, texts);
                default:
                    return (GameHeight - TextHeight(font, texts)) / 2F;
            }
        }

        private float GetHorizontalPosition(SpriteFont font, HorizontalAlignment ha, string text)
        {
            switch (ha)
            {
                case HorizontalAlignment.Center:
                    return (GameWidth - TextWidth(font, text)) / 2F;
                case HorizontalAlignment.Left:
                    return 0F;
                case HorizontalAlignment.Right:
                    return GameWidth - TextWidth(font, text);
                default:
                    return (GameWidth - TextWidth(font, text)) / 2F;
            }
        }

        private float GameWidth
        {
            get
            {
                return ThisGame.GraphicsDevice.Viewport.Width;
            }
        }

        private float GameHeight
        {
            get
            {
                return ThisGame.GraphicsDevice.Viewport.Height;
            }
        }

        private float TextHeight(SpriteFont font, string[] texts)
        {
            float height = 0F;
            foreach (string text in texts)
            {
                height += TextHeight(font, text);
            }
            return height;
        }

        private float TextWidth(SpriteFont font, string[] texts)
        {
            float width = 0F;
            foreach (string text in texts)
            {
                width = Math.Max(width, TextWidth(font, text));
            }
            return width;
        }

        private float TextWidth(SpriteFont font, string text)
        {
            if (text == string.Empty)
                return font.MeasureString("X").X;

            return font.MeasureString(text).X;
        }

        private float TextHeight(SpriteFont font, string text)
        {
            if (text == string.Empty)
                return font.MeasureString("X").Y;

            return font.MeasureString(text).Y;
        }
    }
}
