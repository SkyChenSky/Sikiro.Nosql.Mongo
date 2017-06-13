using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework.Extension;
using Framework.Helper;
using Framework.MongoDB.Model;
using FrameWork.MongoDB.MongoDbConfig;
using MongoDB.Driver;

namespace Framework.MongoDB.Extension
{
    /// <summary>
    /// mongodb扩展方法
    /// </summary>
    internal static class MongoDbExtension
    {
        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static UpdateDefinition<T> GetUpdateDefinition<T>(this T entity)
        {
            var properties = typeof(T).GetEntityProperties();

            var updateDefinitionList = GetUpdateDefinitionList<T>(properties, entity);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            return updateDefinitionBuilder;
        }

        /// <summary>
        /// 获取更新信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyInfos"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal static List<UpdateDefinition<T>> GetUpdateDefinitionList<T>(PropertyInfo[] propertyInfos, object entity)
        {
            var updateDefinitionList = new List<UpdateDefinition<T>>();

            propertyInfos = propertyInfos.Where(a => a.Name != "_id").ToArray();

            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.IsArray || Types.IList.IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var value = propertyInfo.GetValue(entity) as IList;

                    var filedName = propertyInfo.Name;

                    updateDefinitionList.Add(Builders<T>.Update.Set(filedName, value));
                }
                else
                {
                    var value = propertyInfo.GetValue(entity);

                    if (propertyInfo.PropertyType == Types.Decimal)
                        value = value.ToString();
                    else if (propertyInfo.PropertyType.IsEnum)
                        value = (int)value;

                    var filedName = propertyInfo.Name;

                    updateDefinitionList.Add(Builders<T>.Update.Set(filedName, value));
                }
            }

            return updateDefinitionList;
        }

        /// <summary>
        /// 获取特性信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static MongoAttribute GetMongoAttribute(this Type type)
        {
            return AttributeHelper<MongoAttribute>.GetAttribute(type);
        }
    }
}
