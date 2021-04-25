using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monogame.Cyphermaze.Core.Abstractions;
using Monogame.Cyphermaze.Core.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monogame.Cyphermaze.Core.Effects
{
    public class CyphertextEffect : Postprocessor
    {
        public Effect cyphertextEffect { get; private set; }
        public VertexBuffer CyphertextBuffer { get; private set; }
        public VertexPositionTexture[] Vertexes { get; private set; } = new VertexPositionTexture[] {
            new VertexPositionTexture(new Vector3(-1, -1, 0), new Vector2(0, 1)),
            new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1, -1, 0), new Vector2(1, 1)),
            new VertexPositionTexture(new Vector3(-1, 1, 0), new Vector2(0, 0)),
            new VertexPositionTexture(new Vector3(1, 1, 0), new Vector2(1, 0))
        };
        private static readonly int NUM_SLOTS = 124;
        private Texture2D TextTexture { get; }
        public CyphertextEffect(Game game) : base(game)
        {
            cyphertextEffect = Game.Content.Load<Effect>("cyphertext");
            TextTexture = BitmapFontGenerator.GenerateTextTexture(Game.Content.Load<Texture2D>("TextTexture"), NUM_SLOTS, Game.GraphicsDevice);

            TextureReader.ReadTexture(TextTexture);

            CyphertextBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionTexture), Vertexes.Length, BufferUsage.WriteOnly);
            CyphertextBuffer.SetData(Vertexes);

            cyphertextEffect.Parameters["textTex"].SetValue(TextTexture);
            cyphertextEffect.Parameters["numLetters"].SetValue(NUM_SLOTS);
        }

        public override void Apply(RenderTarget2D LastPrint)
        {
            cyphertextEffect.Parameters["dstLetterSize"].SetValue(new Vector2(1.0f / LastPrint.Width, 1.0f / LastPrint.Height));
            cyphertextEffect.Parameters["sourceTex"].SetValue(LastPrint);
            //game.GraphicsDevice.Clear(Color.CornflowerBlue);
            Game.GraphicsDevice.SetVertexBuffer(CyphertextBuffer);

            foreach (EffectPass pass in cyphertextEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
                //game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, 2);
            }
        }

        protected override void PreDrawExtension()
        {
            Game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            base.PreDrawExtension();
        }
    }
}
