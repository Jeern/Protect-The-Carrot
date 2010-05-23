using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private string[] m_Texts = null;

        private Rectangle m_ViewBox = Rectangle.Empty;
        private Resetable<Vector2> m_CurrentScrollPosition = new Resetable<Vector2>();
        private Vector2 m_ScrollSpeed;
        private bool m_UseShadow = false;
        private float m_TextHeight;
        private float m_TextWidth;
        private float m_GameWidth = 0F;
        private float m_GameHeight = 0F;

        public TextUtil(PTCGame game, Vector2 scrollSpeed, SpriteFont font,
            Color color, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va) : base(game)
        {
            m_ScrollSpeed = scrollSpeed;
            m_Font = font;
            m_Color = color;
            m_Offset = offset;
            m_HAlign = ha;
            m_VAlign = va;
            m_CurrentScrollPosition.Value = Vector2.Zero;
            m_CurrentScrollPosition.Set();
            m_GameHeight = ThisGame.GraphicsDevice.Viewport.Height;
            m_GameWidth = ThisGame.GraphicsDevice.Viewport.Width;
        }

        public TextUtil(PTCGame game, Rectangle viewBox, Vector2 scrollSpeed, SpriteFont font, Color color, Vector2 offset, 
            HorizontalAlignment ha, VerticalAlignment va)
            : this(game, scrollSpeed, font, color, offset, ha, va)
        {
            m_ViewBox = viewBox;
            m_Offset.X -= viewBox.X;
            m_Offset.Y -= viewBox.Y;
        }

        public TextUtil(PTCGame game, Vector2 scrollSpeed, SpriteFont font, Color color, Color shadowColor, Vector2 offset, 
            HorizontalAlignment ha, VerticalAlignment va)
            : this(game, scrollSpeed, font, color, offset, ha, va)
        {
            m_ShadowColor = shadowColor;
            m_UseShadow = true;
        }

        public TextUtil(PTCGame game, Rectangle viewBox, Vector2 scrollSpeed, SpriteFont font,
            Color color, Color shadowColor, Vector2 offset, HorizontalAlignment ha, VerticalAlignment va)
            : this(game, viewBox, scrollSpeed, font, color, offset, ha, va)
        {
            m_ShadowColor = shadowColor;
            m_UseShadow = true;
        }

        public void Reset()
        {
            m_CurrentScrollPosition.Reset();
        }

        public bool Repeating { get; set; }

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
            if (texts == null)
                return;

            int index = 0;
            foreach (Vector2 pos in GetPositions(font, ha, va, texts))
            {
                Viewport savedViewport = new Viewport();
                if (m_ViewBox != Rectangle.Empty)
                {
                    ThisGame.CurrentSpriteBatch.End();
                    ThisGame.CurrentSpriteBatch.Begin();
                    savedViewport = ThisGame.GraphicsDevice.Viewport;
                    Viewport currentViewPort = ThisGame.GraphicsDevice.Viewport;
                    currentViewPort.Width = m_ViewBox.Width;
                    currentViewPort.Height = m_ViewBox.Height;
                    currentViewPort.X = m_ViewBox.X;
                    currentViewPort.Y = m_ViewBox.Y;
                    ThisGame.GraphicsDevice.Viewport = currentViewPort;
                }

                m_PositionTopLeft = pos + offset + m_CurrentScrollPosition; // +new Vector2(100F, 0F);
                m_IsAreaSet = true;

                ThisGame.CurrentSpriteBatch.DrawString(font, texts[index], m_PositionTopLeft, color, 0F, Vector2.Zero,
                    Vector2.One, SpriteEffects.None, 0F);

                if (m_ViewBox != Rectangle.Empty)
                {
                    ThisGame.CurrentSpriteBatch.End();
                    ThisGame.CurrentSpriteBatch.Begin();
                    ThisGame.GraphicsDevice.Viewport = savedViewport;
                }

                index++;
            }
        }

        private Vector2 m_PositionTopLeft = new Vector2(100);
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
            if (!Repeating)
                return;

            if (m_CurrentScrollPosition.Value.X > m_ViewBox.Width)
                m_CurrentScrollPosition.Value.X = -m_TextWidth;

            if (m_CurrentScrollPosition.Value.Y > m_ViewBox.Height)
                m_CurrentScrollPosition.Value.Y = -m_ViewBox.Height;

            if (m_CurrentScrollPosition.Value.X < -m_TextWidth)
                m_CurrentScrollPosition.Value.X = m_ViewBox.Width;

            if (m_CurrentScrollPosition.Value.Y < -m_TextHeight)
                m_CurrentScrollPosition.Value.Y = m_ViewBox.Height;
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
                return m_GameWidth;
            }
        }

        private float GameHeight
        {
            get
            {
                return m_GameHeight;
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

        private Rectangle m_Area = Rectangle.Empty;

        public Rectangle Area
        {
            get 
            {
                if (m_Area == Rectangle.Empty)
                {
                    m_Area = new Rectangle((int)PositionTopLeft.X, (int)PositionTopLeft.Y,
                        (int)m_TextWidth, (int)m_TextHeight);
                }
                return m_Area;
            }
        }

        private bool m_IsAreaSet = false;
        public bool IsAreaReady
        {
            get { return m_IsAreaSet; }
        }
    }
}
