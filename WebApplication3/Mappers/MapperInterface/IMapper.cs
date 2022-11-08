namespace WebApplication3.Mappers.MapperInterface
{
    public interface IMapper<TInput, TOutput>
    {
        /// <summary>
        /// Maps from <see cref="TInput"/> to <see cref="TOutput"/>.
        /// </summary>
        /// <param name="toMap"></param>
        /// <returns></returns>
        public TOutput Map(TInput toMap);
    }
}
