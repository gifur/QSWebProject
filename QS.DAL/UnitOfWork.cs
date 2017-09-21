using System.Data.Entity;
using QS.Core.Module;
using QS.Core.Module.CommentAggregate;
using QS.Core.Module.FeedbackAggregate;
using QS.Core.Module.LogAggregate;
using QS.Core.Module.ProfessionAggregate;
using QS.Core.Module.SharedAggregate;
using QS.DAL.EntityConfiguration;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using QS.DAL.Contract;


namespace QS.DAL
{
    public class UnitOfWork : DbContext, IQueryableUnitOfWork
    {
        //UnitOfWork  继承  DbContext 实现对实体模型的增删改查，通过base.SaveChanges()
        //来实现数据的更新，
        //从而把缓存区的上下文更改的数据一次提交到数据来实现事务。
        #region Constructor

        /// <summary>
        /// 初始化使用连接名称为“SQ.DAL.UnitOfWork”的（通过继承）数据访问上下文类
        /// </summary>
        public UnitOfWork()
            : base("name=SQ.DAL.UnitOfWork")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        /// <summary>  
        /// 通过指定数据连接名称或连接串的初始化
        /// </summary>  
        public UnitOfWork(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.ProxyCreationEnabled = true;
            this.Configuration.LazyLoadingEnabled = true;
        }

        #endregion Constructor

        #region IDbSet Members

        public DbSet<User> User { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<FbDocument> FbDocument { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Photo> Photo { get; set; }
        public DbSet<Atlas> Atlas { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<NewsComment> NewsComment { get; set; }
        public DbSet<ArticleComment> ArticleComment { get; set; }
        public DbSet<BookComment> BookComment { get; set; }
        public DbSet<VideoComment> VideoComment { get; set; }
        public DbSet<Suggestion> Suggestion { get; set; }
        public DbSet<RecentActivity> RecentActivity { get; set; }
        public DbSet<LoginLog> UserLog { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Core.Module.Message> Message { get; set; }
        public DbSet<MyMessage> MyMessage { get; set; }
        public DbSet<Tag> Tag { get; set; }

        #endregion

        #region IQueryableUnitOfWork Members

        public DbSet<TEntity> CreateSet<TEntity>()
            where TEntity : class
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// 连接且将状态设为未改变
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="item"></param>
        public void Attach<TEntity>(TEntity item)
            where TEntity : class
        {
            base.Entry<TEntity>(item).State = (EntityState) System.Data.EntityState.Unchanged;
        }

        public void SetModified<TEntity>(TEntity item)
            where TEntity : class
        {
            //this operation also attach item in object state manager
            base.Entry<TEntity>(item).State = (EntityState) System.Data.EntityState.Modified;
        }
        public void ApplyCurrentValues<TEntity>(TEntity original, TEntity current)
            where TEntity : class
        {
            //if it is not attached, attach original and set current values
            base.Entry<TEntity>(original).CurrentValues.SetValues(current);
        }

        #region implement QS.Core.IUnitOfWork

        public void Commit()
        {
            base.SaveChanges();
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                    saveFailed = false;

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                              .ForEach(entry =>
                              {
                                  entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                              });

                }
            } while (saveFailed);

        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            base.ChangeTracker.Entries()
                              .ToList()
                              .ForEach(entry => entry.State = (EntityState) System.Data.EntityState.Unchanged);
        }

        #endregion

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery, params object[] parameters)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery, parameters);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string sqlQuery)
        {
            return base.Database.SqlQuery<TEntity>(sqlQuery);
        }

        public int ExecuteCommand(string sqlCommand, params object[] parameters)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand, parameters);
        }
        public int ExecuteCommand(string sqlCommand)
        {
            return base.Database.ExecuteSqlCommand(sqlCommand);
        }

        #endregion

        #region DbContext Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //移除一对多的级联删除约定，想要级联删除可以在
            //EntityTypeConfiguration<TEntity>的实现类中进行控制
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //多对多启用级联删除约定，不想级联删除可以在删除前判断关联的数据进行拦截  
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //Add entity configurations in a structured way using 'TypeConfiguration’ classes
            modelBuilder.Configurations.Add(new UserConfiguration());

            modelBuilder.Configurations.Add(new FeedbackConfiguration());
            modelBuilder.Configurations.Add(new FbDocumentConfiguration());

            modelBuilder.Configurations.Add(new ReservationConfiguration());

            modelBuilder.Configurations.Add(new ArticleConfiguration());
            modelBuilder.Configurations.Add(new NewsConfiguration());
            modelBuilder.Configurations.Add(new PhotoConfiguration());
            modelBuilder.Configurations.Add(new AtlasConfiguration());
            modelBuilder.Configurations.Add(new BookConfiguration());
            modelBuilder.Configurations.Add(new VideoConfiguration());
            modelBuilder.Configurations.Add(new NewsCommentConfiguration());
            modelBuilder.Configurations.Add(new ArticleCommentConfiguration());
            modelBuilder.Configurations.Add(new BookCommentConfiguration());
            modelBuilder.Configurations.Add(new VideoCommentConfiguration());
            modelBuilder.Configurations.Add(new SuggestionConfiguration());
            modelBuilder.Configurations.Add(new RecentActivityConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
            modelBuilder.Configurations.Add(new MyMessageConfiguration());
            modelBuilder.Configurations.Add(new TagConfiguration());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        #endregion

    }
}
