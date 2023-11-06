namespace ClassLibrary
{
    public class DataAccess : IDataAccess
    {
        public string Data { get; set; }

        public String GetData()
        {
            return Data;
        }

        public void SetData(string device)
        {
            this.Data = device;
        }
    }
}
