namespace ServerApp
{
    public class DataAccess : IDataAccess
    {
        public string Data { get; set; }

        public string GetData()
        {
            return Data;
        }

        public void SetData(string device)
        {
            this.Data = device;
        }
    }
}
