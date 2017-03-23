using System;

namespace FrameWork.MongoDB.MongoDbConfig
{
    public class MongoEntity
    {
        public MongoEntity()
        {
            _id = Guid.NewGuid().ToString("N");
        }

        public string _id { get; set; }
    }
}
