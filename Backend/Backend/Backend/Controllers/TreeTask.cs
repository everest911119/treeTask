using Backend.Filter;
using Backend.Model;
using Backend.Repo;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TreeTask : Controller
    {
        private readonly TreeTaskRepo treeTaskRepo;

        public TreeTask(TreeTaskRepo treeTaskRepo)
        {
            this.treeTaskRepo = treeTaskRepo;
        }

        [HttpGet]
        [ServiceFilter(typeof(CacheActionFilter))]
        public async Task<List<BoM>> Boms()

        {
            var res = await treeTaskRepo.GetBom();
            Response.StatusCode = 200;
            return res;
        }


        [HttpGet]
        [ServiceFilter(typeof(CacheActionFilter))]
        [Route("{parentName}")]
        public async Task<List<Part>> Boms(string parentName)
        {
            var res = await treeTaskRepo.GetParts(parentName);
            if (res == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            return res;

        }
    }
}
