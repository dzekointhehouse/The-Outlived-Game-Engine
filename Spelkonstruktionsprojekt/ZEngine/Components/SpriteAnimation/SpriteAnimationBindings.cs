using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using Microsoft.Xna.Framework;
using ZEngine.Components;

namespace Spelkonstruktionsprojekt.ZEngine.Components.SpriteAnimation
{
    public class SpriteAnimationBindings : IComponent
    {
        public List<SpriteAnimationBinding> Bindings = new List<SpriteAnimationBinding>();
    }

    public class SpriteAnimationBinding
    {
        public List<State> StateConditions { get; set; } = new List<State>();
        public Point StartPosition { get; set; }= default(Point);
        public Point EndPosition { get; set; }= default(Point);
        public double FrameLength { get; set; } = 16;
        public bool IsTransition { get; set; } = false;
    }

    public class SpriteAnimationBindingBuilder
    {
        private readonly SpriteAnimationBinding _spriteAnimationBinding = new SpriteAnimationBinding();

        public SpriteAnimationBindingBuilder StateConditions(params State[] stateConditions)
        {
            _spriteAnimationBinding.StateConditions = stateConditions.ToList();
            return this;
        }

        public SpriteAnimationBindingBuilder Positions(Point startPosition = default(Point), Point endPosition = default(Point))
        {
            _spriteAnimationBinding.StartPosition = startPosition;
            _spriteAnimationBinding.EndPosition = endPosition;
            return this;
        }

        public SpriteAnimationBindingBuilder Length(double lengthInMilliseconds)
        {
            _spriteAnimationBinding.FrameLength = lengthInMilliseconds;
            return this;
        }

        public SpriteAnimationBinding Build()
        {
            return _spriteAnimationBinding;
        }

        public SpriteAnimationBindingBuilder IsTransition(bool shouldEndAndStayOnFinalFrame)
        {
            _spriteAnimationBinding.IsTransition = shouldEndAndStayOnFinalFrame;
            return this;
        }
    }

    public class SpriteAnimationBindingsBuilder
    {
        private readonly SpriteAnimationBindings _spriteAnimationBindings = new SpriteAnimationBindings();

        public SpriteAnimationBindingsBuilder Binding(SpriteAnimationBinding binding)
        {
            _spriteAnimationBindings.Bindings.Add(binding);
            return this;
        }

        public SpriteAnimationBindings Build()
        {
            return _spriteAnimationBindings;
        }
    }
}