namespace Hisoka.Configuration
{
    public interface IHisokaTypeConfiguration<TEntity>
    {
        void Configure(HisokaTypeConfigurationBuilder<TEntity> builder);
    }
}