using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monogame.Cyphermaze.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monogame.Cyphermaze.Core.Effects
{
    public class TraingleEffect : Postprocessor
    {
        public BasicEffect TriangleEffect { get; private set; }
        public VertexBuffer TriangleBuffer { get; private set; }
        public bool IsTriangleDrawn { get; private set; } = false;
        public TraingleEffect(Game game) : base(game)
        {
            TriangleEffect = new BasicEffect(game.GraphicsDevice);
            TriangleEffect.World = Matrix.CreateTranslation(0, 0, 0);
            TriangleEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            TriangleEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);
            TriangleEffect.VertexColorEnabled = true;

            var vertices = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(0, 1, 0), Color.Red),
                new VertexPositionColor(new Vector3(+0.5f, 0, 0), Color.Green),
                new VertexPositionColor(new Vector3(-0.5f, 0, 0), Color.Blue)
            };
            TriangleBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            TriangleBuffer.SetData(vertices);
        }

        public override void Apply()
        {
            if (!IsTriangleDrawn) {
                BeforeDraw();
                Game.GraphicsDevice.SetVertexBuffer(TriangleBuffer);
                foreach (EffectPass pass in TriangleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                }
                AfterDraw();
            }
        }

        protected override void PreDrawExtension()
        {
            Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            base.PreDrawExtension();
        }

        protected override void PostDrawExtension()
        {
            IsTriangleDrawn = true;
            base.PostDrawExtension();
        }
    }
}
