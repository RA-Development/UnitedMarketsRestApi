using System;

namespace UnitedMarkets.Infrastructure.Data
{
    [Serializable]
    public class DataException : Exception
    {
        public DataException(string message)
            : base(message + " (DataException)")
        {
        }
    }
}