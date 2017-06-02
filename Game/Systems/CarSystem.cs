using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Game.Components;
using Microsoft.Xna.Framework;
using Spelkonstruktionsprojekt.ZEngine.Components;
using Spelkonstruktionsprojekt.ZEngine.Constants;
using Spelkonstruktionsprojekt.ZEngine.Managers;
using Spelkonstruktionsprojekt.ZEngine.Systems.InputHandler;
using ZEngine.Components;
using ZEngine.EventBus;
using ZEngine.Managers;

namespace Game.Systems
{
    public class CarSystem : ISystem
    {
        private EventBus EventBus = EventBus.Instance;
        private ComponentManager ComponentManager = ComponentManager.Instance;

        public CarSystem Start()
        {
            EventBus.Subscribe<InputEvent>("MountCar", Mount);
            EventBus.Subscribe<InputEvent>("UnmountCar", Unmount);
            return this;
        }

        public CarSystem Stop()
        {
            return this;
        }

        public void Mount(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var cars = ComponentManager.GetEntitiesWithComponent(typeof(CarComponent));
                if (cars.Count == 0) return;
                var car = cars.First();
                var carComponent = car.Value as CarComponent;
                if (carComponent.Driver != null) return;
                carComponent.Driver = inputEvent.EntityId;
                
                var driverRenderComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(inputEvent.EntityId);
                if (driverRenderComponent != null)
                {
                    driverRenderComponent.IsVisible = false;
                }
                var driverCollisionComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<CollisionComponent>(inputEvent.EntityId);
                if (driverCollisionComponent != null)
                {
                    driverCollisionComponent.Disabled = true;
                }

                var driverActiomBindings =
                    ComponentManager.GetEntityComponentOrDefault<ActionBindings>(carComponent.Driver.Value);

                var carId = car.Key;
                AddActionBindings(carId, driverActiomBindings);
                ComponentManager.AddComponentToEntity(new DriverComponent {Car = carId}, inputEvent.EntityId);
                
                var carLightsComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<CarLightsComponent>(carId);
                if (carLightsComponent != null)
                {
                    var leftLight =
                        ComponentManager.GetEntityComponentOrDefault<LightComponent>(carLightsComponent.LeftLight.Value);
                    var rightLight =
                        ComponentManager.GetEntityComponentOrDefault<LightComponent>(carLightsComponent.RightLight.Value);
                    leftLight.Light.Enabled = true;
                    rightLight.Light.Enabled = true;
                }
            }
        }

        public async void AddActionBindings(uint entity, ActionBindings actionBindings)
        {
            await Task.Delay(100);
            ComponentManager.AddComponentToEntity(actionBindings, entity);
        }

        private void TurnOnCarLights()
        {
            
        }

        private void Unmount(InputEvent inputEvent)
        {
            if (inputEvent.KeyEvent == ActionBindings.KeyEvent.KeyPressed)
            {
                var driverComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<DriverComponent>(inputEvent.EntityId);
                if (driverComponent == null) return;
                var driverRenderComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<RenderComponent>(inputEvent.EntityId);
                if (driverRenderComponent != null)
                {
                    driverRenderComponent.IsVisible = true;
                }
                var driverCollisionComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<CollisionComponent>(inputEvent.EntityId);
                if (driverCollisionComponent != null)
                {
                    driverCollisionComponent.Disabled = false;
                }
                var carComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<CarComponent>(driverComponent.Car);
                carComponent.Driver = null;
                ComponentManager.Instance.RemoveComponentFromEntity<DriverComponent>(inputEvent.EntityId);
                
                var carLightsComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<CarLightsComponent>(driverComponent.Car);
                if (carLightsComponent != null)
                {
                    var leftLight =
                        ComponentManager.GetEntityComponentOrDefault<LightComponent>(carLightsComponent.LeftLight.Value);
                    var rightLight =
                        ComponentManager.GetEntityComponentOrDefault<LightComponent>(carLightsComponent.RightLight.Value);
                    leftLight.Light.Enabled = false;
                    rightLight.Light.Enabled = false;
                }
            }
        }

