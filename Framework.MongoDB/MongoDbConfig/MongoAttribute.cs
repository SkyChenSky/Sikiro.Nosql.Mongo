using System;

namespace FrameWork.MongoDB.MongoDbConfig
{
    #region Mongo实体标签
    /// <summary>
    /// Mongo实体标签
    /// </summary>
    public class MongoAttribute : Attribute
    {
        public MongoAttribute(string database, string collection)
        {
            Database = database;
            Collection = collection;
        }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string Collection { get; private set; }

    }
    #endregion
}
