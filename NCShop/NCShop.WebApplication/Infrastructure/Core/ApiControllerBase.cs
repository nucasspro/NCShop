using NCShop.Model.Models;
using NCShop.Service;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NCShop.WebApplication.Infrastructure.Core
{
    public class ApiControllerBase : ApiController
    {
        public IErrorSercive _errorSercive;

        public ApiControllerBase(IErrorSercive errorSercive)
        {
            this._errorSercive = errorSercive;
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage requestMessage, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;
            try
            {
                response = function.Invoke();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors.");
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Trace.WriteLine($"- Property:\"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                    }
                }
                LogError(ex);
                response = requestMessage.CreateResponse(HttpStatusCode.BadGateway, ex.InnerException.Message);
            }
            catch (DbUpdateException dbx)
            {
                LogError(dbx);

                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, dbx.InnerException.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);

                response = requestMessage.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
            return response;
        }

        private void LogError(Exception ex)
        {
            try
            {
                Error error = new Error();
                error.CreatedDate = DateTime.Now;
                error.Message = ex.Message;
                error.StackTrace = ex.StackTrace;
                _errorSercive.Create(error);
                _errorSercive.Save();
            }
            catch
            {
                throw;
            }
        }
    }
}