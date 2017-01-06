using System;

namespace Narochno.Dapper
{
    public interface ITransaction : IDisposable
    {
        void Commit();
    }
}