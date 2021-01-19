using Hisoka.Configuration;

namespace HisokaExample.Model
{
    public class UserConfiguration : IHisokaTypeConfiguration<User>
    {
        public void Configure(HisokaTypeConfigurationBuilder<User> builder)
        {
            builder.Property(x => x.Id);
            builder.Property(x => x.FirstName, "primeiroNome");
            builder.Property(x => x.DateOfBirth, "dataNascimento");
            builder.Property(x => x.LastName, "ultimoNome");
            builder.Property(x => x.UserRoles, "usuarioPerfis");
        }
    }

    public class UserRoleConfiguration : IHisokaTypeConfiguration<UserRole>
    {
        public void Configure(HisokaTypeConfigurationBuilder<UserRole> builder)
        {
            builder.Property(x => x.Id);
            builder.Property(x => x.Role, "perfil");
            builder.Property(x => x.User, "usuario");
            builder.Property(x => x.UserId, "usuarioId");
            builder.Property(x => x.RoleId, "perfilId");
        }       
    }

    public class RoleConfiguration : IHisokaTypeConfiguration<Role>
    {
        public void Configure(HisokaTypeConfigurationBuilder<Role> builder)
        {
            builder.Property(x => x.Id);
            builder.Property(x => x.Name, "nome");
            builder.Property(x => x.Description, "descricao");
        }
    }
}