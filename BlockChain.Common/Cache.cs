using System;
using System.Runtime.Caching;

namespace BlockChain.Common
{


    /// <summary>
    /// Cache
    /// </summary>
    public class Cache
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static int MinCacheSeconds = 120;    //默认两分钟

        public static int MaxCacheSeconds = 1200;   //默认20分钟

        private static Random rd = new Random();    //使用随机数，避免同时更新所有缓存

        public static int CacheSeconds
        {
            get
            {
//                //调试时候
//#if DEBUG
//                //log.Info("debug: CacheSeconds");
//                return 5;
//#endif

//                ////设计时不要翻译，
//                //if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
//                //{
//                //    return 5;           //如果是设计时，
//                //}

//                //log.Info("normal: CacheSeconds");
                return rd.Next(MinCacheSeconds, MaxCacheSeconds);
            }        
        }

        /// <summary>
        /// 得到某个缓存管理者中某个缓存的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetData<T>(string key)
        {
            return (T)MemoryCache.Default[key];
        }

        public static object GetData(string key)
        {
            return MemoryCache.Default[key];
        }

        /// <summary>
        /// 绝对时间过期策略 缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="AddSeconds"></param>
        public static void AddByAbsoluteTime<T>(string key, T value, int AddSeconds)
        {
            if (value == null) return;
            if (MemoryCache.Default[key] == null)
            {
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = System.DateTime.Now.AddSeconds(AddSeconds); //todo:这种写法可能错误？
                var item = new CacheItem(key, value);
                MemoryCache.Default.Add(item, policy);
            }
        }

        public static void AddByAbsoluteTime<T>(string key, T value)
        {
            if (value == null) return;
            AddByAbsoluteTime<T>(key, value, CacheSeconds);
        }

        /// <summary>
        /// 绝对时间过期策略 缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="AddSeconds"></param>
        public static void AddByAbsoluteTime(string key, object value, int AddSeconds)
        {
            if (value == null) return;
            if (MemoryCache.Default[key] == null)
            {
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = System.DateTime.Now.AddSeconds(AddSeconds);
                var item = new CacheItem(key, value);
                MemoryCache.Default.Add(item, policy);
            }
        }

        public static void AddByAbsoluteTime(string key, object value)
        {
            if (value == null) return;
            AddByAbsoluteTime(key, value, CacheSeconds);
        }

        /// <summary>
        /// 相对时间过期策略 缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheManagerName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="AddSeconds"></param>
        public static void AddBySlidingTime<T>(string key, T value, int AddSeconds)
        {
            if (value == null) return;
            if (MemoryCache.Default[key] == null)
            {
                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = new TimeSpan(0, 0, AddSeconds);
                var item = new CacheItem(key, value);
                MemoryCache.Default.Add(item, policy);
            }
        }

        public static void AddBySlidingTime<T>(string key, T value)
        {
            if (value == null) return;
            AddBySlidingTime<T>(key, value, CacheSeconds);
        }

        /// <summary>
        /// 相对时间过期策略 缓存
        /// </summary>
        /// <param name="cacheManagerName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="AddSeconds"></param>
        public static void AddBySlidingTime(string key, object value, int AddSeconds)
        {
            if (value == null) return;
            if (MemoryCache.Default[key] == null)
            {
                var policy = new CacheItemPolicy();
                policy.SlidingExpiration = new TimeSpan(0, 0, AddSeconds);
                var item = new CacheItem(key, value);
                MemoryCache.Default.Add(item, policy);
            }
        }

        public static void AddBySlidingTime(string key, object value)
        {
            if (value == null) return;
            AddBySlidingTime(key, value, CacheSeconds);
        }


        /// <summary>
        /// 删除，更新的时候调用
        /// </summary>
        /// <param name="cacheManagerName">缓存管理者</param>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            MemoryCache.Default.Remove(key);
        }


        public static bool Contains(string key)
        {
            return MemoryCache.Default.Contains(key);
        }

    }



}
