using System.ServiceModel;

namespace PudderVarsel.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILoadDataService" in both code and config file together.
    [ServiceContract]
    public interface ILoadDataService
    {

        [OperationContract]
        void LoadData(System.Xml.Linq.XElement location, string name);

    }
}
