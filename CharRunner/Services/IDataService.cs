using System.Threading.Tasks;
using System;

namespace CharRunner
{
    public interface IDataService
    {
        string GetData(Object DataRequest, string DataMethod);
    }
}