        public void Update()
        {
            foreach (var entity in ComponentManager.Instance.GetEntitiesWithComponent(typeof(CarComponent)))
            {
                var carId = entity.Key;
                var carComponent = entity.Value as CarComponent;
                if (carComponent.Driver == null){ continue;}

                var driverPosition =
                    ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(carComponent.Driver.Value);
                if (driverPosition == null) continue;
                var driverMoveComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(carComponent.Driver.Value);
                if (driverMoveComponent == null) continue;

                var carPosition =
                    ComponentManager.Instance.GetEntityComponentOrDefault<PositionComponent>(carId);
                if (carPosition == null) continue;
                var carMoveComponent = ComponentManager.Instance.GetEntityComponentOrDefault<MoveComponent>(carId);
                if (carMoveComponent == null) return;

                var carLightsComponent =
                    ComponentManager.Instance.GetEntityComponentOrDefault<CarLightsComponent>(carId);
                if (carLightsComponent != null)
                {
                    var carDimensions = ComponentManager.GetEntityComponentOrDefault<DimensionsComponent>(carId);
                    var xOffset = 120;
                    var yOffset = 15;
                    var transformation =
                        Matrix.CreateTranslation(new Vector3(
                            (float) (-carPosition.Position.X - carDimensions.Width * 0.5 + xOffset),
                            (float) (-carPosition.Position.Y - carDimensions.Height * 0.5 + yOffset), 0)) *
                        Matrix.CreateRotationZ(carMoveComponent.Direction) *
                        Matrix.CreateTranslation(carPosition.Position.X, carPosition.Position.Y, 0f);

                    var lightPos = new Vector2(carPosition.Position.X + xOffset, carPosition.Position.Y + yOffset);
                    var finalPosition = Vector2.Transform(lightPos, transformation);
                    
                    var RxOffset = 120;
                    var RyOffset = 55;
                    var Rtransformation =
                        Matrix.CreateTranslation(new Vector3(
                            (float) (-carPosition.Position.X - carDimensions.Width * 0.5 + RxOffset),
                            (float) (-carPosition.Position.Y - carDimensions.Height * 0.5 + RyOffset), 0)) *
                        Matrix.CreateRotationZ(carMoveComponent.Direction) *
                        Matrix.CreateTranslation(carPosition.Position.X, carPosition.Position.Y, 0f);

                    var RlightPos = new Vector2(carPosition.Position.X + RxOffset, carPosition.Position.Y + RyOffset);
                    var RfinalPosition = Vector2.Transform(RlightPos, Rtransformation);
                    
                    var leftPosition =
                        ComponentManager.GetEntityComponentOrDefault<PositionComponent>(carLightsComponent.LeftLight.Value);
                    var rightPosition =
                        ComponentManager.GetEntityComponentOrDefault<PositionComponent>(carLightsComponent.RightLight.Value);
                    var leftLight =
                        ComponentManager.GetEntityComponentOrDefault<LightComponent>(carLightsComponent.LeftLight.Value);
                    var rightLight =
                        ComponentManager.GetEntityComponentOrDefault<LightComponent>(carLightsComponent.RightLight.Value);
                    leftPosition.Position = finalPosition;
                    leftLight.Light.Rotation = carMoveComponent.Direction;
                    rightPosition.Position = RfinalPosition;
                    rightLight.Light.Rotation = carMoveComponent.Direction;
                }
                driverPosition.Position = new Vector2(carPosition.Position.X, carPosition.Position.Y);

//                carPosition.Position = new Vector2(driverPosition.Position.X, driverPosition.Position.Y);
//                carMoveComponent.Direction = driverMoveComponent.Direction;
            }
        }

        public struct UnmountEvent
        {
            public uint Driver;
        }
    }
}