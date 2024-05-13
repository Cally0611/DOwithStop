namespace DOwithStop.Interfaces
{
    public interface ISqlOperation
    {
      
        public List<T> ExecuteSPsql<T>(string spname) where T : new();
    }
}
