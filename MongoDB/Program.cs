using System;
using System.Collections.Generic;
using System.Linq;
using Framework.MongoDB.Model;
using FrameWork.MongoDB;

namespace MongoDB
{
    class Program
    {
        static void Main(string[] args)
        {
            //增
            var id = Add();

            //列表
            var list = List();

            //批量插入
            BatchInser();

            //改
            Update(id);

            //分页列表
            var pageList = PageList().Items;

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

        static PageList<User> PageList()
        {
            return new MongoDbService().PageList<User>(a => true);
        }

        static List<User> List()
        {
            return new MongoDbService().List<User>(a => true);
        }

        static void BatchInser()
        {
            var listUser = Enumerable.Range(0, 100).Select(i => new User
            {
                _id = Guid.NewGuid().ToString("N"),
                Age = i,
                Name = "chengong" + i,
                Son = new User
                    {
                        Age = i,
                        Name = "xiaochenpi" + i
                    },
                BirthDateTime = DateTime.Now
            }).ToList();

            new MongoDbService().BatchAdd(listUser);
        }
    }
}
