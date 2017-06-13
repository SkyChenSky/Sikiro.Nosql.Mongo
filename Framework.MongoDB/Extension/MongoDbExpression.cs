using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Framework.MongoDB.Model;
using MongoDB.Driver;

namespace Framework.MongoDB.Extension
{
    #region Mongo更新字段表达式解析
    /// <summary>
    /// Mongo更新字段表达式解析
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoDbExpression<T> : ExpressionVisitor
    {
        #region 成员变量
        /// <summary>
        /// 更新列表
        /// </summary>
        internal List<UpdateDefinition<T>> UpdateDefinitionList = new List<UpdateDefinition<T>>();
        private string _fieldname;

        #endregion

        #region 获取更新列表
        /// <summary>
        /// 获取更新列表
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static List<UpdateDefinition<T>> GetUpdateDefinition(Expression<Func<T, T>> expression)
        {
            var mongoDb = new MongoDbExpression<T>();

            mongoDb.Resolve(expression);
            return mongoDb.UpdateDefinitionList;
        }
        #endregion

        #region 解析表达式
        /// <summary>
        /// 解析表达式
        /// </summary>
        /// <param name="expression"></param>
        private void Resolve(Expression<Func<T, T>> expression)
        {
            Visit(expression);
        }
        #endregion

        #region 访问对象初始化表达式

        /// <summary>
        /// 访问对象初始化表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            var bingdings = node.Bindings;

            foreach (var item in bingdings)
            {
                var memberAssignment = (MemberAssignment)item;
                _fieldname = item.Member.Name;

                if (memberAssignment.Expression.NodeType == ExpressionType.MemberInit)
                {
                    var lambda = Expression.Lambda<Func<object>>(Expression.Convert(memberAssignment.Expression, Types.Object));
                    var value = lambda.Compile().Invoke();
                    UpdateDefinitionList.Add(Builders<T>.Update.Set(_fieldname, value));
                }
                else
                {
                    Visit(memberAssignment.Expression);
                }
            }
            return node;
        }

        #endregion

        #region 访问二元表达式

        /// <summary>
        /// 访问二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var value = ((ConstantExpression)node.Right).Value;

            if (node.NodeType == ExpressionType.Decrement)
            {
                if (node.Type == Types.Int)
                {
                    value = -(int)value;
                }
                else if (node.Type == Types.Long)
                {
                    value = -(long)value;
                }
                else if (node.Type == Types.Double)
                {
                    value = -(double)value;
                }
                else if (node.Type == Types.Decimal)
                {
                    value = -(decimal)value;
                }
                else if (node.Type == Types.Float)
                {
                    value = -(float)value;
                }
                else
                {
                    throw new Exception(_fieldname + "不支持该类型操作");
                }
            }

            var updateDefinition = Builders<T>.Update.Inc(_fieldname, value);

            UpdateDefinitionList.Add(updateDefinition);

            return node;
        }
        #endregion

        #region 访问数组表达式

        /// <summary>
        /// 访问数组表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            SetList(node);

            return node;
        }

        /// <summary>
        /// 访问集合表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitListInit(ListInitExpression node)
        {
            SetList(node);

            return node;
        }

        private void SetList(Expression node)
        {
            var listLambda = Expression.Lambda<Func<IList>>(node);
            var list = listLambda.Compile().Invoke();
            UpdateDefinitionList.Add(Builders<T>.Update.Set(_fieldname, list));
        }
        #endregion

        #region 访问常量表达式

        /// <summary>
        /// 访问常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            var value = node.Type.IsEnum ? (int)node.Value : node.Value;

            UpdateDefinitionList.Add(Builders<T>.Update.Set(_fieldname, value));

            return node;
        }
        #endregion

        #region 访问成员表达式

        /// <summary>
        /// 访问成员表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Type.GetInterfaces().Any(a => a.Name == "IList"))
            {
                var lambda = Expression.Lambda<Func<IList>>(node);
                var value = lambda.Compile().Invoke();

                UpdateDefinitionList.Add(Builders<T>.Update.Set(_fieldname, value));
            }
            else
            {
                var lambda = Expression.Lambda<Func<object>>(Expression.Convert(node, Types.Object));
                var value = lambda.Compile().Invoke();

                if (node.Type.IsEnum)
                    value = (int)value;

                UpdateDefinitionList.Add(Builders<T>.Update.Set(_fieldname, value));
            }

            return node;
        }
        #endregion
    }
    #endregion
}
