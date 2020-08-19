using System;
using System.Collections.Generic;
using System.Text;

namespace Sikiro.Nosql.Mongo.Samples
{
    public class UserRepository : MongoRepository
    {
        public UserRepository(string connectionString) : base(connectionString, "chengongtest")
        {
        }

    }
}
