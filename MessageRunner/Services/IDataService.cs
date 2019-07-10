using System.Threading.Tasks;
using System;

namespace MessageRunner
{
    public interface IDataService
    {
        string GetData(Object DataRequest, string DataMethod);
    }
}