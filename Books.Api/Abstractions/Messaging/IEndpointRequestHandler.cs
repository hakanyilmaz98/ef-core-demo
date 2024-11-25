using MediatR;

namespace Books.Api.Abstractions.Messaging;

public interface IEndpointRequestHandler<T> : IRequestHandler<T, IResult> where T : IRequest<IResult>;
