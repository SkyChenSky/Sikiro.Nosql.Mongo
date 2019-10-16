using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Sikiro.Nosql.Mongo;
using Sikiro.Nosql.Mongo.Base;

namespace UnitTest
{
    [TestClass]
    public class Test
    {
        [TestClass]
        public class MongoDbTest
        {
            public MongoRepository MongoRepository;
            #region Add

            public MongoDbTest()
            {
                var url = "";
                MongoRepository = new MongoRepository(url);
            }

            [TestMethod]
            public void Add_Normal_IsTrue()
            {
                MongoRepository.Add(new User
                {
                    Age = 111,
                    Name = "chengongeee"
                });

                MongoRepository.Add(new User
                {
                    Age = 111,
                    Name = "chengong100"
                });
            }

            #endregion

            #region Update

            [TestMethod]
            public void Update_Normal_IsTrue()
            {
                var user = new User
                {
                    Age = 111,
                    Name = "chengongeee"
                };
                MongoRepository.Add(user);

                user.Name = "updateName";
                user.NumList = new List<int>();

                user.Sons = new List<User> { new User { Id = new ObjectId("6B16A8742B8B4920BEE43866D9B81925"), Name = "aads" }, new User { Id = new ObjectId("6B16A8742B8B4920BEE43866D9B81922"), Name = "aads" } };

                user.AddressList = new List<string> { "123123", "asdsdf" };

                var result = MongoRepository.Update(user);

                Assert.IsTrue(result);
            }

            [TestMethod]
            public void Update_Where_IsTrue()
            {
                var user = new User
                {
                    Age = 111,
                    Name = "chengongeee"
                };
                MongoRepository.Add(user);

                var qwe = new List<User> { new User { Id = new ObjectId("6B16A8742B8B4920BEE43866D9B81925"), Name = "aads" }, new User { Id = new ObjectId("6B16A8742B8B4920BEE43866D9B81922"), Name = "aads" } };

                var qwe2 = new List<string> { "123123", "asdsdf" };

                var result = MongoRepository.Update<User>(a => a.Id == user.Id, a => new User { AddressList = new List<string> { "123123", "asdsdf" } });

                Assert.IsTrue(result > 0);
            }

            #endregion

            #region Update

            [TestMethod]
            public void Delete_Normal_IsTrue()
            {
                var user = new User
                {
                    Age = 111,
                    Name = "chengongeee"
                };
                MongoRepository.Add(user);

                var result = MongoRepository.Delete(user);

                Assert.IsTrue(result);
            }

            [TestMethod]
            public void Delete_Where_IsTrue()
            {
                var user = new User
                {
                    Age = 111,
                    Name = "chengongeee"
                };
                MongoRepository.Add(user);

                var result = MongoRepository.Delete<User>(a => a.Id == user.Id);

                Assert.IsTrue(result > 0);
            }

            #endregion

            #region Get
            [TestMethod]
            public void Get_Normal_IsTrue()
            {
                var skychen = MongoRepository.Get<User>(a => true);

                Assert.IsNotNull(skychen);
            }

            [TestMethod]
            public void Get_Selector_IsTrue()
            {
                var skychenName = MongoRepository.Get<User, string>(a => true, a => a.Name);

                Assert.IsNotNull(skychenName);
            }

            [TestMethod]
            public void Get_OrderBy_IsTrue()
            {
                var skychenName = MongoRepository.Get<User>(a => true, a => a.Desc(b => b.Age));

                Assert.IsNotNull(skychenName);
            }

            [TestMethod]
            public void Get_SelectorOrderBy_IsTrue()
            {
                var skychenName = MongoRepository.Get<User, User>(a => true, a => new User { Name = a.Name, Sex = a.Sex }, a => a.Desc(b => b.Age));

                Assert.IsNotNull(skychenName.Name);
            }
            #endregion

            #region ToList
            [TestMethod]
            public void ToList_Normal_IsTrue()
            {
                var skychen = MongoRepository.ToList<User>(a => true);

                Assert.IsTrue(skychen.Any());
            }

