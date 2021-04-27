using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace Monogame.Cyphermaze.Core.Abstractions
{
    public abstract class Postprocessor
    {
        public RenderTarget2D LastPrint { get; protected set; }
        protected Game Game { get; }
        protected Postprocessor(Game game)
        {
            Game = game;
            LastPrint = new RenderTarget2D(game.GraphicsDevice, game.GraphicsDevice.PresentationParameters.BackBufferWidth, game.GraphicsDevice.PresentationParameters.BackBufferHeight);
        }

        protected Postprocessor(Game game, int xRes, int yRes)
        {
            Game = game;
            LastPrint = new RenderTarget2D(game.GraphicsDevice, xRes, yRes,true,SurfaceFormat.Color,DepthFormat.Depth16);
        }
        public void BeforeDraw()
        {
            Game.GraphicsDevice.SetRenderTarget(LastPrint);
            PreDrawExtension();
        }
        protected virtual void PreDrawExtension() { }

        public void AfterDraw()
        {
            Game.GraphicsDevice.SetRenderTarget(null);
            PostDrawExtension();
        }
        protected virtual void PostDrawExtension() { }

        public virtual void Apply() { }
        public virtual void Apply(RenderTarget2D LastPrint) { Apply(); }
    }
}
