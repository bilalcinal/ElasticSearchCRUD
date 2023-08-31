using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearch.DATA.Entity;
using ElasticSearch.DATA.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IElasticsearchService _elasticsearchService;

        public HomeController(IElasticsearchService elasticsearchService)
        {
            _elasticsearchService = elasticsearchService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            await InsertFullData();
            await _elasticsearchService.ChekIndex("cities");
            
            return Ok("");
        }
        private async Task InsertFullData()
        {
            List<Cities> citiesList = new List<Cities>() {
            new Cities{City="Ankara",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=50000,Region="İç Anadolu"},
            new Cities{City="İzmir",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=30500,Region="Ege"},
            new Cities{City="Aydın",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=65000,Region="Ege"},
            new Cities{City="Rize",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=36522,Region="Karadeniz"},
            new Cities{City="İstanbul",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=25620,Region="Marmara"},
            new Cities{City="Sinop",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=50669,Region="Karadeniz"},
            new Cities{City="Kars",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=55500,Region="Doğu Anadolu"},
            new Cities{City="Van",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=55500,Region="Doğu Anadolu"},
            new Cities{City="Adıyaman",CreateDate=System.DateTime.Now,Id=Guid.NewGuid().ToString(),Population=55500,Region="Güneydoğu Anadolu"},
            };
            await _elasticsearchService.InsertBulkDocuments("cities", citiesList);
        }
    }
}