            [TestMethod]
            public void ToList_Top_IsTrue()
            {
                var skychen = MongoRepository.ToList<User>(a => true, 10);

                Assert.AreEqual(skychen.Count, 10);
            }

            [TestMethod]
            public void ToList_Orderby_IsTrue()
            {
                var skychen = MongoRepository.ToList<User>(a => true, a => a.Desc(b => b.Age).Desc(b => b.Sex), 10);

                Assert.AreEqual(skychen.Count, 10);
            }

            [TestMethod]
            public void ToList_Selector_IsTrue()
            {
                var skychen = MongoRepository.ToList<User, string>(a => true, a => a.Name);

                Assert.IsTrue(skychen.Any());
            }

            [TestMethod]
            public void ToList_Selector_Orderby_IsTrue()
            {
                var skychen = MongoRepository.ToList<User, string>(a => true, a => a.Name, a => a.Desc(b => b.Name));

                Assert.IsTrue(skychen.Any());
            }

            [TestMethod]
            public void ToList_Selector_Orderby_Top_IsTrue()
            {
                var skychen = MongoRepository.ToList<User, string>(a => true, a => a.Name, a => a.Desc(b => b.Name), 100);

                Assert.IsTrue(skychen.Any());
            }

            #endregion

            #region PageList
            [TestMethod]
            public void PageList_Normal_IsTrue()
            {
                var skychen = MongoRepository.PageList<User>(a => true, 1, 10);

                Assert.AreEqual(skychen.Items.Count, 10);
                Assert.AreEqual(skychen.HasNext, true);
                Assert.AreEqual(skychen.HasPrev, false);
            }

            [TestMethod]
            public void PageList_Orderby_IsTrue()
            {
                var skychen = MongoRepository.PageList<User>(a => true, a => a.Desc(b => b.Name), 2, 10);

                Assert.AreEqual(skychen.Items.Count, 10);
                Assert.AreEqual(skychen.HasNext, true);
                Assert.AreEqual(skychen.HasPrev, true);
            }

            [TestMethod]
            public void PageList_Selector_IsTrue()
            {
                var skychen = MongoRepository.PageList<User, string>(a => true, a => a.Name, 2, 10);

                Assert.AreEqual(skychen.Items.Count, 10);
                Assert.AreEqual(skychen.HasNext, true);
                Assert.AreEqual(skychen.HasPrev, true);
            }

            [TestMethod]
            public void PageList_Selector_OrderBy_IsTrue()
            {
                var skychen = MongoRepository.PageList<User, string>(a => true, a => a.Name, a => a.Desc(b => b.Name), 2, 20);

                Assert.AreEqual(skychen.Items.Count, 20);
                Assert.AreEqual(skychen.HasNext, true);
                Assert.AreEqual(skychen.HasPrev, true);
            }

            #endregion

            #region Exists

            [TestMethod]
            public void Exists_Normal_IsTrue()
            {
                var isExist = MongoRepository.Exists<User>(a => a.Name == "chengongeee");

                Assert.IsTrue(isExist);
            }

            [TestMethod]
            public void Exists_Normal_IsFalse()
            {
                var isExist = MongoRepository.Exists<User>(a => a.Name == "chengong100");

                Assert.IsFalse(isExist);
            }

            #endregion

            #region MyRegion

            [TestMethod]
            public void AddIfNotExist_Normal_IsTrue()
            {
                var result = MongoRepository.AddIfNotExist("geshiimdb", "UserInfo", new User
                {
                    Age = 2222,
                    Name = "chengongeee2222"
                });

                Assert.IsTrue(result);
            }
            #endregion
        }

        [Mongo("geshiimdb", "User3")]
        public class User : MongoEntity
        {
            public string Name { get; set; }

            public int Age { get; set; }

            [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
            public DateTime BirthDateTime { get; set; }

            public User Son { get; set; }

            public Sex Sex { get; set; }

            public List<int> NumList { get; set; }

            public List<string> AddressList { get; set; }


            public List<User> Sons { get; set; }
        }

        public enum Sex
        {
            Man = 1,
            Woman = 2
        }
    }
}
