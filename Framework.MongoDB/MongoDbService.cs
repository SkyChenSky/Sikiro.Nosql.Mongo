using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.MongoDB.Extension;
using Framework.MongoDB.Model;
using FrameWork.Extension;
using FrameWork.MongoDB.MongoDbConfig;
using MongoDB.Driver;

namespace FrameWork.MongoDB
{
    #region MongoDb操作封装
    /// <summary>
    /// MongoDb操作封装
    /// </summary>
    public class MongoDbService
    {
        #region 初始化

        private readonly string _connString = "MongoDb".ValueOfAppSetting();
        private readonly MongoClient _mongoClient;

        public MongoDbService()
        {
            _mongoClient = new MongoClient(_connString);
        }

        #endregion

        #region 增

        #region 增（同步）

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合（表）</param>
        /// <param name="entity">实体(文档)</param>
        public void Add<T>(string database, string collection, T entity) where T : MongoEntity
        {
            AddAsync(database, collection, entity).Wait();
        }

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        public void Add<T>(T entity) where T : MongoEntity
        {
            AddAsync(entity).Wait();
        }

        #endregion

        #region 增（异步）

        /// <summary>
        /// 增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合（表）</param>
        /// <param name="entity">实体(文档)</param>
        /// <returns></returns>
        public async Task AddAsync<T>(string database, string collection, T entity) where T : MongoEntity
        {
            var db = _mongoClient.GetDatabase(database);
            var coll = db.GetCollection<T>(collection);
            await coll.InsertOneAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// 增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        /// <returns></returns>
        public async Task AddAsync<T>(T entity) where T : MongoEntity
        {
            var mongoAttribute = entity.GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            await AddAsync(mongoAttribute.Database, mongoAttribute.Collection, entity);
        }

        #endregion

        #endregion

        #region 批量增

        #region 批量增（异步）

        /// <summary>
        /// 批量增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合（表）</param>
        /// <param name="entity">实体(文档)</param>
        public async Task BatchAddAsync<T>(string database, string collection, List<T> entity) where T : MongoEntity
        {
            var db = _mongoClient.GetDatabase(database);
            var coll = db.GetCollection<T>(collection);
            await coll.InsertManyAsync(entity).ConfigureAwait(false);
        }

        /// <summary>
        /// 批量增（异步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        public async Task BatchAddAsync<T>(List<T> entity) where T : MongoEntity
        {
            var mongoAttribute = entity.GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            await BatchAddAsync(mongoAttribute.Database, mongoAttribute.Collection, entity);
        }
        #endregion

        #region 批量增（同步）

        /// <summary>
        /// 批量增（同步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合（表）</param>
        /// <param name="entity">实体(文档)</param>
        public void BatchAdd<T>(string database, string collection, List<T> entity) where T : MongoEntity
        {
            BatchAddAsync(database, collection, entity).Wait();
        }

        /// <summary>
        /// 批量增（同步）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体(文档)</param>
        public void BatchAdd<T>(List<T> entity) where T : MongoEntity
        {
            var mongoAttribute = entity.GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            BatchAdd(mongoAttribute.Database, mongoAttribute.Collection, entity);
        }
        #endregion

        #endregion

        #region 删

        #region 删（同步）

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public long Delete<T>(T entity) where T : MongoEntity
        {
            return DeleteAsync(entity).Result;
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">删除条件</param>
        /// <returns></returns>
        public long Delete<T>(Expression<Func<T, bool>> predicate) where T : MongoEntity
        {
            return DeleteAsync(predicate).Result;
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public long Delete<T>(string database, string collection, T entity) where T : MongoEntity
        {
            return DeleteAsync(database, collection, entity).Result;
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public long Delete<T>(string database, string collection, Expression<Func<T, bool>> predicate)
           where T : MongoEntity
        {
            return DeleteAsync(database, collection, predicate).Result;
        }
        #endregion

        #region 删（异步）

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync<T>(T entity) where T : MongoEntity
        {
            return await DeleteAsync<T>(a => a._id == entity._id);
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">实体</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync<T>(Expression<Func<T, bool>> predicate)
          where T : MongoEntity
        {
            var mongoAttribute = typeof(T).GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            return await DeleteAsync(mongoAttribute.Database, mongoAttribute.Collection, predicate);
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync<T>(string database, string collection, T entity) where T : MongoEntity
        {
            return await DeleteAsync<T>(database, collection, e => e._id == entity._id);
        }

        /// <summary>
        /// 删
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">实体</param>
        /// <returns></returns>
        public async Task<long> DeleteAsync<T>(string database, string collection, Expression<Func<T, bool>> predicate)
            where T : MongoEntity
        {
            var db = _mongoClient.GetDatabase(database);
            var coll = db.GetCollection<T>(collection);
            var result = await coll.DeleteManyAsync(predicate).ConfigureAwait(false);
            return result.DeletedCount;
        }
        #endregion

        #endregion

        #region 改

        #region 改（同步）

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">字段</param>
        /// <returns></returns>
        public long Update<T>(T entity) where T : MongoEntity
        {
            return UpdateAsync(a => a._id == entity._id, entity).Result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="entity">字段</param>
        /// <returns></returns>
        public long Update<T>(Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            return UpdateAsync(predicate, entity).Result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public long Update<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda) where T : MongoEntity
        {
            return UpdateAsync(predicate, lambda).Result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">条件</param>
        /// <param name="entity">字段</param>
        /// <returns></returns>
        public long Update<T>(string database, string collection, Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            return UpdateAsync(database, collection, predicate, entity).Result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">条件</param>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public long Update<T>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda)
        {
            return UpdateAsync(database, collection, predicate, lambda).Result;
        }

        #endregion

        #region 改（异步）

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体（根据主键更新）</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(T entity) where T : MongoEntity
        {
            return await UpdateAsync(a => a._id == entity._id, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="entity">实体（根据主键更新）</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            var mongoAttribute = typeof(T).GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            return await UpdateAsync(mongoAttribute.Database, mongoAttribute.Collection, predicate, entity);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">条件</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(string database, string collection,
           Expression<Func<T, bool>> predicate, T entity) where T : MongoEntity
        {
            var db = _mongoClient.GetDatabase(database);
            var coll = db.GetCollection<T>(collection);

            var updateDefinitionList = entity.GetUpdateDefinition();

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = await coll.UpdateOneAsync<T>(predicate, updateDefinitionBuilder).ConfigureAwait(false);

            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">条件</param>
        /// <param name="lambda">实体</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(string database, string collection,
           Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda)
        {
            var db = _mongoClient.GetDatabase(database);
            var coll = db.GetCollection<T>(collection);

            var updateDefinitionList = MongoDbExpression<T>.GetUpdateDefinition(lambda);

            var updateDefinitionBuilder = new UpdateDefinitionBuilder<T>().Combine(updateDefinitionList);

            var result = await coll.UpdateManyAsync<T>(predicate, updateDefinitionBuilder).ConfigureAwait(false);

            return result.ModifiedCount;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">条件</param>
        /// <param name="lambda">实体</param>
        /// <returns></returns>
        public async Task<long> UpdateAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> lambda) where T : MongoEntity
        {
            var mongoAttribute = typeof(T).GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            return await UpdateAsync(mongoAttribute.Database, mongoAttribute.Collection, predicate, lambda);
        }

        #endregion

        #endregion

        #region 查

        #region 查（同步）

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector">查询字段</param>
        /// <returns></returns>
        public T Get<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null)
            where T : MongoEntity
        {
            return GetAsync(predicate, projector).Result;
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector">查询字段</param>
        /// <returns></returns>
        public T Get<T>(string database, string collection, Expression<Func<T, bool>> predicate,
            Expression<Func<T, T>> projector) where T : MongoEntity
        {
            return GetAsync(database, collection, predicate, projector).Result;
        }

        #endregion

        #region 查（异步）

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector">查询字段</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null)
            where T : MongoEntity
        {
            var mongoAttribute = typeof(T).GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            return await GetAsync(mongoAttribute.Database, mongoAttribute.Collection, predicate, projector);
        }

        /// <summary>
        /// 查
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector">查询字段</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string database, string collection,
            Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector) where T : MongoEntity
        {
            var db = _mongoClient.GetDatabase(database);
            var coll = db.GetCollection<T>(collection);

            var find = coll.Find(predicate);
            if (!projector.IsNull())
                find = find.Project(projector);

            return await find.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region 列表

        #region 列表（同步）
        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<T> List<T>(
           Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null, int? limit = null) where T : MongoEntity
        {
            return ListAsync(predicate, projector, limit).Result;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<T> List<T>(string database, string collection,
           Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector, int? limit) where T : MongoEntity
        {
            return ListAsync(database, collection, predicate, projector, limit).Result;
        }
        #endregion

        #region 列表（异步）
        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<T>> ListAsync<T>(
           Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector = null, int? limit = null) where T : MongoEntity
        {
            var mongoAttribute = typeof(T).GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            return await ListAsync(mongoAttribute.Database, mongoAttribute.Collection, predicate, projector, limit);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">查询条件</param>
        /// <param name="projector"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<List<T>> ListAsync<T>(string database, string collection,
           Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector, int? limit) where T : MongoEntity
        {
            var db = _mongoClient.GetDatabase(database);
            var coll = db.GetCollection<T>(collection);

            var find = coll.Find(predicate);

            if (projector.IsNotNull())
                find = find.Project(projector);

            if (limit.IsNotNull())
                find = find.Limit(limit);

            return await find.ToListAsync().ConfigureAwait(false);
        }
        #endregion

        #endregion

        #region 分页

        #region 分页（同步）

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="desc">顺序、倒叙</param>
        /// <returns></returns>
        public PageList<T> PageList<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector, int pageIndex = 1, int pageSize = 20, Expression<Func<T, object>> orderby = null, bool desc = false) where T : MongoEntity
        {
            return PageListAsync(predicate, projector, pageIndex, pageSize, orderby, desc).Result;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="desc">顺序、倒叙</param>
        /// <returns></returns>
        public PageList<T> PageList<T>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector, int pageIndex = 1, int pageSize = 20, Expression<Func<T, object>> orderby = null, bool desc = false) where T : MongoEntity
        {
            return PageListAsync(database, collection, predicate, projector, pageIndex, pageSize, orderby, desc).Result;
        }

        #endregion

        #region 分页（异步）
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="desc">顺序、倒叙</param>
        /// <returns></returns>
        public async Task<PageList<T>> PageListAsync<T>(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector, int pageIndex = 1, int pageSize = 20, Expression<Func<T, object>> orderby = null, bool desc = false) where T : MongoEntity
        {
            var mongoAttribute = typeof(T).GetAttribute<MongoAttribute>();
            if (mongoAttribute.IsNull())
                throw new ArgumentException("MongoAttribute不能为空");

            return await PageListAsync(mongoAttribute.Database, mongoAttribute.Collection, predicate, projector, pageIndex, pageSize, orderby, desc);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database">库</param>
        /// <param name="collection">集合</param>
        /// <param name="predicate">过滤条件</param>
        /// <param name="projector">查询字段</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="desc">顺序、倒叙</param>
        /// <returns></returns>
        public async Task<PageList<T>> PageListAsync<T>(string database, string collection, Expression<Func<T, bool>> predicate, Expression<Func<T, T>> projector, int pageIndex = 1, int pageSize = 20, Expression<Func<T, object>> orderby = null, bool desc = false) where T : MongoEntity
        {
            var db = _mongoClient.GetDatabase(database);
            var coll = db.GetCollection<T>(collection);

            var count = (int)await coll.CountAsync<T>(predicate).ConfigureAwait(false);

            var find = coll.Find(predicate);

            if (projector.IsNotNull())
                find = find.Project(projector);

            if (orderby != null)
            {
                find = desc ? find.SortByDescending(@orderby) : find.SortBy(@orderby);
            }

            find = find.Skip((pageIndex - 1) * pageSize).Limit(pageSize);

            var items = await find.ToListAsync().ConfigureAwait(false);

            return new PageList<T>(pageIndex, pageSize, count, items);
        }
        #endregion

        #endregion
    }
    #endregion
}
