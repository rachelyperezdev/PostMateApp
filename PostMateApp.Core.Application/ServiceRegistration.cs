using Microsoft.Extensions.DependencyInjection;
using PostMateApp.Core.Application.Interfaces.Services;
using PostMateApp.Core.Application.Services;
using System.Reflection;

namespace PostMateApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #region Services
            services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IFriendshipService, FriendshipService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IReplyService, ReplyService>();
            #endregion
        }
    }
}
