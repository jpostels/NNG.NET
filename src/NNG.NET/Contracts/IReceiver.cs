using System;

namespace NNG.Contracts
{
    public interface IReceiver
    {
        Span<byte> Receive();
    }
}