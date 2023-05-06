using GithubFeatured.Infra.Interfaces;

namespace GithubFeatured.Middlewares
{
    public class UnitOfWorkHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public UnitOfWorkHandlerMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
        {
            await _next(context);
            await unitOfWork.CommitAsync();
        }
    }
}
