using System.Net;
using System.Text.Json;
//using FleetTrackAPI.Repositories;

namespace FleetTrackAPI.Middleware
{
    public class ErrorHandlerMiddleware
    {
         private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
         
         public async Task Invoke(HttpContext context)
        {
            try
            {
                //Intenta ejecutar el siguente paso
                await _next(context);
            }catch(Exception ex)
            {
                //Si algo sale mal
                await ManejarErrorAsync(context, ex);
            }
        }
    

    private static Task ManejarErrorAsync(HttpContext context, Exception exception)
        {
            var codigo = HttpStatusCode.InternalServerError; // 500 error
            var mensaje = "Ocurrio un error interno. Intentar mas tarde";

            //Puedes manejar tipos especificos de errores
            if (exception is KeyNotFoundException)
            {
                codigo = HttpStatusCode.NotFound; 
                mensaje = exception.Message;
            }
            else if(exception is ArgumentException)
            {
                codigo = HttpStatusCode.BadRequest;
                mensaje = exception.Message;
            }
            
            var repuesta = JsonSerializer.Serialize(new 
            { 
                error = mensaje,
                status = (int)codigo

            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)codigo;

            return  context.Response.WriteAsync(repuesta);
        }
    }
}