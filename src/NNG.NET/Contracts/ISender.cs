using System;

namespace NNG.Contracts
{
    public interface ISender
    {
        void Send(Span<byte> message);
    }
}