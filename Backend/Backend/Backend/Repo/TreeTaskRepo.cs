using Backend.Data;
using Backend.Model;
using Dapper;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;
using System.Data;

namespace Backend.Repo
{
    public class TreeTaskRepo
    {
        private DapperContext _dapperContext;
        private ILogger<TreeTaskRepo> logger;
        public TreeTaskRepo(DapperContext dapperContext, ILogger<TreeTaskRepo> logger)
        {
            _dapperContext = dapperContext;
            this.logger = logger;
        }


        private ConcurrentDictionary<string,BoM> _cache = new ConcurrentDictionary<string, BoM>();

        public async Task<List<Part>> GetParts(string parentName)
        {
            try
            {
                
                using (var connection = _dapperContext.CreateConnection())
                {
                    var sql = "SELECT t0.COMPONENT_NAME,t1.PART_NUMBER, t1.TITLE,t0.QUANTITY,t1.type, t1.ITEM, t1.MATERIAL\r\n  FROM bom t0\r\n  left join part t1 on t0.COMPONENT_NAME = t1.NAME\r\n  where PARENT_NAME =@Name";
                    var res = await connection.QueryAsync<Part>(sql, new { Name = parentName });
                    return res.ToList();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GetParts with parentName: {parentName}", parentName);
                throw ex;
            }
        }




        public async Task<BoM> GetSubBoM(BoM item)
        {
            try
            {
                var cacheKey = item.name;
                if (_cache.TryGetValue(cacheKey, out var cachedItem))
                {
                    logger.LogInformation("Cache hit for item name: {itemName}", item.name);
                    return cachedItem;
                }
                using (var connection = _dapperContext.CreateConnection())
                {
                    var sql = "SELECT id,COMPONENT_NAME as Name\r\n  FROM bom\r\n  where PARENT_NAME =@Name";

                    var res = await connection.QueryAsync<BoM>(sql, new { Name = item.name });
                    if (res.Count() > 0)
                    {
                        foreach (var subItem in res)
                        {
                            var reItem = await GetSubBoM(subItem);
                            if (item.children == null)
                            {
                                item.children = new List<BoM>();
                            }
                            item.children.Add(reItem);
                        }
                    }
                }
                    _cache.TryAdd(cacheKey, item);
                    logger.LogInformation("Cache added for item name: {itemName}", item.name);
                return item;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in GetSubBoM with item name: {itemName}", item.name);
                throw ex;
            }
        }

        public async Task<List<BoM>> GetBom()

        {
            var results = new List<BoM>();
            using (var connection = _dapperContext.CreateConnection())
            {
                var sql = "select t0.Id,t0.name from dbo.part t0\r\nwhere t0.type = 'Assembly' ";

                foreach (var item in await connection.QueryAsync<BoM>(sql))
                {
                    var itemResult = await GetSubBoM(item);
                    results.Add(itemResult);

                }

            }
            return results;


        }
    }
}
