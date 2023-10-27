using Components.Render.Animation;
using Scellecs.Morpeh.Extended;

// namespace Entitas.Entities
// {
//     using Components.Data;
//
//     public class Player
//     {
//         public Player(Contexts contexts,
//             MovementAnimationComponent movementAnimationComponent,
//             TransformComponent transformComponent,
//             CameraComponent cameraComponent,
//             RectangleCollisionComponent rectangleCollisionComponent)
//         {
//             GameEntity entity = contexts.game.CreateEntity();
//
//             entity.isPlayer = true;
//             entity.isMovable = true;
//             entity.AddMovementAnimation(movementAnimationComponent);
//             entity.AddTransform(transformComponent);
//             entity.AddCamera(cameraComponent);
//             entity.AddRectangleCollision(rectangleCollisionComponent);
//         }
//     }
// }

namespace Entities
{
    using Scellecs.Morpeh;
    using Components.Tags;
    using Components.Data;

    // TODO: Might be good, if we create a base Entity class, and derive from that.
    // Which will contain components list, it might be easier then
    // to debug entities and their components with ImGui.
    // But also, for the factory, ig, the view of the entities class will change.
    // For now, Imma keep it as it is.
    public class PlayerEntity
    {
        private readonly PlayerComponent _player;
        private readonly MovableComponent _movable;
        private readonly TransformComponent _transform;
        private readonly RectangleCollisionComponent _rectangleCollision;
        private readonly MovementAnimationsComponent _movementAnimations;
        private readonly CharacterAnimatorComponent _characterAnimator;

        public PlayerEntity(PlayerComponent player,
            MovableComponent movable,
            TransformComponent transform,
            RectangleCollisionComponent rectangleCollision,
            MovementAnimationsComponent movementAnimations,
            CharacterAnimatorComponent characterAnimator)
        {
            _player = player;
            _movable = movable;
            _transform = transform;
            _rectangleCollision = rectangleCollision;
            _movementAnimations = movementAnimations;
            _characterAnimator = characterAnimator;
        }

        public Entity Create(World @in)
        {
            Entity e = @in.CreateEntity();

            AddTags(e);
            AddData(e);
            AddRender(e);

            return e;
        }

        private void AddTags(Entity e)
        {
            e.AddComponent(_player);
            e.AddComponent(_movable);
        }

        private void AddData(Entity e)
        {
            e.AddComponent(_transform);
            e.AddComponent(_rectangleCollision);
        }

        private void AddRender(Entity e)
        {
            e.AddComponent(_movementAnimations);
            e.AddComponent(_characterAnimator);
        }
    }
}
