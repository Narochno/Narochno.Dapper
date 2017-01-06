namespace Narochno.Dapper
{
    public interface IQuery<out T>
    {
        T Execute(ISession session);
    }
}
