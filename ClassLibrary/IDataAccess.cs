namespace ClassLibrary
{
    public interface IDataAccess
    {
        string Data { get; set; }

        string GetData();
        void SetData(string device);
    }
}