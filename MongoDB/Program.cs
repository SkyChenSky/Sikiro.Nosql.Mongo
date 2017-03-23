using System;
using System.Collections.Generic;
using FrameWork.MongoDB;

namespace MongoDB
{
    class Program
    {
        static void Main(string[] args)
        {
            //增
            var id = Add();

            //改
            Update(id);

            //查
            var data = Get(id);

            //删
            Delete(data);
        }

        static string Add()
        {
            var id = Guid.NewGuid().ToString();
            new MongoDbService().Add(new User
            {
                _id = id,
                Age = 26,
                Name = "chengong",
                Son = new User
                {
                    Age = 1,
                    Name = "xiaochenpi"
                },
                BirthDateTime = DateTime.Now
            });
            return id;
        }

        static void Update(string id)
        {
            new MongoDbService().Update<User>(a => a._id == id, a => new User
            {
                Age = a.Age + 2,
                Name = "chengong",
                Son = new User
                {
                    Age = 3,
                    Name = "xiaochenpi"
                },
                Sex = Sex.Man,
                AddressList = new List<string>
                {
                    "广东省",
                    "江门市"
                }
            });
        }

        static User Get(string id)
        {
            var result = new MongoDbService().Get<User>(a => a._id == id);
            return result;
        }

        static void Delete(User entity)
        {
            new MongoDbService().Delete(entity);
        }
    }
}
