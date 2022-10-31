using MongoDB.Driver;
using MongoDB_Crud.Entities;

namespace MongoDB_Crud.Abstracts
{
    public interface IMobileStoreService
    {
        IMongoCollection<MobileDevice> mobileDeviceCollection { get;}


        IEnumerable<MobileDevice> GetAllMobileDevices();
        MobileDevice GetMobileDeviceDetails(string Name);
        void Create(MobileDevice mobileDevice);
        void Update(string _id, MobileDevice mobileDevice);
        void Delete(string Name);

    }
}
