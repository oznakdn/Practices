using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB_Crud.Abstracts;
using MongoDB_Crud.Data;
using MongoDB_Crud.Entities;

namespace MongoDB_Crud.Concretes
{
    public class MobileStoreService : IMobileStoreService
    {
        private readonly IMongoDatabase mongoDatabase;

        public MobileStoreService(IOptions<Settings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            mongoDatabase = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<MobileDevice> mobileDeviceCollection => mongoDatabase.GetCollection<MobileDevice>("MobileDevice");


        public IEnumerable<MobileDevice> GetAllMobileDevices()
        {
            return mobileDeviceCollection.Find(x => true).ToList();
        }

        public MobileDevice GetMobileDeviceDetails(string Name)
        {
            var mobileDevice = mobileDeviceCollection.Find(x => x.Name == Name).FirstOrDefault();
            return mobileDevice;
        }
        public void Create(MobileDevice mobileDevice)
        {
            mobileDeviceCollection.InsertOne(mobileDevice);
        }

        public void Update(string _id, MobileDevice mobileDevice)
        {
            var filter = Builders<MobileDevice>.Filter.Eq(c => c._id, _id);
            var update = Builders<MobileDevice>.Update
                .Set("Name", mobileDevice.Name)
                .Set("Company", mobileDevice.Company)
                .Set("Color", mobileDevice.Color)
                .Set("Cost", mobileDevice.Cost);

            mobileDeviceCollection.UpdateOne(filter, update);


        }
        public void Delete(string Name)
        {
            var filter = Builders<MobileDevice>.Filter.Eq(c => c.Name, Name);
            mobileDeviceCollection.DeleteOne(filter);
        }


    }
}
