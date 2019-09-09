using System.Threading.Tasks;
using System;

namespace CharRadiologyWeb
{
    public interface IDataService
    {
        string GetData(Object DataRequest, string DataMethod);
    }
}