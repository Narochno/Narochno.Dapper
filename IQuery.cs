namespace Narochno.Data
{
    public interface IQuery<out T>
    {
        T Execute(ISession session);
    }
}
