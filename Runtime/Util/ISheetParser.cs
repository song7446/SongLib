namespace SongLib
{
    public interface ISheetParser<T>
    {
        T Parse(string csv);
    }

}
