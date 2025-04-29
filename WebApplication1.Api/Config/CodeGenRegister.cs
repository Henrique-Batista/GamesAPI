using Mapster;
using WebApplication1.API.Domain;

namespace WebApplication1.API.Config;

public class CodeGenRegister : ICodeGenerationRegister
{
    public void Register(CodeGenerationConfig config)
    {
        config.AdaptTo("[name]RequestDto",MapType.Map)
            .ExcludeTypes(type => type.IsEnum)
            .ExcludeTypes(typeof(BaseEntity))
            .ExcludeTypes(type => type.Name.EndsWith("Dto"))
            .ForType<Game>(cfg =>
            {
                cfg.Ignore(entity => entity.Studio);
                cfg.Ignore(entity => entity.UpdatedAt);
                cfg.Ignore(entity => entity.CreatedAt);
                cfg.Ignore(entity => entity.Id);
                cfg.Map(entity => entity.Name, entity => entity.Name);
            })
            .ForType<Studio>(cfg =>
            {
                cfg.Ignore(entity => entity.Games);
                cfg.Ignore(entity => entity.UpdatedAt);
                cfg.Ignore(entity => entity.CreatedAt);
                cfg.Ignore(entity => entity.Id);
                cfg.Map(entity => entity.Name, entity => entity.Name);
                cfg.Map(entity => entity.Country, entity => entity.Country);
            });
        config.AdaptTo("[name]RequestWithIdDto",MapType.Map)
            .ExcludeTypes(type => type.IsEnum)
            .ExcludeTypes(typeof(BaseEntity))
            .ExcludeTypes(type => type.Name.EndsWith("Dto"))
            .ForType<Game>(cfg =>
            {
                cfg.Ignore(entity => entity.Studio);
                cfg.Ignore(entity => entity.UpdatedAt);
                cfg.Ignore(entity => entity.CreatedAt);
                cfg.Map(entity => entity.Name, entity => entity.Name);
            })
            .ForType<Studio>(cfg =>
            {
                cfg.Ignore(entity => entity.Games);
                cfg.Ignore(entity => entity.UpdatedAt);
                cfg.Ignore(entity => entity.CreatedAt);
                cfg.Map(entity => entity.Id, entity => entity.Id);
                cfg.Map(entity => entity.Name, entity => entity.Name);
                cfg.Map(entity => entity.Country, entity => entity.Country);
            });
        
        config.AdaptTo("[name]ResponseDto")
            .ExcludeTypes(type => type.IsEnum)
            .ExcludeTypes(typeof(BaseEntity))
            .ExcludeTypes(type => type.Name.EndsWith("Dto"))
            .ForType<Game>(cfg =>
            {
                cfg.Ignore(entity => entity.Studio);
            })
            .ForType<Studio>(cfg =>
            {
                cfg.Ignore(entity => entity.Games);
            });

        config.GenerateMapper("[name]Mapper")
            .ForType<Studio>()
            .ForType<Game>();
    }
}