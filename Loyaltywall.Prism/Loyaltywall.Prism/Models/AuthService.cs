using System;

namespace Loyaltywall.Prism.Models
{
    public class AuthService
    {
        private string _state;

        public string GenerateState()
        {
            _state = Guid.NewGuid().ToString();
            return _state;
        }

        public bool ValidateState(string state)
        {
            return state == _state;
        }
    }
}
