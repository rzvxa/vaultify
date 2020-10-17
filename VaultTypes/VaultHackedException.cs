using System;

namespace Vaultify.VaultTypes
{
    public class VaultTypeHackedException : Exception
    {
        public VaultTypeHackedException() { }
        public VaultTypeHackedException(string message) : base(message) { }
        public VaultTypeHackedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
