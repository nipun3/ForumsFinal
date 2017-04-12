using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ForumsFinalApi._3325.Models;
using ForumsFinalApi._3325.Repository;
using ForumsFinalApi._3325.Repository.Operations;
using System.Web.Http.Cors;

namespace ForumWebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoryController : ApiController
    {
        ICategoryRepository _Repository;

        public CategoryController()
        {
            //ForumRepository could be injected if desired
            _Repository = new CategoryRepository();
        }

        // GET api/Categories
        // [Route("api/Categories/Get")]
        public List<Category> Get()
        {
            var custs = _Repository.GetCategories();
            if (custs == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            return custs;
        }
    }
}
