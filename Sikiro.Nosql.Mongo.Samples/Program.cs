using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sikiro.Nosql.Mongo.Base;
using Sikiro.Nosql.Mongo.Extension;

namespace Sikiro.Nosql.Mongo.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "mongodb://192.168.88.141:27017/chengongtest";
            var mongoRepository = new MongoRepository(url);

            var u = new User
            {
                Name = "skychen",
                BirthDateTime = new DateTime(1991, 2, 2),
                AddressList = new List<string> { "guangdong", "guangzhou" },
                Sex = Sex.Son,
                Son = new User
                {
                    Name = "xiaochenpi",
                    BirthDateTime = DateTime.Now
                }
            };

            mongoRepository.Add(u);

            mongoRepository.Update<User>(a => a.Id == u.Id, a => new User
            {
                Sex = Sex.Son,
                Age = a.Age + 2
            });

            var upResulr = mongoRepository.GetAndUpdate<User>(a => a.Id == u.Id, a => new User { Sex = Sex.Son });

            u.AddressList = new List<string> {"111", "2222"};
            mongoRepository.Update(u);

            var q = new User { AddressList = new List<string> { "111", "2222" } };
            mongoRepository.Update<User>(a => a.Id == u.Id, a => new User
            {
                AddressList = q.AddressList
            });


            mongoRepository.Update<User>(a => a.Id == u.Id, a => new User
            {
                AddressList = a.AddressList.Pull("asd1231123")
            });

            mongoRepository.Update<User>(a => a.Id == u.Id, a => new User
            {
                AddressList = a.AddressList.Push("asd1231123")
            });

            var getResult = mongoRepository.Get<User>(a => a.Id == u.Id);
            getResult.Name = "superskychen";

            mongoRepository.Update(getResult);

            mongoRepository.Exists<User>(a => a.Id == u.Id);

            mongoRepository.Delete<User>(a => a.Id == u.Id);

        }
    }

    [Mongo("chengongtest", "User3")]
    public class User : MongoEntity
    {
        public string Name { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime BirthDateTime { get; set; }

        public User Son { get; set; }

        public Sex Sex { get; set; }

        public int Age { get; set; }

        public List<string> AddressList { get; set; }
    }

    public enum Sex
    {
        Woman,
        Son
    }
}
