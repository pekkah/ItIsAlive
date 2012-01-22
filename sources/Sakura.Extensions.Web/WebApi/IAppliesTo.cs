namespace Sakura.Extensions.Web.WebApi
{
    public interface IApplies<in T>
    {
        bool To(T to);
    }
}