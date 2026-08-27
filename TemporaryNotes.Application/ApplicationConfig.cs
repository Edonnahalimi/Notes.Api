using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TemporaryNotes.Application.Behaviors;
using TemporaryNotes.Application.Note.Commands;

namespace TemporaryNotes.Application;

public static class ApplicationConfig
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(
                typeof(CreateNoteCommand).Assembly);
        });

        services.AddValidatorsFromAssembly(
            typeof(CreateNoteCommandValidator).Assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}