# EntityModelExtensions
Additional helpers for EntityFramework Core

## Installing
* You can use nuget to install package `Install-Package EntityModelExtensions`.
* Or you can use dotnet cli `dotnet add package EntityModelExtensions`.

## Auto registration for model configuration

Apply on DbContext.

```sh
public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseAutoRegisterModelConfiguration<AppDbContext>();
        }
    }
```

Then you can add IEntityTypeConfiguration to model.

```sh
    public class EntityModel
    {
        public long DreamId { get; set; }
        public long FriendId { get; set; }

        public class EntityModelConfig : IEntityTypeConfiguration<EntityModel>
        {
            public void Configure(EntityTypeBuilder<EntityModel> builder)
            {
                builder.HasKey(x => new { x.DreamId, x.FriendId });
            }
        }
    }
```

And this configuration automatically apply to DbContext, without having to add DbSet manually.

## SqlDefaultValue attribute

Apply on DbContext.

```sh
public class AppDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.UseAdditionalAttributes();
        }
    }
```

Then you can add SqlDefaultValue attribute to model.

```sh
    public class EntityModel
    {
        [SqlDefaultValue("getutcdate()")]
        public DateTime Created { get; set; }
    }
```

And if you make migration, then you can see:


```sh
   migrationBuilder.CreateTable(name: "EntityModel",
        columns: table => new
        {
            Created = table.Column<DateTime>(nullable: false, defaultValueSql: "getutcdate()"),
        });
```