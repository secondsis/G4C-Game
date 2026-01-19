using System;

namespace Main.Scripts
{
    public class Interactable
    {
        private readonly InteractionType _interactType;
        private readonly Action _onInteract;
        // TempDisabled acts as a way to disable Interact (for ex. in dialog choice select) while still blocking others
        public bool TempDisabled;

        public Interactable(InteractionType interactType,  Action onInteract)
        {
            _interactType = interactType;
            _onInteract = onInteract;
            TempDisabled = false;
        }

        public void Interact()
        {
            _onInteract();
        }

        public InteractionType GetInteractType()
        {
            return _interactType;
        }

        public bool RawEquals(InteractionType interactionType, Action onInteract)
        {
            return _interactType == interactionType && _onInteract == onInteract;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Interactable) return false;
            Interactable other = (Interactable)obj;
            return other._interactType == _interactType && other._onInteract.Equals(_onInteract);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_interactType, _onInteract);
        }
    }
}