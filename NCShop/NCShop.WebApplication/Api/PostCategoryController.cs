using NCShop.Model.Models;
using NCShop.Service;
using NCShop.WebApplication.Infrastructure.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NCShop.WebApplication.Api
{
    [RoutePrefix("api/postcategory")]
    public class PostCategoryController : ApiControllerBase
    {
        private IPostCategoryService _postCategoryService;

        public PostCategoryController(IErrorSercive errorService, IPostCategoryService postCategoryService) : base(errorService)
        {
            this._postCategoryService = postCategoryService;
        }

        public HttpResponseMessage Post(HttpRequestMessage requestMessage, PostCategory postCategory)
        {
            return CreateHttpResponse(requestMessage,
                () =>
                {
                    HttpResponseMessage responseMessage = null;
                    if (ModelState.IsValid)
                    {
                        requestMessage.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                    }
                    else
                    {
                        var category = _postCategoryService.Add(postCategory);
                        _postCategoryService.Save();
                        responseMessage = requestMessage.CreateResponse(HttpStatusCode.Created, category);
                    }
                    return responseMessage;
                });
        }

        [Route("getall")]
        public HttpResponseMessage Get(HttpRequestMessage requestMessage)
        {
            return CreateHttpResponse(requestMessage,
                () =>
                {
                    var listCategory = _postCategoryService.GetAll();

                    HttpResponseMessage response = requestMessage.CreateResponse(HttpStatusCode.OK, listCategory);

                    return response;
                });
        }

        public HttpResponseMessage Put(HttpRequestMessage requestMessage, PostCategory postCategory)
        {
            return CreateHttpResponse(requestMessage,
                () =>
                {
                    HttpResponseMessage responseMessage = null;
                    if (ModelState.IsValid)
                    {
                        requestMessage.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                    }
                    else
                    {
                        _postCategoryService.Update(postCategory);
                        _postCategoryService.Save();
                        responseMessage = requestMessage.CreateResponse(HttpStatusCode.OK);
                    }
                    return responseMessage;
                });
        }

        public HttpResponseMessage Delete(HttpRequestMessage requestMessage, int id)
        {
            return CreateHttpResponse(requestMessage,
                () =>
                {
                    HttpResponseMessage responseMessage = null;
                    if (ModelState.IsValid)
                    {
                        requestMessage.CreateErrorResponse(HttpStatusCode.BadGateway, ModelState);
                    }
                    else
                    {
                        _postCategoryService.Delete(id);
                        _postCategoryService.Save();
                        responseMessage = requestMessage.CreateResponse(HttpStatusCode.OK);
                    }
                    return responseMessage;
                });
        }
    }
}