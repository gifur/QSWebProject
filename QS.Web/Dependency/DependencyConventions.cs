using System.Web.Mvc;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using QS.Common.Logging;
using QS.Common.Validator;
using QS.Core.IRepository;
using QS.DAL;
using QS.DAL.Contract;
using QS.Repository.Module;
using QS.Service;
using QS.Service.Effection;

namespace QS.Web.Dependency
{
    /// <summary>
    /// 控制反转（Inversion of Control, IoC)，应用本身不负责依赖对象的创建和维护，而交给一个外部容器来负责，
    /// 这样控制权就由该应用转移到外部IoC容器，控制权就实现了所谓的反转
    /// 有时又将IoC称为依赖注入（Dependency Injection， DI）就是由外部容器在运行时动态地将依赖的对象注入到
    /// 组件之中
    /// </summary>
    public class DependencyConventions : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                    .BasedOn<IController>()
                    .LifestyleTransient());

            container.Register(
                    Component.For<IQueryableUnitOfWork, UnitOfWork>().ImplementedBy<UnitOfWork>().LifeStyle.Transient,
                    Component.For<IUserRepository, UserRepository>().ImplementedBy<UserRepository>().LifeStyle.Transient,
                    Component.For<IFeedbackRepository, FeedbackRepository>().ImplementedBy<FeedbackRepository>().LifeStyle.Transient,
                    Component.For<IFbDocumentRepository, FbDocumentRepository>().ImplementedBy<FbDocumentRepository>().LifeStyle.Transient,
                    Component.For<IReservationRepository, ReservationRepository>().ImplementedBy<ReservationRepository>().LifeStyle.Transient,
                    Component.For<INewsRepository, NewsRepository>().ImplementedBy<NewsRepository>().LifeStyle.Transient,
                    Component.For<IArticleRepository, ArticleRepository>().ImplementedBy<ArticleRepository>().LifeStyle.Transient,
                    Component.For<IPhotoRepository, PhotoRepository>().ImplementedBy<PhotoRepository>().LifeStyle.Transient,
                    Component.For<IAtlasRepository, AtlasRepository>().ImplementedBy<AtlasRepository>().LifeStyle.Transient,
                    Component.For<IBookRepository, BookRepository>().ImplementedBy<BookRepository>().LifeStyle.Transient,
                    Component.For<IVideoRepository, AtlasRepository>().ImplementedBy<VideoRepository>().LifeStyle.Transient,
                    Component.For<INewsCommentRepository, NewsCommentRepository>().ImplementedBy<NewsCommentRepository>().LifeStyle.Transient,
                    Component.For<IArticleCommentRepository, ArticleCommentRepository>().ImplementedBy<ArticleCommentRepository>().LifeStyle.Transient,
                    Component.For<IBookCommentRepository, BookCommentRepository>().ImplementedBy<BookCommentRepository>().LifeStyle.Transient,
                    Component.For<IVideoCommentRepository, VideoCommentRepository>().ImplementedBy<VideoCommentRepository>().LifeStyle.Transient,
                    Component.For<ISuggestionRepository, SuggestionRepository>().ImplementedBy<SuggestionRepository>().LifeStyle.Transient,
                    Component.For<IRecentActivityRepository, RecentActivityRepository>().ImplementedBy<RecentActivityRepository>().LifeStyle.Transient,
                    Component.For<ILogRepository, LogRepository>().ImplementedBy<LogRepository>().LifeStyle.Transient,
                    Component.For<ILoginLogRepository, LoginLogRepository>().ImplementedBy<LoginLogRepository>().LifeStyle.Transient,
                    Component.For<IMessageRepository, MessageRepository>().ImplementedBy<MessageRepository>().LifeStyle.Transient,
                    Component.For<IMyMessageRepository, MyMessageRepository>().ImplementedBy<MyMessageRepository>().LifeStyle.Transient,
                    Component.For<ITagRepository, TagRepository>().ImplementedBy<TagRepository>().LifeStyle.Transient,

                    Component.For<IUserService>().ImplementedBy<UserService>().LifeStyle.Transient,
                    Component.For<IFeedbackService>().ImplementedBy<FeedbackService>().LifeStyle.Transient,
                    Component.For<IFbDocumentService>().ImplementedBy<FbDocumentService>().LifeStyle.Transient,
                    Component.For<IReservationService>().ImplementedBy<ReservationService>().LifeStyle.Transient,
                    Component.For<INewsService>().ImplementedBy<NewsService>().LifeStyle.Transient,
                    Component.For<IArticleService>().ImplementedBy<ArticleService>().LifeStyle.Transient,
                    Component.For<IPhotoService>().ImplementedBy<PhotoService>().LifeStyle.Transient,
                    Component.For<IAtlasService>().ImplementedBy<AtlasService>().LifeStyle.Transient,
                    Component.For<IBookService>().ImplementedBy<BookService>().LifeStyle.Transient,
                    Component.For<IVideoService>().ImplementedBy<VideoService>().LifeStyle.Transient,
                    Component.For<ICommentService>().ImplementedBy<CommentService>().LifeStyle.Transient,
                    Component.For<ISuggestionService>().ImplementedBy<SuggestionService>().LifeStyle.Transient,
                    Component.For<IRecentActivityService>().ImplementedBy<RecentActivityService>().LifeStyle.Transient,
                    Component.For<ILoginLogService>().ImplementedBy<LoginLogService>().LifeStyle.Transient,
                    Component.For<ILogService>().ImplementedBy<LogService>().LifeStyle.Transient,
                    Component.For<IMessageService>().ImplementedBy<MessageService>().LifeStyle.Transient,
                    Component.For<IMyMessageService>().ImplementedBy<MyMessageService>().LifeStyle.Transient,
                    Component.For<ITagService>().ImplementedBy<TagService>().LifeStyle.Transient

                    //AllTypes.FromThisAssembly().BasedOn<IHttpController>().LifestyleTransient()
                ).AddFacility<LoggingFacility>(f => f.UseLog4Net());
            //扩张单元插件（Facilities）可以在不更改原有组件的基础上注入你所需要的功能代码，
            //AddFacility方法来添加扩展单元来注册并管理我们的组件。
            //组件的生命周期基本上都被指定成为Transient类型，即当请求发生时创建，在处理结束后销毁。

            LoggerFactory.SetCurrent(new TraceSourceLogFactory());
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }
    }
}