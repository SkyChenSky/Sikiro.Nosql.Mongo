using System.Collections.Generic;
using FrameWork.MongoDB.MongoDbConfig;

namespace Framework.MongoDB.Model
{
    public class PageList<T> where T : MongoEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页项</param>
        /// <param name="totalCount">总数</param>
        /// <param name="items">元素</param>
        public PageList(int pageIndex, int pageSize, int totalCount, List<T> items)
        {
            _totalCount = totalCount;
            _pageSize = pageSize;
            _pageIndex = pageIndex;
            _items = items;
            _totalPage = _totalCount % _pageSize == 0 ? _totalCount / _pageSize : _totalCount / _pageSize + 1;
        }

        private readonly int _totalCount;
        /// <summary>
        /// 总数
        /// </summary>
        public int Total
        {
            get { return _totalCount; }
        }

        private readonly List<T> _items;
        /// <summary>
        /// 元素
        /// </summary>
        public List<T> Items
        {
            get { return _items; }
        }


        private readonly int _pageSize;
        /// <summary>
        /// 页项
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
        }

        private readonly int _pageIndex;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
        }

        private readonly int _totalPage;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get { return _totalPage; }
        }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPrev
        {
            get { return _pageIndex > 1; }
        }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNext
        {
            get { return _pageIndex < _totalPage; }
        }
    }
}
