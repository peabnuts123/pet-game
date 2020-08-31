export default abstract class ApiModel<TDto> {
  public abstract toDto(): TDto;
}
