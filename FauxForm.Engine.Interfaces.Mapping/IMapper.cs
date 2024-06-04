namespace FauxForm.Engine.Interfaces.Mapping;
public interface IMapper<T, TMapping>
{
    TMapping Map(T? entity);
    T Map(TMapping? entity);
}
