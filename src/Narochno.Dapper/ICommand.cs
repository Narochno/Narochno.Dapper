namespace Narochno.Dapper
{
    public interface ICommand
    {
        void Execute(ISession session);
    }
}