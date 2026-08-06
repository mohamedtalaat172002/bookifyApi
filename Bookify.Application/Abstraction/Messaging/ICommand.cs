using bookify.domain.Abstractions;
using MediatR;

namespace Bookify.Application.Abstraction.Messaging
{
    public interface ICommand : IRequest<Result>, IBaseCommand
    {

    }

    public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
    {

    }


    public interface IBaseCommand
    {

    }

}
