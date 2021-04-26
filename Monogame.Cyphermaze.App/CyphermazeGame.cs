using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Monogame.Cyphermaze.Core.Abstractions;
using Monogame.Cyphermaze.Core.Effects;
using System.Collections.Generic;

namespace Monogame.Cyphermaze.App
{
    public class CyphermazeGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D LastPrint;
        private IList<Postprocessor> Postprocessors { get; } = new List<Postprocessor>();

        public CyphermazeGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            var MagicNumbers = new
            {
                WidthMult = (1.0f / 10.0f),
                HeightMult = (1.0f / 10.0f),
                NumSlot = 256
            };

            var WidthHeight = new
            {
                Width = (int)(GraphicsDevice.Viewport.Width * MagicNumbers.WidthMult),
                Height = (int)(GraphicsDevice.Viewport.Height * MagicNumbers.HeightMult)
            };

            var geometryEffect = new IcosahedronEffect(this, WidthHeight.Width, WidthHeight.Height);
            LastPrint = geometryEffect.LastPrint;
            Postprocessors.Add(geometryEffect);

            Postprocessors.Add(new CyphertextEffect(this, WidthHeight.Width, WidthHeight.Height, MagicNumbers.NumSlot));
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            foreach (var postprocessor in Postprocessors)
            {
                postprocessor.Apply(LastPrint);
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
