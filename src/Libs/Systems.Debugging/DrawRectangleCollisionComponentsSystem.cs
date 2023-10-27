// using Entitas;
// using Entitas.Extended;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Serilog;
//
// namespace Systems.Debugging;
//
// public class DrawRectangleCollisionComponentsSystem : IDrawSystem
// {
//     private readonly Contexts _contexts;
//     private readonly IGroup<GameEntity> _group;
//     private readonly ILogger _logger;
//
//     public DrawRectangleCollisionComponentsSystem(Contexts contexts, IGroup<GameEntity> group, ILogger logger)
//     {
//         _contexts = contexts;
//         _group = group;
//         _logger = logger;
//     }
//
//     // TODO: Move the logic in the release features
//     // TODO: Pivot & anchors system
//     // TODO: Make circle colliders & relative system (sloped collision)
//     public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
//     {
//         GameEntity[] entities = _group.GetEntities();
//
//         spriteBatch.Begin(samplerState: SamplerState.PointClamp);
//
//         Color[] colors = { Color.Black, Color.White };
//         int k = 0;
//
//         foreach (GameEntity e in entities)
//         {
//             var texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
//             texture.SetData(new[] { colors[k] });
//
//             ++k;
//             if (k == colors.Length)
//                 k = 0;
//
//             spriteBatch.Draw(texture, e.transform.Position, sourceRectangle: e.rectangleCollision.Size,
//                 Color.White);
//         }
//
//         spriteBatch.End();
//     }
//
// }
