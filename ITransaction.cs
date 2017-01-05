using System;

namespace Narochno.Data
{
    public interface ITransaction : IDisposable
    {
        void Commit();
    }
}